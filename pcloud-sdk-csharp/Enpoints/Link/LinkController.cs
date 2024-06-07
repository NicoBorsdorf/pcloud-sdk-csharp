using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Responses;
using pcloud_sdk_csharp.Link.Requests;
using pcloud_sdk_csharp.Link.Responses;
using pcloud_sdk_csharp.Streaming.Responses;
using pcloud_sdk_csharp.Thumbnails.Requests;
using pcloud_sdk_csharp.Thumbnails.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Link.Controller
{

    public class LinkController
    {
        public LinkController(string access_token, string clientURL)
        {
            _token = access_token;
            _baseUrl = clientURL;
        }

        private readonly string _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        public async Task<LinkResponse?> GetFilePubLink(LinkRequest req) => await HandleRequest("getfilepublink", req);
        public async Task<LinkResponse?> GetFolderPubLink(LinkRequest req) => await HandleRequest("getfolderpublink", req);
        public async Task<LinkResponse?> GetTreePubLink(LinkRequest req) => await HandleRequest("gettreepublink", req);
        public async Task<LinkResponse?> GetPubLinkDownload(LinkRequest req) => await HandleRequest("getpublinkdownload", req, false);
        // same request to different endpoints
        private async Task<LinkResponse?> HandleRequest(string endpoint, LinkRequest req, bool? auth = true)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            // auth is not necessary for publinkdownload
            if (auth == true) header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() }
            };
            if (req.ShortLink != null) query.Add("shortlink", req.ShortLink.ToString()!);
            if (req.Expire != null) query.Add("expire", req.Expire.Value.ToLongDateString());
            if (req.MaxDownloads != null) query.Add("maxdownloads", req.MaxDownloads.ToString()!);
            if (req.MaxTraffic != null) query.Add("maxtraffic", req.MaxTraffic.ToString()!);
            if (req.LinkPassword != null) query.Add("linkpassword", req.LinkPassword);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + endpoint, query)));

            return await response.Content.ReadFromJsonAsync<LinkResponse?>();
        }

        public async Task<SingleFileResponse?> ShowPubLink(string code)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "code", code }
            };

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "showpublink", query)));

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }

        public async Task<SingleFileResponse?> CopyPubFile(CopyLinkRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "code", req.Code },
                { "fileid", req.FileId.ToString()},
                { "tofolderid", req.ToFolderId.ToString() }
            };

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "copypubfile", query)));

            return await response.Content.ReadFromJsonAsync<SingleFileResponse?>();
        }

        public async Task<PubLinksResponse?> ListPubLinks()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "listpublinks");

            return await response.Content.ReadFromJsonAsync<PubLinksResponse?>();
        }

        public async Task<PubLinksResponse?> ListPLShort()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "listplshort");

            return await response.Content.ReadFromJsonAsync<PubLinksResponse?>();
        }

        public async Task<Response?> DeletePubLink(long linkId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"linkid", linkId.ToString() }
            };

            var response = await _client.DeleteAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getpubthumb", query)));

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        public async Task<Response?> ChangePubLink(ChangeLinkRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"linkid", req.LinkId.ToString() }
            };
            if (req.ShortLink != null) reqBody.Add("shortlink", req.ShortLink.ToString()!);
            if (req.DeleteShortLink != null) reqBody.Add("deleteshortlink", req.DeleteShortLink.ToString()!);
            if (req.Expire != null) reqBody.Add("expire", req.Expire.Value.ToLongDateString());
            if (req.DeleteExpire != null) reqBody.Add("deleteexpire", req.DeleteExpire.Value.ToLongDateString());
            if (req.MaxTraffic != null) reqBody.Add("maxtraffic", req.MaxTraffic.ToString()!);
            if (req.MaxDownloads != null) reqBody.Add("maxdownloads", req.MaxDownloads.ToString()!);
            if (req.LinkPasswort != null) reqBody.Add("linkpassword", req.LinkPasswort);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PutAsync(_baseUrl + "changepublink", content);

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        public async Task<StreamingResponse?> GetPubThumb(ThumbLinkRequest req) => await HandleThumbRequest("getpubthumb", req);

        public async Task<StreamingResponse?> GetPubThumbLink(ThumbLinkRequest req) => await HandleThumbRequest("getpubthumblink", req);

        public async Task<StreamingResponse?> GetPubThumbsLinks(ThumbLinkRequest req) => await HandleThumbRequest("getpubthumbslinks", req);

        private async Task<StreamingResponse?> HandleThumbRequest(string endpoint, ThumbLinkRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"code", req.Code },
                {"fileid", req.FileId.ToString() },
                {"size", req.Size }
            };
            if (req.Crop != null) query.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) query.Add("type", req.Type);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + endpoint, query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }


        public async Task<SaveThumbnailResponse?> SavePubThumb(SavePubThumbRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"code", req.Code},
                {"fileid", req.FileId.ToString()},
                {"size", req.Size.ToString()},
                {"tofolderid", req.ToFolderId.ToString()},
                {"toname", req.ToName }
            };
            if (req.Crop != null) reqBody.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) reqBody.Add("type", req.Type);
            if (req.NoOver != null) reqBody.Add("noover", req.NoOver.ToString()!);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "savepubthumb", content);

            return await response.Content.ReadFromJsonAsync<SaveThumbnailResponse?>();
        }

        public async Task<Stream?> GetPubZip(PubZipRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));

            var query = new Dictionary<string, string>
            {
                {"code", req.Code}
            };
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.Filename != null) query.Add("filename", req.Filename);
            if (req.TimeOffset != null) query.Add("timeoffset", req.TimeOffset);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getpubzip", query)));

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<StreamingResponse?> GetPubZipLink(PubZipRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));

            var query = new Dictionary<string, string>
            {
                {"code", req.Code}
            };
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.Filename != null) query.Add("filename", req.Filename);
            if (req.TimeOffset != null) query.Add("timeoffset", req.TimeOffset);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getpubziplink", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse>();
        }

        public async Task<SingleFileResponse?> SavePubZip(SavePubZipRequest req)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));

            var reqBody = new Dictionary<string, string>
            {
                {"code", req.Code}
            };
            if (req.TimeOffset != null) reqBody.Add("timeoffset", req.TimeOffset);
            if (req.ToFolderId != null) reqBody.Add("tofolderid", req.ToFolderId.ToString()!);
            if (req.ToName != null) reqBody.Add("toname", req.ToName);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "getpubziplink", content);

            return await response.Content.ReadFromJsonAsync<SingleFileResponse>();
        }

        public async Task<VideoStreamingResponse?> GetPubVideoLinks(PubVideoLinkRequest req)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "code", req.Code },
            };
            if (req.FileId != 0) query.Add("fileid", req.FileId.ToString()!);
            if (req.ContentType != null) query.Add("contenttype", req.ContentType);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.MaxSpeed != null) query.Add("maxspeed", req.MaxSpeed.ToString()!);
            if (req.SkipFilename != null) query.Add("skipfilename", req.SkipFilename.ToString()!);
            if (req.ABitrate != null) query.Add("skipfilename", req.ABitrate.ToString()!);
            if (req.VBitrate != null) query.Add("skipfilename", req.VBitrate.ToString()!);
            if (req.Resolution != null) query.Add("skipfilename", req.Resolution);
            if (req.FixedBitrate != null) query.Add("skipfilename", req.FixedBitrate.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getvideolink", query)));

            return await response.Content.ReadFromJsonAsync<VideoStreamingResponse?>();
        }

        public async Task<StreamingResponse?> GetPubAudioLink(PubAudioLinkRequest req)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            // TODO: content type
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "code", req.Code },
            };
            if (req.FileId != 0) query.Add("fileid", req.FileId.ToString()!);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.ABitrate != null) query.Add("skipfilename", req.ABitrate.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getvideolink", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }

        public async Task<Stream?> GetPubTextFile(PubTextLinkRequest req)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            // content type changes depending on forced download
            if (req.ForceDownload != null) headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

            var query = new Dictionary<string, string>
            {
                { "code", req.Code },
            };
            if (req.FileId != 0) query.Add("fileid", req.FileId.ToString()!);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.FromEncoding != null) query.Add("maxspeed", req.FromEncoding);
            if (req.ToEncoding != null) query.Add("skipfilename", req.ToEncoding);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "gettextfile", query)));

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<LinkResponse?> GetCollectionPubLink(CollectionLinkRequest req)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "collectionid", req.CollectionId.ToString() },
            };
            if (req.Expire != null) query.Add("expire", req.Expire.Value.ToLongDateString());
            if (req.MaxDownloads != null) query.Add("maxdownloads", req.MaxDownloads.ToString()!);
            if (req.MaxTraffic != null) query.Add("maxtraffic", req.MaxTraffic.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getcollectionpublink", query)));

            return await response.Content.ReadFromJsonAsync<LinkResponse>();
        }

        public async Task<ShortLinkResponse?> GetCollectionPubLink(CollectionShortLinkRequest req)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "collectionid", req.CollectionId.ToString() },
            };
            if (req.Expire != null) query.Add("expire", req.Expire.Value.ToLongDateString());
            if (req.MaxDownloads != null) query.Add("maxdownloads", req.MaxDownloads.ToString()!);
            if (req.MaxTraffic != null) query.Add("maxtraffic", req.MaxTraffic.ToString()!);
            if (req.ShortLink != null) query.Add("shortlink", req.ShortLink.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getcollectionpublink", query)));

            return await response.Content.ReadFromJsonAsync<ShortLinkResponse>();
        }

    }
}
