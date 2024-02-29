
namespace PJ.Native.Proto
{
    public partial class Message
    {
        public Message(string key)
        {
            Key = key;
        }

        public Message(string key, Container container)
        {
            Key = key;
            Container = container;
        }
    }
}