using IL2CPP_Core.Objects;
using Photon;
using System;
using System.Linq;


namespace Assembly_CSharp.VRC.Core
{
    internal class VRCNetworkingClient : LoadBalancingClient
    {
        public VRCNetworkingClient(IntPtr ptr) : base(ptr) { }

        public static new IL2Class Instance_Class = IL2CPP.AssemblyList["Assembly-CSharp"].GetClasses().First(x => x.GetField(y => y.Instance) != null && x.GetMethod("OnEvent") != null);
    }
}
