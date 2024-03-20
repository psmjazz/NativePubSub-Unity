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

        public void Publish(Envelope envelope, Tag tag)
        {
            if(envelope.HasReceiverID)
            {
                int receiverID = envelope.ReceiverID;
                if(idFilter.ContainsKey(receiverID))
                {
                    Channel channel = new ChannelConnection(envelope, receiverID, tag);
                    Receivable replied = idFilter[receiverID];
                    replied.OnReceive(channel);
                    
                }
                else
                {
                    Broadcast(envelope, tag);
                }
            }
            else
            {
                Broadcast(envelope, tag);
            }
        }

        private void Broadcast(Envelope envelope, Tag tag)
        {
            foreach(var node in idFilter.Values.Where(node => node.MatchTag(tag) && envelope.SenderID != node.ID))
            {
                Channel channel = new ChannelConnection(envelope, node.ID, tag);
                node.OnReceive(channel);
            }
        }
    }
}