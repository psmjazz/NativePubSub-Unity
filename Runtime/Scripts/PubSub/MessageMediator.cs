using System.Collections;
using System.Collections.Generic;
using PJ.Native.PubSub;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.PubSub
{
    public interface MessageMediator
    {
        void Register(ReceivablePublisher node);
        void Publish(Message message, Tag tag);
        void Reply(Message message);

        
    }

}
