using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Streaming.Responses
{
    // Streaming 
    public class StreamingResponse : Response
    {
        public string path { get; protected set; } = null!;
        public DateTime expires { get; protected set; }
        public List<string>? hosts { get; protected set; }
    }

    public class VideoStreamingResponse : Response
    {
        public List<Metadata>? variants { get; protected set; }

    }
}
