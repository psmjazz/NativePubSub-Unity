using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJ.Native.Bridge
{
    public class EditorBridge : INativeBridge
    {
        private NativeDataCallback callback;
        public void SetNativeDataListener(NativeDataCallback listener)
        {
            callback -= listener;
            callback += listener;
        }

        public void Send(byte[] data)
        {
            callback?.Invoke(data);
        }
    }
}
