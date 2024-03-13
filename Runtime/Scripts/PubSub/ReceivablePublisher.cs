using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.PubSub
{
    public interface Receivable
    {
        bool HasKey(string key);
        void OnReceive(Envelope envelope);
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
        private Tag tag;
        public Tag Tag => tag;
        public int ID = IDCounter.GetID();

        public Publisher(Tag tag)
        {
            this.tag = tag;
        }

        public void Publish(Message message, Tag tag)
        {
            Envelope envelope = new Envelope(message, this.ID);
            MessageManager.Instance.Mediator.Publish(envelope, tag);
        }
        internal void Publish(Envelope envelope, Tag tag)
        {
            MessageManager.Instance.Mediator.Publish(envelope, tag);
        }
    }
    
    public abstract class ReceivablePublisher : Publisher, Receivable
    {
        internal ReceivablePublisher(Tag tag) : base(tag)
        {
        }

        public abstract bool HasKey(string key);

        public abstract void OnReceive(Envelope envelope);   
    }
}