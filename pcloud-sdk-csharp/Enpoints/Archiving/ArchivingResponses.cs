using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Archiving.Responses
{
    public class ExtractArchiveResponse : Response
    {
        public string? progresshash { get; protected set; } = null!;
        public string? ip { get; protected set; } = null!;
        public string? hostname { get; protected set; } = null!;
        public string? ipv6 { get; protected set; } = null!;
        public int lines { get; protected set; }
        public string? ipbin { get; protected set; } = null!;
        public bool finished { get; protected set; }
        public List<string>? output { get; protected set; }
    }

    public class SaveZipProgoressResponse : Response
    {
        public int files { get; protected set; }
        public int totalfiles { get; protected set; }
        public int bytes { get; protected set; }
        public int totalbytes { get; protected set; }
    }
}
