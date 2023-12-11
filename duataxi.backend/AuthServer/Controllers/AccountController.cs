using System;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Extensions;
using AuthServer.Infrastructure.Constants;
using AuthServer.Infrastructure.Data.Identity;
using AuthServer.Models;
using DuaTaxi.AuthServer.Controllers;
using DuaTaxi.AuthServer.Messages.Customers;
using DuaTaxi.AuthServer.Messages.Customers.Delete;
using DuaTaxi.AuthServer.Messages.Payments;
using DuaTaxi.Common;
using DuaTaxi.Common.Mvc;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.Types;
using DuaTaxi.Entities.Core.Models;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace AuthServer.Controllers
{
    
    [SecurityHeaders]
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IBusPublisher _busPublisher;



        public AccountController(UserManager<AppUser> userManager,
                                 IBusPublisher busPublisher,
                                 ITracer tracer,
                                 IIdentityServerInteractionService interaction,
                                 IAuthenticationSchemeProvider schemeProvider,
                                 IClientStore clientStore,
                                 IEventService events) : base(busPublisher, tracer)

        {
            _userManager = userManager;
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
            _events = events;
            _busPublisher = busPublisher;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                // validate username/password
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.Name));

                    // only set explicit expiration here if user chooses "remember me". 
                    // otherwise we rely upon expiration configured in cookie middleware.
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    // issue authentication cookie with subject ID and username
                    await HttpContext.SignInAsync(user.Id, user.UserName, props);

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/account/Logout")]
        public async Task<IActionResult> Logout([FromBody] Guid logoutId  )
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoginViewModelAsync(logoutId.ToString());

            var user = HttpContext.User;
            if (user?.Identity.IsAuthenticated == true) {
                // delete local authentication cookie
                // await HttpContext.SignOutAsync("Bearer");
                // Signout oidc
                //await HttpContext.SignOutAsync("oidc");
                await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

                await _events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetName()));

            }
            return Redirect("http://localhost:4200/index");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/account/delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequestViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null) {
                throw new DuaTaxiException($"User with email : {model.Email} does not exist ");
            }
            var getclaims = await _userManager.GetClaimsAsync(user);
            var role = getclaims.Where(x => x.Type.Contains("role")).Select(z => z.Value).FirstOrDefault();

            switch (role ) {               
                case Roles.TaxiDriver:
                    var deletedriver = new DeleteTaxiDriverCustomer(Guid.Empty.ToString(), user.Id );
                    await SendAsync(deletedriver.BindId(c => c.Id), resourceId: deletedriver.Id.ToGuid(), resource: "taxiapi");
                    break;
                case Roles.MiniBusDriver:
                    var deletedriver2 = new DeleteMiniBusDriverCustomer(Guid.Empty.ToString(), user.Id);
                    await SendAsync(deletedriver2.BindId(c => c.Id), resourceId: deletedriver2.Id.ToGuid(), resource: "minibusapi");
                    break;
                case Roles.BusDriver:
                    var deletedriver3 = new DeleteBusDriverCustomer(Guid.Empty.ToString(), user.Id);
                    await SendAsync(deletedriver3.BindId(c => c.Id), resourceId: deletedriver3.Id.ToGuid(), resource: "busapi");
                    break;
                default:
                    break;
            }

            await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            await _userManager.DeleteAsync(user);

            return Ok();
        }



        [HttpPost]
        [Route("api/account/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel model)
        {                         

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser { UserName = model.Email, Name = model.Name, Email = model.Email ,PhoneNumber=model.PhoneNumber};

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("name", user.Name));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", Roles.Consumer));
         
            return Ok(new RegisterResponseViewModel(user));
        }

        [HttpPost]
        [Route("api/account/registerdriver")]
        public async Task<IActionResult> RegisterDriver([FromBody] DriverRegisterRequestViewModel model)
        {          

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            var user = new AppUser { UserName = model.Email, Name = model.Name, Email = model.Email ,PhoneNumber=model.PhoneNumber};

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("name", user.Name));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));



            switch (model.Type)
            {

                case DriverTypes.TaxiDriver:
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", Roles.TaxiDriver));                    
                               
                    var payment = new CreateFirstPayment(Guid.Empty.ToString(), user.Id, user.Name, user.Email, user.PhoneNumber, DriverTypes.TaxiDriver);
                    await SendAsync(payment.BindId(c => c.Id), resourceId: payment.Id.ToGuid(), resource: "payment");
                    
                    
                    var createTaxiDriver = new CreateTaxiDriverCustomer(Guid.Empty.ToString(), user.Id, user.Name, user.Email, user.PhoneNumber, DriverTypes.TaxiDriver);                  
                    await SendAsync(createTaxiDriver.BindId(c => c.Id), resourceId: createTaxiDriver.Id.ToGuid(), resource: "taxiapi");
                    
                    break;   
                case DriverTypes.MiniBusDriver:
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", Roles.MiniBusDriver));

                    var paymentMiniBus = new CreateFirstPayment(Guid.Empty.ToString(), user.Id, user.Name, user.Email, user.PhoneNumber, DriverTypes.TaxiDriver);
                    await SendAsync(paymentMiniBus.BindId(c => c.Id), resourceId: paymentMiniBus.Id.ToGuid(), resource: "payment");

                    var createMiniBusDriver = new CreateMiniBusDriverCustomer(user.Id, user.Name, user.Email, user.PhoneNumber, DriverTypes.TaxiDriver);
                    await SendAsync(createMiniBusDriver.BindId(c => c.Id), resourceId: createMiniBusDriver.Id.ToGuid(), resource: "minibusapi");

                    break;
                case DriverTypes.BusDriver:
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", Roles.BusDriver));

                    var paymentBusDriver = new CreateFirstPayment(Guid.Empty.ToString(), user.Id, user.Name, user.Email, user.PhoneNumber, DriverTypes.TaxiDriver);
                    await SendAsync(paymentBusDriver.BindId(c => c.Id), resourceId: paymentBusDriver.Id.ToGuid(), resource: "payment");

                    var createBusDriver = new CreateBusDriverCustomer(user.Id, user.Name, user.Email, user.PhoneNumber, DriverTypes.TaxiDriver);                    
                    await SendAsync(createBusDriver.BindId(c => c.Id), resourceId: createBusDriver.Id.ToGuid(), resource: "busapi");

                    break;
                default:
                    return BadRequest(result.Errors);
                  
            }


            return Ok(new RegisterResponseViewModel(user));
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes.Where(x => x.DisplayName != null || (x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
    }
}