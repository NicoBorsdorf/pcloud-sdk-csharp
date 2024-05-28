using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using pcloud_sdk_csharp.Folder.Responses;
using pcloud_sdk_csharp.Folder.Requests;

namespace pcloud_sdk_csharp.Folder.Controller
{
    public class FolderController
    {
        public FolderController(string access_token, string clientURL)
        {
            _token = access_token;
            _baseUrl = clientURL;
        }

        private readonly string _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        public async Task<FolderResponse?> CearteFolder(long folderId, string name)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "createfolder", $"{{folderid: {folderId}, name: {name}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();

        }

        public async Task<FolderResponse?> CearteFolderIfNotExists(long folderId, string name)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "createfolderifnotexists", $"{{folderid: {folderId}, name: {name}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();

        }

        public async Task<FolderResponse?> ListFolder(ListFolderRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "folderid", req.FolderId.ToString() },
                { "recursive", req.Recursive.ToString() },
                { "showdeleted", req.ShowDeleted.ToString() },
                { "nofiles", req.NoFiles.ToString() },
                { "noshares", req.NoShares.ToString() }
            };

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "listfolder", query)));

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        public async Task<FolderResponse?> RenameFolder(long folderId, string toName)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "renamefolder", $"{{folderid: {folderId}, toName: {toName}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }
        public async Task<FolderResponse?> MoveFolder(long folderId, long toFolderId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "movefolder", $"{{folderid: {folderId}, tofolderid: {toFolderId}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }


        public async Task<FolderResponse?> DeleteFolder(long folderId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "deletefolder", $"{{folderid: {folderId}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        public async Task<DeleteFolderResponse?> DeleteFolderRecursive(int folderId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "deletefolderrecursive", $"{{folderid: {folderId}}}");

            return await response.Content.ReadFromJsonAsync<DeleteFolderResponse?>();
        }

        public async Task<FolderResponse?> CopyFolder(CopyFolderRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
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

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "copyfolder", query)));

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }
    }

}
