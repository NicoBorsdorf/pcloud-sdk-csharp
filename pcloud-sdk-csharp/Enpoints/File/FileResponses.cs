using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.File.Responses
{
    // Files
    public class UploadedFileResponse : Response
    {
        public List<long>? fileids { get; set; }
        public List<Metadata>? metadata { get; set; }
    }

    public class UploadProgressResponse : Response
    {
        public long? total { get; set; }
        public long? uploaded { get; set; }
        public string? currentfile { get; set; }
        public List<Metadata>? files { get; set; }
        public bool? finished { get; set; }
    }

    public class SingleFileResponse : Response
    {
        public Metadata? metadata { get; set; }
    }

    public class DeleteFileResponse : SingleFileResponse
    {
        public string? id { get; set; }
    }
}
