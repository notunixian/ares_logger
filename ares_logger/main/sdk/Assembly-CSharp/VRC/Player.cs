using Assembly_CSharp.VRC.Core;
using IL2CPP_Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assembly_CSharp.VRC
{
    internal class Player : MonoBehaviour
    {
        public Player(IntPtr ptr) : base(ptr) { }

        public VRCPlayer vrc_player
        {
            get
            {
                IL2Property property = Instance_Class.GetProperty(nameof(vrc_player));
                if (property == null)
                    (property = Instance_Class.GetProperty(VRCPlayer.Instance_Class)).Name = nameof(vrc_player);
                return property?.GetGetMethod().Invoke(this)?.GetValue<VRCPlayer>();
            }
        }

        public APIUser api_user
        {
            get
            {
                IL2Property property = Instance_Class.GetProperty(nameof(api_user));
                if (property == null)
                    (property = Instance_Class.GetProperty(APIUser.Instance_Class)).Name = nameof(api_user);
                return property?.GetGetMethod().Invoke(this)?.GetValue<APIUser>();
            }
        }


        public static new IL2Class Instance_Class = IL2CPP.AssemblyList["Assembly-CSharp"].GetClasses().FirstOrDefault(x => x.GetField("_USpeaker") != null);
    }
}
