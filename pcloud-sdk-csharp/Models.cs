using System.Text.Json;

namespace pcloud_sdk_csharp.Requests
{

    public class AuthorizeRequest : BaseRequest
    {
        public AuthorizeRequest(string client_Id, ResponseType responseType, string? redirectUri = null, string? state = null, bool? forceApprove = false)
        {
            if (responseType == ResponseType.token && redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            Client_Id = client_Id;
            Type = responseType;
            RedirectUri = redirectUri;
            State = state;
            ForceApprove = forceApprove ?? false;
        }

        public string Client_Id { get; set; } = null!;
        public ResponseType Type { get; set; }
        public string? RedirectUri { get; set; }
        public string? State { get; set; }
        public bool ForceApprove { get; set; }

        public enum ResponseType
        {
            code = 0,
            token = 1
        }
    }

    public class ListFolderRequest : BaseRequest
    {
        public ListFolderRequest(long folderId, bool recursive = false, bool showDeleted = false, bool noFiles = false, bool noShares = false)
        {
            FolderId = folderId;
            Recursive = recursive ? 1 : 0;
            ShowDeleted = showDeleted ? 1 : 0;
            NoFiles = noFiles ? 1 : 0;
            NoShares = noShares ? 1 : 0;
        }

        public long FolderId { get; set; }
        public int Recursive { get; set; }
        public int ShowDeleted { get; set; }
        public int NoFiles { get; set; }
        public int NoShares { get; set; }

    }

    public class RenameFolderRequest : BaseRequest
    {
        public RenameFolderRequest(long folderId, long? toFolderId, string toName)
        {
            FolderId = folderId;
            ToFolderId = toFolderId ?? folderId;
            ToName = toName;
        }

        public long FolderId { get; set; }
        public long ToFolderId { get; set; }
        public string? ToName { get; set; }
    }

    public class CopyFolderRequest : BaseRequest
    {
        public CopyFolderRequest(long folderId, long toFolderId, bool noOver = false, bool skipExisting = false, bool copyContentOnly = false)
        {
            FolderId = folderId;
            ToFolderId = toFolderId;
            NoOver = noOver ? 1 : 0;
            SkipExisting = skipExisting ? 1 : 0;
            CopyContentOnly = copyContentOnly ? 1 : 0;
        }

        public long FolderId { get; set; }
        public long ToFolderId { get; set; }
        public int NoOver { get; set; }
        public int SkipExisting { get; set; }
        public int CopyContentOnly { get; set; }
    }

    public class UploadFileRequest : BaseRequest
    {
        public UploadFileRequest(long folderId, string fileName, Stream uploadFile, string? progressHash = null, bool noPartial = true, bool renameIfExists = true)
        {
            FolderId = folderId;
            FileName = fileName;
            UploadFile = uploadFile;
            ProgressHash = progressHash;
            RenameIfExists = renameIfExists ? 1 : 0;
            NoPartial = noPartial ? 1 : 0;
        }

        public long FolderId { get; set; }
        public string FileName { get; set; }
        public Stream UploadFile { get; set; }
        public string? ProgressHash { get; set; }
        public int NoPartial { get; set; }
        public int RenameIfExists { get; set; }
    }

    public class BaseRequest
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

// response properties are written in small letters due to json mapping of ws reponse
namespace pcloud_sdk_csharp.Responses
{
    // Auth
    public class AuthResponse : Response
    {
        public string code { get; set; } = null!;
        public string access_token { get; set; } = null!;
        public string token_type { get; set; } = null!;
        public string state { get; set; } = null!;
        public int locationid { get; set; }
        public int uid { get; set; }
        public string hostname { get; set; } = null!;
    }


    // Files
    public class UploadedFile : Response
    {
        public List<long> fileids { get; set; } = null!;
        public List<Metadata> metadata { get; set; } = null!;
    }

    public class UploadProgress
    {
        public long total { get; set; }
        public long uploaded { get; set; }
        public string currentfile { get; set; } = null!;
        public List<Metadata> files { get; set; } = null!;
        public bool finished { get; set; }
    }

    // Folders
    public class Folder : Response
    {
        public bool created { get; set; }
        public string id { get; set; } = null!;
        public Metadata metadata { get; set; } = null!;
    }

    public class DeleteFolder : Response
    {
        public int deletedfiles { get; set; }
        public int deletedfolders { get; set; }
    }

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
        //public bool ismine { get; set; }
        //public bool isshared { get; set; }
        public string name { get; set; } = null!;
        //public string id { get; set; }
        //public string? deletedfileid { get; set; }
        public string created { get; set; } = null!;
        public string modified { get; set; } = null!;
        //public string icon { get; set; }
        //public long category { get; set; }
        //public bool thumb { get; set; }
        public long size { get; set; }
        //public string contenttype { get; set; }
        //public long hash { get; set; }
        public List<Metadata>? contents { get; set; }
        //public bool isdeleted { get; set; }
        //public string path { get; set; }

        // For image files
        //public int? width { get; set; } -- see video
        //public int? weight { get; set; }

        // For Audio files
        //public string? artist { get; set; }
        //public string? album { get; set; }
        //public string? title { get; set; }
        //public string? genre { get; set; }
        //public string? trackno { get; set; }

        //// For video files
        //public int? width { get; set; }
        //public int? height { get; set; }
        //public float? duration { get; set; }
        //public float? fps { get; set; }
        //public string? videocodec { get; set; }
        //public string? audiocodec { get; set; }
        //public int? videobitrate { get; set; }
        //public int? audiobitrate { get; set; }
        //public int? audiosamplerate { get; set; }
        //public int? rotate { get; set; }
    }
}