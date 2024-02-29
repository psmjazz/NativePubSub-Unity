using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJ.Native.Bridge
{
    public static class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            Native.Instance.Start();
        }
    }
}