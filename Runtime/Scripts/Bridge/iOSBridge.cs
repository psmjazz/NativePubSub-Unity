#if UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace PJ.Native.Bridge
{
    public class iOSBridge : INativeBridge
    {
        private delegate void ObjcDataCallback(IntPtr ptr, int length);
        private static NativeDataCallback dataCallback;
        [MonoPInvokeCallback(typeof(ObjcDataCallback))]
        private static void OniOSEvent(IntPtr ptr, int length)
        {
            byte[] data = new byte[length];
            Marshal.Copy(ptr, data, 0, length);
            dataCallback?.Invoke(data);
        }

        [DllImport("__Internal")]
        private static extern void __iOSInitialize(ObjcDataCallback del);
        [DllImport("__Internal")]
        private static extern void __iOSSend(IntPtr data, int length);

        public iOSBridge()
        {
            __iOSInitialize(OniOSEvent);
        }

        public void SetNativeDataListener(NativeDataCallback listener)
        {
            dataCallback -= listener;
            dataCallback += listener;
        }

        public void Send(byte[] data)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            __iOSSend(handle.AddrOfPinnedObject(), data.Length);
            handle.Free();
        }

    }
}

#endif