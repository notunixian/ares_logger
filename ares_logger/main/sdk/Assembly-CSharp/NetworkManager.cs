using IL2CPP_Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assembly_CSharp
{
    public class NetworkManager : MonoBehaviour
    {
        static NetworkManager()
        {
            var methodsPlayer = Instance_Class.GetMethods(x => x.GetParameters().Length == 1 && x.GetParameters()[0].ReturnType.Name == VRC.Player.Instance_Class.FullName);
            try
            {
                methodsPlayer[0].Name = "OnPlayerJoined";
            }
            catch { }

            try
            {
                methodsPlayer[2].Name = "OnPlayerLeft";
            }
            catch { }
        }
        public NetworkManager(IntPtr ptr) : base(ptr) { }



        public static new IL2Class Instance_Class = IL2CPP.AssemblyList["Assembly-CSharp"].GetClasses().FirstOrDefault(x => x.GetMethod("OnCustomAuthenticationResponse") != null && x.GetMethod("Awake") != null);
    }
}
