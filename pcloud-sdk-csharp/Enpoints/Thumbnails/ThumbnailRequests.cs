namespace pcloud_sdk_csharp.Thumbnails.Requests
{
    public class ThumbRequest
    {
        public ThumbRequest(long fileId, string size, bool crop = false, string? type = null)
        {
            FileId = fileId;
            Size = size;
            Crop = crop ? 1 : null;
            Type = type;
        }

        public long FileId { get; private set; }
        public string Size { get; private set; } = string.Empty;
        public int? Crop { get; private set; }
        public string? Type { get; private set; }
    }

    public class SaveThumbRequest : ThumbRequest
    {
        public SaveThumbRequest(long toFolderId, string toName, long fileId, string size, bool crop = false, string? type = null, bool noOver = false) : base(fileId, size, crop, type)
        {
            ToFolderId = toFolderId;
            ToName = toName;
            NoOver = noOver ? 1 : null;
        }

        public long ToFolderId { get; private set; }
        public string ToName { get; private set; } = string.Empty;
        public int? NoOver { get; private set; }
    }

    public class GetThumbLinksRequest
    {
        public GetThumbLinksRequest(List<long> fileIds, string size, bool crop = false, string? type = null)
        {
            FileIds = fileIds;
            Size = size;
            Crop = crop ? 1 : null;
            Type = type;
        }

        public List<long> FileIds { get; private set; }
        public string Size { get; private set; }
        public int? Crop { get; private set; }
        public string? Type { get; private set; }
    }
}
