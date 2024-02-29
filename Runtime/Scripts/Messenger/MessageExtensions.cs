using PJ.Native.Proto;
using Google.Protobuf;
using System.Collections.Generic;

namespace PJ.Native.Messenger
{
    public static class ContainerExtensions
    {
        public static void Add(this Container self, string key, bool value)
        {
            self.Booleans.Add(key, value);
        }
        public static void Add(this Container self, string key, int value)
        {
            self.Integers.Add(key, value);
        }
        public static void Add(this Container self, string key, float value)
        {
            self.Floats.Add(key, value);
        }
        public static void Add(this Container self, string key, string value)
        {
            self.Strings.Add(key, value);
        }
        public static void Add(this Container self, string key, byte[] value)
        {
            self.Bytes.Add(key, ByteString.CopyFrom(value));
        }
        public static void Add(this Container self, string key, Container container)
        {
            self.Containers.Add(key, container);
        }

        public static bool TryGetValue(this Container self, string key, out bool value)
        {
            return self.Booleans.TryGetValue(key, out value);
        }
        public static bool TryGetValue(this Container self, string key, out int value)
        {
            return self.Integers.TryGetValue(key, out value);
        }
        public static bool TryGetValue(this Container self, string key, out float value)
        {
            return self.Floats.TryGetValue(key, out value);
        }
        public static bool TryGetValue(this Container self, string key, out string value)
        {
            return self.Strings.TryGetValue(key, out value);
        }
        public static bool TryGetValue(this Container self, string key, out byte[] value)
        {
            if(self.Bytes.TryGetValue(key, out ByteString byteString))
            {
                value = byteString.ToByteArray();
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
        public static bool TryGetValue(this Container self, string key, out Container value)
        {
            return self.Containers.TryGetValue(key, out value);
        }
    }
}
