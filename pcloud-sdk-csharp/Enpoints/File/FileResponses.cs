using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.File.Responses
{
    // Files
    public class UploadedFile : Response
    {
        public List<long> fileids { get; protected set; } = null!;
        public List<Metadata> metadata { get; protected set; } = null!;
    }

    public class UploadProgress : Response
    {
        public long total { get; protected set; }
        public long uploaded { get; protected set; }
        public string currentfile { get; protected set; } = null!;
        public List<Metadata> files { get; protected set; } = null!;
        public bool finished { get; protected set; }
    }

    public class SingleFileResponse : Response
    {
        public Metadata metadata { get; protected set; } = null!;
    }

    public class DeleteFileResponse : SingleFileResponse
    {
        public string id { get; protected set; } = null!;
    }
}
