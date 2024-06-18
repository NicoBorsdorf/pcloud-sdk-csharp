using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Archiving.Responses
{
    public class ExtractArchiveResponse : Response
    {
        public string? progresshash { get; set; }
        public string? ip { get; set; }
        public string? hostname { get; set; }
        public string? ipv6 { get; set; }
        public int? lines { get; set; }
        public string? ipbin { get; set; }
        public bool? finished { get; set; }
        public List<string>? output { get; set; }
    }

    public class SaveZipProgoressResponse : Response
    {
        public int? files { get; set; }
        public int? totalfiles { get; set; }
        public int? bytes { get; set; }
        public int? totalbytes { get; set; }
    }
}
