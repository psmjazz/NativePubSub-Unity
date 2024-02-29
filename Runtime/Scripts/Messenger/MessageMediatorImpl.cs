using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PJ.Native.Proto;
using UnityEngine;
using UnityEngine.UIElements;

namespace PJ.Native.Messenger
{
    public class MessageMediatorImpl : MessageMediator
    {
        private Dictionary<int, MessageNode> idFilter;

        public MessageMediatorImpl()
        {
            idFilter = new Dictionary<int, MessageNode>();
        }

        public void Register(MessageNode node)
        {
            idFilter[node.ID] = node;
        }

        public void Notify(Message message, Tag tag, Notifier notifier)
        {
            MessageHolder holder = new MessagePostman(message, linkReceiver(notifier));
            foreach(var node in idFilter.Values.Where(node => node.Tag.Contains(tag)))
            {
                if(node.HasKey(message.Key) && notifier.ID != node.ID)
                    node.OnReceive(holder);
            }
        }

        private Receivable linkReceiver(Notifier publisher)
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