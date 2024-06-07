using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Auth.Responses
{
    // Auth
    public class AuthResponse : Response
    {
        public string? code { get; protected set; }
        public string? access_token { get; protected set; }
        public string? token_type { get; protected set; }
        public string? state { get; protected set; }
        public int? locationid { get; protected set; }
        public int? uid { get; protected set; }
        public string? hostname { get; protected set; }
    }
}
