using pcloud_sdk_csharp.Base.Responses;
using System.Text.Json.Serialization;

namespace pcloud_sdk_csharp.General.Responses
{
    public class GetDigestResponse : Response
    {
        public string? digest { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? expires { get; set; }
    }

    public class UserInfoResponse : Response
    {
        public string? email { get; set; }
        public bool? emailverified { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? registered { get; set; }
        public bool? premium { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? premiumexpires { get; set; }
        public long? quota { get; set; }
        public long? usedquota { get; set; }
        public string? language { get; set; }
    }

    public class LanguagesResponse : Response
    {
        public Dictionary<string, string>? languages { get; set; }
    }

    public class CurrentServerResponse : Response
    {
        public string? ip { get; set; }
        public string? ipbin { get; set; }
        public string? ipv6 { get; set; }
        public string? hostname { get; set; }
    }

    public class DiffResponse : Response
    {
        public long? diffid { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? time { get; set; }
        [JsonPropertyName("event")]
        public Event? eventType { get; set; }

        // optional depending on type of event
        public Metadata? metadata { get; set; }
        public UserInfoResponse? userinfo { get; set; }
    }

    public class FileHistoryResponse : Response
    {
        public List<DiffResponse>? entries { get; set; }
    }

    public class GetIpResponse : Response
    {
        public string? ip { get; set; }
        public string? country { get; set; }
    }

    public class GetApiServerResponse : Response
    {
        public List<string>? binapi { get; set; }
        public List<string>? api { get; set; }
    }

    public enum Event
    {
        reset,
        createfolder,
        deletefolder,
        modifyfolder,
        createfile,
        modifyfile,
        deletefile,
        requestsharein,
        acceptedsharein,
        declinedsharein,
        declinedshareout,
        cancelledsharein,
        removedsharein,
        modifiedsharein,
        modifyuserinfo
    }
}
