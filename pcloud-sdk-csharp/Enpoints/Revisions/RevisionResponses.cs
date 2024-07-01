using pcloud_sdk_csharp.Base.Responses;
using System.Text.Json.Serialization;

namespace pcloud_sdk_csharp.Revisions.Responses
{
    public class RevisionReponse : Response
    {
        public List<RevisionMetadata> revisions { get; set; } = null!;

        public class RevisionMetadata
        {
            public long revisionsid { get; set; }
            public long size { get; set; }
            public string hash { get; set; } = string.Empty;
            [JsonConverter(typeof(CustomDateTimeConverter))]
            public DateTime created { get; set; }
            public Metadata metadata { get; set; } = null!;
        }
    }
}
