using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyCompany.Visitors.Client.iOS.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;

namespace MyCompany.Visitors.Client.Services.Authenitcator
{
    public enum AuthenticationStatus
    {
        Failed,
        Succeeded
    }
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string RefreshToken { get; set; }
        public AuthenticationStatus Status { get; set; }
    }
    class AzureAuthenticator : WebRedirectAuthenticator
    {
        private string resource;
        private string clientId;
        private string redirectUri;
        private string loginHint;
        string extraQueryParameters;
        private Uri authorizeUrl;

        public AzureAuthenticator(string url,string resource, string clientId, string redirectUri, string loginHint,
            string extraQueryParameters) : base (new Uri(url),new Uri(redirectUri))
{

    authorizeUrl = new Uri(url);
      this.resource = resource;
            this.clientId = clientId;
            this.redirectUri = redirectUri;
            this.loginHint = loginHint;
            this.extraQueryParameters = extraQueryParameters;
     }

        /// <summary>
        /// Method that returns the initial URL to be displayed in the web browser.
        /// </summary>
        /// <returns>
        /// A task that will return the initial URL.
        /// </returns>
        public override Task<Uri> GetInitialUrlAsync()
        {
            var url = new Uri(string.Format(
                "{0}/oauth2/authorize?client_id={1}&redirect_uri={2}&response_type=code&resource={3}&login_hint={4}",
                authorizeUrl.AbsoluteUri,
                Uri.EscapeDataString(clientId),
                Uri.EscapeDataString(redirectUri),
                Uri.EscapeDataString(resource),
                Uri.EscapeDataString(loginHint)));

            var tcs = new TaskCompletionSource<Uri>();
            tcs.SetResult(url);
            return tcs.Task;
        }

        public event Action<AuthenticationResult> LoggedIn;

        /// <summary>
        /// Raised when a new page has been loaded.
        /// </summary>
        /// <param name='url'>
        /// URL of the page.
        /// </param>
        /// <param name='query'>
        /// The parsed query of the URL.
        /// </param>
        /// <param name='fragment'>
        /// The parsed fragment of the URL.
        /// </param>
        protected override void OnPageEncountered(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            var all = new Dictionary<string, string>(query);
            foreach (var kv in fragment)
                all[kv.Key] = kv.Value;


     //
            // Continue processing
            //
            base.OnPageEncountered(url, query, fragment);
        }

        /// <summary>
        /// Raised when a new page has been loaded.
        /// </summary>
        /// <param name='url'>
        /// URL of the page.
        /// </param>
        /// <param name='query'>
        /// The parsed query string of the URL.
        /// </param>
        /// <param name='fragment'>
        /// The parsed fragment of the URL.
        /// </param>
        protected override async void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            //
            // Look for the access_token
            //
            if (query.ContainsKey("code"))
            {
                var dict = await RequestAccessTokenAsync(query["code"]);
                AppSettings.SecurityToken = dict["access_token"];
                OnRetrievedAccountProperties(dict);

                //var result = new AuthenticationResult
                //{
                //    AccessToken = dict["access_token"],
                //    Status = AuthenticationStatus.Succeeded,
                //    RefreshToken = dict["refresh_token"],
                //};
                //if (LoggedIn != null)
                //    LoggedIn(result);
            }

        }


        /// <summary>
        /// Event handler that is fired when an access token has been retreived.
        /// </summary>
        /// <param name='accountProperties'>
        /// The retrieved account properties
        /// </param>
        protected virtual void OnRetrievedAccountProperties(IDictionary<string, string> accountProperties)
        {
            ////
            //// Now we just need a username for the account
            ////
            //if (getUsernameAsync != null)
            //{
            //    getUsernameAsync(accountProperties).ContinueWith(task =>
            //    {
            //        if (task.IsFaulted)
            //        {
            //            OnError(task.Exception);
            //        }
            //        else
            //        {
            //            OnSucceeded(task.Result, accountProperties);
            //        }
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //}
            //else
            //{
                OnSucceeded("", accountProperties);
           // }
        }

        /// <summary>
        /// Asynchronously requests an access token with an authorization <paramref name="code"/>.
        /// </summary>
        /// <returns>
        /// A dictionary of data returned from the authorization request.
        /// </returns>
        /// <param name='code'>The authorization code.</param>
        /// <remarks>Implements: http://tools.ietf.org/html/rfc6749#section-4.1</remarks>
        Task<IDictionary<string, string>> RequestAccessTokenAsync(string code)
        {
            var queryValues = new Dictionary<string, string> {
				{ "grant_type", "authorization_code" },
				{ "code", code },
				{ "redirect_uri", redirectUri },
				{ "client_id", clientId },
			};

            return RequestAccessTokenAsync(queryValues);
        }

        /// <summary>
        /// Asynchronously makes a request to the access token URL with the given parameters.
        /// </summary>
        /// <param name="queryValues">The parameters to make the request with.</param>
        /// <returns>The data provided in the response to the access token request.</returns>
        protected async Task<IDictionary<string, string>> RequestAccessTokenAsync(IDictionary<string, string> queryValues)
        {
            var query = queryValues.FormEncode();

            var req = (HttpWebRequest)WebRequest.Create(string.Format("{0}/oauth2/token",authorizeUrl.AbsoluteUri));
            req.Method = "POST";
            var body = Encoding.UTF8.GetBytes(query);
            req.ContentLength = body.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            using (var s = req.GetRequestStream())
            {
                s.Write(body, 0, body.Length);
            }
            var response = new Response(await req.GetResponseAsync() as HttpWebResponse);
            var text = response.GetResponseText();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(text);

        }
    }
}
