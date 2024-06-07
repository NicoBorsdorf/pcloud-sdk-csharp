using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Sharing.Responses
{
    public class ListSharesResponse : Response
    {
        public ShareResponse? shares { get; protected set; }
        public ShareResponse? requests { get; protected set; }

        public class ShareResponse
        {
            public List<Share>? incoming;
            public List<Share>? outgoing;
        }
    }

    public class ShareRequestInfoResponse : Response
    {
        public Share? share { get; protected set; }
    }

    public class Share
    {
        public string tomail { get; protected set; } = string.Empty;
        public bool cancreate { get; protected set; }
        public long folderid { get; protected set; }
        public string sharerequestid { get; protected set; } = string.Empty;
        public bool canread { get; protected set; }
        public DateTime expires { get; protected set; }
        public bool canmodify { get; protected set; }
        public string message { get; protected set; } = string.Empty;
        public bool candelete { get; protected set; }
        public string sharename { get; protected set; } = string.Empty;
        public DateTime created { get; protected set; }
    }

}
