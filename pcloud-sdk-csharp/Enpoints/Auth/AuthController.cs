using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Auth.Requests;
using pcloud_sdk_csharp.Auth.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Auhtorize
{
    public class Auhtorize
    {
        private readonly string _oAuthUrl = "https://my.pcloud.com/oauth2/";
        private readonly HttpClient _client = new();

        public Uri GetOAuthUrl(AuthorizeRequest req)
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

        public async Task<AuthResponse?> GetOAuthToken(string client_id, string client_secret, string code, string? baseURL = @"https://eapi.pcloud.com/")
        {
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
