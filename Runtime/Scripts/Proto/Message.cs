
using PJ.Native.PubSub;

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

    public partial class Envelope
    {
        public Envelope(Message message, int senderID)
        {
            Message = message;
            SenderID = senderID;
        }

        public Envelope(Message message, int senderID, int receiverID)
        {
            Message = message;
            SenderID = senderID;
            ReceiverID = receiverID;
        }
    }
}