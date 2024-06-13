
namespace pcloud_sdk_csharp.UploadLinks.Requests
{
    public class UploadLinksRequests
    {
        public UploadLinksRequests(long folderId, string comment, DateTime? expire, int? maxSpace, int? maxFiles)
        {
            FolderId = folderId;
            Comment = comment;
            Expire = expire;
            MaxSpace = maxSpace;
            MaxFiles = maxFiles;
        }

        public long FolderId { get; private set; }
        public string Comment { get; private set; } = string.Empty;
        public DateTime? Expire { get; private set; }
        public int? MaxSpace { get; private set; }
        public int? MaxFiles { get; private set; }
    }

    public class ChangeUploadLinkRequest
    {
        public ChangeUploadLinkRequest(long uploadLinkId, DateTime? expire, int? maxSpace, int? maxFiles, bool deleteExpire = false)
        {
            UploadLinkId = uploadLinkId;
            Expire = expire;
            DeleteExpire = deleteExpire ? 1 : null;
            MaxSpace = maxSpace;
            MaxFiles = maxFiles;
        }

        public long UploadLinkId { get; private set; }
        public DateTime? Expire { get; private set; }
        public int? DeleteExpire { get; private set; }
        public int? MaxSpace { get; private set; }
        public int? MaxFiles { get; private set; }
    }

    public class UploadToLinkRequests
    {
        public UploadToLinkRequests(string code, bool noPartial = true, string? progressHash = null)
        {
            Code = code;
            NoPartial = noPartial ? 1 : null;
            ProgressHash = progressHash;
        }

        public string Code { get; private set; } = string.Empty;
        public int? NoPartial { get; private set; }
        public string? ProgressHash { get; private set; }
    }
}
