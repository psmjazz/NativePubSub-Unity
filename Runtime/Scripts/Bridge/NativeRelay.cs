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
        private Messenger messenger;

        private void OnReceiveFromNative(byte[] rawData)
        {
            messenger.Publish(ToEnvelope(rawData), Tag.Game);
        }

        private byte[] ToRawData(Envelope envelope)
        {
            return envelope.ToByteArray();
        }

        private Envelope ToEnvelope(byte[] rawData)
        {
            var parsed = Envelope.Parser.ParseFrom(rawData);
            return parsed;
        }

        private void OnReceiveFromNative(Channel channel)
        {
            if(channel is ChannelConnection)
            {
                ChannelConnection connection = channel as ChannelConnection;
                bridge.Send(ToRawData(connection.Envelope));
            }    
        }

        internal void Start()
        {
            bridge = new NativeBridge();
            bridge.SetNativeDataListener(OnReceiveFromNative);
            messenger = new Messenger(Tag.Native);
            messenger.Subscribe(OnReceiveFromNative, (message) => true);
        }
    }

}

