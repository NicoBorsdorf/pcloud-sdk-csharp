using pcloud_sdk_csharp.Base.Responses;

namespace pcloud_sdk_csharp.Folder.Responses
{

    // Folders
    public class FolderResponse : Response
    {
        public bool created { get; protected set; }
        public string id { get; protected set; } = null!;
        public Metadata metadata { get; protected set; } = null!;
    }

    public class DeleteFolderResponse : Response
    {
        public int deletedfiles { get; protected set; }
        public int deletedfolders { get; protected set; }
    }
}
