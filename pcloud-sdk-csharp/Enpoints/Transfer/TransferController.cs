using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace pcloud_sdk_csharp.Transfer.Controller
{
    public class TransferController
    {
        /// <summary>
        /// Creates new instance of collection for trash endpoint.
        /// </summary>
        /// <param name="access_token">Access Token passed from the <see cref="Client.PCloudClient"/></param>
        /// <param name="clientURL">API URL passed from the <see cref="Client.PCloudClient"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="access_token"/> or <paramref name="clientURL"/> is null.
        /// </exception>
        public TransferController(Uri clientURL)
        {
            _baseUrl = clientURL ?? throw new ArgumentNullException(nameof(clientURL));

            if (!_baseUrl.ToString().EndsWith("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
        }

        private readonly HttpClient _client = new();
        private readonly Uri _baseUrl;

        /// <summary>
        /// Does a file(s) transfer in way that creates and sends transfer links to receiver emails.
        /// </summary>
        /// <param name="senderMail">Mail of the sender.</param>
        /// <param name="receiverMail">Mail(s) of the receivers(up to 20) separated by ",".</param>
        /// <param name="message">Short message(up to 160 characters) acting as a comment to the receivers. Will be cut automatically after 160 characters.</param>
        /// <param name="progressHash">Hash used for observing transfer progress</param>
        /// <exception cref="ArgumentNullException">When <paramref name="senderMail"/> or <paramref name="receiverMails"/> are null.</exception>
        /// <returns><see cref="Response">Base response</see></returns>
        public async Task<Response?> UploadTransfer(string senderMail, List<string> receiverMails, string? message, string? progressHash)
        {
            if (senderMail == null) throw new ArgumentNullException(nameof(senderMail));
            if (receiverMails == null) throw new ArgumentNullException(nameof(receiverMails));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var reqBody = new Dictionary<string, string>
            {
                {"sendermail", senderMail },
                {"receivermails", string.Join(",", receiverMails) }
            };
            if (message != null) reqBody.Add("message", message);
            if (progressHash != null) reqBody.Add("progresshash", progressHash);

            var content = new FormUrlEncodedContent(reqBody);
            var response = await _client.PostAsync(_baseUrl + "uploadtransfer", content);

            return await response.Content.ReadFromJsonAsync<Response>();
        }

        /// <summary>
        /// Monitor the progress of transfered files.
        /// </summary>
        /// <param name="progressHash">Hash defining the transfer, same as sent to <see cref="UploadTransfer(string, List{string}, string?, string?)">uploadtransfer</see>.</param>
        /// <returns>Returns same data as <see cref="File.Controller.FileController.UploadProgress(string)">uploadprogress</see>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<UploadProgressResponse?> UploadTransferProgress(string progressHash)
        {
            if (progressHash == null) throw new ArgumentNullException(nameof(progressHash));

            var header = _client.DefaultRequestHeaders;
            header.Clear();
            header.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>
            {
                {"progresshash", progressHash },
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(_baseUrl + "uploadtransferprogress", query));

            return JsonConvert.DeserializeObject<UploadProgressResponse?>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { Converters = { new CustomDateTimeConverter() } });
        }
    }
}
