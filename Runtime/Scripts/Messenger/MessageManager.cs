using System;
using System.Collections;
using System.Collections.Generic;
using PJ.Core.Util;
using UnityEngine;

namespace PJ.Native.Messenger
{
    public class MessageManager : Singleton<MessageManager>
    {
        private Lazy<MessageMediator> lazyMediator = new Lazy<MessageMediator>(()=> new MessageMediatorImpl());
        public MessageMediator Mediator => lazyMediator.Value;
    }
}