using DesertImage.ECS;
using UnityEngine;

namespace Entities
{
    public abstract class EntityExtension : MonoBehaviour
    {
        [SerializeField] protected EntityMono Entity;

        protected virtual void OnValidate()
        {
            if (Entity != null) return;

            Entity = GetComponent<EntityMono>();
        }
    }
}