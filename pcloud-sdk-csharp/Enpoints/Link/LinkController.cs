using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Responses;
using pcloud_sdk_csharp.Link.Requests;
using pcloud_sdk_csharp.Link.Responses;
using pcloud_sdk_csharp.Streaming.Responses;
using pcloud_sdk_csharp.Thumbnails.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Link.Controller
{

    public class LinkController
    {

        /// <summary>
        /// Creates new instance of controller for publik link endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public LinkController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.AbsoluteUri.Last().Equals("/")) throw new ArgumentException(@"Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly Uri _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        /// <summary>
        /// Creates and return a public link to a file. 
        /// </summary>
        /// <param name="req"></param>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <returns>On success returns <see cref="LinkResponse"/></returns>
        /// <remarks>
        /// <br/> <see cref="LinkRequest">Request</see> parameters:
        /// <br/> - fileid: File id of the file for public link.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - expire: Datetime when the link will stop working.
        /// <br/> - maxdownloads: Maximum number of downloads for this file.
        /// <br/> - maxtraffic: Maximum traffic that this link will consume (in bytes, started downloads will not be cut to fit in this limit).
        /// <br/> - linkpassword: Sets password for the link.
        /// <br/> 
        /// </remarks>
        public async Task<LinkResponse?> GetFilePubLink(LinkRequest req) => await HandleRequest("getfilepublink", req);

        /// <summary>
        /// Creates and return a public link to a folder. 
        /// </summary>
        /// <param name="req"></param>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <returns>On success returns <see cref="LinkResponse"/></returns>
        /// <remarks>
        /// <br/> <see cref="LinkRequest">Request</see> parameters:
        /// <br/> - fileid: File id of the file for public link.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - expire: Datetime when the link will stop working.
        /// <br/> - maxdownloads: Maximum number of downloads for this file.
        /// <br/> - maxtraffic: Maximum traffic that this link will consume (in bytes, started downloads will not be cut to fit in this limit).
        /// <br/> - linkpassword: Sets password for the link.
        /// <br/> 
        /// </remarks>
        public async Task<LinkResponse?> GetFolderPubLink(LinkRequest req) => await HandleRequest("getfolderpublink", req);

        /// <summary>
        /// Creates and returns a public link to a virtual folder that is defined by requested tree.  
        /// </summary>
        /// <param name="req"></param>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <returns>On success returns <see cref="LinkResponse"/></returns>
        /// <remarks>
        /// <br/> <see cref="LinkRequest">Request</see> parameters:
        /// <br/> - fileid: File id of the file for public link.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - expire: Datetime when the link will stop working.
        /// <br/> - maxdownloads: Maximum number of downloads for this file.
        /// <br/> - maxtraffic: Maximum traffic that this link will consume (in bytes, started downloads will not be cut to fit in this limit).
        /// <br/> - linkpassword: Sets password for the link.
        /// <br/> 
        /// </remarks>
        public async Task<LinkResponse?> GetTreePubLink(LinkRequest req) => await HandleRequest("gettreepublink", req);

        // same request to different endpoints
        private async Task<LinkResponse?> HandleRequest(string endpoint, LinkRequest req, bool? auth = true)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
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

        /// <summary>
        /// Returns link(s) where the file can be downloaded.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Returns link(s) where the file can be downloaded (same as getfilelink <see cref="StreamingResponse.hosts">hosts</see>, <see cref="StreamingResponse.path">path</see> and <see cref="StreamingResponse.expires">expires</see> are returned).</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="DownloadLinkRequest">Request</see> parameters:
        /// <br/> - fileid: File id of the file for public link.
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: Download with 'Content-Type' = 'application/octet-stream'.
        /// <br/> - contenttype: Set 'Content-Type'.
        /// <br/> - maxspeed: Limit the download speed.
        /// <br/> - skipfilename: Include the name of the file in the generated link.
        /// <br/> 
        /// </remarks>
        public async Task<StreamingResponse?> GetPubLinkDownload(DownloadLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue(req.ForceDownload != null ? "application/octet-stream" : "application /json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
                { "code", req.Code }
            };
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.MaxSpeed != null) query.Add("maxspeed", req.MaxSpeed.ToString()!);
            if (req.SkipFileName != null) query.Add("skipfilename", req.SkipFileName.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getpublinkdownload", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }

        /// <summary>
        /// Gets metadata of link the provided code points to.
        /// </summary>
        /// <param name="code">Either 'code' or 'shortcode'</param>
        /// <returns>Returns <see cref="SingleFileResponse.metadata">metadata</see> of the object the link points to.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="code"/> is null.</exception>
        public async Task<SingleFileResponse?> ShowPubLink(string code)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

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

        /// <summary>
        /// Copies the file from the public link to the current user's filesystem.
        /// </summary>
        /// <param name="req"></param>
        /// <returns><When successful, copies the file from the public link to the current user's account and returns the new file's <see cref="SingleFileResponse.metadata">metadata</see>./returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>
        /// <br/> <see cref="CopyLinkRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/> - fileid: File id, if the link is to a folder.
        /// <br/> - tofolderid: Id of destination folder.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - toname: Name of the destination file. If omitted, then the original filename is used.
        /// <br/> - noover: If it is set and file with the specified name already exists, no overwriting will be preformed.
        /// <br/> 
        /// </remarks>
        public async Task<SingleFileResponse?> CopyPubFile(CopyLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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

        /// <summary>
        /// Return a list of current user's public links.
        /// </summary>
        /// <returns>Returns all user's public links in array <see cref="PubLinksResponse">publinks</see>.</returns>
        public async Task<PubLinksResponse?> ListPubLinks()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "listpublinks");

            return await response.Content.ReadFromJsonAsync<PubLinksResponse?>();
        }

        /// <summary>
        /// Return a list of current user's public links <see cref="ListPubLinks">listpublinks</see>.
        /// </summary>
        /// <returns>Returns all user's public links in array <see cref="PubLinksResponse">publinks</see>.</returns>
        public async Task<PubLinksResponse?> ListPLShort()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "listplshort");

            return await response.Content.ReadFromJsonAsync<PubLinksResponse?>();
        }

        /// <summary>
        /// Delete a specified public link.
        /// </summary>
        /// <param name="linkId">Id of specified link.</param>
        /// <returns>Result with response code.</returns>
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

        /// <summary>
        /// Modify a specified public link.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Result with response code.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ChangeLinkRequest">Request</see> parameters:
        /// <br/> - linkid: The ID of the link to be changed.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - shortlink: Setting this will create a short link for the link. The response will contain shortcode and shortlink fields.
        /// <br/> - deleteshortlink: Setting this will delete the short link associated with the link.
        /// <br/> - expire: Sets a new expiration date for the link.
        /// <br/> - deleteexpire: If set, deletes link's expiration time (the link will not expire).
        /// <br/> - maxtraffic: Modifies the traffic limit, set to 0 for unlimited.
        /// <br/> - maxdownloads: Modifies the downloads limit, set to 0 for unlimited.
        /// <br/> - linkpassword: Sets password for the link.
        /// <br/> 
        /// </remarks>
        public async Task<Response?> ChangePubLink(ChangeLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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

        /// <summary>
        /// Get a thumbnail of a public file.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Stream of public thumbnail.</returns>
        /// <remarks>
        /// <br/> <see cref="ThumbLinkRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/> - linkid: The ID of the link to be changed.
        /// <br/> - size: WIDTHxHEIGHT.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - crop: If set, then the thumb will be cropped.
        /// <br/> - type: If set to png, then the thumb will be in png format.
        /// <br/> 
        /// </remarks>
        public async Task<Stream?> GetPubThumb(ThumbLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();

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
        /// Get a link to a thumbnatil of a public file.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>On success the <see cref="GetThumbResponse">metadata</see> of public thumbnail link is returned.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="ThumbLinkRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/> - linkid: The ID of the link to be changed.
        /// <br/> - size: WIDTHxHEIGHT.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - crop: If set, then the thumb will be cropped.
        /// <br/> - type: If set to png, then the thumb will be in png format.
        /// <br/> 
        /// </remarks>
        public async Task<GetThumbResponse?> GetPubThumbLink(ThumbLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
                { "size", req.Size}
            };
            if (req.Crop != null) query.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) query.Add("type", req.Type);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getpubthumblink", query)));

            return await response.Content.ReadFromJsonAsync<GetThumbResponse?>();
        }

        /// <summary>
        /// Get a thumbnail of a public file.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>The method returns an array <see cref="GetThumbLinksResponse.thumbs">thumbs</see> with objects. Each object has result and fileid set. </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> ist null.</exception>
        /// <remarks>
        /// <br/> <see cref="ThumbLinkRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/> - linkid: The ID of the link to be changed.
        /// <br/> - size: WIDTHxHEIGHT.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - crop: If set, then the thumb will be cropped.
        /// <br/> - type: If set to png, then the thumb will be in png format.
        /// <br/> 
        /// </remarks>
        public async Task<GetThumbLinksResponse?> GetPubThumbsLinks(ThumbLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "code", req.Code },
                { "fileid", req.FileId.ToString() },
                { "size", req.Size}
            };
            if (req.Crop != null) query.Add("crop", req.Crop.ToString()!);
            if (req.Type != null) query.Add("type", req.Type);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getpubthumbslinks", query)));

            return await response.Content.ReadFromJsonAsync<GetThumbLinksResponse?>();
        }

        /// <summary>
        /// Create a thumbnail of a public link file and save it in the current user's filesystem.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>On success returns <see cref="SaveThumbnailResponse.metadata">metadata</see>, <see cref="SaveThumbnailResponse.width">width</see> and <see cref="SaveThumbnailResponse.height">height</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="SavePubThumbRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/> - linkid: The ID of the link to be changed.
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
        public async Task<SaveThumbnailResponse?> SavePubThumb(SavePubThumbRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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

        /// <summary>
        /// Create a zip archive file of a public link file and download it.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>When successful it returns a zip archive over the current API connection with all the files and directories in the requested tree.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="PubZipRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: If it is set, the content-type will be 'application/octet-stream', if not - 'application/zip'.
        /// <br/> - filename: If it is provided, this is sent back as 'Content-Disposition' header, forcing the browser to adopt this filename when downloading the file.
        /// <br/> - timeoffset: Desired time offset.
        /// <br/> 
        /// </remarks>
        public async Task<Stream?> GetPubZip(PubZipRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue(req.ForceDownload != null ? "application/octet-stream" : "application /zip"));

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

        /// <summary>
        /// Create a link to a zip archive file of a public link file.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>On success it will return array <see cref="StreamingResponse.hosts">hosts</see> with servers that have the file.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="PubZipRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: If it is set, the content-type will be 'application/octet-stream', if not - 'application/zip'.
        /// <br/> - filename: If it is provided, this is sent back as 'Content-Disposition' header, forcing the browser to adopt this filename when downloading the file.
        /// <br/> - timeoffset: Desired time offset.
        /// <br/> 
        /// </remarks>
        public async Task<Stream?> GetPubZipLink(PubZipRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue(req.ForceDownload != null ? "application/octet-stream" : "application /zip"));

            var query = new Dictionary<string, string>
            {
                {"code", req.Code}
            };
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.Filename != null) query.Add("filename", req.Filename);
            if (req.TimeOffset != null) query.Add("timeoffset", req.TimeOffset);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getpubziplink", query)));

            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Create a zip archive file of a public link file in the current user filesystem.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>If successful creates the zip archive and returns its <see cref="SingleFileResponse.metadata">metadata</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="SavePubZipRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - timeoffset: Desired time offset.
        /// <br/> - tofolderid: Folder id of the folder, where to save the zip archive.
        /// <br/> - toname: Filename of the desired zip archive.
        /// <br/> 
        /// </remarks>
        public async Task<SingleFileResponse?> SavePubZip(SavePubZipRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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
            var response = await _client.PostAsync(_baseUrl + "savepubzip", content);

            return await response.Content.ReadFromJsonAsync<SingleFileResponse>();
        }

        /// <summary>
        /// Returns variants array of different quality/resolution versions of a video, identified by fileid (or path). 
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Explained above.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="PubVideoLinkRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - fileid: Id of the file, if the public link is to a folder.
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> - maxspeed: Limit the download speed.
        /// <br/> - skipfilename: Include the name of the file in the generated link.
        /// <br/> 
        /// </remarks>
        public async Task<VideoStreamingResponse?> GetPubVideoLinks(PubVideoLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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

        /// <summary>
        /// Create a link to a audio file of a public link file. The link could be used for streaming.  
        /// </summary>
        /// <param name="req"></param>
        /// <returns>On success it will return array <see cref="StreamingResponse.hosts">hosts</see> with servers that have the file. The first server is the one we consider best for current download.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="PubAudioLinkRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - fileid: Id of the file, if the public link is to a folder.
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> - abitrate: Audio bit rate in kilobits, from 16 to 320.
        /// <br/> 
        /// </remarks>
        public async Task<StreamingResponse?> GetPubAudioLink(PubAudioLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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

        /// <summary>
        /// Download a file in different character encoding The file is streamed as response to this method by the content server.   
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Stream of the requested text file.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="PubAudioLinkRequest">Request</see> parameters:
        /// <br/> - code: Either 'code' or 'shortcode'.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - fileid: Id of the file, if the public link is to a folder.
        /// <br/> - fromencoding: The original character encoding of the file (default: guess).
        /// <br/> - toencoding: Requested character encoding of the output (default: utf-8).
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> 
        /// </remarks>
        public async Task<Stream?> GetPubTextFile(PubTextLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue(req.ForceDownload != null ? "application/octet-stream" : "text/plain"));

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

        /// <summary>
        /// Generates a public link to a collection, owned by the current user.    
        /// </summary>
        /// <param name="req"></param>
        /// <returns>On success returns <see cref="LinkResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="PubAudioLinkRequest">Request</see> parameters:
        /// <br/> - collectionid: The id of the collection.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - expire: Datetime when the link will stop working.
        /// <br/> - maxdownloads: int Maximum number of downloads from this folder (even for the same file).
        /// <br/> - maxtraffic: Maximum traffic that this link will consume (in bytes, started downloads will not be cut to fit in this limit).
        /// <br/> - shortlink: If set, a short link will also be generated.
        /// <br/> 
        /// </remarks>
        public async Task<LinkResponse?> GetCollectionPubLink(CollectionLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

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
    }
}
