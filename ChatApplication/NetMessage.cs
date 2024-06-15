using System.Text.Json;

namespace ChatCommon
{
    public class NetMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Command Command { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public NetMessage(string txt, Command command, string from, string to)
        {
            Text = txt;
            Date = DateTime.Now;
            Command = command;
            From = from;
            To = to;
        }

        public NetMessage()
        {
        }

        public string SerializeToJson() => JsonSerializer.Serialize(this);
        public static NetMessage? DeserializeFromJson(string msg) 
            => JsonSerializer.Deserialize<NetMessage>(msg);
        public override string ToString()
        {
            return $"{Date} From: {From} To: {To} Text: {Text}";
        }
    }
}
