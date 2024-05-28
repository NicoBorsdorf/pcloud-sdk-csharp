using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcloud_sdk_csharp.Streaming.Requests
{
    // Streaming
    public class StreamingRequest
    {
        public StreamingRequest(long fileId, bool forceDownload = false, string? contentType = null, bool maxSpeed = true, bool skipFilename = false)
        {
            FileId = fileId;
            ForceDownload = forceDownload ? 1 : 0;
            ContentType = contentType;
            MaxSpeed = maxSpeed ? 1 : 0;
            SkipFilename = skipFilename ? 1 : 0;
        }

        public long FileId { get; private set; }
        public int? ForceDownload { get; private set; }
        public string? ContentType { get; private set; }
        public int? MaxSpeed { get; private set; }
        public int? SkipFilename { get; private set; }
    }

    public class VideoStreamingRequest : StreamingRequest
    {
        public VideoStreamingRequest(long fileId, bool forceDownload = false, string? contentType = null, bool maxSpeed = true, bool skipFilename = false, int? aBitrate = null, int? vBitrate = null, string? resolution = null, bool? fixedBitrate = null) : base(fileId, forceDownload, contentType, maxSpeed, skipFilename)
        {
            if (aBitrate != null && (aBitrate < 16 || aBitrate > 320)) throw new Exception("ABitrate needs to be between 16 and 320 kilobits, when set to not null");
            if (vBitrate != null && (vBitrate < 16 || vBitrate > 4000)) throw new Exception("VBitrate needs to be between 16 and 320 kilobits, when set to not null");

            ABitrate = aBitrate;
            VBitrate = vBitrate;
            Resolution = resolution;
            FixedBitrate = fixedBitrate;
        }

        public int? ABitrate { get; private set; }
        public int? VBitrate { get; private set; }
        public string? Resolution { get; private set; }
        public bool? FixedBitrate { get; private set; }
    }

    public class AudioStreamingRequest
    {

        public AudioStreamingRequest(long fileId, bool forceDownload = false, string? contentType = null, int? aBitrate = null)
        {
            if (aBitrate != null && (aBitrate < 16 || aBitrate > 320)) throw new Exception("ABitrate needs to be between 16 and 320 kilobits, when set to not null");

            FileId = fileId;
            ForceDownload = forceDownload ? 1 : 0;
            ContentType = contentType;
            ABitrate = aBitrate;
        }

        public long FileId { get; private set; }
        public int? ForceDownload { get; private set; }
        public string? ContentType { get; private set; }
        public int? ABitrate { get; private set; }
    }

    public class HSLLinkRequest
    {
        public HSLLinkRequest(long fileId, bool forceDownload = false, string? resolution = null, int? aBitrate = null, int? vBitrate = null)
        {
            if (aBitrate != null && (aBitrate < 16 || aBitrate > 320)) throw new Exception("ABitrate needs to be between 16 and 320 kilobits, when set to not null");
            if (vBitrate != null && (vBitrate < 16 || vBitrate > 4000)) throw new Exception("VBitrate needs to be between 16 and 4000 kilobits, when set to not null");

            FileId = fileId;
            ForceDownload = forceDownload ? 1 : 0;
            Resolution = resolution;
            ABitrate = aBitrate;
            VBitrate = vBitrate;
        }

        public long FileId { get; private set; }
        public int? ForceDownload { get; private set; }
        public string? Resolution { get; private set; }
        public int? ABitrate { get; private set; }
        public int? VBitrate { get; private set; }
    }

    public class TextFileRequest
    {
        public TextFileRequest(long fileId, string? fromEncoding = null, string? toEncoding = null, bool forceDownload = false, string? contentType = null)
        {
            FileId = fileId;
            FromEncoding = fromEncoding;
            ToEncoding = toEncoding;
            ForceDownload = forceDownload ? 1 : 0;
            ContentType = contentType;
        }

        public long FileId { get; private set; }
        public string? FromEncoding { get; private set; }
        public string? ToEncoding { get; private set; }
        public int? ForceDownload { get; private set; }
        public string? ContentType { get; private set; }

    }
}
