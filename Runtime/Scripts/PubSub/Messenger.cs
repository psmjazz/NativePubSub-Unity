using System;
using System.Collections.Generic;
using PJ.Native.Proto;

namespace PJ.Native.PubSub
{
    public sealed class Messenger : ReceivablePublisher
    {
        private Tag allTag = Tag.None;
        private readonly Dictionary<string, Action<Message>> handlerMap;
        private readonly List<Action<Message>> handlerList;

        public Messenger()
        {
            MessageManager.Instance.Mediator.Register(this);
            handlerMap = new Dictionary<string, Action<Message>>();
            handlerList = new List<Action<Message>>();
        }

        public override void SetReceivingRule(Tag all)
        {
            allTag = all;
        }

        public override bool MatchTag(Tag tag)
        {
            // if(this.allTag.Equals(Tag.None))
            //     return true;
            return tag.Contains(allTag);
        }

        public override void OnReceive(EnvelopeHolder envelopeHolder)
        {
            Envelope envelope = envelopeHolder.Envelope;
            if(handlerMap.TryGetValue(envelope.Message.Key, out Action<Message> handler))
            {
                handler.Invoke(envelope.Message);
            }
            foreach(Action<Message> callback in handlerList)
            {
                callback.Invoke(envelope.Message);
            }
        }

        public void Subscribe(string key, Action<Message> handler)
        {
            handlerMap[key] = handler;

        }

        public void Unsubscribe(string key)
        {
            handlerMap.Remove(key);
        }

        public void Subscribe(Action<Message> handler)
        {
            handlerList.Add(handler);
        }

        public void Unsubscribe(Action<Message> handler)
        {
            handlerList.RemoveAll(member => member == handler);
        }

    }
}