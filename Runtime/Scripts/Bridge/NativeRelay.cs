using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using PJ.Core.Util;
using PJ.Native.PubSub;
using PJ.Native.Proto;
using UnityEngine;
using System.Linq;

namespace PJ.Native.Bridge
{
    public class NativeRelay : Singleton<NativeRelay>
    {
        private INativeBridge bridge;
        private Messenger messenger;

        private void OnReceiveFromNative(byte[] rawData)
        {
            Envelope envelope = ToEnvelope(rawData);
            Tag tag = Tag.Named(envelope.TagNames);
            Tag unjoinedTag = tag.Unjoin(Tag.Relay);
            messenger.Publish(envelope, unjoinedTag);
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

        private void OnReceiveFromGame(Channel channel)
        {
            if(channel is ChannelConnection)
            {
                ChannelConnection connection = channel as ChannelConnection;
                connection.SerializeTag();
                bridge.Send(ToRawData(connection.Envelope));
            }    
        }

        internal void Start()
        {
            bridge = new NativeBridge();
            bridge.SetNativeDataListener(OnReceiveFromNative);
            messenger = new Messenger();
            Debug.Log("nativeRelay handler id : " + messenger.ID);
            messenger.SetTagRule(Tag.Relay);
            messenger.Subscribe(OnReceiveFromGame, (message) => true);
        }
    }

}

