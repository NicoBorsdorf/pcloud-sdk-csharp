using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Requests;
using pcloud_sdk_csharp.File.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pcloud_sdk_csharp.File.Controller
{
    public class FileController
    {
        /// <summary>
        /// Creates new instance of controller for file endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public FileController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.ToString().EndsWith("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly Uri _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        /// <summary>
        /// Upload a file to your PCloud storage.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="req"/> is null.
        /// </exception>
        /// <param name="req"><see cref="UploadFileRequest"/></param>
        /// <returns>Returns two arrays - <see cref="UploadedFileResponse.fileids">fileids</see> and <see cref="UploadedFileResponse.metadata">metadata</see>.</returns>
        /// <remarks>       
        /// <see cref="UploadFileRequest">Request</see> parameters:  
        /// <br /> - folderid: Id of folder to upload to.  
        /// <br /> - filename: Name of the file.
        /// <br />
        /// Optional: 
        /// <br /> - nopartial: If is set, partially uploaded files will not be saved
        /// <br /> - renameifexists: If set, the uploaded file will be renamed, if file with the requested name exists in the folder.  
        /// <br /> - progresshash: Hash used for observing upload progress.  
        /// <br /> - mtime: If set, file modified time is set. Have to be unix time seconds.  
        /// <br /> - ctime: If set, file created time is set. It's required to provide mtime to set ctime. Have to be unix time seconds. 
        /// </remarks>
        public async Task<UploadedFileResponse?> UploadFile(UploadFileRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(req.FolderId.ToString()), "folderid");
            formData.Add(new StringContent(req.FileName), "filename");
            if (req.NoPartial != null) formData.Add(new StringContent(req.NoPartial.ToString()!), "nopartial");
            if (req.RenameIfExists != null) formData.Add(new StringContent(req.RenameIfExists.ToString()!), "renameifexists");
            if (req.ProgressHash != null) formData.Add(new StringContent(req.ProgressHash), "´progresshash");

            var fileContent = new StreamContent(req.UploadFile);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"files\"",
                FileName = $"\"{req.FileName}\"",
            };
            formData.Add(fileContent);

            var response = await _client.PostAsync(_baseUrl + "uploadfile", formData);

            return JsonConvert.DeserializeObject<UploadedFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Check the progress of the uploading file with given hash.
        /// </summary>
        /// <param name="hash">Hash of currently uploading file. Same hash provided to <see cref="UploadFile(UploadFileRequest)"/> function for inital upload trigger.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="hash"/> is null.
        /// </exception>
        /// <returns>Returns <see cref="UploadProgressResponse">metadata</see> of upload progress found with given hash parameter.</returns>
        public async Task<UploadProgressResponse?> UploadProgress(string hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"hash", hash }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "uplaodprogress", query));

            return JsonConvert.DeserializeObject<UploadProgressResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Downloads the files that can be accessed via the provided URLs.
        /// </summary>
        /// <param name="urls">URL list of the files that should be downloaded.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="url"/> is null.
        /// </exception>
        /// <returns>The method returns when all files are downloaded (which might take time). On success <see cref="UploadedFileResponse.metadata">metadata</see> array with metadata of all downloaded files is returned.</returns>

        public async Task<UploadedFileResponse?> DownloadFile(List<string> urls)
        {
            if (urls == null) throw new ArgumentNullException(nameof(urls));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"url", string.Join(" ", urls) }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "downloadfile", query));

            return JsonConvert.DeserializeObject<UploadedFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Downloads the files asynchronously that can be accessed via the provided URLs.
        /// </summary>
        /// <param name="urls">URL list of the files to be downloaded.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="urls"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// When <paramref name="urls"/> contains not URLs.
        /// </exception>
        /// <returns>The method returns when all files are downloaded (which might take time). On success <see cref="UploadedFileResponse.metadata">metadata</see> array with metadata of all downloaded files is returned.</returns>
        public async Task<UploadedFileResponse?> DownloadFileAsync(List<string> urls)
        {
            if (urls == null) throw new ArgumentNullException(nameof(urls));
            if (urls.Count == 0) throw new ArgumentOutOfRangeException(nameof(urls));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"url", string.Join(" ", urls) }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "downloadfileasync", query));

            return JsonConvert.DeserializeObject<UploadedFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Copy a file to a different folder.
        /// </summary>
        /// <param name="fileId">Id of file that should be copied.</param>
        /// <param name="toFolderId">Id of the folder the file should be copied to.</param>
        /// <returns>Upon success returns <see cref="SingleFileResponse.metadata">metadata</see> of the destination file ( the copy result ).</returns>
        public async Task<SingleFileResponse?> CopyFile(long fileId, long toFolderId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"fileid", fileId.ToString() },
                {"tofolderid", toFolderId.ToString() }
            };

            var content = new FormUrlEncodedContent(query);
            var response = await _client.PostAsync(_baseUrl + "downloadfileasync", content);


            return JsonConvert.DeserializeObject<SingleFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Gets the checksum of the file with given id.
        /// </summary>
        /// <param name="fileId">Id of file the checksum should be retrived from.</param>
        /// <returns>Upon success returns <see cref="UploadedFileResponse.metadata">metadata</see>. sha1 checksum is returned from both US and Europe API servers. md5 is returned only from US API servers, not added in Europe as it's quite old and has collions. sha256 is returned in Europe only.</returns>
        public async Task<UploadedFileResponse?> ChecksumFile(long fileId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var query = new Dictionary<string, string>
            {
                {"fileId", fileId.ToString() }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "checksumfile", query));

            return JsonConvert.DeserializeObject<UploadedFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Deletes a file with given id.
        /// </summary>
        /// <param name="fileId">Id of file that should be deleted.</param>
        /// <returns>On success returns file's <see cref="DeleteFileResponse.metadata">metadata</see> with isdeleted set.</returns>
        public async Task<DeleteFileResponse?> DeleteFile(long fileId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"fileId", fileId.ToString() }
            };

            var response = await _client.DeleteAsync(QueryHelpers.AddQueryString(_baseUrl + "checksumfile", query));

            return JsonConvert.DeserializeObject<DeleteFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Renames a file with given name.
        /// </summary>
        /// <param name="fileId">Id of file that should be renamed.</param>
        /// <param name="toName">New name of file.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="toName"/> is null.
        /// </exception>
        /// <returns>On success returns renamed file's <see cref="SingleFileResponse.metadata">metadata</see> with deletedfileid if merged file.</returns>
        public async Task<SingleFileResponse?> RenameFile(long fileId, string toName)
        {
            if (string.IsNullOrEmpty(toName)) throw new ArgumentNullException(nameof(toName));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"fileid", fileId.ToString() },
                {"toName", toName }
            };

            var content = new FormUrlEncodedContent(query);
            var response = await _client.PostAsync(_baseUrl + "renamefile", content);

            return JsonConvert.DeserializeObject<SingleFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Gets information about the file with given id.
        /// </summary>
        /// <param name="fileId">Id of file for wanted information.</param>
        /// <returns>Returns <see cref="SingleFileResponse.metadata">metadata</see>.</returns>
        public async Task<SingleFileResponse?> Stat(long fileId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"fileId", fileId.ToString() }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "stat", query));

            return JsonConvert.DeserializeObject<SingleFileResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }
    }
}
