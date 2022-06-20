namespace DesertImage.ECS
{
    public abstract class InitSystem : SystemBase, IInitSystem
    {
        public abstract void Initialize();
    }
}