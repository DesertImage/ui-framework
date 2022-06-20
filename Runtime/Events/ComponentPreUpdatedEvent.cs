using DesertImage.ECS;

namespace DesertImage
{
    public struct ComponentPreUpdatedEvent
    {
        public IComponentHolder Holder;

        public IComponent PreviousValue;
        public IComponent FutureValue;
    }
}