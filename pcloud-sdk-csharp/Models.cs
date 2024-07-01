using Newtonsoft.Json.Converters;
using System.Numerics;
using System.Text.Json.Serialization;

namespace pcloud_sdk_csharp.Base.Requests
{
    public class Tree
    {
        public List<long> FolderIds { get; } = new List<long> { };
        public List<long> FileIds { get; } = new List<long> { };
        public List<long> ExcludeFolderIds { get; } = new List<long> { };
        public List<long> ExcludeFileIds { get; } = new List<long> { };
    }
}

// response properties are written in small letters due to json mapping of ws reponse
namespace pcloud_sdk_csharp.Base.Responses
{
    public class Response
    {
        public int result { get; set; }
        public string? error { get; set; }
    }

    // Metadata and all super categories
    public class Metadata
    {
        public long parentfolderid { get; set; }
        public long? fileid { get; set; }
        public long? folderid { get; set; }
        public bool isfolder { get; set; }
        public bool ismine { get; set; }
        public bool isshared { get; set; }
        public string name { get; set; } = null!;
        public string id { get; set; } = null!;
        public string? deletedfileid { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime created { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime modified { get; set; }
        public string? icon { get; set; }
        public long category { get; set; }
        public bool thumb { get; set; }
        public long size { get; set; }
        public string? contenttype { get; set; }
        public BigInteger hash { get; set; }
        public List<Metadata>? contents { get; set; }
        public bool isdeleted { get; set; }
        public string? path { get; set; }

        // For image files
        //public int? width { get; set; } --> see video
        public long? weight { get; set; }

        // For Audio files
        public string? artist { get; set; }
        public string? album { get; set; }
        public string? title { get; set; }
        public string? genre { get; set; }
        public string? trackno { get; set; }

        // For video files
        public long? width { get; set; }
        public long? height { get; set; }
        public float? duration { get; set; }
        public float? fps { get; set; }
        public string? videocodec { get; set; }
        public string? audiocodec { get; set; }
        public long? videobitrate { get; set; }
        public long? audiobitrate { get; set; }
        public long? audiosamplerate { get; set; }
        public long? rotate { get; set; }
    }

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "ddd, dd MMM yyyy HH:mm:ss K";
        }
    }
}