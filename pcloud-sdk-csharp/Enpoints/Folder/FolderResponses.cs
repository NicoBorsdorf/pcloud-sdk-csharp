using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Folder.Responses
{

    // Folders
    public class FolderResponse : Response
    {
        public bool? created { get; set; }
        public string? id { get; set; }
        public Metadata? metadata { get; set; }
    }

    public class DeleteFolderResponse : Response
    {
        public int? deletedfiles { get; set; }
        public int? deletedfolders { get; set; }
    }
}
