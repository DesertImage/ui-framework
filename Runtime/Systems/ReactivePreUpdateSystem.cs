namespace DesertImage.ECS
{
    public abstract class ReactivePreUpdateSystem<TComponent> : SystemBase, IReactivePreUpdateSystem<TComponent>
        where TComponent : class, IComponent
    {
        public abstract IMatcher Matcher { get; }

        public abstract ushort GetTargetComponentId();

        public void Execute(IEntity entity, IComponent currentComponent, IComponent newValues)
        {
            Execute(entity, currentComponent as TComponent, newValues as TComponent);
        }

        public abstract void Execute(IEntity entity, TComponent currentComponent, TComponent futureComponent);
    }
}