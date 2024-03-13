using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.PubSub
{
    public sealed class Messenger : ReceivablePublisher
    {
        private Dictionary<string, Action<Channel>> handlerMap;
        private List<(Action<Channel>, Predicate<Message>)> conditionHandlers;

        public Messenger(Tag tag) : base(tag)
        {
            MessageManager.Instance.Mediator.Register(this);
            handlerMap = new Dictionary<string, Action<Channel>>();
            conditionHandlers = new List<(Action<Channel>, Predicate<Message>)>();
        }

        public override bool HasKey(string key)
        {
            return handlerMap.ContainsKey(key);
        }

        public override void OnReceive(Envelope envelope)
        {
            Channel channel = new ChannelConnection(envelope, this.ID);
            if(handlerMap.TryGetValue(envelope.Message.Key, out Action<Channel> handler))
            {
                handler.Invoke(channel);
            }
            foreach((Action<Channel> callback, Predicate<Message> condition) in conditionHandlers)
            {
                if(condition.Invoke(envelope.Message))
                {
                    callback.Invoke(channel);
                }
            }
        }

        public void Subscribe(string key, Action<Channel> handler)
        {
            handlerMap[key] = handler;

        }

        public void Unsubscribe(string key)
        {
            handlerMap.Remove(key);
        }

        public void Subscribe(Action<Channel> handler, Predicate<Message> condition)
        {
            conditionHandlers.Add((handler, condition));
        }

        public void Unsubscribe(Action<Channel> handler)
        {
            conditionHandlers.RemoveAll(conditionHandler => conditionHandler.Item1 == handler);
        }
    }
}