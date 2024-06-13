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
        public SharingController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.AbsoluteUri.Last().Equals("/")) throw new ArgumentException(@"Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly Uri _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        /// <summary>
        /// Shares a folder with another user.
        /// </summary>
        /// <param name="req">Request definition for /sharefolder endpoint.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ShareFolderRequest">Request</see> parameters:
        /// <br/> - folderid: Folder id of the shared folder.
        /// <br/> - mail: Mail of the user with whom you are sharing the folder.
        /// <br/> - permissions: Bitwise combination of permission flags.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - name: Name of the share. Default - the folder name.
        /// <br/> - message: Message to pass to the receiving user.
        /// <br/> 
        /// </remarks>
        public async Task<Response?> ShareFolder(ShareFolderRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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

        /// <summary>
        /// List current shares and share requests.
        /// </summary>
        /// <param name="req">Request definition for /listshares endpoint.</param>
        /// <returns>Returns two objects <see cref="ListSharesResponse.shares">shares</see> and <see cref="ListSharesResponse.requests">requests</see> both with sub-objects incoming and outgoing.</returns>
        /// <remarks>
        /// <br/> <see cref="ListSharesReqeust">Request</see> parameters (all optional):
        /// <br/> - norequests: If set, share requests will not be returned.
        /// <br/> - noshares: If set, established shares will not be returned.
        /// <br/> - noincoming: If set, hide incoming sub-objects in the result.
        /// <br/> - nooutgoing: If set, hide outgoing sub-objects in the result.
        /// <br/> 
        /// </remarks>
        public async Task<ListSharesResponse?> ListShares(ListSharesReqeust? req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            if (req != null)
            {
                var query = new Dictionary<string, string>();
                if (req?.NoRequests != null) query.Add("norequests", req.NoRequests.ToString()!);
                if (req?.NoShares != null) query.Add("noshares", req.NoShares.ToString()!);
                if (req?.NoIncoming != null) query.Add("noincoming", req.NoIncoming.ToString()!);
                if (req?.NoOutgoing != null) query.Add("noincoming", req.NoOutgoing.ToString()!);

                response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "listshares", query));
            }
            else
            {
                response = await _client.GetAsync(_baseUrl + "listshares");
            }

            return await response.Content.ReadFromJsonAsync<ListSharesResponse?>();
        }

        /// <summary>
        /// Get information about a share request from the <paramref name="code">code</paramref> that was sent to the user's email.
        /// </summary>
        /// <param name="code">The code that was sent to the user's email.</param>
        /// <returns>Return information about a share.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="code"/> is null.</exception>
        public async Task<ShareRequestInfoResponse?> ShareRequestInfo(string code)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

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

        /// <summary>
        /// Cancels a share request sent by the current user.
        /// </summary>
        /// <param name="sharerequestid">Identificator of the request.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        public async Task<Response?> CancelShareRequest(long sharerequestid)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "sharerequestinfo", $"{{sharerequestid: {sharerequestid}}}");

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        /// <summary>
        /// Accept a share request. 
        /// </summary>
        /// <param name="req">Request definition for /acceptshare endpoint.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="AcceptShareRequest">Request</see> parameters:
        /// <br/> - sharerequestid: The id of the share request.
        /// <br/> - code: The code that was sent to the user's email.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - name: Specify the folder name. Otherwise, use the share name.
        /// <br/> - folderid: The id of the folder where to mount the share.
        /// <br/> - always: If set, the accepting user from now on will auto-accept requests from the sharing user to the default share folder.
        /// <br/> 
        /// </remarks>
        public async Task<Response?> AcceptShare(AcceptShareRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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

        /// <summary>
        /// Decline a share request. 
        /// </summary>
        /// <param name="sharerequestId">The id of the share request.</param>
        /// <param name="block">(Optional) If set, all future share requests from the offering user will be automatically declined.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
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

        /// <summary>
        /// Remove an active share.
        /// </summary>
        /// <param name="shareId">The id of the share request, returned by listshares.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
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

        /// <summary>
        /// Change permissions of a share. 
        /// </summary>
        /// <param name="shareId">The id of the share request, returned by listshares.</param>
        /// <param name="permissions">The new permissions.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="permissions"/> is null.</exception>
        public async Task<Response?> ChangeShare(long shareId, List<Permission> permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

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
