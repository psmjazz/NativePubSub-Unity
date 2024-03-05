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
        private Dictionary<string, Action<MessageHolder>> handlerMap;
        private List<(Action<MessageHolder>, Predicate<Message>)> conditionHandlers;

        public Messenger(Tag tag) : base(tag)
        {
            MessageManager.Instance.Mediator.Register(this);
            handlerMap = new Dictionary<string, Action<MessageHolder>>();
            conditionHandlers = new List<(Action<MessageHolder>, Predicate<Message>)>();
        }

        public override bool HasKey(string key)
        {
            return handlerMap.ContainsKey(key);
        }

        public override void OnReceive(MessageHolder messageHolder)
        {
            if(handlerMap.TryGetValue(messageHolder.Message.Key, out Action<MessageHolder> handler))
            {
                handler.Invoke(messageHolder);
            }
            foreach((Action<MessageHolder> callback, Predicate<Message> condition) in conditionHandlers)
            {
                if(condition.Invoke(messageHolder.Message))
                {
                    callback.Invoke(messageHolder);
                }
            }
        }

        public void Subscribe(string key, Action<MessageHolder> handler)
        {
            handlerMap[key] = handler;

        }

        public void Unsubscribe(string key)
        {
            handlerMap.Remove(key);
        }

        public void Subscribe(Action<MessageHolder> handler, Predicate<Message> condition)
        {
            conditionHandlers.Add((handler, condition));
        }

        public void Unsubscribe(Action<MessageHolder> handler)
        {
            conditionHandlers.RemoveAll(conditionHandler => conditionHandler.Item1 == handler);
        }
    }
}