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

        public async Task<Folder> CearteFolder(long folderId, string name, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", folderId.ToString());
            header.Add("name", name);

            var response = await client.PostAsync(baseURL + "createfolder", null);

            return JsonSerializer.Deserialize<Folder>(await response.Content.ReadAsStringAsync());

        }

        public async Task<Folder> CearteFolderIfNotExists(long folderId, string name, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", folderId.ToString());
            header.Add("name", name);

            var response = await client.PostAsync(baseURL + "createfolderifnotexists", null);

            return JsonSerializer.Deserialize<Folder>(await response.Content.ReadAsStringAsync());

        }

        public async Task<Folder> ListFolder(long folderId, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", folderId.ToString());

            var response = await client.GetAsync(baseURL + "listfolder");

            return JsonSerializer.Deserialize<Folder>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Folder> RenameFolder(long folderId, string toName, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", folderId.ToString());
            header.Add("toName", toName);

            var response = await client.PostAsync(baseURL + "renamefolder", null);

            return JsonSerializer.Deserialize<Folder>(await response.Content.ReadAsStringAsync());
        }
        public async Task<Folder> MoveFolder(long folderId, long toFolderId, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", folderId.ToString());
            header.Add("tofolderid", toFolderId.ToString());

            var response = await client.PostAsync(baseURL + "renamefolder", null);

            return JsonSerializer.Deserialize<Folder>(await response.Content.ReadAsStringAsync());
        }


        public async Task<Folder> DeleteFolder(long folderId, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", folderId.ToString());

            var response = await client.PostAsync(baseURL + "deletefolder", null);

            return JsonSerializer.Deserialize<Folder>(await response.Content.ReadAsStringAsync());
        }

        public async Task<DeleteFolder> DeleteFolderRecursive(int folderId, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", folderId.ToString());

            var response = await client.PostAsync(baseURL + "deletefolderrecursive", null);

            return JsonSerializer.Deserialize<DeleteFolder>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Folder> CopyFolder(CopyFolderRequest req, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("folderid", req.FolderId.ToString());
            header.Add("tofolderid", req.ToFolderId.ToString());
            header.Add("noover", req.NoOver.ToString());
            header.Add("skipexisting", req.SkipExisting.ToString());
            header.Add("copycontentonly", req.CopyContentOnly.ToString());

            var response = await client.PostAsync(baseURL + "copyfolder", null);

            return JsonSerializer.Deserialize<Folder>(await response.Content.ReadAsStringAsync());
        }
    }

    public static class FileController
    {
        private static readonly string baseURL = "https://eapi.pcloud.com/";
        private static readonly HttpClient client = new();

        public static async Task<UploadedFile?> UploadFile(UploadFileRequest req, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

            var response = await client.PostAsync(baseURL + "uploadfile", formData);

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<UploadProgress> UploadProgress(string hash, string token)
        {
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
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await client.GetAsync(baseURL + "downloadfile");

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }
        public static async Task<UploadedFile> DownloadFileAsync(Array url, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await client.GetAsync(baseURL + "downloadfileasync");

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }
        public static async Task<UploadedFile> CopyFile(long fileId, long toFolderId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());
            headers.Add("tofolderid", toFolderId.ToString());

            var response = await client.PostAsync(baseURL + "copyfile", null);

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }
        public static async Task<UploadedFile> ChecksumFile(long fileId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.GetAsync(baseURL + "checksumfile");

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }
        public static async Task<UploadedFile> DeleteFile(long fileId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.DeleteAsync(baseURL + "deletefile");

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<UploadedFile> RenameFile(long fileId, string toName, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());
            headers.Add("toName", toName);

            var response = await client.PostAsync(baseURL + "renamefile", null);

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }
        public static async Task<UploadedFile> Stat(long fileId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.GetAsync(baseURL + "stat");

            return JsonSerializer.Deserialize<UploadedFile>(await response.Content.ReadAsStringAsync());
        }
    }

}