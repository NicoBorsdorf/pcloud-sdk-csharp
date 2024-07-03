using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.General.Requests;
using pcloud_sdk_csharp.General.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.General.Controller
{
    public class GeneralController
    {
        /// <summary>
        /// Creates new instance of controller for general endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public GeneralController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.ToString().EndsWith("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly Uri _baseUrl;
        private readonly string _token;
        private readonly HttpClient _client = new();

        /// <summary>
        /// Returns a digest for digest authentication. Digests are valid for 30 seconds.
        /// </summary>
        /// <returns>Returns response containing <see cref="GetDigestResponse.digest">digest</see> and <see cref="GetDigestResponse.expires">expiration</see>.</returns>
        public async Task<GetDigestResponse?> GetDigest()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "getdigest");

            return JsonConvert.DeserializeObject<GetDigestResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Returns information about the current user. As there is no specific login method as credentials can be passed to any method, this is an especially good place for logging in with no particular action in mind.
        /// </summary>
        /// <returns>Returns <see cref="UserInfoResponse">info</see> of current user.</returns>
        public async Task<UserInfoResponse?> UserInfo()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "userinfo");

            return JsonConvert.DeserializeObject<UserInfoResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Lists supported languages in the returned languages hash, where keys are language codes and values are languages names.
        /// </summary>
        /// <returns>List of supported <see cref="LanguagesResponse.languages">languages</see>.</returns>
        public async Task<LanguagesResponse?> SupportedLanguages()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "supportedlanguages");

            return await response.Content.ReadFromJsonAsync<LanguagesResponse?>();
        }

        /// <summary>
        /// Sets user's language to language.
        /// </summary>
        /// <param name="lang">The language to be set.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="lang"/> is null or empty.</exception>
        public async Task<Response?> SetLanguage(string lang)
        {
            if (string.IsNullOrEmpty(lang)) throw new ArgumentNullException(nameof(lang));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.PostAsJsonAsync(_baseUrl + "setlanguage", $"{{\"language\": \"{lang}\"}}");

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        /// <summary>
        /// Sends message to pCloud support
        /// </summary>
        /// <param name="req">Request definition of /feedback endpoint.</param>
        /// <returns><see cref="Response">Base response</see>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        /// <remarks>       
        /// <see cref="FeedbackRequest">Request</see> parameters:  
        /// <br /> - mail: Email of the user.  
        /// <br /> - reason: Subject of the request.
        /// <br /> - message: The message itself.
        /// <br />
        /// Optional: 
        /// <br /> - name: String can be provided with users full name.
        /// </remarks>
        public async Task<Response?> Feedback(FeedbackRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"mail", req.Mail },
                {"reason", req.Reason },
                {"message", req.Message},
            };
            if (req.Name != null) reqBody.Add("name", req.Name);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsJsonAsync(_baseUrl + "feedback", content);

            return await response.Content.ReadFromJsonAsync<Response?>();
        }

        /// <summary>
        /// Gets ip and hostname of current server. The hostname is guaranteed to resolve only to the IP address(es) pointing to the same server. This call is useful when you need to track the upload progress.
        /// </summary>
        /// <returns>Returns <see cref="CurrentServerResponse.ip">ip</see> and <see cref="CurrentServerResponse.hostname">hostname</see> of the server you are currently connected to.</returns>
        public async Task<CurrentServerResponse?> CurrentServer()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "currentserver");

            return await response.Content.ReadFromJsonAsync<CurrentServerResponse?>();
        }

        /// <summary>
        /// List updates of the user's folders/files. 
        /// </summary>
        /// <param name="req">Request definition of /diff endpoint.</param>
        /// <returns>On success in the reply there will be entries array of objects and <see cref="DiffResponse.diffid">diffid</see>. Set your current <see cref="DiffResponse.diffid">diffid</see> to the provided <see cref="DiffResponse.diffid">diffid</see> after you process all events, during processing set your state to the <see cref="DiffResponse.diffid">diffid</see> of the event preferably in a single transaction with the event itself. </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="req"/> is null.</exception>
        public async Task<DiffResponse?> Diff(DiffRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>();
            if (req.DiffId != null) query.Add("diffid", req.DiffId.ToString()!);
            if (req.After != null) query.Add("after", req.After.Value.ToString("R"));
            if (req.Last != null) query.Add("last", req.Last.ToString()!);
            if (req.Block != null) query.Add("block", req.Block.ToString()!);
            if (req.Limit != null) query.Add("limit", req.Limit.ToString()!);

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "diff", query));

            return JsonConvert.DeserializeObject<DiffResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Returns event history of a file identified by fileid. File might be a deleted one. The output format is the same as of diff method.
        /// </summary>
        /// <param name="fileId">Fileid of a file that history is requested for.</param>
        /// <returns>Returns event history of a file identified by fileid.</returns>
        public async Task<FileHistoryResponse?> GetFileHistory(long fileId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"fileid", fileId.ToString() }
            };
            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "getfilehistory", query));

            return JsonConvert.DeserializeObject<FileHistoryResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }

        /// <summary>
        /// Get the IP address of the remote device from which the user connects to the API.
        /// </summary>
        /// <returns>Returns <see cref="GetIpResponse.ip">ip</see> - the remote address of the user that is connecting to the API. Also, returns <see cref="GetIpResponse.country">country</see> - lowercase two-letter code of the country that is defined according to the remote address. If the country could not be defined, then this fields is false.</returns>
        public async Task<GetIpResponse?> GetIp()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "getip");

            return await response.Content.ReadFromJsonAsync<GetIpResponse?>();
        }

        /// <summary>
        /// This method returns closest API server to the requesting client. The biggest speed gain will be with upload methods. Clients should have fallback logic. If request to API server different from api.pcloud.com fails (network error) the client should fallback to using api.pcloud.com.
        /// </summary>
        /// <returns>
        /// <see cref="GetApiServerResponse.binapi">binapi</see> - array with API servers that support connections via pCloud's binary protocol <br />
        /// <see cref="GetApiServerResponse.api">api</see> - array with API servers that support connections via HTTP/HTTPS protocol
        /// </returns>
        public async Task<GetApiServerResponse?> GetApiServer()
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(_baseUrl + "getapiserver");

            return await response.Content.ReadFromJsonAsync<GetApiServerResponse?>();
        }
    }
}
