
using pcloud_sdk_csharp.Streaming.Requests;
using pcloud_sdk_csharp.Thumbnails.Requests;

namespace pcloud_sdk_csharp.Link.Requests
{
    public class LinkRequest
    {
        public LinkRequest(long fileId, DateTime? expire = null, int? maxDownloads = null, int? maxTraffic = null, string? linkPassword = null, bool shortLink = false)
        {
            FileId = fileId;
            Expire = expire;
            MaxDownloads = maxDownloads;
            MaxTraffic = maxTraffic;
            LinkPassword = linkPassword;
            ShortLink = shortLink ? 1 : null;
        }

        public long FileId { get; private set; }
        public DateTime? Expire { get; private set; }
        public int? MaxDownloads { get; private set; }
        public int? MaxTraffic { get; private set; }
        public string? LinkPassword { get; private set; }
        public int? ShortLink { get; private set; }
    }

    public class CopyLinkRequest
    {
        public CopyLinkRequest(long fileId, string code, long toFolderId, string? toName, bool noOver = false)
        {
            FileId = fileId;
            Code = code;
            ToFolderId = toFolderId;
            ToName = toName;
            NoOver = noOver ? 1 : null;
        }

        public long FileId { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public long ToFolderId { get; private set; }
        public string? ToName { get; private set; }
        public int? NoOver { get; private set; }
    }

    public class ChangeLinkRequest
    {
        public ChangeLinkRequest(long linkId, DateTime? expire, DateTime? deleteExpire, int? maxTraffic, int? maxDownloads, string? linkPasswort, bool shortLink = false, bool deleteShortLink = false,)
        {
            LinkId = linkId;
            ShortLink = shortLink ? 1 : null;
            DeleteShortLink = deleteShortLink ? 1 : null;
            Expire = expire;
            DeleteExpire = deleteExpire;
            MaxTraffic = maxTraffic;
            MaxDownloads = maxDownloads;
            LinkPasswort = linkPasswort;
        }

        public long LinkId { get; private set; }
        public int? ShortLink { get; private set; }
        public int? DeleteShortLink { get; private set; }
        public DateTime? Expire { get; private set; }
        public DateTime? DeleteExpire { get; private set; }
        public int? MaxTraffic { get; private set; }
        public int? MaxDownloads { get; private set; }
        public string? LinkPasswort { get; private set; }
    }

    public class ThumbLinkRequest : ThumbRequest
    {
        public ThumbLinkRequest(string code, long fileId, string size, bool crop = false, string? type = null) : base(fileId, size, crop, type)
        {
            Code = code;
        }

        public string Code { get; private set; } = string.Empty;
    }

    public class SavePubThumbRequest : SaveThumbRequest
    {
        public SavePubThumbRequest(string code, long toFolderId, string toName, long fileId, string size, bool crop = false, string? type = null, bool noOver = false) : base(toFolderId, toName, fileId, size, crop, type, noOver)
        {
            Code = code;
        }

        public string Code { get; private set; } = string.Empty;
    }

    public class PubZipRequest
    {
        public PubZipRequest(string code, bool forceDownload = false, string? filename = null, string? timeOffset = null)
        {
            if (filename != null && !filename.Contains(".zip")) throw new Exception("Filename must contain .zip file extension if set to not null");
            Code = code;
            ForceDownload = forceDownload ? 1 : 0;
            Filename = filename;
            TimeOffset = timeOffset;
        }

        public string Code { get; private set; } = string.Empty;
        public int? ForceDownload { get; private set; }
        public string? Filename { get; private set; }
        public string? TimeOffset { get; private set; }
    }

    public class SavePubZipRequest
    {
        public SavePubZipRequest(string code, string? timeOffset, long? toFolderId, string? toName, string? progressHash)
        {
            if (toFolderId != null && toName == null) throw new Exception("A Name is required when a Folder Id is set");
            if (toName != null && toFolderId == null) throw new Exception("A Folder Id is required when a Name is set");

            Code = code;
            TimeOffset = timeOffset;
            ToFolderId = toFolderId;
            ToName = toName;
            ProgressHash = progressHash;
        }

        public string Code { get; private set; } = string.Empty;
        public string? TimeOffset { get; private set; }
        public long? ToFolderId { get; private set; }
        public string? ToName { get; private set; }
        public string? ProgressHash { get; private set; }
    }

    public class PubVideoLinkRequest : VideoStreamingRequest
    {
        public PubVideoLinkRequest(string code, bool forceDownload = false, string? contentType = null, bool maxSpeed = true, bool skipFilename = false, int? aBitrate = null, int? vBitrate = null, string? resolution = null, bool? fixedBitrate = null) : base(0, forceDownload, contentType, maxSpeed, skipFilename, aBitrate, vBitrate, resolution, fixedBitrate)
        {
            Code = code;
        }

        public string Code { get; private set; } = string.Empty;
    }

    public class PubAudioLinkRequest : AudioStreamingRequest
    {
        public PubAudioLinkRequest(string code, bool forceDownload = false, string? contentType = null, int? aBitrate = null) : base(0, forceDownload, contentType, aBitrate)
        {
            Code = code;
        }

        public string Code { get; private set; } = string.Empty;
    }

    public class PubTextLinkRequest : TextFileRequest
    {
        public PubTextLinkRequest(string code, string? fromEncoding = null, string? toEncoding = null, bool forceDownload = false, string? contentType = null) : base(0, fromEncoding, toEncoding, forceDownload, contentType)
        {
            Code = code;
        }

        public string Code { get; private set; } = string.Empty;
    }

    public class CollectionLinkRequest
    {
        public CollectionLinkRequest(long collectionId, DateTime? expire, int? maxDownloads, int? maxTraffic)
        {
            CollectionId = collectionId;
            Expire = expire;
            MaxDownloads = maxDownloads;
            MaxTraffic = maxTraffic;
        }

        public long CollectionId { get; private set; }
        public DateTime? Expire { get; private set; }
        public int? MaxDownloads { get; private set; }
        public int? MaxTraffic { get; private set; }
    }

    public class CollectionShortLinkRequest : CollectionLinkRequest
    {
        public CollectionShortLinkRequest(long collectionId, DateTime? expire, int? maxDownloads, int? maxTraffic) : base(collectionId, expire, maxDownloads, maxTraffic)
        {
            ShortLink = 1;
        }

        public int? ShortLink { get; private set; }
    }
}
