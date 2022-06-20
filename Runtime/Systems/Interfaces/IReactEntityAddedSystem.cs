namespace DesertImage.ECS
{
    public interface IReactEntityAddedSystem : IMatchSystem
    {
        void Execute(IEntity entity);
    }
}