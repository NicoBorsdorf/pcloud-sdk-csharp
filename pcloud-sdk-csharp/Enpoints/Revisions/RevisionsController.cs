using Microsoft.AspNetCore.WebUtilities;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Responses;
using pcloud_sdk_csharp.Revisions.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Revisions.Controller
{
    public class RevisionsController
    {
        /// <summary>
        /// Creates new instance of controller for revisions endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public RevisionsController(string access_token, Uri clientURL)
        {
            _token = access_token ?? throw new ArgumentNullException(nameof(access_token));
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.AbsoluteUri.Last().Equals("/")) throw new ArgumentException(@"Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly HttpClient _client = new();
        private readonly string _token;
        private readonly Uri _baseUrl;

        /// <summary>
        /// Lists revisions for a given  <paramref name="fileId">fileId</paramref>.
        /// </summary>
        /// <param name="fileId">Id of the revisioned file.</param>
        /// <returns>Lists the <see cref="RevisionReponse.revisions">revisions</see> as array.</returns>
        public async Task<RevisionReponse?> ListRevisions(long fileId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"fileid", fileId.ToString() },
            };
            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "createuploadlink", query));

            return await response.Content.ReadFromJsonAsync<RevisionReponse>();
        }

        /// <summary>
        /// Takes <paramref name="fileId">fileid</paramref> and <paramref name="revisionId">revisionid</paramref> as parameters and reverts the file to a given revision. 
        /// </summary>
        /// <param name="fileId">Id of the revisioned file.</param>
        /// <param name="revisionId">Id of the revistion, to which the file is reverted.</param>
        /// <returns>On success returns new metadata of the file.</returns>
        public async Task<SingleFileResponse?> RevertRevision(long fileId, long revisionId)
        {
            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"fileid", fileId.ToString() },
                {"revisionid", revisionId.ToString() },
            };
            var response = await _client.PostAsJsonAsync(_baseUrl + "createuploadlink", $"{{\"fileid\": {fileId}, \"revisionid\":{revisionId}}}");

            return await response.Content.ReadFromJsonAsync<SingleFileResponse>();
        }
    }
}
