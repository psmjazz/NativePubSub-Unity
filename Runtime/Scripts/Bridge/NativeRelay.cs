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
        private Bridge bridgeMessenger;

        private void OnReceiveFromNative(byte[] rawData)
        {
            Envelope envelope = ToEnvelope(rawData);
            Tag tag = Tag.Named(envelope.TagNames);
            bridgeMessenger.Publish(envelope, tag);
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

        private void OnReceiveFromGame(EnvelopeHolder envelopeHolder)
        {
            envelopeHolder.SerializeTag();
            bridge.Send(ToRawData(envelopeHolder.Envelope)); 
        }

        internal void Start()
        {
            bridge = new NativeBridge();
            bridge.SetNativeDataListener(OnReceiveFromNative);
            bridgeMessenger = new Bridge();
            Debug.Log("nativeRelay handler id : " + bridgeMessenger.ID);
            bridgeMessenger.SetTagRule(Tag.Native);
            bridgeMessenger.Subscribe(OnReceiveFromGame);
        }
    }

}

