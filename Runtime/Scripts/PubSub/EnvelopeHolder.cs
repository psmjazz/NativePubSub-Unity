using System.Collections;
using System.Collections.Generic;
using PJ.Native.Proto;
using PJ.Native.PubSub;
using UnityEngine;

public struct EnvelopeHolder
{
    public Envelope Envelope { get; set; }
    public Tag Tag{ get; set; }

    public EnvelopeHolder(Envelope envelope, Tag tag)
    {
        this.Envelope = envelope;
        this.Tag = tag;
    }

    internal void SerializeTag()
    {
        foreach(string tagName in Tag.Names)
        {
            Envelope.TagNames.Add(tagName);
        }
    }
}
