using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Thumbnails.Requests;
using pcloud_sdk_csharp.Thumbnails.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Thumbnails.Controller
{
    public class ThumbnailController
    {
        public ThumbnailController(string access_token, string baseUrl)
        {
            _token = access_token;
            _baseUrl = baseUrl;
        }

        private readonly HttpClient _client = new();
        private readonly string _token;
        private readonly string _baseUrl;

        public async Task<GetThumbResponse?> GetThumbLink(ThumbRequest req)
        {
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

        public async Task<GetThumbLinksResponse?> GetThumbsLinks(GetThumbLinksRequest req)
        {
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

        public async Task<Stream?> GetThumb(ThumbRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);

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

        public async Task<SaveThumbnailResponse?> SaveThumb(SaveThumbRequest req)
        {
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
