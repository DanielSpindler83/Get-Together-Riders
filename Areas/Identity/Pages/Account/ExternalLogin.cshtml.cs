// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;


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

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // We land here - immediatley after we Click the Facebook button on Login page

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

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {

                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);

                var claims = info.Principal.Claims.ToList();

                // log out all claims
                foreach (Claim claim in claims)
                {
                    _logger.LogInformation("{ClaimType} : {ClaimValue}", claim.Type, claim.Value);
                }


                var profilePictureUrl = claims?.FirstOrDefault(x => x.Type.Equals("Picture", StringComparison.OrdinalIgnoreCase))?.Value;
                _logger.LogInformation("This user's profile picture URL = {profilePictureUrl}", profilePictureUrl);

                //I added the below to pull email from claim returned by facebook
                _logger.LogInformation("{Email} logged in with {LoginProvider} provider.", info.Principal.FindFirstValue(ClaimTypes.Email), info.LoginProvider);
                _logger.LogInformation("{NameIdentifier} logged in with {LoginProvider} provider.", info.Principal.FindFirstValue(ClaimTypes.NameIdentifier), info.LoginProvider);
                //_logger.LogInformation("{NameIdentifier} logged in with {LoginProvider} provider.", info.Principal.FindFirstValue(ClaimTypes.Picture), info.LoginProvider);

                /* pull back a URL for the user's profile picture
                string nameIdentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string thumbnailUrl = $"https://graph.facebook.com/{nameIdentifier}/picture?fields=url";

                //string profilePicUrl = null;
                using (HttpClient httpClient = new HttpClient())
                {
                    var profilePicUrl = await httpClient.GetAsync(thumbnailUrl);
                    _logger.LogInformation(profilePicUrl.ToString());
                }
                */

                /* https://developers.facebook.com/docs/graph-api/reference/user/picture/
                // Apps in Development mode that make tokenless requests on ASIDs will receive a silhouette image in response.
                // Saving picture to a file
                byte[] thumbnailBytes = null;
                string nameIdentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string thumbnailUrl = $"https://graph.facebook.com/{nameIdentifier}/picture?redirect=false";
                using (HttpClient httpClient = new HttpClient())
                {
                    thumbnailBytes = await httpClient.GetByteArrayAsync(thumbnailUrl);
                }

                string path = $"C:\\temp\\{nameIdentifier}.jpg";

                System.IO.File.WriteAllBytes(path, thumbnailBytes);
                */


                /*
                // https://stackoverflow.com/questions/9620278/how-do-i-make-calls-to-a-rest-api-using-c
                string urlParameters = "?redirect=false";
                string nameIdentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string URL = $"https://graph.facebook.com/{nameIdentifier}/picture";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // Get data response
                var response = client.GetAsync(urlParameters).Result;

                _logger.LogInformation(response.Content.);

                client.Dispose();
                */


                /*
                // this works but only with a temp access token added in - no idea where to get an access token for the call
                string nameIdentifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string thumbnailUrl = $"https://graph.facebook.com/{nameIdentifier}/picture?fields=url";
                var accessToken = "EAA1CMNQ6SgQBAJLrvYwc3zUySZB1snI1M42pqHavHxPA0bcf0sYQPD7VvodjEi0NR9gVZC0iOOP6f3twmqBDeG4IeYGboO2iEOBfvqhskZCZBdvZAlotyLA1EVdp7K3BCEyMly67ZBQdZBTZCvpcJPjNfLjvsoSNBtgnDbhB7QGC3ZCROU2pPqL0L";
                var client = new FacebookClient(accessToken);
                dynamic me = client.Get($"{nameIdentifier}/picture?redirect=false");
                */

                return LocalRedirect(returnUrl);
            }


            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
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
