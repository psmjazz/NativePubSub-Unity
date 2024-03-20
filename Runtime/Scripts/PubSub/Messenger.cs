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
        private Dictionary<string, Action<Channel>> handlerMap;
        private List<(Action<Channel>, Predicate<Message>)> conditionHandlers;

        public Messenger()
        {
            MessageManager.Instance.Mediator.Register(this);
            handlerMap = new Dictionary<string, Action<Channel>>();
            conditionHandlers = new List<(Action<Channel>, Predicate<Message>)>();
        }

        public override void SetTagRule(Tag all)
        {
            allTag = all;
        }

        public override bool MatchTag(Tag tag)
        {
            // if(this.allTag.Equals(Tag.None))
            //     return true;
            return tag.Contains(allTag);
        }

        public override void OnReceive(Channel channel)
        {
            if(handlerMap.TryGetValue(channel.Message.Key, out Action<Channel> handler))
            {
                handler.Invoke(channel);
            }
            foreach((Action<Channel> callback, Predicate<Message> condition) in conditionHandlers)
            {
                if(condition.Invoke(channel.Message))
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