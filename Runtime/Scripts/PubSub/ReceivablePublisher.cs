using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.PubSub
{
    public interface Receivable
    {
        void SetTagRule(Tag all);
        bool MatchTag(Tag tag);
        void OnReceive(Channel channel);
    }

    public class Publisher
    {
        private static class IDCounter
        {
            private static int id = 0;
            public static int GetID()
            {
                return id+=2;
            }
        }

        public int ID = IDCounter.GetID();

        public void Publish(Message message)
        {
            Envelope envelope = new Envelope(message, this.ID);
            MessageManager.Instance.Mediator.Publish(envelope, Tag.Relay);
        }

        public void Publish(Message message, Tag tag)
        {
            Envelope envelope = new Envelope(message, this.ID);
            tag = tag.Join(Tag.Relay);
            MessageManager.Instance.Mediator.Publish(envelope, tag);
        }
        internal void Publish(Envelope envelope, Tag tag)
        {
            MessageManager.Instance.Mediator.Publish(envelope, tag);
        }
    }
    
    public abstract class ReceivablePublisher : Publisher, Receivable
    {
        internal ReceivablePublisher()
        {
        }

        public abstract void SetTagRule(Tag all);
        public abstract bool MatchTag(Tag tag);
        public abstract void OnReceive(Channel envelope);
    }
}