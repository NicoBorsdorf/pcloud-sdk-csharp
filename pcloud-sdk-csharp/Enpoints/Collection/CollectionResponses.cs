using pcloud_sdk_csharp.Base.Responses;
using pcloud_sdk_csharp.File.Responses;

namespace pcloud_sdk_csharp.Collection.Responses
{
    public class CollectionResponse : Response
    {
        public List<Metadata>? collection { get; set; }
    }

    public class CollectionsResponse : Response
    {
        public List<Metadata>? collections { get; set; }
    }

    public class CollectionDetailsResponse : Response
    {
        public Metadata? collection { get; set; }
    }

    public class LinkFilesResponse : CollectionDetailsResponse
    {
        public List<SingleFileResponse>? linkresult { get; set; }
    }
}
