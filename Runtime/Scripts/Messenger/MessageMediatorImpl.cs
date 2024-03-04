using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PJ.Native.Proto;
using UnityEngine;
using UnityEngine.UIElements;

namespace PJ.Native.PubSub
{
    internal class MessageMediatorImpl : MessageMediator
    {
        private Dictionary<int, ReceivablePublisher> idFilter;

        public MessageMediatorImpl()
        {
            idFilter = new Dictionary<int, ReceivablePublisher>();
        }

        public void Register(ReceivablePublisher node)
        {
            idFilter[node.ID] = node;
        }

        public void Publish(Message message, Tag tag, Publisher notifier)
        {
            MessageHolder holder = new MessagePostman(message, linkReceiver(notifier));
            foreach(var node in idFilter.Values.Where(node => node.Tag.Contains(tag)))
            {
                if(node.HasKey(message.Key) && notifier.ID != node.ID)
                    node.OnReceive(holder);
            }
        }

        private Receivable linkReceiver(Publisher publisher)
        {
            return idFilter[publisher.ID];
        } 

        public void GiveBack(Message message, Receivable giveBacked)
        {
            MessageHolder holder = new MessagePostman(message);
            giveBacked.OnReceive(holder);
        }
    }
}