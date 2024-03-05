using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using PJ.Core.Util;
using PJ.Native.PubSub;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.Bridge
{
    public class NativeRelay : Singleton<NativeRelay>
    {
        private INativeBridge bridge;
        private Messenger collector;

        private void OnReceiveFromNative(byte[] rawData)
        {
            collector.Publish(ToMessage(rawData), Tag.Game);
        }

        private byte[] ToRawData(Message message)
        {
            return message.ToByteArray();
        }

        private Message ToMessage(byte[] rawData)
        {
            var parsed = Message.Parser.ParseFrom(rawData);
            return parsed;
        }

        private void OnReceiveFromNative(MessageHolder messageHolder)
        {
            this.SendToNative(messageHolder.Message);
        }

        private void SendToNative(Message message)
        {
            bridge.Send(ToRawData(message));
        }

        internal void Start()
        {
            bridge = new NativeBridge();
            bridge.SetNativeDataListener(OnReceiveFromNative);
            collector = new Messenger(Tag.Native);
            collector.Subscribe(OnReceiveFromNative, (message) => true);
        }
    }

}

