using System.Net.Http.Headers;
using pcloud_sdk_csharp.Requests;
using pcloud_sdk_csharp.Responses;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Controllers
{
    public static class FolderController
    {
        private static readonly string BaseURL = @"https://eapi.pcloud.com/";
        private static readonly HttpClient client = new();

        public static async Task<Folder> CearteFolder(long folderId, string name, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync(BaseURL + "createfolder", $"{{folderid: {folderId}, name: {name}}}");

            return await response.Content.ReadFromJsonAsync<Folder>();

        }

        public static async Task<Folder> CearteFolderIfNotExists(long folderId, string name, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync(BaseURL + "createfolderifnotexists", $"{{folderid: {folderId}, name: {name}}}");

            return await response.Content.ReadFromJsonAsync<Folder>();

        }

        public static async Task<Folder> ListFolder(ListFolderRequest req, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "folderid", req.FolderId.ToString() },
                { "recursive", req.Recursive.ToString() },
                { "showdeleted", req.ShowDeleted.ToString() },
                { "nofiles", req.NoFiles.ToString() },
                { "noshares", req.NoShares.ToString() }
            };

            var response = await client.GetAsync(new Uri(QueryHelpers.AddQueryString(BaseURL + "listfolder", query)));

            return await response.Content.ReadFromJsonAsync<Folder>();
        }

        public static async Task<Folder> RenameFolder(long folderId, string toName, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync(BaseURL + "renamefolder", $"{{folderid: {folderId}, toName: {toName}}}");

            return await response.Content.ReadFromJsonAsync<Folder>();
        }
        public static async Task<Folder> MoveFolder(long folderId, long toFolderId, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync(BaseURL + "movefolder", $"{{folderid: {folderId}, tofolderid: {toFolderId}}}");

            return await response.Content.ReadFromJsonAsync<Folder>();
        }


        public static async Task<Folder> DeleteFolder(long folderId, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync(BaseURL + "deletefolder", $"{{folderid: {folderId}}}");

            return await response.Content.ReadFromJsonAsync<Folder>();
        }

        public static async Task<DeleteFolder> DeleteFolderRecursive(int folderId, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync(BaseURL + "deletefolderrecursive", $"{{folderid: {folderId}}}");

            return await response.Content.ReadFromJsonAsync<DeleteFolder>();
        }

        public static async Task<Folder> CopyFolder(CopyFolderRequest req, string token)
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

            var query = new Dictionary<string, string>
            {
                {"folderid", req.FolderId.ToString()},
                {"tofolderid", req.ToFolderId.ToString()},
                {"noover", req.NoOver.ToString()},
                {"skipexisting", req.SkipExisting.ToString()},
                {"copycontentonly", req.CopyContentOnly.ToString() }
            };

            var response = await client.GetAsync(new Uri(QueryHelpers.AddQueryString(BaseURL + "copyfolder", query)));

            return await response.Content.ReadFromJsonAsync<Folder>();
        }
    }

    public static class FileController
    {
        private static readonly string BaseURL = "https://eapi.pcloud.com/";
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

            var response = await client.PostAsync(BaseURL + "uploadfile", formData);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }

        public static async Task<UploadProgress> UploadProgress(string hash, string token)
        {
            var header = client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            header.Add("hash", hash);

            var response = await client.GetAsync(BaseURL + "uplaodprogress");

            return await response.Content.ReadFromJsonAsync<UploadProgress>();
        }


        public static async Task<UploadedFile> DownloadFile(string url, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await client.GetAsync(BaseURL + "downloadfile");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> DownloadFileAsync(Array url, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("url", string.Join(" ", url));

            var response = await client.GetAsync(BaseURL + "downloadfileasync");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> CopyFile(long fileId, long toFolderId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());
            headers.Add("tofolderid", toFolderId.ToString());

            var response = await client.PostAsync(BaseURL + "copyfile", null);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> ChecksumFile(long fileId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.GetAsync(BaseURL + "checksumfile");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> DeleteFile(long fileId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.DeleteAsync(BaseURL + "deletefile");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }

        public static async Task<UploadedFile> RenameFile(long fileId, string toName, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());
            headers.Add("toName", toName);

            var response = await client.PostAsync(BaseURL + "renamefile", null);

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
        public static async Task<UploadedFile> Stat(long fileId, string token)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            headers.Add("fileid", fileId.ToString());

            var response = await client.GetAsync(BaseURL + "stat");

            return await response.Content.ReadFromJsonAsync<UploadedFile>();
        }
    }

    public static class AuthController
    {
        private static readonly string OAuthUrl = "https://my.pcloud.com/oauth2/";
        private static readonly string BaseURL = "https://eapi.pcloud.com/";
        private static readonly HttpClient client = new();

        public static Uri GetOAuthUrl(AuthorizeRequest req)
        {
            var query = new Dictionary<string, string>
            {
                { "client_id", req.Client_Id },
                { "response_type", req.Type.ToString() },
                { "force_reapprove", req.ForceApprove.ToString() }
            };
            if (req.RedirectUri != null) query.Add("redirect_uri", req.RedirectUri.ToString());
            if (req.State != null) query.Add("state", req.State.ToString());

            return new Uri(QueryHelpers.AddQueryString(OAuthUrl + "authorize", query));
        }

        public static async Task<AuthResponse> GetOAuth_Token(string client_id, string client_secret, string code)
        {
            var headers = client.DefaultRequestHeaders;
            headers.Clear();
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "client_id", client_id },
                { "client_secret", client_secret },
                { "code", code },
            };

            var response = await client.GetAsync(new Uri(QueryHelpers.AddQueryString(BaseURL + "oauth2_token", query)));

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
    }
}