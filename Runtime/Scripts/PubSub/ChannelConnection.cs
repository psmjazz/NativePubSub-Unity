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
        private int receiverID;
        public Envelope Envelope => envelope;
        public Message Message => envelope.Message;
        internal ChannelConnection(Envelope envelope, int receiverID)
        {
            this.envelope = envelope;
            this.receiverID = receiverID;
        }

        public void Reply(Message message)
        {
            Envelope envelope = new Envelope();
            envelope.SenderID = receiverID;
            envelope.ReceiverID = envelope.SenderID;
            envelope.Message = message;
            MessageManager.Instance.Mediator.Publish(envelope, Tag.Native);
        }
    }
}