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
        public UploadFileRequest(int folderId, string fileName, string uploadFile)
        {
            FolderId = folderId;
            FileName = fileName;
            UploadFile = uploadFile;
        }

        public UploadFileRequest(int folderId, string fileName, Stream uploadFile)
        {
            StreamReader sr = new StreamReader(uploadFile);
            FolderId = folderId;
            FileName = fileName;
            UploadFile = sr.ReadToEnd();
        }

        public UploadFileRequest(int folderId, string fileName, StreamReader uploadFile)
        {
            FolderId = folderId;
            FileName = fileName;
            UploadFile = uploadFile.ReadToEnd();
        }

        public int FolderId { get; set; }
        public string FileName { get; set; }
        public string UploadFile { get; set; }
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
        public UploadedFile(int result, int[] fileIds, FileMetadata[] metadata) : base(result)
        {
            FileIds = fileIds;
            Metadata = metadata;
        }

        public int[] FileIds { get; set; }
        public FileMetadata[] Metadata { get; set; }
    }

    // Folders
    public class Folder : Response
    {
        public Folder(int result, bool? created, string? id, FolderMetadata? metadata) : base(result)
        {
            Created = created;
            Id = id;
            Metadata = metadata;
        }

        public bool? Created { get; set; }
        public string? Id { get; set; }
        public FolderMetadata? Metadata { get; set; }
    }

    public class DeleteFolder : Response
    {
        public DeleteFolder(int result, int deletedFiles, int deletedFolders) : base(result)
        {

            DeletedFiles = deletedFiles;
            DeletedFolders = deletedFolders;
        }

        public int DeletedFiles { get; set; }
        public int DeletedFolders { get; set; }
    }

    public class Response
    {
        public Response(int result)
        {
            Result = result;
            switch (Result)
            {
                case 1000:
                    Message = "Log in required.";
                    break;
                case 2000:
                    Message = "Log in failed.";
                    break;
                case 2001:
                    Message = "Invalid file/ folder name.";
                    break;
                case 2003:
                    Message = "Access denied. You do not have permissions to preform this operation.";
                    break;
                case 2005:
                    Message = "Directory does not exist.";
                    break;
                case 2008:
                    Message = "User is over quota.";
                    break;
                case 2041:
                    Message = "Connection broken.";
                    break;
                case 4000:
                    Message = "Too many login tries from this IP address.";
                    break;
                case 5000:
                    Message = "Internal error. Try again later.";
                    break;
                case 5001:
                    Message = "Internal upload error.";
                    break;
                default:
                    Message = null;
                    break;
            }
        }

        public int Result { get; set; }
        public string? Message { get; set; }
    }

    // Metadata and all super categories
    public class Metadata
    {
        public int ParentFolderId { get; set; }
        public bool IsFolder { get; set; }
        public bool IsMine { get; set; }
        public bool IsShared { get; set; }
        public string Name { get; set; } = "";
        public string Id { get; set; } = "";
        public string DeletedFileId { get; set; } = "";
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Icon { get; set; } = "";
        public int Category { get; set; }
        public bool Thumb { get; set; }
        public int Size { get; set; }
        public string ContentType { get; set; } = "";
        public int Hash { get; set; }
        public Array? Contents { get; set; }
        public bool IsDeleted { get; set; }
        public string Path { get; set; } = "";
    }

    public class FileMetadata : Metadata
    {
        // in case of file -> fileid is returned
        public int FileId { get; set; }

    }

    public class FolderMetadata : Metadata
    {
        // in case of folder -> folderid is returned
        public int FolderId { get; set; }

    }

    public class ImageMetadata : Metadata
    {
        // relevant for image files
        public int? Width { get; set; }
        public int? Height { get; set; }
    }

    public class AudioMetadata : Metadata
    {
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? TrackNo { get; set; }
    }

    public class VideoMetadata : Metadata
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public float? Duration { get; set; }
        public float? Fps { get; set; }
        public string? VideoCodec { get; set; }
        public string? AudioCodec { get; set; }
        public int? VideoVitrate { get; set; }
        public int? AudioVitrate { get; set; }
        public int? AudioSamplerate { get; set; }
        public int? Rotate { get; set; }
    }
}