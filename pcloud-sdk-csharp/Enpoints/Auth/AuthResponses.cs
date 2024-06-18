using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Auth.Responses
{
    // Auth
    public class AuthResponse : Response
    {
        public string? code { get; set; }
        public string? access_token { get; set; }
        public string? token_type { get; set; }
        public string? state { get; set; }
        public int? locationid { get; set; }
        public int? uid { get; set; }
        public string? hostname { get; set; }
    }
}
