using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Sharing.Responses
{
    public class ListSharesResponse : Response
    {
        public ShareResponse? shares { get; set; }
        public ShareResponse? requests { get; set; }

        public class ShareResponse
        {
            public List<Share>? incoming;
            public List<Share>? outgoing;
        }
    }

    public class ShareRequestInfoResponse : Response
    {
        public Share? share { get; set; }
    }

    public class Share
    {
        public string tomail { get; set; } = string.Empty;
        public bool cancreate { get; set; }
        public long folderid { get; set; }
        public string sharerequestid { get; set; } = string.Empty;
        public bool canread { get; set; }
        public DateTime expires { get; set; }
        public bool canmodify { get; set; }
        public string message { get; set; } = string.Empty;
        public bool candelete { get; set; }
        public string sharename { get; set; } = string.Empty;
        public DateTime created { get; set; }
    }

}
