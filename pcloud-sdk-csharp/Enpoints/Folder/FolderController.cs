using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using pcloud_sdk_csharp.Folder.Responses;
using pcloud_sdk_csharp.Folder.Requests;

namespace pcloud_sdk_csharp.Folder.Controller
{
    public class FolderController
    {

        /// <summary>
        /// Creates new instance of controller for folder endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public FolderController(string access_token, string clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));
        }

        private readonly string _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        /// <summary>
        /// Creates a new folder in either a folder with given Id or the Root folder wiht Id = 0.
        /// </summary>
        /// <param name="name">Name of the created folder.</param>
        /// <param name="folderId">Id of the parent folder. Default 0 is the Id of the Root folder.</param>
        /// <returns>Returns <see cref="FolderResponse">metadata</see> of the created folder.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="name"/> is null.
        /// </exception>
        public async Task<FolderResponse?> CreateFolder(string name, long folderId = 0)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "createfolder", $"{{folderid: {folderId}, name: {name}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        /// <summary>
        /// Creates a folder if the folder doesn't exist or returns the existing folder's metadata. 
        /// </summary>
        /// <param name="name">Name of the created folder.</param>
        /// <param name="folderId">Id of the parent folder. Default 0 is the Id of the Root folder.</param>
        /// <returns>Returns <see cref="FolderResponse">metadata</see> of the created folder.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="name"/> is null.
        /// </exception>
        public async Task<FolderResponse?> CearteFolderIfNotExists(string name, long folderId = 0)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "createfolderifnotexists", $"{{folderid: {folderId}, name: {name}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        /// <summary>
        /// Gets data for a folder. 
        /// </summary>
        /// <param name="req">Request definition for query parameters.</param>
        /// <returns>Returns folder's <see cref="FolderResponse">metadata</see>. The metadata will have contents field that is array of metadatas of folder's contents.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="req"/> is null.
        /// </exception>
        /// <remarks>       
        /// <see cref="ListFolderRequest">Request</see> parameters:  
        /// <br /> - folderid: Id of folder.  
        /// <br />
        /// Optional: 
        /// <br /> - recursive: If is set full directory tree will be returned, which means that all directories will have contents filed.
        /// <br /> - showdeleted: If is set, deleted files and folders that can be undeleted will be displayed.
        /// <br /> - nofiles: If is set, only the folder (sub)structure will be returned.
        /// <br /> - noshares:  If is set, only user's own folders and files will be displayed.
        /// </remarks>
        public async Task<FolderResponse?> ListFolder(ListFolderRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "folderid", req.FolderId.ToString() },
            };
            if (req.Recursive != null) query.Add("recursive", req.Recursive.ToString()!);
            if (req.ShowDeleted != null) query.Add("showdeleted", req.ShowDeleted.ToString()!);
            if (req.NoFiles != null) query.Add("nofiles", req.NoFiles.ToString()!);
            if (req.NoShares != null) query.Add("noshares", req.NoShares.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "listfolder", query)));

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        /// <summary>
        /// Renames a folder.
        /// </summary>
        /// <param name="folderId">Id of folder to rename.</param>
        /// <param name="toName">New name of folder.</param>
        /// <returns>Returns <see cref="FolderResponse">metadata</see> of the renamed folder.</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="toName"/> is null.
        /// </exception>
        public async Task<FolderResponse?> RenameFolder(long folderId, string toName)
        {
            if (string.IsNullOrEmpty(toName)) throw new ArgumentNullException(nameof(toName));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "renamefolder", $"{{folderid: {folderId}, toName: {toName}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        /// <summary>
        /// Moves a folder.
        /// </summary>
        /// <param name="folderId">Id of folder which should be moved.</param>
        /// <param name="toFolderId">Id of folder contents should be moved to.</param>
        /// <returns>Returns <see cref="FolderResponse">metadata</see> of the moved folder.</returns>
        public async Task<FolderResponse?> MoveFolder(long folderId, long toFolderId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "movefolder", $"{{folderid: {folderId}, tofolderid: {toFolderId}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        /// <summary>
        /// Deletes a folder. Folder must be empty. Else you need to call <see cref="DeleteFolderRecursive(long)"/>
        /// </summary>
        /// <param name="folderId">Id of folder that should be deleted.</param>
        /// <returns>Returns <see cref="FolderResponse">metadata</see> structure of the deleted folder.</returns>
        public async Task<FolderResponse?> DeleteFolder(long folderId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "deletefolder", $"{{folderid: {folderId}}}");

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }

        /// <summary>
        /// Deletes a folder and all of its content.
        /// </summary>
        /// <param name="folderId">Id of folder which should be deleted.</param>
        /// <returns>Returns the number of <see cref="DeleteFolderResponse.deletedfiles">deletedfiles</see> and number of <see cref="DeleteFolderResponse.deletedfolders">deletedfolders</see></returns>
        public async Task<DeleteFolderResponse?> DeleteFolderRecursive(long folderId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "deletefolderrecursive", $"{{folderid: {folderId}}}");

            return await response.Content.ReadFromJsonAsync<DeleteFolderResponse?>();
        }

        /// <summary>
        /// Copies a folder. 
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Returns <see cref="FolderResponse">metadata</see> of the created folder.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>       
        /// <see cref="CopyFolderRequest">Request</see> parameters:  
        /// <br /> - folderid: Id of the source folder.  
        /// <br /> - tofolderid: Id of destination folder.
        /// <br />
        /// Optional: 
        /// <br /> - noover: If it is set and files with the same name already exist, no overwriting will be preformed and error 2004 will be returned.
        /// <br /> - skipexisting: If set will skip files that already exist.
        /// <br /> - copycontentonly: If it is set only the content of source folder will be copied otherwise the folder itself is copied.
        /// </remarks>
        public async Task<FolderResponse?> CopyFolder(CopyFolderRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"folderid", req.FolderId.ToString()},
                {"tofolderid", req.ToFolderId.ToString()},
            };
            if (req.NoOver != null) query.Add("noover", req.NoOver.ToString()!);
            if (req.SkipExisting != null) query.Add("skipexisting", req.SkipExisting.ToString()!);
            if (req.CopyContentOnly != null) query.Add("copycontentonly", req.CopyContentOnly.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "copyfolder", query)));

            return await response.Content.ReadFromJsonAsync<FolderResponse?>();
        }
    }

}
