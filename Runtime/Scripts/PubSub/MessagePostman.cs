using System.Collections;
using System.Collections.Generic;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.PubSub
{
    internal class MessagePostman : MessageHolder
    {
        private Receivable messageHost;
        public Message Message 
        {
            get;
            private set;
        }
        public MessagePostman(Message message)
        {
            this.Message = message;
            this.messageHost = null;
        }
        internal MessagePostman(Message message, Receivable receivable)
        {
            this.Message = message;
            this.messageHost = receivable;
        }

        public void GiveBack(Message message)
        {
            MessageManager.Instance.Mediator.GiveBack(message, messageHost);
        }
    }
}