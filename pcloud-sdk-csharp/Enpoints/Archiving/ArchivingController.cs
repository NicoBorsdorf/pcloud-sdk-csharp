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
        public ArchivingController(string access_token, string clientUrl)
        {
            _token = access_token;
            _baseUrl = clientUrl;
        }

        private readonly string _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        public async Task<Stream> GetZip(ZipRequest req)
        {
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

        public async Task<StreamingResponse?> GetZiplink(ZipLinkRequest req)
        {
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

        public async Task<SingleFileResponse?> SaveZip(SaveZipRequest req)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>();
            var tree = req.Tree;

            reqBody.Add("folderids", string.Join(",", tree.FolderIds));
            reqBody.Add("fileids", string.Join(",", tree.FileIds));
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

        public async Task<ExtractArchiveResponse?> ExtractArchive(ExtractArchiveRequest req)
        {
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

        public async Task<ExtractArchiveResponse?> ExtractArchiveProgress(string progressHash, int? maxLines)
        {
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

        public async Task<SaveZipProgoressResponse?> SaveZipProgress(string progressHash)
        {
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
