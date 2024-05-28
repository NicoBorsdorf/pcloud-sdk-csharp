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
            formData.Add(new StringContent(req.NoPartial.ToString()), "nopartial");
            formData.Add(new StringContent(req.RenameIfExists.ToString()), "renameifexists");
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

            header.Add("hash", hash);

            var response = await _client.GetAsync(_baseUrl + "uplaodprogress");

            return await response.Content.ReadFromJsonAsync<UploadProgress?>();
        }


        public async Task<UploadedFile?> DownloadFile(string url)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await _client.GetAsync(_baseUrl + "downloadfile");

            return await response.Content.ReadFromJsonAsync<UploadedFile?>();
        }
        public async Task<UploadedFile?> DownloadFileAsync(Array url)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await _client.GetAsync(_baseUrl + "downloadfileasync");

            return await response.Content.ReadFromJsonAsync<UploadedFile?>();
        }
        public async Task<SingleFileResponse?> CopyFile(long fileId, long toFolderId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());
            headers.Add("tofolderid", toFolderId.ToString());

            var response = await _client.PostAsync(_baseUrl + "copyfile", null);

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }
        public async Task<UploadedFile?> ChecksumFile(long fileId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await _client.GetAsync(_baseUrl + "checksumfile");

            return await response.Content.ReadFromJsonAsync<UploadedFile?>();
        }
        public async Task<DeleteFileResponse?> DeleteFile(long fileId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await _client.DeleteAsync(_baseUrl + "deletefile");

            return await response.Content.ReadFromJsonAsync<DeleteFileResponse?>();
        }

        public async Task<SingleFileResponse?> RenameFile(long fileId, string toName)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());
            headers.Add("toName", toName);

            var response = await _client.PostAsync(_baseUrl + "renamefile", null);

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }
        public async Task<SingleFileResponse?> Stat(long fileId)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await _client.GetAsync(_baseUrl + "stat");

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }
    }
}
