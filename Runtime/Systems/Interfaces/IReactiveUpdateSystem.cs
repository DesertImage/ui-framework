namespace DesertImage.ECS
{
    public interface IReactiveUpdateSystem : IMatchSystem
    {
        bool IsTriggerComponent(IComponent component);

        void Execute(IEntity entity);
    }
}