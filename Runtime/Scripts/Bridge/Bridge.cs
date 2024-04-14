using System;
using PJ.Native.Proto;
using PJ.Native.PubSub;

namespace PJ.Native.Bridge
{
    internal sealed class Bridge : ReceivablePublisher
    {
        private Tag allTag = Tag.None;
        private Action<EnvelopeHolder> handler;

        internal Bridge()
        {
            MessageManager.Instance.Mediator.Register(this);
        }

        public override bool MatchTag(Tag tag)
        {
            return tag.Contains(allTag);
        }

        public override void OnReceive(EnvelopeHolder envelope)
        {
            this.handler?.Invoke(envelope);
        }

        public override void SetTagRule(Tag all)
        {
            this.allTag = all;
        }

        public void Subscribe(Action<EnvelopeHolder> handler)
        {
            this.handler -= handler;
            this.handler += handler;
        }

        public void Unsubscribe(Action<EnvelopeHolder> handler)
        {
            this.handler -= handler;
        }
    }
}