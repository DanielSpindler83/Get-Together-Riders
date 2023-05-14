// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Get_Together_Riders.Models;


namespace Get_Together_Riders.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly IRiderRepository _riderRepository;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IRiderRepository riderRepository
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _riderRepository = riderRepository;
        }


        public string Email { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ReturnUrl { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet() => RedirectToPage("/");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // We land here - immediately after we click the Facebook Login button
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            // here is where we get the info back from Facebook
            // something similar to here --> https://www.c-sharpcorner.com/article/retrieve-user-details-from-facebook-in-asp-net-core-applications/
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Attempt to sign user in locally using details from external login provider
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded) // if user already exists locally
            {
                // get details of the local user that already exists
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider. Local identity id = {IdentityId}", info.Principal.Identity.Name, info.LoginProvider, user.Id);

                // display all claims to log
                var claims = info.Principal.Claims.ToList();
                foreach (Claim claim in claims)
                {
                    _logger.LogInformation("{ClaimType} : {ClaimValue}", claim.Type, claim.Value);
                }

                var profilePictureUrl = claims?.FirstOrDefault(x => x.Type.Equals("Picture", StringComparison.OrdinalIgnoreCase))?.Value;
                _logger.LogInformation("This user's profile picture URL = {profilePictureUrl}", profilePictureUrl);

                //I added the below to pull email from claim returned by facebook
                _logger.LogInformation("{Email} logged in with {LoginProvider} provider.", info.Principal.FindFirstValue(ClaimTypes.Email), info.LoginProvider);
                _logger.LogInformation("{NameIdentifier} logged in with {LoginProvider} provider.", info.Principal.FindFirstValue(ClaimTypes.NameIdentifier), info.LoginProvider);

                // try and match facebook email to a rider email - is this an existing rider that exists in the DB?
                var rider = _riderRepository.GetRiderByEmail(info.Principal.FindFirstValue(ClaimTypes.Email));

                if (rider != null)
                {
                    // existing rider
                    _logger.LogInformation("{Email} is an existing Rider. They already exist in the DB with IdentityId = {IdentityId}", info.Principal.FindFirstValue(ClaimTypes.Email), rider.IdentityUserId );
                    _logger.LogInformation("{Rider}", rider.ToString()); // just outputs the type - need a rider to string override coded up

                    // for now do nothing - this user simply logs in - happy path - existing user with a local user
                    return LocalRedirect(returnUrl);

                } else
                {
                    // no existing rider associated with this facebook email
                    _logger.LogInformation("{Email} is NOT existing Rider.", info.Principal.FindFirstValue(ClaimTypes.Email));
                    _logger.LogInformation("MESSAGE - please contact GTR admin to be setup in this app");
                    return RedirectToPage("/ContactAdmin");
                }

            }


            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, create one based on their email
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {

                    // try and match facebook email to a rider - is this an existing user?
                    var rider = _riderRepository.GetRiderByEmail(info.Principal.FindFirstValue(ClaimTypes.Email));

                    if (rider != null)
                    {
                        // existing rider
                        _logger.LogInformation("{Email} exists as a rider. Lets create the local user.", info.Principal.FindFirstValue(ClaimTypes.Email));
                        _logger.LogInformation("{Rider}", rider.ToString()); // just outputs the type - need a rider to string override coded up

                        Email = info.Principal.FindFirstValue(ClaimTypes.Email);

                        var user = CreateUser();

                        await _userStore.SetUserNameAsync(user, Email, CancellationToken.None);
                        await _emailStore.SetEmailAsync(user, Email, CancellationToken.None);

                        var createResult = await _userManager.CreateAsync(user);
                        if (createResult.Succeeded)
                        {
                            createResult = await _userManager.AddLoginAsync(user, info);
                            if (createResult.Succeeded)
                            {
                                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                                var userId = await _userManager.GetUserIdAsync(user);

                                //Set Rider Identity Id    
                                rider.IdentityUserId = userId;
                                _riderRepository.UpdateRider(rider);
                                _riderRepository.SaveChanges();
                                _logger.LogInformation("Rider Identity Id set as {userId}", userId);


                                await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                            }
                        }

                        ProviderDisplayName = info.ProviderDisplayName;
                    }
                    else
                    {
                        // no existing rider associated with this facebook email
                        _logger.LogInformation("{Email} is NOT existing Rider and has no local login. A login will NOT be created.", info.Principal.FindFirstValue(ClaimTypes.Email));
                        _logger.LogInformation("MESSAGE - please contact GTR admin to be setup in this app");
                        return RedirectToPage("/ContactAdmin");
                    }

                }
                return LocalRedirect(returnUrl);
            }
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
