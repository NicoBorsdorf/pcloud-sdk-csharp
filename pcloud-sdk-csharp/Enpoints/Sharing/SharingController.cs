using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.Sharing.Requests;
using pcloud_sdk_csharp.Sharing.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Sharing.Controller
{
    public class SharingController
    {

        /// <summary>
        /// Creates new instance of controller for sharing endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public SharingController(string access_token, string clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));
        }

        private readonly string _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        public async Task<Response?> ShareFolder(ShareFolderRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                { "folderid", req.FolderId.ToString() },
                { "mail", req.Email },
                { "permissions", req.AllowedPermissions.Select((v) => (int)v).Sum().ToString() }
            };
            if (req.Name != null) reqBody.Add("name", req.Name);
            if (req.Message != null) reqBody.Add("message", req.Message);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "sharefolder", content);

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        public async Task<ListSharesResponse?> ListShares(ListSharesReqeust req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                { "norequests", req.NoRequests.ToString()! },
                { "noshares", req.NoShares.ToString()! },
                { "noincoming", req.NoIncoming.ToString()! },
                { "noincoming", req.NoOutgoing.ToString()! }
            };

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "listshares", content);

            return await response.Content.ReadFromJsonAsync<ListSharesResponse?>();
        }

        public async Task<ShareRequestInfoResponse?> ShareRequestInfo(string code)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "code", code}
            };

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "sharerequestinfo", query)));

            return await response.Content.ReadFromJsonAsync<ShareRequestInfoResponse?>();
        }

        public async Task<Response?> CancelShareRequest(long sharerequestid)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "sharerequestinfo", $"{{sharerequestid: {sharerequestid}}}");

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        public async Task<Response?> AcceptShare(AcceptShareRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"sharerequestid", req.ShareRequestId.ToString()}
            };
            if (req.Name != null) reqBody.Add("name", req.Name);
            if (req.FolderId != null) reqBody.Add("folderid", req.FolderId.ToString()!);
            if (req.Always != null) reqBody.Add("always", req.Always.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "sharerequestinfo", content);

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        public async Task<Response?> DeclineShare(long sharerequestId, int? block)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"sharerequestid", sharerequestId.ToString()}
            };
            if (block != null) reqBody.Add("name", block.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "declineshare", content);

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        public async Task<Response?> RemoveShare(long shareId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"shareid", shareId.ToString()}
            };

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "removeshare", content);

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        public async Task<Response?> ChangeShare(long shareId, List<Permission> permissions)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"shareid", shareId.ToString()},
                {"permissions", permissions.Select((v) => (int)v).Sum().ToString() }
            };

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "changeshare", content);

            return await response.Content.ReadFromJsonAsync<Response?>();
        }
    }
}
