using System.Text.Json;

namespace pcloud_sdk_csharp.Requests
{
    public class CreateFolderRequest : BaseRequest
    {
        public CreateFolderRequest(string path, int folderId, string name)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            FolderId = folderId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Path { get; set; } = "";
        public int FolderId { get; set; }
        public string Name { get; set; } = "";
    }

    public class ListFolderRequest : BaseRequest
    {
        public ListFolderRequest(string path, int folderId, int? recursive, int? showDeleted, int? noFiles, int? noShares)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            FolderId = folderId;
            Recursive = recursive;
            ShowDeleted = showDeleted;
            NoFiles = noFiles;
            NoShares = noShares;
        }

        public string Path { get; set; } = "";
        public int FolderId { get; set; }
        public int? Recursive { get; set; }
        public int? ShowDeleted { get; set; }
        public int? NoFiles { get; set; }
        public int? NoShares { get; set; }

    }

    public class RenameFolderRequest : BaseRequest
    {
        public RenameFolderRequest(int folderId, string? path, string? toPath, int toFolderId, string? toName)
        {
            FolderId = folderId;
            Path = path;
            ToPath = toPath;
            ToFolderId = toFolderId;
            ToName = toName;
        }

        public int FolderId { get; set; }
        public string? Path { get; set; }
        public string? ToPath { get; set; }
        public int ToFolderId { get; set; }
        public string? ToName { get; set; }
    }

    public class CopyFolderRequest : BaseRequest
    {
        public CopyFolderRequest(int folderId, int toFolderId, int? noOver, int? skipExisting, int? copyContentOnly)
        {
            FolderId = folderId;
            ToFolderId = toFolderId;
            NoOver = noOver;
            SkipExisting = skipExisting;
            CopyContentOnly = copyContentOnly;
        }

        public int FolderId { get; set; }
        public int ToFolderId { get; set; }
        public int? NoOver { get; set; }
        public int? SkipExisting { get; set; }
        public int? CopyContentOnly { get; set; }
    }

    public class UploadFileRequest : BaseRequest
    {
        public UploadFileRequest(int folderId, string fileName, Stream uploadFile, string? progressHash = "", bool? noPartial = true, bool? renameIfExists = true)
        {
            FolderId = folderId;
            FileName = fileName;
            UploadFile = uploadFile;
            ProgressHash = progressHash;
            RenameIfExists = renameIfExists;
            NoPartial = noPartial;
        }

        public int FolderId { get; set; }
        public string FileName { get; set; }
        public Stream UploadFile { get; set; }
        public string? ProgressHash { get; set; }
        public bool? NoPartial { get; set; }
        public bool? RenameIfExists { get; set; }
    }

    public class BaseRequest
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

namespace pcloud_sdk_csharp.Responses
{
    // Files
    public class UploadedFile : Response
    {
        public List<long>? fileids { get; set; }
        public List<FileMetadata>? metadata { get; set; }
    }

    public class UploadProgress
    {
        public long Total { get; set; }
        public long Uploaded { get; set; }
        public string CurrentFile { get; set; }
        public List<FileMetadata> Files { get; set; }
        public bool Finished { get; set; }
    }

    // Folders
    public class Folder : Response
    {
        public bool? created { get; set; }
        public string? id { get; set; }
        public FolderMetadata? metadata { get; set; }
    }

    public class DeleteFolder : Response
    {
        public int deletedFiles { get; set; }
        public int deletedFolders { get; set; }
    }

    public class Response
    {
        public int result { get; set; }
        public string? error { get; set; }
    }

    // Metadata and all super categories
    public class Metadata
    {
        public long? parentfolderid { get; set; }
        public bool? isfolder { get; set; }
        public bool? ismine { get; set; }
        public bool? isshared { get; set; }
        public string? name { get; set; }
        public string? id { get; set; }
        public string? deletedfileid { get; set; }
        public string? created { get; set; }
        public string? modified { get; set; }
        public string? icon { get; set; }
        public long? category { get; set; }
        public bool? thumb { get; set; }
        public long? size { get; set; }
        public string? contenttype { get; set; }
        public long? hash { get; set; }
        public List<Metadata>? contents { get; set; }
        public bool? isDeleted { get; set; }
        public string? path { get; set; }
    }

    public class FileMetadata : Metadata
    {
        // in case of file -> fileid is returned
        public long? fileid { get; set; }

    }

    public class FolderMetadata : Metadata
    {
        // in case of folder -> folderid is returned
        public long folderid { get; set; }

    }

    public class ImageMetadata : Metadata
    {
        // relevant for image files
        public int? width { get; set; }
        public int? weight { get; set; }
    }

    public class AudioMetadata : Metadata
    {
        public string? artist { get; set; }
        public string? album { get; set; }
        public string? title { get; set; }
        public string? genre { get; set; }
        public string? trackNo { get; set; }
    }

    public class VideoMetadata : Metadata
    {
        public int? width { get; set; }
        public int? height { get; set; }
        public float? duration { get; set; }
        public float? fps { get; set; }
        public string? videocodec { get; set; }
        public string? audiocodec { get; set; }
        public int? videobitrate { get; set; }
        public int? audiobitrate { get; set; }
        public int? audiosamplerate { get; set; }
        public int? rotate { get; set; }
    }
}