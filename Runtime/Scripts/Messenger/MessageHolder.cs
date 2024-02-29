using System.Collections;
using System.Collections.Generic;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.Messenger
{
    public interface MessageHolder
    {
        Message Message {get;}
        void GiveBack(Message message);
    }    
}
