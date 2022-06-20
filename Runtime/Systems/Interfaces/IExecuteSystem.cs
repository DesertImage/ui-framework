namespace DesertImage.ECS
{
    public interface IExecuteSystem : IMatchSystem
    {
        void Execute(IEntity entity);
    }
}