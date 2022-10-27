using IL2CPP_Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assembly_CSharp
{
    public abstract class VRCNetworkBehaviour : MonoBehaviour
    {
        public VRCNetworkBehaviour(IntPtr ptr) : base(ptr) { }

        public int actor_id
        {
            get
            {
                IL2Property property = Instance_Class.GetProperty(nameof(actor_id));
                if (property == null)
                {
                    (property = Instance_Class.GetProperty(x => x.GetGetMethod().ReturnType.Name == typeof(int).FullName)).Name = nameof(actor_id);
                    if (property == null)
                        return default;
                }
                return property.GetGetMethod().Invoke<int>(this).GetValue();
            }
        }

        public static new IL2Class Instance_Class = VRCPlayer.Instance_Class.BaseType;
    }
}
