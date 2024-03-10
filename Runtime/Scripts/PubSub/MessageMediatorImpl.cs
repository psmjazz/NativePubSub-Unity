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

        public void Publish(Message message, Tag tag)
        {
            if(message.Envelope.HasReceiverID)
            {
                int receiverID = message.Envelope.ReceiverID;
                if(idFilter.ContainsKey(receiverID))
                {
                    Receivable giveBacked = idFilter[receiverID];
                    MessageHolder holder = new MessagePostman(message);
                    giveBacked.OnReceive(holder);
                    
                }
            }
            else
            {
                MessageHolder holder = new MessagePostman(message);
                foreach(var node in idFilter.Values.Where(node => node.Tag.Contains(tag) && message.Envelope.SenderID != node.ID))
                {
                    node.OnReceive(holder);
                }
            }
        }

        private Receivable linkReceiver(Publisher publisher)
        {
            return idFilter[publisher.ID];
        } 

        public void Reply(Message message)
        {
            if(!message.Envelope.HasReceiverID)
                return;
            int receiverID = message.Envelope.ReceiverID;
            if(idFilter.ContainsKey(receiverID))
            {
                Receivable giveBacked = idFilter[receiverID];
                MessageHolder holder = new MessagePostman(message);
                giveBacked.OnReceive(holder);
                
            }
            else
            {
                MessageHolder holder = new MessagePostman(message);
                foreach(var node in idFilter.Values.Where(node => node.Tag.Contains(Tag.Game) && message.Envelope.SenderID != node.ID))
                {
                    node.OnReceive(holder);
                }
            }
        }
    }
}