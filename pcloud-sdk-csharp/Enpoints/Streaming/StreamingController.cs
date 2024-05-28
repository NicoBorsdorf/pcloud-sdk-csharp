using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Streaming.Requests;
using pcloud_sdk_csharp.Streaming.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Streaming.Controller
{
    public class StreamingController
    {
        public StreamingController(string token, string clientURL)
        {
            _baseUrl = clientURL;
            _token = token;
        }

        private readonly string _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        public async Task<StreamingResponse?> GetFileLink(StreamingRequest req)
        {
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

        public async Task<StreamingResponse?> GetVideoLink(VideoStreamingRequest req)
        {
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

        public async Task<VideoStreamingResponse?> GetVideoLinks(StreamingRequest req)
        {
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

        public async Task<StreamingResponse?> GetAudioLink(AudioStreamingRequest req)
        {
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

        public async Task<StreamingResponse?> GetHLSLink(HSLLinkRequest req)
        {
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

        public async Task<Stream> GetTextFile(TextFileRequest req)
        {
            var headers = _client.DefaultRequestHeaders;
            headers.Clear();
            headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            // content type changes depending on forced download
            if (req.ForceDownload != null) headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

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
