using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SAML_SSO_2_0.Models;
using SAML_SSO_2_0.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SAML_SSO_2_0.Controllers
{
    [AllowAnonymous]
    [Route("Auth")]
    public class AuthController : Controller
    {
        const string relayStateReturnUrl = "ReturnUrl";
        private readonly Saml2Configuration config;

        public AuthController(IOptions<Saml2Configuration> configAccessor)
        {
            config = configAccessor.Value;
        }

        [Route("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            var binding = new Saml2RedirectBinding();
            binding.SetRelayStateQuery(new Dictionary<string, string> { { relayStateReturnUrl, returnUrl ?? Url.Content("~/") } });

            return binding.Bind(new Saml2AuthnRequest(config)).ToActionResult();
        }

        [Route("AssertionConsumerService")]
        public async Task<IActionResult> AssertionConsumerService()
        {
            var binding = new Saml2PostBinding();
            var saml2AuthnResponse = new Saml2AuthnResponse(config);

            binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);
            if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
            {
                throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
            }
            binding.Unbind(Request.ToGenericHttpRequest(), saml2AuthnResponse);
            await saml2AuthnResponse.CreateSession(HttpContext, claimsTransform: (claimsPrincipal) => ClaimsTransform.Transform(claimsPrincipal));

            var relayStateQuery = binding.GetRelayStateQuery();
            var returnUrl = relayStateQuery.ContainsKey(relayStateReturnUrl) ? relayStateQuery[relayStateReturnUrl] : Url.Content("~/");
            
            return RedirectToAction("Register");
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Content("~/"));
            }

            var binding = new Saml2PostBinding();
            var saml2LogoutRequest = await new Saml2LogoutRequest(config, User).DeleteSession(HttpContext);
            return Redirect("~/");
        }

        

        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Content("~/"));
            }

            return View();
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegViewModel param)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Content("~/"));
            }
            AddUserModel addModel = new AddUserModel();

            addModel.profile = new Profile();
            addModel.credentials = new Credentials();
            addModel.credentials.password = new Password();
            addModel.credentials.recovery_question = new Recovery_question();

            addModel.profile = JsonConvert.DeserializeObject<Profile>(JsonConvert.SerializeObject(param));

            addModel.profile.login = param.email;

            addModel.credentials.password.value = param.password;
            addModel.credentials.recovery_question.question = param.question;
            addModel.credentials.recovery_question.answer = param.answer;

            //"https://${yourOktaDomain}/api/v1/users?activate=true"
            // https://dev-77394467-admin.okta.com/

            //client
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://dev-77394467-admin.okta.com/");

            //"Content-Type: application/json"
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //client.DefaultRequestHeaders.Add("Authorization", "SSWS 00wHCb-coT8EwiZq5yAW6s4GZwXTdt5KPh17YhsDoE");
            client.DefaultRequestHeaders.Add("Authorization", "SSWS 00Thl4g7pcUhw1H605d6");

            // res model
            ResponseModel response = new ResponseModel();

            try
            {
                //HttpResponseMessage resUserRegistration = await client.PostAsync("api/v1/users?activate=true", new StringContent(JsonConvert.SerializeObject(addModel), Encoding.UTF8, "application/json"));
                HttpResponseMessage resUserRegistration = await client.PostAsync("api/v1/users?activate=true", new StringContent(JsonConvert.SerializeObject(addModel), Encoding.UTF8, "application/json"));
                if (resUserRegistration.IsSuccessStatusCode)
                {
                    //RACMSWebUiLogger.LogInfo("== == == \"api/UserRegistration/GetUserAttributeData\" Api is called.  With parameter " + login.userOID + " From GetUserAttributeData static methode of UIServices class ...");

                    var result = resUserRegistration.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<ResponseModel>(result);
                }
            }
            catch (Exception ex)
            {
                //RACMSWebUiLogger.LogError(ex);
            }

            //if (response != null || !string.IsNullOrEmpty(response.id))
            //{
            //    return Redirect("~/");
            //}

            return View(param);
        }



    }


}
