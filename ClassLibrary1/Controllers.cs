using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using pcloud_sdk_csharp.Requests;
using pcloud_sdk_csharp.Responses;
using System.Text.Json;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

namespace pcloud_sdk_csharp.Controllers
{
    public class FolderController
    {
        private readonly string baseURL = @"https://eapi.pcloud.com/";
        private readonly HttpClient client = new();

        public async Task<Folder> CearteFolder(CreateFolderRequest req, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, baseURL + "createFolder");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(req.ToJson(), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<Folder>();

        }

        public async Task<Folder> CearteFolderIfNotExists(CreateFolderRequest req, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, baseURL + "createfolderifnotexists");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(req.ToJson(), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<Folder>();

        }

        public async Task<Folder> ListFolder(ListFolderRequest req, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "listfolder");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(req.ToJson(), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<Folder>();
        }

        public async Task<Folder> RenameFolder(RenameFolderRequest req, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "renamefolder");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(req.ToJson(), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<Folder>();
        }

        public async Task<Folder> DeleteFolder(int folderId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "deletefolder");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent($"{{folderid: {folderId}}}", Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<Folder>();
        }

        public async Task<DeleteFolder> DeleteFolderRecursive(int folderId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "deletefolderrecursive");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent($"{{folderid: {folderId}}}", Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<DeleteFolder>();
        }

        public async Task<Folder> CopyFolder(int folderId, int toFolderId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "copyfolder");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent($"{{ folderid: {folderId}, tofolderid: {toFolderId} }}", Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<Folder>();
        }
    }

    public static class FileController
    {
        private static readonly string baseURL = "https://eapi.pcloud.com/";
        private static readonly HttpClient client = new();

        public static async Task<UploadedFile?> UploadFile(UploadFileRequest req, string token)
        {
            client.CancelPendingRequests();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(req.FolderId.ToString()), "folderid");
            formData.Add(new StringContent(req.FileName), "filename");

            var fileContent = new StreamContent(req.UploadFile);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"files\"",
                FileName = $"\"{req.FileName}\"",
            };
            formData.Add(fileContent);

            var response = await client.PostAsync(baseURL + "uploadfile", formData);

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<UploadProgress> UploadProgress(string hash, string token)
        {
            client.CancelPendingRequests();
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("hash", hash);

            var response = await client.GetAsync(baseURL + "uplaodprogress");

            return JsonSerializer.Deserialize<UploadProgress>(await response.Content.ReadAsStringAsync());
        }


        public static async Task<UploadedFile> DownloadFile(string url, string token)
        {
            client.CancelPendingRequests();
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await client.GetAsync(baseURL + "downloadfile");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> DownloadFileAsync(Array url, string token)
        {
            client.CancelPendingRequests();
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await client.GetAsync(baseURL + "downloadfileasync");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> CopyFile(long fileId, long toFolderId, string token)
        {
            client.CancelPendingRequests();
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());
            headers.Add("tofolderid", toFolderId.ToString());

            var response = await client.PostAsync(baseURL + "copyfile", null);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> ChecksumFile(long fileId, string token)
        {
            client.CancelPendingRequests();
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.GetAsync(baseURL + "checksumfile");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> DeleteFile(long fileId, string token)
        {
            client.CancelPendingRequests();
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.DeleteAsync(baseURL + "deletefile");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> RenameFile(string token)
        {
            client.CancelPendingRequests();
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> Stat(string token)
        {
            client.CancelPendingRequests();
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
    }

}