using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PJ.Native.Messenger
{
    public sealed class MessageHandler : MessageNode
    {
        private Dictionary<string, Action<MessageHolder>> handlerMap;

        public MessageHandler(Tag tag) : base(tag)
        {
            MessageManager.Instance.Mediator.Register(this);
            handlerMap = new Dictionary<string, Action<MessageHolder>>();
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
        }

        public void SetHandler(string key, Action<MessageHolder> handler)
        {
            handlerMap[key] = handler;
        }
    }
}