using DesertImage.ECS;

namespace DesertImage
{
    public struct ComponentAddedEvent
    {
        public IComponentHolder Holder;
        public IComponent Value;

    }
}