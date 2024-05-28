namespace pcloud_sdk_csharp.Auth.Requests
{
    // Auth
    public class AuthorizeRequest
    {
        public AuthorizeRequest(string clientId, ResponseType responseType, string? redirectUri = null, string? state = null, bool? forceApprove = false)
        {
            if (responseType == ResponseType.token && redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            ClientId = clientId;
            Type = responseType;
            RedirectUri = redirectUri;
            State = state;
            ForceApprove = forceApprove ?? false;
        }

        public string ClientId { get; private set; }
        public ResponseType Type { get; private set; }
        public string? RedirectUri { get; private set; }
        public string? State { get; private set; }
        public bool ForceApprove { get; private set; }

        public enum ResponseType
        {
            code = 0,
            token = 1
        }
    }
}
