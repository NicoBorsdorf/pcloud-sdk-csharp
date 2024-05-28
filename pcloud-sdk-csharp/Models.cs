using System.Text.Json;


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
        public int result { get; protected set; }
        public string? error { get; protected set; }
    }

    // Metadata and all super categories
    public class Metadata
    {
        public long parentfolderid { get; protected set; }
        public long? fileid { get; protected set; }
        public long? folderid { get; protected set; }
        public bool isfolder { get; protected set; }
        public bool ismine { get; protected set; }
        public bool isshared { get; protected set; }
        public string name { get; protected set; } = null!;
        public string id { get; protected set; } = null!;
        public string? deletedfileid { get; protected set; }
        public DateTime created { get; protected set; }
        public DateTime modified { get; protected set; }
        public string? icon { get; protected set; }
        public long category { get; protected set; }
        public bool thumb { get; protected set; }
        public long size { get; protected set; }
        public string? contenttype { get; protected set; }
        public long hash { get; protected set; }
        public List<Metadata>? contents { get; protected set; }
        public bool isdeleted { get; protected set; }
        public string? path { get; protected set; }

        // For image files
        //public int? width { get; set; } --> see video
        public long? weight { get; protected set; }

        // For Audio files
        public string? artist { get; protected set; }
        public string? album { get; protected set; }
        public string? title { get; protected set; }
        public string? genre { get; protected set; }
        public string? trackno { get; protected set; }

        // For video files
        public long? width { get; protected set; }
        public long? height { get; protected set; }
        public float? duration { get; protected set; }
        public float? fps { get; protected set; }
        public string? videocodec { get; protected set; }
        public string? audiocodec { get; protected set; }
        public long? videobitrate { get; protected set; }
        public long? audiobitrate { get; protected set; }
        public long? audiosamplerate { get; protected set; }
        public long? rotate { get; protected set; }
    }
}