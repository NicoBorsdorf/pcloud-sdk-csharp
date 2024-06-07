using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Link.Responses
{
    public class ShortLinkResponse : Response
    {
        public long? linkid { get; protected set; }
        public string? shortcode { get; protected set; }
        public string? shortlink { get; protected set; }
    }

    public class LinkResponse : Response
    {
        public long? linkid { get; protected set; }
        public string? link { get; protected set; }
        public string? code { get; protected set; }
    }

    public class PubLinksResponse : Response
    {
        public List<PubLinkMeta>? publinks { get; protected set; }

        // only used internally
        public class PubLinkMeta
        {
            public long linkid { get; protected set; }
            public string code { get; protected set; } = string.Empty;
            public string link { get; protected set; } = string.Empty;
            public DateTime created { get; protected set; }
            public DateTime modified { get; protected set; }
            public Metadata? metadata { get; protected set; }
            public bool? isfolder { get; protected set; }
            public long? folderid { get; protected set; }
            public long? fileid { get; protected set; }
            public int? downloads { get; protected set; }
            public int traffic { get; protected set; }

            public string? shortcode { get; protected set; }
            public string? shortlink { get; protected set; }

            public DateTime expires { get; protected set; }
            public int? maxdownloads { get; protected set; }
            public int? maxtraffic { get; protected set; }
        }
    }
}
