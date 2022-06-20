using System.Collections.Generic;
using DesertImage.ECS;
using UnityEngine;

namespace DesertImage
{
    [CreateAssetMenu(fileName = "SoundsLibrary", menuName = "Factories/SoundsLibrary")]
    public class ScriptableSoundsLibrary : ScriptableObject, IAwake, ISoundsLibrary
    {
        public List<SoundNode> Nodes;

        public void OnAwake()
        {
            var factorySound = Core.Instance.Get<FactorySound>();

            if (factorySound == null)
            {
#if DEBUG
                UnityEngine.Debug.LogError("[FactorySoundLibrary] there is no FactorySound");
#endif
                return;
            }

            foreach (var node in Nodes)
            {
                factorySound.Register(node);
            }
        }
    }
}