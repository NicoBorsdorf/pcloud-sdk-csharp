namespace pcloud_sdk_csharp.Folder.Requests
{
    public class ListFolderRequest
    {
        public ListFolderRequest(long folderId, bool recursive = false, bool showDeleted = false, bool noFiles = false, bool noShares = false)
        {
            FolderId = folderId;
            Recursive = recursive ? 1 : 0;
            ShowDeleted = showDeleted ? 1 : 0;
            NoFiles = noFiles ? 1 : 0;
            NoShares = noShares ? 1 : 0;
        }

        public long FolderId { get; private set; }
        public int Recursive { get; private set; }
        public int ShowDeleted { get; private set; }
        public int NoFiles { get; private set; }
        public int NoShares { get; private set; }

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
            NoOver = noOver ? 1 : 0;
            SkipExisting = skipExisting ? 1 : 0;
            CopyContentOnly = copyContentOnly ? 1 : 0;
        }

        public long FolderId { get; private set; }
        public long ToFolderId { get; private set; }
        public int NoOver { get; private set; }
        public int SkipExisting { get; private set; }
        public int CopyContentOnly { get; private set; }
    }
}
