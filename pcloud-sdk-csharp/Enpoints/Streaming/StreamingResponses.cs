using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Streaming.Responses
{
    // Streaming 
    public class StreamingResponse : Response
    {
        public string? path { get; set; }
        public DateTime? expires { get; set; }
        public List<string>? hosts { get; set; }
    }

    public class VideoStreamingResponse : Response
    {
        public List<Metadata>? variants { get; set; }
    }
}
