using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.Streaming.Responses;

namespace pcloud_sdk_csharp.Thumbnails.Responses
{
    public class SaveThumbnailResponse : Response
    {
        public long? height { get; set; }
        public long? width { get; set; }
        public Metadata? metadata { get; set; }
    }

    public class GetThumbResponse : StreamingResponse
    {
        public string? size { get; set; }
    }

    public class GetThumbLinksResponse : Response
    {
        public List<Thumbs>? thumbs { get; set; }

        public class Thumbs : Response
        {
            public string? size { get; set; }
            public string? path { get; set; }
            public long? fileid { get; set; }
            public List<string>? hosts { get; set; }
        }
    }

}
