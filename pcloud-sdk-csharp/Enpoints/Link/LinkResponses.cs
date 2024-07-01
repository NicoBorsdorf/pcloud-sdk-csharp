using pcloud_sdk_csharp.Base.Responses;
using System.Text.Json.Serialization;

namespace pcloud_sdk_csharp.Link.Responses
{
    public class ShortLinkResponse : Response
    {
        public long? linkid { get; set; }
        public string? shortcode { get; set; }
        public string? shortlink { get; set; }
    }

    public class LinkResponse : Response
    {
        public long? linkid { get; set; }
        public string? link { get; set; }
        public string? code { get; set; }
    }

    public class PubLinksResponse : Response
    {
        public List<PubLinkMeta>? publinks { get; set; }

        // only used internally
        public class PubLinkMeta
        {
            public long linkid { get; set; }
            public string code { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            [JsonConverter(typeof(CustomDateTimeConverter))]
            public DateTime created { get; set; }
            [JsonConverter(typeof(CustomDateTimeConverter))]
            public DateTime modified { get; set; }
            public Metadata? metadata { get; set; }
            public bool? isfolder { get; set; }
            public long? folderid { get; set; }
            public long? fileid { get; set; }
            public int? downloads { get; set; }
            public int traffic { get; set; }

            public string? shortcode { get; set; }
            public string? shortlink { get; set; }

            [JsonConverter(typeof(CustomDateTimeConverter))]
            public DateTime expires { get; set; }
            public int? maxdownloads { get; set; }
            public int? maxtraffic { get; set; }
        }
    }
}
