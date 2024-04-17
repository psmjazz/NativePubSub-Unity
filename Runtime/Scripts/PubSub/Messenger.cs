using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PJ.Native.Proto;
using UnityEngine;
using UnityEngine.UIElements;

namespace PJ.Native.PubSub
{
    public sealed class Messenger : ReceivablePublisher
    {
        private Tag allTag = Tag.None;
        private Dictionary<string, Action<Message>> handlerMap;
        private List<(Action<Message>, Predicate<Message>)> conditionHandlers;

        public Messenger()
        {
            MessageManager.Instance.Mediator.Register(this);
            handlerMap = new Dictionary<string, Action<Message>>();
            conditionHandlers = new List<(Action<Message>, Predicate<Message>)>();
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
            foreach((Action<Message> callback, Predicate<Message> condition) in conditionHandlers)
            {
                if(condition.Invoke(envelope.Message))
                {
                    callback.Invoke(envelope.Message);
                }
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

        public void Subscribe(Action<Message> handler, Predicate<Message> condition)
        {
            conditionHandlers.Add((handler, condition));
        }

        public void Unsubscribe(Action<Message> handler)
        {
            conditionHandlers.RemoveAll(conditionHandler => conditionHandler.Item1 == handler);
        }

    }
}