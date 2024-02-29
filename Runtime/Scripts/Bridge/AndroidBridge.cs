#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using JetBrains.Annotations;
using UnityEngine;

namespace PJ.Native.Bridge
{
    public class AndroidBridgeProxy : AndroidJavaProxy
    {
        public NativeDataCallback DataCallback;

        public AndroidBridgeProxy() : base("com.pj.core.unity.NativeBridgeCallback")
        {
            
        }

        public void onReceive(sbyte[] sbytes)
        {
            byte[] bytes = new byte[sbytes.Length];
            for (int i = 0; i < sbytes.Length; i++)
            {
                bytes[i] = (byte) sbytes[i];
            }
            DataCallback?.Invoke(bytes);
        }
    }

    public class AndroidBridge : INativeBridge 
    {
        private AndroidBridgeProxy androidBridgeProxy;

        private Lazy<AndroidJavaObject> androidBridge = new Lazy<AndroidJavaObject>(()=>
        {
            AndroidJavaObject obj = new AndroidJavaObject("com.pj.core.unity.NativeBridge");
            return obj;
        }) ;

        public AndroidBridge()
        {
            androidBridgeProxy = new AndroidBridgeProxy();
            androidBridge.Value.Call("initialize", androidBridgeProxy);
        }

        public void SetNativeDataListener(NativeDataCallback listener)
        {
            androidBridgeProxy.DataCallback -= listener;
            androidBridgeProxy.DataCallback += listener;
        }

        public void Send(byte[] data)
        {
            sbyte[] sbytes = new sbyte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                sbytes[i] = (sbyte) data[i];
            }
            androidBridge.Value.Call("send", sbytes);
        }

    }
    
}

#endif