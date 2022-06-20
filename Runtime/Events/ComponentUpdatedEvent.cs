using DesertImage.ECS;

namespace DesertImage
{
    public struct ComponentUpdatedEvent
    {
        public IComponentHolder Holder;

        public IComponent Value;
    }
}