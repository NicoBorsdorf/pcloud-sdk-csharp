using pcloud_sdk_csharp.Archiving.Controller;
using pcloud_sdk_csharp.File.Controller;
using pcloud_sdk_csharp.Folder.Controller;
using pcloud_sdk_csharp.Streaming.Controller;
using pcloud_sdk_csharp.Sharing.Controller;
using pcloud_sdk_csharp.Link.Controller;
using pcloud_sdk_csharp.Thumbnails.Controller;

namespace pcloud_sdk_csharp.Client
{
    public class PCloudClient
    {
        /// <summary>
        /// Create instance of PCloud client which connects to the API.
        /// The different endpoints can be used with the corresponding controller.
        /// </summary>
        /// <param name="token">Access token for API connection</param>
        /// <param name = "clientURL" > API URL depending on region, either api.plcoud.com or default eapi.plcoud.com in case if no URL is provided.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="token"/> is null.
        /// </exception>
        public PCloudClient(string token, Uri? clientURL = null)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            string baseURL = clientURL?.ToString() ?? @"https://eapi.pcloud.com/";

            Folders = new(token, baseURL);
            Files = new(token, baseURL);
            Streaming = new(token, baseURL);
            Archiving = new(token, baseURL);
            Sharing = new(token, baseURL);
            Links = new(token, baseURL);
            Thumbnails = new(token, baseURL);
        }

        public FolderController Folders { get; private set; }
        public FileController Files { get; private set; }
        public StreamingController Streaming { get; private set; }
        public ArchivingController Archiving { get; private set; }
        public SharingController Sharing { get; private set; }
        public LinkController Links { get; private set; }
        public ThumbnailController Thumbnails { get; private set; }
    }
}
