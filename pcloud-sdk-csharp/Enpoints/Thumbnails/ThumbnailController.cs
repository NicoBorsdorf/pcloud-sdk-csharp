using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Thumbnails.Requests;
using pcloud_sdk_csharp.Thumbnails.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Thumbnails.Controller
{
    public class ThumbnailController
    {

        /// <summary>
        /// Creates new instance of controller for thumbnail endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="pcloud_sdk_csharp.Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public ThumbnailController(string access_token, string clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));
        }

        private readonly HttpClient _client = new();
        private readonly string _token;
        private readonly string _baseUrl;

        /// <summary>
        /// Get a link to a thumbnail of a file.
        /// </summary>
        /// <param name="req">Request definition for /getthumblink endpoint.</param>
        /// <returns>On success the same data as with getfilelink is returned with additional size property.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ThumbRequest">Request</see> parameters:
        /// <br/> - fileid: Id of the file for thumb.
        /// <br/> - size: WIDTHxHEIGHT.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - crop: If set, then the thumb will be cropped.
        /// <br/> - type: If set to png, then the thumb will be in png format.
        /// <br/> 
        /// </remarks>
        public async Task<GetThumbResponse?> GetThumbLink(ThumbRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
                { "size", req.Size}
            };
            if (req.Crop != null) query.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) query.Add("type", req.Type);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getthumblink", query)));

            return await response.Content.ReadFromJsonAsync<GetThumbResponse?>();
        }

        /// <summary>
        /// Get a link to thumbnails of a list of files.
        /// </summary>
        /// <param name="req">Request definition for /getthumbslinks endpoint.</param>
        /// <returns>The method returns an array <see cref="GetThumbLinksResponse.thumbs">thumbs</see> with objects. Each object has result and fileid set.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="GetThumbLinksRequest">Request</see> parameters:
        /// <br/> - fileids: Coma-separated list of fileids.
        /// <br/> - size: WIDTHxHEIGHT.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - crop: If set, then the thumb will be cropped.
        /// <br/> - type: If set to png, then the thumb will be in png format.
        /// <br/> 
        /// </remarks>
        public async Task<GetThumbLinksResponse?> GetThumbsLinks(GetThumbLinksRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileids", string.Join(",", req.FileIds) },
                { "size", req.Size}
            };
            if (req.Crop != null) query.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) query.Add("type", req.Type);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getthumbslinks", query)));

            return await response.Content.ReadFromJsonAsync<GetThumbLinksResponse?>();
        }

        /// <summary>
        /// Get a thumbnail of a file.
        /// </summary>
        /// <param name="req">Request definition for /getthumb endpoint.</param>
        /// <returns>Resturns Stream of thumbnail for files.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ThumbRequest">Request</see> parameters:
        /// <br/> - fileid: Id of the file for thumb.
        /// <br/> - size: WIDTHxHEIGHT.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - crop: If set, then the thumb will be cropped.
        /// <br/> - type: If set to png, then the thumb will be in png format.
        /// <br/> 
        /// </remarks>
        public async Task<Stream?> GetThumb(ThumbRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("image/*"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
                { "size", req.Size}
            };
            if (req.Crop != null) query.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) query.Add("type", req.Type);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getthumb", query)));

            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Create a thumbnail of a file and save it in the current user's filesystem.
        /// </summary>
        /// <param name="req">Request definition for /savethumb endpoint.</param>
        /// <returns>On success returns <see cref="SaveThumbnailResponse.metadata">metadata</see>, <see cref="SaveThumbnailResponse.width">width</see> and <see cref="SaveThumbnailResponse.height">height</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="SaveThumbRequest">Request</see> parameters:
        /// <br/> - fileid: Id of the file for thumb.
        /// <br/> - size: WIDTHxHEIGHT.
        /// <br/> - tofolderid: Folder id of the folder where to save the thumb.
        /// <br/> - toname: Filename to save the thumb.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - crop: If set, then the thumb will be cropped.
        /// <br/> - type: If set to png, then the thumb will be in png format.
        /// <br/> - noover: If set, then will rise error on overwriting.
        /// <br/> 
        /// </remarks>
        public async Task<SaveThumbnailResponse?> SaveThumb(SaveThumbRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"fileid", req.FileId.ToString()},
                {"size", req.Size.ToString()},
                {"tofolderid", req.ToFolderId.ToString()},
                {"toname", req.ToName }
            };
            if (req.Crop != null) reqBody.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) reqBody.Add("type", req.Type);
            if (req.NoOver != null) reqBody.Add("noover", req.NoOver.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "savethumb", content);

            return await response.Content.ReadFromJsonAsync<SaveThumbnailResponse?>();
        }
    }
}
