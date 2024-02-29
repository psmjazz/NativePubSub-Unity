
namespace PJ.Native.Bridge
{
    public class NativeBridge : INativeBridge
    {
        private INativeBridge bridge;

        public NativeBridge()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            bridge = new AndroidBridge();
#elif UNITY_IOS && !UNITY_EDITOR
            bridge = new iOSBridge();
#else
            bridge = new EditorBridge();
#endif 
        }

        public void SetNativeDataListener(NativeDataCallback listener)
        {
            bridge?.SetNativeDataListener(listener);
        }
    
        public void Send(byte[] data)
        {
            bridge?.Send(data);
        }

    }
}
