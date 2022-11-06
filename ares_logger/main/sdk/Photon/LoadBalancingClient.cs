using ares_logger.main.util;
using Assembly_CSharp.VRC.Core;
using IL2CPP_Core.Objects;
using System;

namespace Photon
{
    internal class LoadBalancingClient : IL2Object
    {
        public LoadBalancingClient(IntPtr ptr) : base(ptr) { }

        public static IL2Class Instance_Class = VRCNetworkingClient.Instance_Class.BaseType;
    }
}
