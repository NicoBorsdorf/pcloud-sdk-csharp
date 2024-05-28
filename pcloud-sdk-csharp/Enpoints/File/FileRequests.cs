namespace pcloud_sdk_csharp.File.Requests
{
    public class UploadFileRequest
    {
        public UploadFileRequest(long folderId, string fileName, Stream uploadFile, string? progressHash = null, bool noPartial = true, bool renameIfExists = true)
        {
            FolderId = folderId;
            FileName = fileName;
            UploadFile = uploadFile;
            ProgressHash = progressHash;
            RenameIfExists = renameIfExists ? 1 : null;
            NoPartial = noPartial ? 1 : null;
        }

        public long FolderId { get; private set; }
        public string FileName { get; private set; }
        public Stream UploadFile { get; private set; }
        public string? ProgressHash { get; private set; }
        public int? NoPartial { get; private set; }
        public int? RenameIfExists { get; private set; }
    }
}
