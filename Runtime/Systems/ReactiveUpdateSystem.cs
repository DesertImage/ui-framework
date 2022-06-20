namespace DesertImage.ECS
{
    public abstract class ReactiveUpdateSystem : SystemBase, IReactiveUpdateSystem
    {
        public abstract IMatcher Matcher { get; }

        public abstract bool IsTriggerComponent(IComponent component);

        public abstract void Execute(IEntity entity);
    }
}