using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.File.Requests;
using pcloud_sdk_csharp.File.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.File.Controller
{
    public class FileController
    {
        public FileController(string access_token, string clientURL)
        {
            _token = access_token;
            _baseUrl = clientURL;
        }

        private readonly string _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        public async Task<UploadedFile?> UploadFile(UploadFileRequest req)
        {
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

            return await response.Content.ReadFromJsonAsync<UploadedFile?>();
        }

        public async Task<UploadProgress?> UploadProgress(string hash)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"hash", hash }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "uplaodprogress", query));

            return await response.Content.ReadFromJsonAsync<UploadProgress?>();
        }


        public async Task<UploadedFile?> DownloadFile(string url)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"url", string.Join(" ", url) }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "downloadfile", query));

            return await response.Content.ReadFromJsonAsync<UploadedFile?>();
        }
        public async Task<UploadedFile?> DownloadFileAsync(Array url)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"url", string.Join(" ", url) }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "downloadfileasync", query));

            return await response.Content.ReadFromJsonAsync<UploadedFile?>();
        }
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


            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }
        public async Task<UploadedFile?> ChecksumFile(long fileId)
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

            return await response.Content.ReadFromJsonAsync<UploadedFile?>();
        }
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

            return await response.Content.ReadFromJsonAsync<DeleteFileResponse?>();
        }

        public async Task<SingleFileResponse?> RenameFile(long fileId, string toName)
        {
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

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }
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

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }
    }
}
