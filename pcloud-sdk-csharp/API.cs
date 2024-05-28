using pcloud_sdk_csharp.Archiving.Controller;
using pcloud_sdk_csharp.File.Controller;
using pcloud_sdk_csharp.Folder.Controller;
using pcloud_sdk_csharp.Streaming.Controller;
using pcloud_sdk_csharp.Sharing.Controller;

namespace pcloud_sdk_csharp.Client
{
    public class PCloudClient
    {
        public PCloudClient(string token, Uri? clientURL = null)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            string baseURL = clientURL?.ToString() ?? new Uri("https://eapi.pcloud.com/").ToString();

            Folders = new(token, baseURL);
            Files = new(token, baseURL);
            Streaming = new(token, baseURL);
            Archiving = new(token, baseURL);
            Sharing = new(token, baseURL);
        }

        public PCloudClient(string token, string? clientURL = null)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            string baseURL = clientURL ?? new Uri("https://eapi.pcloud.com/").ToString();

            Folders = new(token, baseURL);
            Files = new(token, baseURL);
            Streaming = new(token, baseURL);
            Archiving = new(token, baseURL);
            Sharing = new(token, baseURL);
        }

        public FolderController Folders { get; private set; }
        public FileController Files { get; private set; }
        public StreamingController Streaming { get; private set; }
        public ArchivingController Archiving { get; private set; }
        public SharingController Sharing { get; private set; }
    }
}
