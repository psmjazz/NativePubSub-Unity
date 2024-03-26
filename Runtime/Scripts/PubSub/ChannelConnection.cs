using System.Collections;
using System.Collections.Generic;
using PJ.Native.Proto;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace PJ.Native.PubSub
{
    internal class ChannelConnection : Channel
    {
        private Envelope envelope;
        private Tag tag;
        private int receiverID;
        public Envelope Envelope => envelope;
        public Message Message => envelope.Message;

        internal ChannelConnection(Envelope envelope, int receiverID, Tag tag)
        {
            this.envelope = envelope;
            this.receiverID = receiverID;
            this.tag = tag;
        }

        internal void SerializeTag()
        {
            foreach(string tagName in tag.Names)
            {
                envelope.TagNames.Add(tagName);
            }
        }

        public void Reply(Message message)
        {
            Envelope envelope = new Envelope();
            envelope.SenderID = receiverID;
            envelope.ReceiverID = envelope.SenderID;
            envelope.Message = message;
            MessageManager.Instance.Mediator.Publish(envelope, Tag.None);
        }
    }
}