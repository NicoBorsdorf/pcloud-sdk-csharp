
namespace pcloud_sdk_csharp.General.Requests
{
    public class FeedbackRequest
    {
        public FeedbackRequest(string mail, string reason, string message, string? name = null)
        {
            Mail = mail ?? throw new ArgumentNullException(nameof(mail));
            Reason = reason ?? throw new ArgumentNullException(nameof(reason));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Name = name;
        }

        public string Mail { get; private set; }
        public string Reason { get; private set; }
        public string Message { get; private set; }
        public string? Name { get; private set; }
    }

    public class DiffRequest
    {
        public DiffRequest(long? diffId, DateTime? after, int? limit, bool last = false, bool block = false)
        {
            DiffId = diffId;
            After = after;
            Last = last ? 1 : null;
            Block = block ? 1 : null;
            Limit = limit;

            if (Block != null && diffId == null) throw new Exception("A Diff Id must be set when using Block property.");
        }

        public long? DiffId { get; private set; }
        public DateTime? After { get; private set; }
        public int? Last { get; private set; }
        public int? Block { get; private set; }
        public int? Limit { get; private set; }

    }
}
