using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PJ.Native.PubSub
{
    public sealed class Messenger : ReceivablePublisher
    {
        private Dictionary<string, Action<MessageHolder>> handlerMap;
        private List<(Action<MessageHolder>, Predicate<MessageHolder>)> conditionHandlers;

        public Messenger(Tag tag) : base(tag)
        {
            MessageManager.Instance.Mediator.Register(this);
            handlerMap = new Dictionary<string, Action<MessageHolder>>();
            conditionHandlers = new List<(Action<MessageHolder>, Predicate<MessageHolder>)>();
        }

        public override bool HasKey(string key)
        {
            return handlerMap.  ContainsKey(key);
        }

        public override void OnReceive(MessageHolder messageHolder)
        {
            if(handlerMap.TryGetValue(messageHolder.Message.Key, out Action<MessageHolder> handler))
            {
                handler.Invoke(messageHolder);
            }
            foreach(var conditionHandler in conditionHandlers)
            {
                if(conditionHandler.Item2.Invoke(messageHolder))
                {
                    conditionHandler.Item1.Invoke(messageHolder);
                }
            }
        }

        public void Subscribe(string key, Action<MessageHolder> handler)
        {
            handlerMap[key] = handler;

        }

        public void UnSubscribe(string key)
        {
            handlerMap.Remove(key);
        }

        public void Subscribe(Action<MessageHolder> handler, Predicate<MessageHolder> condition)
        {
            conditionHandlers.Add((handler, condition));
        }

        public void UnSubscribe(Action<MessageHolder> handler)
        {
            conditionHandlers.RemoveAll(conditionHandler => conditionHandler.Item1 == handler);
        }
    }
}