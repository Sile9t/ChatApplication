
namespace ChatCommon
{
    public class User
    {
        public int Id {  get; set; }
        public string? FullName { get; set; }
        public virtual List<Message>? MessagesTo { get; set; } = new List<Message>();
        public virtual List<Message>? MessagesFrom { get; set; } = new List<Message>();
    }
}
