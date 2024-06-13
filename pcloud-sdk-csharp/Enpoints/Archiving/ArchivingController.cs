using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Archiving.Requests;
using pcloud_sdk_csharp.Archiving.Responses;
using pcloud_sdk_csharp.Base.Requests;
using pcloud_sdk_csharp.File.Responses;
using pcloud_sdk_csharp.Streaming.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Archiving.Controller
{
    public class ArchivingController
    {
        /// <summary>
        /// Creates new instance of controller for archiving endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public ArchivingController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.AbsoluteUri.Last().Equals("/")) throw new ArgumentException(@"Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly Uri _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        /// <summary>
        /// Gets a ZIP file stream from the pCloud API based on the specified <see cref="ZipRequest"/>.
        /// </summary>
        /// <param name="req">The <see cref="ZipRequest"/> object containing the request parameters.</param>
        /// <returns>When successful it returns a zip archive over the current API connection with all the files and directories in the requested tree.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// The method constructs the request headers and query parameters based on the properties of the <paramref name="req"/>.
        /// If <paramref name="req.ForceDownload"/> is specified, the content type is set to "application/octet-stream",
        /// otherwise it is set to "application/zip".
        /// <br/> 
        /// <br/> <see cref="ZipRequest">Request</see> parameters:
        /// <br/> - tree: <see cref="Tree"/>
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: Boolean flag to force download of the ZIP file.
        /// <br/> - filename: Optional filename for the ZIP file.
        /// <br/> - timeoffset: Optional time offset parameter.
        /// <br/> 
        /// </remarks>
        public async Task<Stream> GetZip(ZipRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            // receiving content type changes based on forced download
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue(req.ForceDownload != null ? "application/octet-stream" : "application/zip"));

            var query = new Dictionary<string, string>();
            var tree = req.Tree;
            if (tree.FolderIds.Count > 0) query.Add("folderids", string.Join(",", tree.FolderIds));
            if (tree.FileIds.Count > 0) query.Add("fileids", string.Join(",", tree.FileIds));
            if (tree.ExcludeFolderIds.Count > 0) query.Add("excludefolderids", string.Join(",", tree.ExcludeFolderIds));
            if (tree.ExcludeFileIds.Count > 0) query.Add("excludefileids", string.Join(",", tree.ExcludeFileIds));
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.Filename != null) query.Add("filename", req.Filename);
            if (req.TimeOffset != null) query.Add("timeoffset", req.TimeOffset);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getzip", query)));

            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Gets a ZIP file link from the pCloud API based on the specified <see cref="ZipLinkRequest"/>.
        /// </summary>
        /// <param name="req">The <see cref="ZipLinkRequest"/> object containing the request parameters.</param>
        /// <returns>On success it will return array hosts with servers that have the file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> 
        /// <br/> <see cref="ZipLinkRequest">Request</see> parameters:
        /// <br/> - tree: <see cref="Tree"/>
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - maxspeed: Limits the download speed (in bytes per second) for this link.
        /// <br/> - forcedownload: Boolean flag to force download of the ZIP file.
        /// <br/> - filename: Optional filename for the ZIP file.
        /// <br/> - timeoffset: Optional time offset parameter.
        /// <br/> 
        /// </remarks>
        public async Task<StreamingResponse?> GetZiplink(ZipLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>();
            var tree = req.Tree;
            query.Add("folderids", string.Join(",", tree.FolderIds));
            query.Add("fileids", string.Join(",", tree.FileIds));
            if (tree.ExcludeFolderIds.Count > 0) query.Add("excludefolderids", string.Join(",", tree.ExcludeFolderIds));
            if (tree.ExcludeFileIds.Count > 0) query.Add("excludefileids", string.Join(",", tree.ExcludeFileIds));

            if (req.MaxSpeed != null) query.Add("maxspeed", req.MaxSpeed.ToString()!);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.Filename != null) query.Add("filename", req.Filename);
            if (req.TimeOffset != null) query.Add("timeoffset", req.TimeOffset);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getziplink", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }

        /// <summary>
        /// Saves a ZIP file on the pCloud server based on the specified <see cref="SaveZipRequest"/>.
        /// </summary>
        /// <param name="req">The <see cref="SaveZipRequest"/> object containing the request parameters.</param>
        /// <returns><see cref="Task"/><![CDATA[<]]><see cref="SingleFileResponse"/><![CDATA[>]]> | null</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> 
        /// <br/> <see cref="ZipLinkRequest">Request</see> parameters:
        /// <br/> - tree: <see cref="Tree"/>
        /// <br/>
        /// <br/> Optional: 
        /// <br /> - progresshash: Key to retrieve the progress for the zipping process.
        /// <br /> - timeoffset: Desired time offset.
        /// <br /> - tofolderid: Folder id of the folder, where to save the ZIP archive.
        /// <br /> - toname: Filename of the desired ZIP archive.
        /// </remarks>
        public async Task<SingleFileResponse?> SaveZip(SaveZipRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var tree = req.Tree;
            var reqBody = new Dictionary<string, string>
            {
                { "folderids", string.Join(",", tree.FolderIds) },
                { "fileids", string.Join(",", tree.FileIds) }
            };
            if (tree.ExcludeFolderIds.Count > 0) reqBody.Add("excludefolderids", string.Join(",", tree.ExcludeFolderIds));
            if (tree.ExcludeFileIds.Count > 0) reqBody.Add("excludefileids", string.Join(",", tree.ExcludeFileIds));

            if (req.ProgressHash != null) reqBody.Add("progresshash", req.ProgressHash);
            if (req.TimeOffset != null) reqBody.Add("timeoffset", req.TimeOffset);
            if (req.ToFolderId != null) reqBody.Add("tofolderid", req.ToFolderId.ToString()!);
            if (req.ToName != null) reqBody.Add("toname", req.ToName);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "savezip", content);

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }

        /// <summary>
        /// Gets an archive on the pCloud server based on the specified <see cref="ExtractArchiveRequest"/>.
        /// </summary>
        /// <param name="req">The <see cref="ExtractArchiveRequest"/> object containing the request parameters.</param>
        /// <returns>Returns <see cref="ExtractArchiveResponse">metadata</see> from requested archive file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> 
        /// <br/> <see cref="ExtractArchiveRequest">Request</see> parameters:
        /// <br/> - fileid: Id of archive file. 
        /// <br/> - tofolderid : Id of destination folder. 
        /// <br/>
        /// <br/> Optional: 
        /// <br /> - nooutput: If set extraction output is not returned.
        /// <br /> - overwrite: Specifies what to do if file to extract already exists in the folder, can be one of 'rename' (default), 'overwrite' and 'skip'.
        /// <br /> - password: Password to use to extract a password protected archive.
        /// </remarks>
        public async Task<ExtractArchiveResponse?> ExtractArchive(ExtractArchiveRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                { "fileid ", req.FileId.ToString() },
                { "tofolderid", req.ToFolderId.ToString() }
            };
            if (req.NoOutput != null) reqBody.Add("nooutput", req.NoOutput.ToString()!);
            if (req.Overwrite != null) reqBody.Add("overwrite", req.Overwrite.ToString()!.ToLower());
            if (req.Password != null) reqBody.Add("password", req.Password);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "extractarchive", content);

            return await response.Content.ReadFromJsonAsync<ExtractArchiveResponse?>();
        }

        /// <summary>
        /// Gets the progress of an archive extraction operation from the pCloud server based on the specified progress hash.
        /// </summary>
        /// <param name="progressHash">The progress hash for tracking the extraction operation.</param>
        /// <param name="maxLines">Optional maximum number of lines to retrieve from the progress log.</param>
        /// <returns>Returns <see cref="ExtractArchiveResponse">metadata</see> with current exctraction progress.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="progressHash"/> is null.</exception>
        public async Task<ExtractArchiveResponse?> ExtractArchiveProgress(string progressHash, int? maxLines)
        {
            if (progressHash == null) throw new ArgumentNullException(nameof(progressHash));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>()
            {
                {"progresshash", progressHash }
            };
            if (maxLines != null) query.Add("lines", maxLines.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "extractarchiveprogress", query)));

            return await response.Content.ReadFromJsonAsync<ExtractArchiveResponse?>();
        }

        /// <summary>
        /// Asynchronously retrieves the progress of a ZIP file saving operation from the pCloud server based on the specified progress hash.
        /// </summary>
        /// <param name="progressHash">The progress hash for tracking the save operation.</param>
        /// <returns>If there exists such zipping process, then the method returns <see cref="SaveZipProgoressResponse">metadata</see> of current saving progress.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="progressHash"/> is null.</exception>
        public async Task<SaveZipProgoressResponse?> SaveZipProgress(string progressHash)
        {
            if (progressHash == null) throw new ArgumentNullException(nameof(progressHash));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>()
            {
                {"progresshash", progressHash }
            };

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "extractarchiveprogress", query)));

            return await response.Content.ReadFromJsonAsync<SaveZipProgoressResponse?>();
        }
    }
}
