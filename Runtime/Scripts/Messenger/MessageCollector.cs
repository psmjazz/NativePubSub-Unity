using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJ.Native.Messenger
{
    public sealed class MessageCollector : MessageNode
    {
        private Action<MessageHolder> handler;

        public MessageCollector(Tag tag) : base(tag)
        {
            MessageManager.Instance.Mediator.Register(this);
        }

        public override bool HasKey(string key)
        {
            return true;
        }

        public override void OnReceive(MessageHolder messageHolder)
        {
            this.handler?.Invoke(messageHolder);
        }

        public void SetHandler(Action<MessageHolder> handler)
        {
            this.handler = handler;
        }
    }
}