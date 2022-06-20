namespace DesertImage.ECS
{
    public interface IReactivePreUpdateSystem : IMatchSystem
    {
        ushort GetTargetComponentId();

        void Execute(IEntity entity, IComponent currentComponent, IComponent newValues);
    }

    public interface IReactivePreUpdateSystem<in TValue> : IReactivePreUpdateSystem
    {
        void Execute(IEntity entity, TValue currentComponent, TValue futureComponent);
    }
}