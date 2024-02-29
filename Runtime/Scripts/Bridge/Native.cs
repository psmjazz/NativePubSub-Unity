using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using PJ.Core.Util;
using PJ.Native.Messenger;
using PJ.Native.Proto;
using UnityEngine;

namespace PJ.Native.Bridge
{
    public class Native : Singleton<Native>
    {
        private INativeBridge bridge;
        private MessageCollector collector;

        private void OnReceiveFromNative(byte[] rawData)
        {
            collector.Notify(ToMessage(rawData), Tag.Game);
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
            collector = new MessageCollector(Tag.Native);
            collector.SetHandler(OnReceiveFromNative);
        }
    }

}

