using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.File.Responses
{
    // Files
    public class UploadedFileResponse : Response
    {
        public List<long>? fileids { get; protected set; }
        public List<Metadata>? metadata { get; protected set; }
    }

    public class UploadProgressResponse : Response
    {
        public long? total { get; protected set; }
        public long? uploaded { get; protected set; }
        public string? currentfile { get; protected set; }
        public List<Metadata>? files { get; protected set; }
        public bool? finished { get; protected set; }
    }

    public class SingleFileResponse : Response
    {
        public Metadata? metadata { get; protected set; }
    }

    public class DeleteFileResponse : SingleFileResponse
    {
        public string? id { get; protected set; }
    }
}
