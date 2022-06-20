namespace DesertImage.ECS
{
    public abstract class ExecuteSystem : SystemBase, IExecuteSystem
    {
        public abstract IMatcher Matcher { get; }

        public abstract void Execute(IEntity entity);
    }
}