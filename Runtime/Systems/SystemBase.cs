namespace DesertImage.ECS
{
    public abstract class SystemBase : ISystem
    {
        public bool IsActive { get; protected set; }

        protected IWorld World;

        public virtual void Activate()
        {
            IsActive = true;
        }

        public virtual void Deactivate()
        {
            IsActive = false;
        }

        public virtual void Init(IWorld world)
        {
            World = world;
        }
    }
}