using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.Streaming.Responses;

namespace pcloud_sdk_csharp.Thumbnails.Responses
{
    public class SaveThumbnailResponse : Response
    {
        public long? height { get; protected set; }
        public long? width { get; protected set; }
        public Metadata? metadata { get; protected set; }
    }

    public class GetThumbResponse : StreamingResponse
    {
        public string? size { get; protected set; }
    }

    public class GetThumbLinksResponse : Response
    {
        public List<Thumbs>? thumbs { get; protected set; }

        public class Thumbs : Response
        {
            public string? size { get; protected set; }
            public string? path { get; protected set; }
            public long? fileid { get; protected set; }
            public List<string>? hosts { get; protected set; }
        }
    }

}
