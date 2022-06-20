namespace DesertImage.ECS
{
    public interface IReactEntityRemovedSystem : IMatchSystem
    {
        void Execute(IEntity entity);
    }
}