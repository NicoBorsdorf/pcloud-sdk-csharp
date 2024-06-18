using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.UploadLinks.Responses
{
    public class UploadLinksResponse : Response
    {
        public long uploadlinkid { get; set; }
        public string link { get; set; } = string.Empty;
        public string mail { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
    }

    public class ListUploadLinksResponse : Response
    {
        public List<UploadLinks> uploadlinks { get; set; } = null!;

        public class UploadLinks : UploadLinksResponse
        {
            public long space { get; set; }
            public long? maxspace { get; set; }
            public DateTime created { get; set; }
            public DateTime modified { get; set; }
            public long files { get; set; }
            public long? maxfiles { get; set; }
            public string comment { get; set; } = string.Empty;
            public Metadata metadata { get; set; } = null!;
            public DateTime? expire { get; set; }
        }
    }
}
