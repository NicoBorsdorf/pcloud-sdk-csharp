

using pcloud_sdk_csharp.Base.Requests;

namespace pcloud_sdk_csharp.Archiving.Requests
{
    public class ZipRequest
    {
        public ZipRequest(Tree tree, bool forceDownload = false, string? filename = null, string? timeOffset = null)
        {
            if (filename != null && !filename.Contains(".zip")) throw new Exception("Filename must contain .zip file extension if set to not null");
            Tree = tree;
            ForceDownload = forceDownload ? 1 : 0;
            Filename = filename;
            TimeOffset = timeOffset;
        }

        public Tree Tree { get; private set; } = null!;
        public int? ForceDownload { get; private set; }
        public string? Filename { get; private set; }
        public string? TimeOffset { get; private set; }
    }

    public class ZipLinkRequest : ZipRequest
    {
        public ZipLinkRequest(Tree tree, int? maxSpeed, bool forceDownload = false, string? filename = null, string? timeOffset = null) : base(tree, forceDownload, filename, timeOffset)
        {
            MaxSpeed = maxSpeed;
        }
        public int? MaxSpeed { get; private set; }
    }

    public class SaveZipRequest
    {
        public SaveZipRequest(Tree tree, string? timeOffset, long? toFolderId, string? toName, string? progressHash)
        {
            if (toFolderId != null && toName == null) throw new Exception("A Name is required when a Folder Id is set");
            if (toName != null && toFolderId == null) throw new Exception("A Folder Id is required when a Name is set");

            Tree = tree;
            TimeOffset = timeOffset;
            ToFolderId = toFolderId;
            ToName = toName;
            ProgressHash = progressHash;
        }

        public Tree Tree { get; private set; } = null!;
        public string? TimeOffset { get; private set; }
        public long? ToFolderId { get; private set; }
        public string? ToName { get; private set; }
        public string? ProgressHash { get; private set; }
    }

    public class ExtractArchiveRequest
    {
        public ExtractArchiveRequest(long fileId, long toFolderId, bool? noOutput, Overwrites? overwrite, string? password)
        {
            FileId = fileId;
            ToFolderId = toFolderId;
            NoOutput = noOutput;
            Overwrite = overwrite;
            Password = password;
        }

        public long FileId { get; private set; }
        public long ToFolderId { get; private set; }
        public bool? NoOutput { get; private set; }
        public Overwrites? Overwrite { get; private set; }
        public string? Password { get; private set; }

        public enum Overwrites
        {
            Rename,
            Overwrite,
            Skip
        }
    }
}


