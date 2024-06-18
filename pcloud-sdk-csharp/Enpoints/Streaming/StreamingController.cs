using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Streaming.Requests;
using pcloud_sdk_csharp.Streaming.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Streaming.Controller
{
    public class StreamingController
    {

        /// <summary>
        /// Creates new instance of controller for streaming endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public StreamingController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.ToString().EndsWith("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly Uri _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        /// <summary>
        /// Get a download link for file Takes fileid (or path) as parameter and provides links from which the file can be downloaded. 
        /// </summary>
        /// <param name="req">Request definition for /getfilelink endpoint.</param>
        /// <returns>On success it will return array <see cref="StreamingResponse.hosts">hosts</see> with servers that have the file. The first server is the one we consider best for current download. </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="StreamingRequest">Request</see> parameters:
        /// <br/> - fileid: ID of the renamed file.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> - maxspeed: Limit the download speed.
        /// <br/> - skipfilename: Include the name of the file in the generated link.
        /// <br/> 
        /// </remarks>
        public async Task<StreamingResponse?> GetFileLink(StreamingRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
            };
            if (req.ContentType != null) query.Add("contenttype", req.ContentType);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.MaxSpeed != null) query.Add("maxspeed", req.MaxSpeed.ToString()!);
            if (req.SkipFilename != null) query.Add("skipfilename", req.SkipFilename.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getfilelink", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }

        /// <summary>
        /// Get a streaming link for video file Takes fileid  of a video file and provides links (same way getfilelink does with hosts and path) from which the video can be streamed with lower bitrate (and/or resolution).  
        /// </summary>
        /// <param name="req">Request definition for /getvideolink endpoint.</param>
        /// <returns>On success it will return array <see cref="StreamingResponse.hosts">hosts</see> with servers that have the file. The first server is the one we consider best for current download. </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="StreamingRequest">Request</see> parameters:
        /// <br/> - fileid: ID of the renamed file.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> - maxspeed: Limit the download speed.
        /// <br/> - skipfilename: Include the name of the file in the generated link.
        /// <br/> - abitrate: Audio bit rate in kilobits, from 16 to 320.
        /// <br/> - vbitrate: Video bitrate in kilobits, from 16 to 4000.
        /// <br/> - resolution: In pixels, from 64x64 to 1280x960, WIDTHxHEIGHT.
        /// <br/> - fixedbitrate: If set, turns off adaptive streaming and the stream will be with a constant bitrate.
        /// <br/> 
        /// </remarks>
        public async Task<StreamingResponse?> GetVideoLink(VideoStreamingRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
            };
            if (req.ContentType != null) query.Add("contenttype", req.ContentType);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.MaxSpeed != null) query.Add("maxspeed", req.MaxSpeed.ToString()!);
            if (req.SkipFilename != null) query.Add("skipfilename", req.SkipFilename.ToString()!);
            if (req.ABitrate != null) query.Add("skipfilename", req.ABitrate.ToString()!);
            if (req.VBitrate != null) query.Add("skipfilename", req.VBitrate.ToString()!);
            if (req.Resolution != null) query.Add("skipfilename", req.Resolution);
            if (req.FixedBitrate != null) query.Add("skipfilename", req.FixedBitrate.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getvideolink", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }

        /// <summary>
        /// Get a streaming links for videos.   
        /// </summary>
        /// <param name="req">Request definition for /getvideolink endpoint.</param>
        /// <returns>Returns variants array of different quality/resolution versions of a video.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="StreamingRequest">Request</see> parameters:
        /// <br/> - fileid: ID of the renamed file.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> - maxspeed: Limit the download speed.
        /// <br/> - skipfilename: Include the name of the file in the generated link.
        /// <br/> - abitrate: Audio bit rate in kilobits, from 16 to 320.
        /// <br/> - vbitrate: Video bitrate in kilobits, from 16 to 4000.
        /// <br/> - resolution: In pixels, from 64x64 to 1280x960, WIDTHxHEIGHT.
        /// <br/> - fixedbitrate: If set, turns off adaptive streaming and the stream will be with a constant bitrate.
        /// <br/> 
        /// </remarks>
        public async Task<VideoStreamingResponse?> GetVideoLinks(StreamingRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
            };
            if (req.ContentType != null) query.Add("contenttype", req.ContentType);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.MaxSpeed != null) query.Add("maxspeed", req.MaxSpeed.ToString()!);
            if (req.SkipFilename != null) query.Add("skipfilename", req.SkipFilename.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getvideolinks", query)));

            return await response.Content.ReadFromJsonAsync<VideoStreamingResponse?>();
        }

        /// <summary>
        /// Get a streaming link for audio file Takes fileid of an audio (or video) file and provides links from which audio can be streamed in mp3 format. (Same way getfilelink does with hosts and path).
        /// </summary>
        /// <param name="req">Request definition for /getaudiolink endpoint.</param>
        /// <returns>On success it will return array <see cref="StreamingResponse.hosts">hosts</see> with servers that have the file. The first server is the one we consider best for current download.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="AudioStreamingRequest">Request</see> parameters:
        /// <br/> - fileid: ID of the renamed file.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> - abitrate: Audio bit rate in kilobits, from 16 to 320.
        /// <br/> 
        /// </remarks>
        public async Task<StreamingResponse?> GetAudioLink(AudioStreamingRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
            };
            if (req.ContentType != null) query.Add("contenttype", req.ContentType);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.ABitrate != null) query.Add("skipfilename", req.ABitrate.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "getaudiolink", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }

        /// <summary>
        /// Get a m3u8 playlist for live streaming for video file.
        /// </summary>
        /// <param name="req">Request definition for /gethlslink endpoint.</param>
        /// <returns>On success it will return array <see cref="StreamingResponse.hosts">hosts</see> with servers that have the file. The first server is the one we consider best for current download.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="HSLLinkRequest">Request</see> parameters:
        /// <br/> - fileid: ID of the renamed file.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - abitrate: Audio bit rate in kilobits, from 16 to 320.
        /// <br/> - vbitrate: Video bitrate in kilobits, from 16 to 4000.
        /// <br/> - resolution: In pixels, from 64x64 to 1280x960, WIDTHxHEIGHT.
        /// <br/> - skipfilename: Include the name of the file in the generated link.
        /// <br/> 
        /// </remarks>
        public async Task<StreamingResponse?> GetHLSLink(HSLLinkRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
            };
            if (req.Resolution != null) query.Add("contenttype", req.Resolution);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.ABitrate != null) query.Add("maxspeed", req.ABitrate.ToString()!);
            if (req.VBitrate != null) query.Add("skipfilename", req.VBitrate.ToString()!);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "gethlslink", query)));

            return await response.Content.ReadFromJsonAsync<StreamingResponse?>();
        }

        /// <summary>
        /// Download a file in different character encoding Takes fileid as parameter and returns contents of the file in different character encoding. The file is streamed as response to this method by the content server.
        /// </summary>
        /// <param name="req">Request definition for /gettextfile endpoint.</param>
        /// <returns>On success this method outputs the data by the API server. No links to content servers are provided. Unless you provide invalid encodings in fromecoding or toencoding you can safely assume that this method will not fail.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>
        /// <br/> <see cref="TextFileRequest">Request</see> parameters:
        /// <br/> - fileid: ID of the renamed file.
        /// <br/>
        /// <br/> Optional: 
        /// <br/> - fromencoding: The original character encoding of the file (default: guess).
        /// <br/> - toencoding: Requested character encoding of the output (default: utf-8).
        /// <br/> - forcedownload: Download with Content-Type = application/octet-stream.
        /// <br/> - contenttype: Set Content-Type.
        /// <br/> 
        /// </remarks>
        public async Task<Stream> GetTextFile(TextFileRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            // content type changes depending on forced download
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue(req.ForceDownload != null ? "application/octet-stream" : "text/plain"));

            var query = new Dictionary<string, string>
            {
                { "fileid", req.FileId.ToString() },
            };
            if (req.ContentType != null) query.Add("contenttype", req.ContentType);
            if (req.ForceDownload != null) query.Add("forcedownload", req.ForceDownload.ToString()!);
            if (req.FromEncoding != null) query.Add("maxspeed", req.FromEncoding);
            if (req.ToEncoding != null) query.Add("skipfilename", req.ToEncoding);

            var response = await _client.GetAsync(new Uri(QueryHelpers.AddQueryString(_baseUrl + "gettextfile", query)));

            return await response.Content.ReadAsStreamAsync();
        }

    }
}
