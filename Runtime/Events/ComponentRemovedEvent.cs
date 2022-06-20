using DesertImage.ECS;

namespace DesertImage
{
    public struct ComponentRemovedEvent
    {
        public IComponentHolder Holder;

        public IComponent Value;
    }
}