using System.Data;

namespace ChatCommon
{
    public class Message
    {
        public int Id { get; set; }
        public string? Text {  get; set; }
        public DataSetDateTime Date { get; set; }
        public bool IsSent {  get; set; }
        public int FromId { get; set; }
        public virtual User From { get; set; }
        public int ToId { get; set; }
        public virtual User To { get; set; }
    }
}
