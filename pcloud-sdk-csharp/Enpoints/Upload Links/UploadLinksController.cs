
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.UploadLinks.Requests;
using pcloud_sdk_csharp.UploadLinks.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.UploadLinks.Controller
{
    public class UploadLinksController
    {
        /// <summary>
        /// Creates new instance of controller for upload links endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public UploadLinksController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.ToString().EndsWith("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly HttpClient _client = new();
        private readonly string _token;
        private readonly Uri _baseUrl;

        /// <summary>
        /// Creates upload link. 
        /// </summary>
        /// <param name="req">Request definition of /createuploadlink endpoint.</param>
        /// <returns>On success returns <see cref="UploadLinksResponse">metadata</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="UploadLinksRequests">Request</see> parameters:
        /// <br/> - folderid: Folder id of the folder, where the uploaded files will be saved.
        /// <br/> - comment: Comment the user is willing to provide to uploading users.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - expire: Date/time at which the link will stop working.
        /// <br/> - maxspace: Limit maximum total size (in bytes).
        /// <br/> - maxfiles: Total number of files that can be uploaded.
        /// <br/> 
        /// </remarks>
        public async Task<UploadLinksResponse?> CreateUploadLink(UploadLinksRequests req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"folderid", req.FolderId.ToString() },
                {"comment", req.Comment }
            };
            if (req.Expire != null) reqBody.Add("expire", req.Expire.Value.ToString("R"));
            if (req.MaxSpace != null) reqBody.Add("maxspace", req.MaxSpace.ToString()!);
            if (req.MaxFiles != null) reqBody.Add("maxfiles", req.MaxFiles.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "createuploadlink", content);

            return JsonConvert.DeserializeObject<UploadLinksResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Lists all upload links in uploadlinks.
        /// </summary>
        /// <returns>Returns <see cref="ListUploadLinksResponse.uploadlinks">uploadlinks</see> list of metadata.</returns>
        public async Task<ListUploadLinksResponse?> ListUploadLinks()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "createuploadlink");

            return JsonConvert.DeserializeObject<ListUploadLinksResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Deletes upload link identified by uploadlinkid.
        /// </summary>
        /// <param name="uploadLinkId">Id of the deleted upload link.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        public async Task<Response?> DeleteUploadLink(long uploadLinkId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "deleteuploadlink", $"{{\"uploadlinkid\" : {uploadLinkId}}}");

            return await response.Content.ReadFromJsonAsync<Response>();
        }

        /// <summary>
        /// Modify upload link identified by uploadlinkid. 
        /// </summary>
        /// <param name="req">Request definition of /changeuploadlink endpoint.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ChangeUploadLinkRequest">Request</see> parameters:
        /// <br/> - uploadlinkid: Id of the upload link.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - expire: Set expiration date of the link.
        /// <br/> - deleteexpire: If set, link's expiration date is removed.
        /// <br/> - maxspace: Alter the maximum available space (in bytes) of the link.
        /// <br/> - maxfiles: Alter the maximum available files of the link.
        /// <br/> 
        /// </remarks>
        public async Task<Response?> ChangeUploadLink(ChangeUploadLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"uploadlinkid", req.UploadLinkId.ToString() }
            };
            if (req.Expire != null) reqBody.Add("expire", req.Expire.Value.ToString("R"));
            if (req.DeleteExpire != null) reqBody.Add("deleteexpire", req.DeleteExpire.ToString()!);
            if (req.MaxSpace != null) reqBody.Add("maxspace", req.MaxSpace.ToString()!);
            if (req.MaxFiles != null) reqBody.Add("maxfiles", req.MaxFiles.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "changeuploadlink", content);

            return await response.Content.ReadFromJsonAsync<Response>();
        }

        /// <summary>
        /// Official Docu seems of, but this is what it says.
        /// Expects upload link code and returns back the link's comment and mail. 
        /// </summary>
        /// <param name="req">Request definition of /showuploadlink endpoint.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ChangeUploadLinkRequest">Request</see> parameters:
        /// <br/> - uploadlinkid: Id of the upload link.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - expire: Set expiration date of the link.
        /// <br/> - deleteexpire: If set, link's expiration date is removed.
        /// <br/> - maxspace: Alter the maximum available space (in bytes) of the link.
        /// <br/> - maxfiles: Alter the maximum available files of the link.
        /// <br/> 
        /// </remarks>
        public async Task<Response?> ShowUploadLink(ChangeUploadLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"uploadlinkid", req.UploadLinkId.ToString() }
            };
            if (req.Expire != null) query.Add("expire", req.Expire.Value.ToString("R"));
            if (req.DeleteExpire != null) query.Add("deleteexpire", req.DeleteExpire.ToString()!);
            if (req.MaxSpace != null) query.Add("maxspace", req.MaxSpace.ToString()!);
            if (req.MaxFiles != null) query.Add("maxfiles", req.MaxFiles.ToString()!);

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "showuploadlink", query));

            return await response.Content.ReadFromJsonAsync<Response>();
        }

        /// <summary>
        /// Upload file(s) to a upload link. Expects code.  
        /// </summary>
        /// <param name="req">Request definition of /uploadtolink endpoint.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ChangeUploadLinkRequest">Request</see> parameters:
        /// <br/> - code: Code of the link.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - nopartial: If is set, partially uploaded files will not be saved.
        /// <br/> - progresshash: Hash used for observing upload progress.
        /// <br/> 
        /// </remarks>
        public async Task<Response?> UploadToLink(UploadToLinkRequests req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"code", req.Code }
            };
            if (req.NoPartial != null) reqBody.Add("nopartial", req.NoPartial.ToString()!);
            if (req.ProgressHash != null) reqBody.Add("progresshash", req.ProgressHash);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "uploadtolink", content);

            return await response.Content.ReadFromJsonAsync<Response>();
        }

        /// <summary>
        /// Monitor the progress of uploaded files.
        /// </summary>
        /// <param name="code">Code of the upload link.</param>
        /// <param name="progressHash">Hash for monitoring passed to <see cref="UploadToLink(UploadToLinkRequests)">uploadtolink</see>.</param>
        /// <returns>Returns same data as <see cref="File.Controller.FileController.UploadProgress(string)">uploadprogress</see> but without files.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="code"/> od <paramref name="progressHash"/> is null.</exception>
        public async Task<Response?> UploadLinkProgress(string code, string progressHash)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (progressHash == null) throw new ArgumentNullException(nameof(progressHash));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"code", code },
                {"progresshash", progressHash }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "uploadtolink", query));

            return await response.Content.ReadFromJsonAsync<Response>();
        }

        /// <summary>
        /// Copy a file from the current user's filesystem to a upload link.
        /// </summary>
        /// <param name="code">Code of the upload link.</param>
        /// <param name="fileid">Id of the copied file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Response?> CopyToLink(string code, long fileid, string? toName)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"code", code },
                {"fileid", fileid.ToString() }
            };
            if (toName != null) reqBody.Add("toname", toName);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "copytolink", content);

            return await response.Content.ReadFromJsonAsync<Response>();
        }
    }
}
