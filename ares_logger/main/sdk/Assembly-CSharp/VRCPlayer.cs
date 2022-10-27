using Assembly_CSharp.VRC;
using Assembly_CSharp.VRC.Core;
using IL2CPP_Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembly_CSharp
{
    internal class VRCPlayer : VRCNetworkBehaviour
    {
        public VRCPlayer(IntPtr ptr) : base(ptr) { }

        public static new IL2Class Instance_Class = IL2CPP.AssemblyList["Assembly-CSharp"].GetClass(VRC.Player.Instance_Class.GetField("_vrcplayer").ReturnType.Name);

        public Player player
        {
            get
            {
                IL2Property property = Instance_Class.GetProperty(nameof(player));
                if (property == null)
                    (property = Instance_Class.GetProperty(Player.Instance_Class)).Name = nameof(player);
                return property?.GetGetMethod().Invoke(this)?.GetValue<Player>();
            }
        }

        public ApiAvatar AvatarModel
        {
            get
            {
                IL2Property property = Instance_Class.GetProperty(nameof(AvatarModel));
                if (property == null)
                    (property = Instance_Class.GetProperty(ApiAvatar.Instance_Class)).Name = nameof(AvatarModel);
                return property?.GetGetMethod().Invoke(this)?.GetValue<ApiAvatar>();
            }
        }

        public static VRCPlayer Instance
        {
            get
            {
                IL2Field field = Instance_Class.GetField(nameof(Instance));
                if (field == null)
                    (field = Instance_Class.GetField(x => x.Instance)).Name = nameof(Instance);
                return field.GetValue()?.GetValue<VRCPlayer>();
            }
        }

    }
}
