using System.Collections;
using System.Collections.Generic;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.PubSub
{
    internal class MessagePostman : MessageHolder
    {
        public Message Message 
        {
            get;
            private set;
        }
        internal MessagePostman(Message message)
        {
            this.Message = message;
        }

        public void Reply(Message message)
        {
            MessageManager.Instance.Mediator.Reply(message);
        }
    }
}