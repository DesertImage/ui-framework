namespace DesertImage.ECS
{
    public abstract class EndSystem : SystemBase, IEndSystem
    {
        public abstract void ExecuteEnd();
    }
}