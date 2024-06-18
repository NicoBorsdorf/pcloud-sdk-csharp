using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.Collection.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Collection.Controller
{
    public class CollectionController
    {
        /// <summary>
        /// Creates new instance of collection for trash endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public CollectionController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.ToString().EndsWith("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly HttpClient _client = new();
        private readonly string _token;
        private readonly Uri _baseUrl;

        /// <summary>
        /// Get a list of the collections, that are owned from the current user. 
        /// </summary>
        /// <param name="type">Filter type of the collection. 1 is for playlists. 0 is for generic</param>
        /// <param name="showFiles">If set, then contents of the collection will be filled with metadata of the files in the collection.</param>
        /// <param name="pageSize">If set and showfiles is set, then the items in contents will be limited to this count.</param>
        /// <returns>On success returns the <see cref="CollectionsResponse.collections">collections</see> and optionally the metadata of the first items in the collection in the field contents.</returns>
        public async Task<CollectionsResponse?> List(CollectionType? type, bool? showFiles, int? pageSize)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqUrl = _baseUrl + "collection_list";
            var query = new Dictionary<string, string>();
            if (type != null) query.Add("type", ((long)type).ToString());
            if (showFiles != null) query.Add("showfiles", (bool)showFiles ? "1" : "0");
            if (pageSize != null) query.Add("pagesize", pageSize.ToString()!);

            if (query.Count > 0) reqUrl = QueryHelpers.AddQueryString(reqUrl, query);

            var response = await _client.GetAsync(reqUrl);

            return await response.Content.ReadFromJsonAsync<CollectionsResponse>();
        }

        /// <summary>
        /// Available collection types. 1 for playlist. 0 for generic <see href="https://docs.pcloud.com/methods/collection/">see Docs</see>
        /// </summary>
        public enum CollectionType
        {
            Generic = 0,
            Playlist = 1
        }

        /// <summary>
        /// Get details for a given collection and the items in it. 
        /// </summary>
        /// <param name="collectionId">The id of the collection.</param>
        /// <param name="page">The number of the page, for which results are shown./param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>On success returns the <see cref="CollectionDetailsResponse.collection">collection</see> and the metadata of the items in the field contents.</returns>
        public async Task<CollectionDetailsResponse?> Details(long collectionId, int? page, int? pageSize)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"collectionid", collectionId.ToString() }
            };
            if (page != null) query.Add("page", page.ToString()!);
            if (pageSize != null) query.Add("pagesize", pageSize.ToString()!);

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "collection_details", query));

            return await response.Content.ReadFromJsonAsync<CollectionDetailsResponse>();
        }

        /// <summary>
        /// Create a new collection for the current user. 
        /// </summary>
        /// <param name="name">The name of the new collection.</param>
        /// <param name="fileIds">List of file ids to fill the collection.</param>
        /// <param name="type">Type of the collection.</param>
        /// <returns>On success returns the new <see cref="CollectionResponse.collection">collection</see> and the metadata of the items in the field contents, if such were given. </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="name"/> is null.</exception>
        public async Task<CollectionResponse?> Create(string name, List<long>? fileIds, CollectionType? type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"name", name }
            };
            if (fileIds != null) reqBody.Add("fileIds", string.Join(",", fileIds));
            if (type != null) reqBody.Add("type", ((long)type.Value).ToString());

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "collection_create", content);

            return await response.Content.ReadFromJsonAsync<CollectionResponse>();
        }

        /// <summary>
        /// Renames a given collection owned by the current user.
        /// </summary>
        /// <param name="collectionId">The id of the collection.</param>
        /// <param name="name">The new name of the collection.</param>
        /// <returns>On success returns the modified <see cref="CollectionResponse.collection">collection</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="name"/> is null.</exception>
        public async Task<CollectionResponse?> Rename(long collectionId, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"collectionid", collectionId.ToString() },
                {"name", name }
            };

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PutAsync(_baseUrl + "collection_rename", content);

            return await response.Content.ReadFromJsonAsync<CollectionResponse>();
        }

        /// <summary>
        /// Delete a given collection owned by the current user. 
        /// </summary>
        /// <param name="collectionId">The id of the collection.</param>
        /// <returns><see cref="Response">Base response.</see></returns>
        public async Task<Response?> Delete(long collectionId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"collectionid", collectionId.ToString() }
            };

            var response = await _client.DeleteAsync(QueryHelpers.AddQueryString(_baseUrl + "collection_delete", query));

            return await response.Content.ReadFromJsonAsync<Response>();
        }

        /// <summary>
        /// Appends files to the collection. 
        /// </summary>
        /// <param name="collectionId">The id of the collection.</param>
        /// <param name="fileIds">List of ids of the files to be added.</param>
        /// <param name="noItems">If set, then linkresult will be empty</param>
        /// <returns>On success returns the updated collection.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="fileIds"/> is null or empty.</exception>
        public async Task<LinkFilesResponse?> LinkFiles(long collectionId, List<long> fileIds, bool? noItems)
        {
            if (fileIds == null || fileIds.Count == 0) throw new ArgumentNullException(nameof(fileIds));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"collectionid", collectionId.ToString() },
                {"fileids", string.Join(",", fileIds) }
            };
            if (noItems != null) reqBody.Add("noitems", (bool)noItems ? "1" : "0");

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "collection_linkfiles", content);

            return await response.Content.ReadFromJsonAsync<LinkFilesResponse>();
        }

        /// <summary>
        /// Remove files from a current collection. Only one optional parameter is suport. The first fount not null value will be used in the request, the others will be ignored. <see href="https://docs.pcloud.com/methods/collection/collection_unlinkfiles.html">see Docs</see>
        /// </summary>
        /// <param name="collectionId">The id of the collection.</param>
        /// <param name="all">If set, all files from the collection are unlinked.</param>
        /// <param name="positions">List of positions to be unlinked.</param>
        /// <param name="fileIds">List of fileids to be unlinked.</param>
        /// <returns>On success returns the modified <see cref="CollectionResponse.collection">collection</see>.</returns>
        public async Task<CollectionResponse?> UnlinkFiles(long collectionId, bool? all, List<long>? positions, List<long>? fileIds)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"collectionid", collectionId.ToString() },
            };
            switch (true)
            {
                case true when all != null:
                    reqBody.Add("all", (bool)all ? "1" : "0");
                    break;
                case true when positions != null:
                    reqBody.Add("positions", string.Join(",", positions));
                    break;
                case true when fileIds != null:
                    reqBody.Add("fileids", string.Join(",", fileIds));
                    break;
            }

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "collection_unlinkfiles", content);

            return await response.Content.ReadFromJsonAsync<CollectionResponse>();
        }

        /// <summary>
        /// Changes the position of an item in a given colleciton, owned by the current user.
        /// </summary>
        /// <param name="collectionId">The id of the collection.</param>
        /// <param name="item">The position of the item in the collection.</param>
        /// <param name="fileId">The id of the file to be moved in the collection.</param>
        /// <param name="position">The position to which the items to be placed.</param>
        /// <returns><see cref="Response">Base response</see></returns>
        public async Task<Response?> Move(long collectionId, long item, long fileId, long position)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"collectionid", collectionId.ToString() },
                {"item", item.ToString() },
                {"fileid", fileId.ToString() },
                {"position", position.ToString() },
            };

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "collection_move", content);

            return await response.Content.ReadFromJsonAsync<CollectionResponse>();
        }

    }
}
