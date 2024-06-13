using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Revisions.Responses
{
    public class RevisionReponse : Response
    {
        public List<RevisionMetadata> revisions { get; protected set; } = null!;

        public class RevisionMetadata
        {
            public long revisionsid { get; protected set; }
            public long size { get; protected set; }
            public string hash { get; protected set; } = string.Empty;
            public DateTime created { get; protected set; }
            public Metadata metadata { get; protected set; } = null!;
        }
    }
}
