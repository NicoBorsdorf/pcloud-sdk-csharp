using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Auth.Requests;
using pcloud_sdk_csharp.Auth.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Auth.Controller
{
    public static class Authorize
    {
        private static readonly string _oAuthUrl = "https://my.pcloud.com/oauth2/";
        private static readonly HttpClient _client = new();

        /// <summary>
        /// Generates URL for OAuth.
        /// </summary>
        /// <param name="req">The <see cref="AuthorizeRequest"/> object containing the request parameters.</param>
        /// <returns><see cref="Uri"/> of the OAuth endpoint.</returns>
        public static Uri GetOAuthUrl(AuthorizeRequest req)
        {
            var query = new Dictionary<string, string>
            {
                { "client_id", req.ClientId },
                { "response_type", req.Type.ToString() },
                { "force_reapprove", req.ForceApprove.ToString() }
            };
            if (req.RedirectUri != null) query.Add("redirect_uri", req.RedirectUri.ToString());
            if (req.State != null) query.Add("state", req.State.ToString());

            return new Uri(QueryHelpers.AddQueryString(_oAuthUrl + "authorize", query));
        }

        /// <summary>
        /// Gets OAuth token for API connection.
        /// </summary>
        /// <param name="client_id">Clinet Id of your PCloud application.</param>
        /// <param name="client_secret">Clinet Secret of your PCloud application.</param>
        /// <param name="code">Code returned after teh redirect from the <see cref="GetOAuthUrl(AuthorizeRequest)"/> URL</param>
        /// <param name="baseURL">API URL depending on region, either api.plcoud.com or default eapi.plcoud.com in case if no URL is provided.</param>
        /// <returns><see cref="AuthResponse"/></returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="client_id"/>, <paramref name="client_secret"/> or <paramref name="code"/> is null.
        /// </exception>
        public static async Task<AuthResponse?> GetOAuthToken(string client_id, string client_secret, string code, string baseURL = @"https://eapi.pcloud.com/")
        {
            if (client_id == null) throw new ArgumentNullException(nameof(client_id));
            if (client_secret == null) throw new ArgumentNullException(nameof(client_secret));
            if (code == null) throw new ArgumentNullException(nameof(code));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "client_id", client_id },
                { "client_secret", client_secret },
                { "code", code },
            };

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(baseURL + "oauth2_token", query)));

            return await response.Content.ReadFromJsonAsync<AuthResponse?>();
        }
    }

}
