using pcloud_sdk_csharp.Archiving.Controller;
using pcloud_sdk_csharp.File.Controller;
using pcloud_sdk_csharp.Folder.Controller;
using pcloud_sdk_csharp.Streaming.Controller;
using pcloud_sdk_csharp.Sharing.Controller;
using pcloud_sdk_csharp.Link.Controller;
using pcloud_sdk_csharp.Thumbnails.Controller;
using pcloud_sdk_csharp.UploadLinks.Controller;
using pcloud_sdk_csharp.Revisions.Controller;
using pcloud_sdk_csharp.Trash.Controller;
using pcloud_sdk_csharp.Collection.Controller;

namespace pcloud_sdk_csharp.Client
{
    public class PCloudClient
    {
        /// <summary>
        /// Create instance of PCloud client which connects to the API. <br/>
        /// The different endpoints can be used with the corresponding controller.<br/>
        /// Only Auth required controllers are used in the Client. Others can be initiated outside of is.
        /// </summary>
        /// <param name="token">Access token for API connection</param>
        /// <param name = "clientURL" > API URL depending on region, either https://api.plcoud.com/ or default https://eapi.plcoud.com/ in case if no URL is provided. / at the end of the URL is required.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="token"/> is null.
        /// </exception>
        public PCloudClient(string token, Uri? clientURL = null)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (clientURL != null && !clientURL.AbsoluteUri.Last().Equals("/")) throw new Exception("Please append a / to the clientURL. E.g. https://eapi.pcloud.com/");
            if (clientURL == null) clientURL = new Uri("https://eapi.pcloud.com/");

            Folders = new(token, clientURL);
            Files = new(token, clientURL);
            Streaming = new(token, clientURL);
            Archiving = new(token, clientURL);
            Sharing = new(token, clientURL);
            Links = new(token, clientURL);
            Thumbnails = new(token, clientURL);
            UploadLinks = new(token, clientURL);
            Revisions = new(token, clientURL);
            Trash = new(token, clientURL);
            Collection = new(token, clientURL);
        }


        public FolderController Folders { get; private set; }
        public FileController Files { get; private set; }
        public StreamingController Streaming { get; private set; }
        public ArchivingController Archiving { get; private set; }
        public SharingController Sharing { get; private set; }
        public LinkController Links { get; private set; }
        public ThumbnailController Thumbnails { get; private set; }
        public UploadLinksController UploadLinks { get; private set; }
        public RevisionsController Revisions { get; private set; }
        public TrashController Trash { get; private set; }
        public CollectionController Collection { get; private set; }
    }
}
