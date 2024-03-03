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
        // void RegisterType(MessageNode node, string messageType);
        // void Notify(Message message, Notifier notifier);
        void Notify(Message message, Tag tag, Publisher notifier);
        void GiveBack(Message message, Receivable giveBacked);
    }

}
