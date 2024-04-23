using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.PubSub
{
    public interface Receivable
    {
        void SetReceivingRule(Tag all);
        bool MatchTag(Tag tag);
        void OnReceive(EnvelopeHolder envelopeHolder);
    }

    public enum PublisherType
    {
        Android     = 10000,
        IOS         = 20000,
        Unity       = 30000,
        Unreal      = 40000
    }

    public class Publisher
    {
        private static class IDCounter
        {
            private static int id = (int) PublisherType.Unity;
            public static int GetID()
            {
                return ++id;
            }
        }

        private Tag baseTag = Tag.None;
        public readonly int ID = IDCounter.GetID();

        public void SetBasePublishingTag(Tag tag)
        {
            baseTag = tag;
        }

        public void Publish(Message message)
        {
            Envelope envelope = new Envelope(message, this.ID);
            MessageManager.Instance.Mediator.Publish(envelope, baseTag);
        }

        public void Publish(Message message, Tag tag)
        {
            Envelope envelope = new Envelope(message, this.ID);
            Tag joined = Tag.Join(tag, baseTag);
            MessageManager.Instance.Mediator.Publish(envelope, joined);
        }
        internal void Publish(Envelope envelope, Tag tag)
        {
            Tag joined = Tag.Join(tag, baseTag);
            MessageManager.Instance.Mediator.Publish(envelope, joined);
        }
    }
    
    public abstract class ReceivablePublisher : Publisher, Receivable
    {
        internal ReceivablePublisher()
        {
        }

        public abstract void SetReceivingRule(Tag all);
        public abstract bool MatchTag(Tag tag);
        public abstract void OnReceive(EnvelopeHolder envelopeHolder);
    }
}