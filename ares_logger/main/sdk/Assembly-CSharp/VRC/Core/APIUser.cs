using IL2CPP_Core.Objects;
using System;

namespace Assembly_CSharp.VRC.Core
{
    public class APIUser : ApiModel
    {
        public APIUser(IntPtr ptr) : base(ptr) { }

        public static bool IsFriendsWith(string userId) => IsFriendsWith(new IL2String_utf8(userId));
        public static bool IsFriendsWith(IL2String userId)
        {
            return Instance_Class.GetMethod(nameof(IsFriendsWith)).Invoke<bool>(new IntPtr[] { userId.Pointer }).GetValue();
        }

        public string authToken
        {
            get => Instance_Class.GetProperty(nameof(authToken)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
        }

        public static new IL2Class Instance_Class = IL2CPP.AssemblyList["VRCCore-Standalone"].GetClass("APIUser", "VRC.Core");
    }
}
