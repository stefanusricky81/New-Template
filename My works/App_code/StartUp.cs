using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect; // Required for ResponseType and Scope
using Microsoft.IdentityModel.Tokens; // Required for TokenValidationParameters
using System.Threading.Tasks;
using System.Configuration; // To read settings from Web.config
using System.Security.Claims; // To work with claims
using Microsoft.Owin.Security.Notifications; // For Notifications
using System; // For logging exceptions, Uri
using Microsoft.IdentityModel.Logging;
using System.Security.Cryptography;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Logging;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Globalization;
using System.Net;

[assembly: OwinStartup(typeof(Desktop.Startup))]
/// <summary>
/// Summary description for StartUp
/// </summary>
/// 
namespace Desktop
{
    public class Startup
    {

        private static string clientId = ConfigurationManager.AppSettings["oidc:ClientId"];//
        private static string aadInstance = ConfigurationManager.AppSettings["oidc:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["oidc:Tenant"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["oidc:PostLogoutRedirectUri"];
        private static string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        private ILogger _logger;
        public void Configuration(IAppBuilder app)
        {
            ConfigureNetworkSettings();

            // Configure authentication
            ConfigureAuth(app);
        }

        private void ConfigureNetworkSettings()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;

            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;
        }

        /// <summary>
        /// Configures the OWIN application pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(
                CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                LoginPath = new PathString("/Public/Login.aspx")
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["oidc:ClientId"],
                Authority = ConfigurationManager.AppSettings["oidc:AADInstance"] +
                           ConfigurationManager.AppSettings["oidc:TenantId"],
                RedirectUri = ConfigurationManager.AppSettings["oidc:RedirectUri"],
                PostLogoutRedirectUri = ConfigurationManager.AppSettings["oidc:PostLogoutRedirectUri"],

                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = ConfigurationManager.AppSettings["ida:AADInstance"] +
                                 ConfigurationManager.AppSettings["ida:TenantId"]
                },

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();

                        //string errorMessage = "Authentication error: {" + context.Exception.Message + "}";
                        //context.Response.Redirect("/Public/Error.aspx?message=" + Uri.EscapeDataString(errorMessage));

                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = async context =>
                    {
                        context.ProtocolMessage.State = "123456";
                        context.ProtocolMessage.Nonce = Guid.NewGuid().ToString();

                        if (context.ProtocolMessage.Error == "server_error")
                        {
                            await Task.Delay(1000);
                            context.Response.Redirect(context.ProtocolMessage.CreateAuthenticationRequestUrl());
                        }
                    },
                    MessageReceived = context =>
                    {
                        return Task.FromResult(0);
                    },
                    SecurityTokenReceived = context =>
                    {
                        context.HandleResponse();
                        //context.Response.Redirect("/Public/Error.aspx?message=" + "here2");

                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = context =>
                    {
                        context.HandleResponse();
                        //context.Response.Redirect("/Public/Error.aspx?message=" + "here");

                        return Task.FromResult(0);
                    }
                },

            });
            
        }
        private Task OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.ProtocolMessage.Prompt = "login";

            notification.ProtocolMessage.State = "123456";
            notification.ProtocolMessage.Nonce = Guid.NewGuid().ToString();

            return Task.FromResult(0);
        }

        private async Task OnMessageReceived(MessageReceivedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            var protocolMessage = notification.ProtocolMessage;
            string clientId = protocolMessage.ClientId;
            string redirectUri = protocolMessage.RedirectUri;
            string scope = protocolMessage.Scope;


            var input = protocolMessage.IdToken + " " + notification.ProtocolMessage.IssuerAddress;

            await Task.Yield();
        }

        private Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            ClaimsIdentity identity = notification.AuthenticationTicket.Identity;

            identity.AddClaim(new Claim("tester", "test"));
            string name = "Unknown";

            if (identity.FindFirst(identity.NameClaimType) != null)
            {
                name = identity.FindFirst(identity.NameClaimType).Value;
            }
            Claim tenantIdClaim = identity.FindFirst("tid"); // Example for Azure AD tenant ID
            if (tenantIdClaim != null)
            {
                identity.AddClaim(new Claim("custom_tenant_id", tenantIdClaim.Value));
            }

            if (notification.ProtocolMessage.IdToken != null)
            {
                identity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
            }

            identity.AddClaim(new Claim("tester2", "test2"));

            return Task.FromResult(0);
        }

        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();

            string errorMessage = "Authentication error: {" + notification.Exception.Message + "}";
            notification.Response.Redirect("/Public/Error.aspx?message=" + Uri.EscapeDataString(errorMessage));

            return Task.FromResult(0);
        }

    }
}