
namespace pcloud_sdk_csharp.Sharing.Requests
{
    public class ShareFolderRequest
    {
        public ShareFolderRequest(long folderId, string email, string? name, string? message, List<Permission> allowedPermissions)
        {
            if (allowedPermissions.Count == 0) throw new Exception("At least one permission must be provided");

            FolderId = folderId;
            Email = email;
            Name = name;
            Message = message;
            AllowedPermissions = allowedPermissions;
        }

        public long FolderId { get; private set; }
        public string Email { get; private set; } = null!;
        public string? Name { get; private set; }
        public string? Message { get; private set; }
        public List<Permission> AllowedPermissions { get; private set; } = null!;
    }

    public class AcceptShareRequest
    {
        public AcceptShareRequest(long shareRequestId, string? name, long? folderId, bool always = false)
        {
            ShareRequestId = shareRequestId;
            Name = name;
            FolderId = folderId;
            Always = always ? 1 : null;
        }

        public long ShareRequestId { get; private set; }
        public string? Name { get; private set; }
        public long? FolderId { get; private set; }
        public int? Always { get; private set; }
    }

    public class ListSharesReqeust
    {
        public ListSharesReqeust(bool noRequests = false, bool noShares = false, bool noIncoming = false, bool noOutgoing = false)
        {
            NoRequests = noRequests ? 1 : null;
            NoShares = noShares ? 1 : null;
            NoIncoming = noIncoming ? 1 : null;
            NoOutgoing = noOutgoing ? 1 : null;
        }

        public int? NoRequests { get; private set; }
        public int? NoShares { get; private set; }
        public int? NoIncoming { get; private set; }
        public int? NoOutgoing { get; private set; }
    }

    public enum Permission
    {
        Create = 1,
        Modify = 2,
        Delete = 4,
    }
}
