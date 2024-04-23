using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using pcloud_sdk_csharp.Requests;
using pcloud_sdk_csharp.Responses;
using System.Text.Json;

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
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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


            HttpResponseMessage response = await client.PostAsync(baseURL + "uploadfile", formData);

            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<UploadedFile>(responseContent);
        }

        /*
        public async Task<UploadedFile> UploadProgress(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "uploadprogress");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public async Task<UploadedFile> DownloadFile(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "downloadfile");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public async Task<UploadedFile> DownloadFileAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "downloadfileasync");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public async Task<UploadedFile> CopyFile(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, baseURL + "copyfile");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public async Task<UploadedFile> ChecksumFile(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "checksumfile");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public async Task<UploadedFile> DeleteFile(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, baseURL + "deletefile");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public async Task<UploadedFile> RenameFile(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, baseURL + "renamefile");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public async Task<UploadedFile> Stat(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseURL + "stat");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }*/
    }

}