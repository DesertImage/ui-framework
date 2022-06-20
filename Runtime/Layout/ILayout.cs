using System;
using UnityEngine;

namespace DesertImage.UI
{
    public interface ILayout
    {
        Action OnPreUpdate { get; set; }
        Action OnUpdate { get; set; }
        
        float Spacing { get; }

        Transform Content { get; }

        void Align();
    }
}