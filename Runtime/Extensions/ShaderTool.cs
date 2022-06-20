using UnityEngine;

namespace DesertImage.Extensions
{
    public static class ShaderTool
    {
        public static readonly int Color = Shader.PropertyToID("_Color");
        public static readonly int PaintColor = Shader.PropertyToID("_PaintColor");
        public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        public static readonly int MainText = Shader.PropertyToID("_MainTex");
    }
}