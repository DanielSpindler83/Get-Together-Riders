﻿#nullable disable

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Get_Together_Riders.Models.Interfaces;

namespace Get_Together_Riders.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly IRiderRepository _riderRepository;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IRiderRepository riderRepository
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _logger = logger;
            _riderRepository = riderRepository;
        }


        public string Email { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ReturnUrl { get; set; } = "/";
        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet() => RedirectToPage("/");

        public IActionResult OnPost(string provider)
        {
            // We land here - immediately after we click the Facebook Login button
            // We then request a redirect to the external login provider(Facebook)
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { ReturnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string remoteError = null)
        {
            //// Return here after calling External Login provider

            if (remoteError != null)
            {
                throw new Exception($"Error from external provider: {remoteError}");
            }

            var externalLoginInfo = await GetExternalLoginInfoAsync();

            // Attempt to sign user in locally using details from external login provider
            var result = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded) // if user already exists locally in AspNetUsers table
            {
                return await SignInUserLocally(externalLoginInfo);
            }
            else // user does not exist in AspNetUsers table - we have not seen this user before
            {
                return await NewUserChecks(externalLoginInfo);
            }

        } //end of method

        private async Task<IActionResult> NewUserChecks(ExternalLoginInfo externalLoginInfo)
        {
            // If the user does not have an account, create one based on their email
            ProviderDisplayName = externalLoginInfo.ProviderDisplayName;
            if (externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {

                // try and match facebook email to a rider - is this an existing user?
                var rider = _riderRepository.GetRiderByEmail(externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email));

                if (rider != null)
                {
                    // existing rider
                    _logger.LogInformation("{Email} exists as a rider. Lets create the local user.", externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email));
                    _logger.LogInformation("{Rider}", rider.ToString()); // just outputs the type - need a rider to string override coded up

                    Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

                    var user = CreateUser();

                    await _userStore.SetUserNameAsync(user, Email, CancellationToken.None);

                    var createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        createResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                        if (createResult.Succeeded)
                        {
                            _logger.LogInformation("User created an account using {Name} provider.", externalLoginInfo.LoginProvider);

                            var userId = await _userManager.GetUserIdAsync(user);

                            //Set Rider Identity Id    
                            rider.IdentityUserId = userId;
                            _riderRepository.UpdateRider(rider);
                            _riderRepository.SaveChanges();
                            _logger.LogInformation("Rider Identity Id set as {userId}", userId);


                            await _signInManager.SignInAsync(user, isPersistent: false, externalLoginInfo.LoginProvider);
                        }
                    }

                    ProviderDisplayName = externalLoginInfo.ProviderDisplayName;
                }
                else
                {
                    // no existing rider associated with this facebook email
                    _logger.LogInformation("{Email} is NOT existing Rider and has no local login. A login will NOT be created.", externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email));
                    _logger.LogInformation("MESSAGE - please contact GTR admin to be setup in this app");
                    return RedirectToPage("/ContactAdmin");
                }

            }
            return LocalRedirect(ReturnUrl);
        }

        private async Task<IActionResult> SignInUserLocally(ExternalLoginInfo externalLoginInfo)
        {
            // get details of the local user that already exists
            var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider. Local identity id = {IdentityId}", externalLoginInfo.Principal.Identity.Name, externalLoginInfo.LoginProvider, user.Id);

            // display all claims to log
            var claims = externalLoginInfo.Principal.Claims.ToList();
            foreach (Claim claim in claims)
            {
                _logger.LogInformation("{ClaimType} : {ClaimValue}", claim.Type, claim.Value);
            }

            var profilePictureUrl = claims?.FirstOrDefault(x => x.Type.Equals("Picture", StringComparison.OrdinalIgnoreCase))?.Value;
            _logger.LogInformation("This user's profile picture URL = {profilePictureUrl}", profilePictureUrl);

            //I added the below to pull email from claim returned by facebook
            _logger.LogInformation("{Email} logged in with {LoginProvider} provider.", externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email), externalLoginInfo.LoginProvider);
            _logger.LogInformation("{NameIdentifier} logged in with {LoginProvider} provider.", externalLoginInfo.Principal.FindFirstValue(ClaimTypes.NameIdentifier), externalLoginInfo.LoginProvider);

            // try and match facebook email to a rider email - is this an existing rider that exists in the DB?
            var rider = _riderRepository.GetRiderByEmail(externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email));

            if (rider != null)
            {
                // existing rider
                _logger.LogInformation("{Email} is an existing Rider. They already exist in the DB with IdentityId = {IdentityId}", externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email), rider.IdentityUserId);
                _logger.LogInformation("{Rider}", rider.ToString()); // just outputs the type - need a rider to string override coded up

                // for now do nothing - this user simply logs in - happy path - existing user with a local user
                return LocalRedirect(ReturnUrl);

            }
            else
            {
                // no existing rider associated with this facebook email
                _logger.LogInformation("{Email} is NOT existing Rider.", externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email));
                _logger.LogInformation("MESSAGE - please contact GTR admin to be setup in this app");
                return RedirectToPage("/ContactAdmin");
            }
        }

        private async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            // here is where we get the externalLoginInfo back from Facebook
            // something similar to here --> https://www.c-sharpcorner.com/article/retrieve-user-details-from-facebook-in-asp-net-core-applications/
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new Exception("Error loading external login information."); 
            }
            return info;
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

    }
}
