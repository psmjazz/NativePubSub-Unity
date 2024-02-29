using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.Messenger
{
    public interface Receivable
    {
        bool HasKey(string key);
        void OnReceive(MessageHolder messageHolder);
    }

    public class Notifier
    {
        private static class IDCounter
        {
            private static int id = 0;
            public static int GetID()
            {
                return id++;
            }
        }
        private Tag tag;
        public Tag Tag => tag;
        public int ID = IDCounter.GetID();

        public Notifier(Tag tag)
        {
            this.tag = tag;
        }

        public void Notify(Message message, Tag tag)
        {
            MessageManager.Instance.Mediator.Notify(message, tag, this);
        }
    }
    
    public abstract class MessageNode : Notifier, Receivable
    {
        public MessageNode(Tag tag) : base(tag)
        {
        }

        public abstract bool HasKey(string key);

        public abstract void OnReceive(MessageHolder messageHolder);   
    }
}