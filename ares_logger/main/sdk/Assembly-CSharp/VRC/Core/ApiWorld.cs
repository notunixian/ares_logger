using IL2CPP_Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembly_CSharp.VRC.Core
{
    public class ApiWorld : ApiModel
    {
        public ApiWorld(IntPtr ptr) : base(ptr) { }

        public string currentInstanceIdWithTags
        {
            get => Instance_Class.GetField(nameof(currentInstanceIdWithTags)).GetValue(this)?.GetValue<IL2String>().ToString();
        }

        public string authorId
        {
            get => Instance_Class.GetProperty(nameof(authorId)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(authorId)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf8(value).Pointer });
        }

        public string releaseStatus
        {
            get => Instance_Class.GetProperty(nameof(releaseStatus)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(releaseStatus)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf8(value).Pointer });
        }

        public string assetUrl
        {
            get => Instance_Class.GetProperty(nameof(assetUrl)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(assetUrl)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf8(value).Pointer });
        }

        public string name
        {
            get => Instance_Class.GetProperty(nameof(name)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(name)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf16(value).Pointer });
        }

        public string authorName
        {
            get => Instance_Class.GetProperty(nameof(authorName)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(authorName)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf16(value).Pointer });
        }

        public string imageUrl
        {
            get => Instance_Class.GetProperty(nameof(imageUrl)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(imageUrl)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf8(value).Pointer });
        }

        public string description
        {
            get => Instance_Class.GetProperty(nameof(description)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(description)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf16(value).Pointer });
        }

        public string unityVersion
        {
            get => Instance_Class.GetProperty(nameof(unityVersion)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(unityVersion)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf16(value).Pointer });
        }

        public string thumbnailImageUrl
        {
            get => Instance_Class.GetProperty(nameof(thumbnailImageUrl)).GetGetMethod().Invoke(this)?.GetValue<IL2String>().ToString();
            set => Instance_Class.GetProperty(nameof(thumbnailImageUrl)).GetSetMethod().Invoke(this, new IntPtr[] { new IL2String_utf16(value).Pointer });
        }

        public string[] tags
        {
            get
            {
                IL2Object result = Instance_Class.GetProperty(nameof(tags)).GetGetMethod().Invoke(this);
                if (result == null)
                    return null;
                return new IL2ListObject<IL2String>(result.Pointer).ToArray().Select(x => x.ToString()).ToArray();
            }
        }

        public static new IL2Class Instance_Class = IL2CPP.AssemblyList["VRCCore-Standalone"].GetClass("ApiWorld", "VRC.Core");
    }
}
