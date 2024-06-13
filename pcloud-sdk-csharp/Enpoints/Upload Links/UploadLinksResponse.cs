using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.UploadLinks.Responses
{
    public class UploadLinksResponse : Response
    {
        public long uploadlinkid { get; protected set; }
        public string link { get; protected set; } = string.Empty;
        public string mail { get; protected set; } = string.Empty;
        public string code { get; protected set; } = string.Empty;
    }

    public class ListUploadLinksResponse : Response
    {
        public List<UploadLinks> uploadlinks { get; protected set; } = null!;

        public class UploadLinks : UploadLinksResponse
        {
            public long space { get; protected set; }
            public long? maxspace { get; protected set; }
            public DateTime created { get; protected set; }
            public DateTime modified { get; protected set; }
            public long files { get; protected set; }
            public long? maxfiles { get; protected set; }
            public string comment { get; protected set; } = string.Empty;
            public Metadata metadata { get; protected set; } = null!;
            public DateTime? expire { get; protected set; }
        }
    }
}
