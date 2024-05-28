namespace pcloud_sdk_csharp.Folder.Requests
{
    public class ListFolderRequest
    {
        public ListFolderRequest(long folderId, bool recursive = false, bool showDeleted = false, bool noFiles = false, bool noShares = false)
        {
            FolderId = folderId;
            Recursive = recursive ? 1 : null;
            ShowDeleted = showDeleted ? 1 : null;
            NoFiles = noFiles ? 1 : null;
            NoShares = noShares ? 1 : null;
        }

        public long FolderId { get; private set; }
        public int? Recursive { get; private set; }
        public int? ShowDeleted { get; private set; }
        public int? NoFiles { get; private set; }
        public int? NoShares { get; private set; }

    }

    public class RenameFolderRequest
    {
        public RenameFolderRequest(long folderId, long? toFolderId, string toName)
        {
            FolderId = folderId;
            ToFolderId = toFolderId ?? folderId;
            ToName = toName;
        }

        public long FolderId { get; private set; }
        public long ToFolderId { get; private set; }
        public string? ToName { get; private set; }
    }

    public class CopyFolderRequest
    {
        public CopyFolderRequest(long folderId, long toFolderId, bool noOver = false, bool skipExisting = false, bool copyContentOnly = false)
        {
            FolderId = folderId;
            ToFolderId = toFolderId;
            NoOver = noOver ? 1 : null;
            SkipExisting = skipExisting ? 1 : null;
            CopyContentOnly = copyContentOnly ? 1 : null;
        }

        public long FolderId { get; private set; }
        public long ToFolderId { get; private set; }
        public int? NoOver { get; private set; }
        public int? SkipExisting { get; private set; }
        public int? CopyContentOnly { get; private set; }
    }
}
