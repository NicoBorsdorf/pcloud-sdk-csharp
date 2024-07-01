
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Responses;
using pcloud_sdk_csharp.Trash.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Trash.Controller
{
    public class TrashController
    {
        /// <summary>
        /// Creates new instance of controller for trash endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public TrashController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.ToString().EndsWith("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly HttpClient _client = new();
        private readonly string _token;
        private readonly Uri _baseUrl;

        /// <summary>
        /// Lists the contents of a folder in the Trash. 
        /// </summary>
        /// <param name="folderid">The id of the Trash folder. The default is 0 - the root of the Trash.</param>
        /// <param name="noFiles">If set, then no files will be included in the Trash list - only folders.</param>
        /// <param name="recursive">If set, then the list will be recursive - the subfolders will have their folders and files included.</param>
        /// <returns>On success returns the <see cref="SingleFileResponse.metadata">metadata</see> and the contents of the folder from the Trash.</returns>
        public async Task<SingleFileResponse?> List(long folderid = 0, bool? noFiles = null, bool? recursive = null)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"folderid", folderid.ToString() },
            };
            if (noFiles != null) query.Add("nofiles", noFiles.ToString()!);
            if (recursive != null) query.Add("recursive", recursive.ToString()!);

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "trash_list", query));

            return JsonConvert.DeserializeObject<SingleFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// For a desired file or folder from the Trash, calculates where to restore.
        /// <br/> Only provide fileId OR folderId.
        /// </summary>
        /// <param name="fileId">File id of the file that would be restored</param>
        /// <param name="folderId">Folder id of the folder that would be restored</param>
        /// <returns>On success returns  <see cref="SingleFileResponse.metadata">metadata</see> and <see cref="TrashReponse.destination">destination</see>.</returns>
        /// <exception cref="Exception">When both <paramref name="fileId"/> and <paramref name="folderId"/> ARE NOT provided or if both ARE provided.</exception>
        public async Task<TrashReponse?> RestorePath(long? fileId, long? folderId)
        {
            if ((fileId == null && folderId == null) || (fileId != null && folderId != null)) throw new Exception("Please provide only fileId OR folderId.");

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>();
            if (folderId != null) reqBody.Add("folderid", folderId.ToString()!);
            if (fileId != null) reqBody.Add("fileid", fileId.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "trash_restorepath", content);

            return JsonConvert.DeserializeObject<TrashReponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Restores files or folders from the Trash back to the filesystem. 
        /// </summary>
        /// <param name="fileId">File id of the restored file.</param>
        /// <param name="folderId">Folder id of the restored folder.</param>
        /// <param name="restoreTo">If given, then this folder will be chosen as a destination of the restored data.</param>
        /// <param name="metadata">If set and restoring a folder, then the metadata of the folder will have contents filled with the information about files and folders in the restired folder.</param>
        /// <exception cref="Exception">When both <paramref name="fileId"/> and <paramref name="folderId"/> ARE NOT provided or if both ARE provided.</exception>
        /// <exception cref="Exception"></exception>
        public async Task<SingleFileResponse?> Restore(long? fileId, long? folderId, long? restoreTo, bool? metadata)
        {
            if ((fileId == null && folderId == null) || (fileId != null && folderId != null)) throw new Exception("Please provide only fileId OR folderId.");

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>();
            if (folderId != null) reqBody.Add("folderid", folderId.ToString()!);
            if (fileId != null) reqBody.Add("fileid", fileId.ToString()!);
            if (restoreTo != null) reqBody.Add("restoreto", restoreTo.ToString()!);
            if (metadata != null) reqBody.Add("metadata", metadata.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "trash_restore", content);

            return JsonConvert.DeserializeObject<SingleFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Deletes <b>permanently</b> files or folders from the Trash. 
        /// </summary>
        /// <param name="fileId">File id of the file that is removed from Trash.</param>
        /// <param name="folderId">Folder id of the folder that is removed from Trash.</param>
        /// <returns><see cref="Response">Base Response</see></returns>
        /// <exception cref="Exception">When both <paramref name="fileId"/> and <paramref name="folderId"/> ARE NOT provided or if both ARE provided.</exception>
        public async Task<Response?> Clear(long? fileId, long? folderId)
        {
            if ((fileId == null && folderId == null) || (fileId != null && folderId != null)) throw new Exception("Please provide only fileId OR folderId.");

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>();
            if (folderId != null) reqBody.Add("nofiles", folderId.ToString()!);
            if (fileId != null) reqBody.Add("nofiles", fileId.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "trash_clear", content);

            return await response.Content.ReadFromJsonAsync<Response>();
        }
    }
}
