using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJ.Native.Bridge
{
    public delegate void NativeDataCallback(byte[] data);
    public interface INativeBridge
    {
        void SetNativeDataListener(NativeDataCallback listener);
        void Send(byte[] data);
    }
}

 