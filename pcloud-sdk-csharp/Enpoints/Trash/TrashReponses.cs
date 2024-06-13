using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Responses;
namespace pcloud_sdk_csharp.Trash.Responses
{
    public class TrashReponse : SingleFileResponse
    {
        public Metadata destination { get; protected set; } = null!;
    }
}
