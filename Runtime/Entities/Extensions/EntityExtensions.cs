namespace DesertImage.ECS
{
    public static class EntityExtensions
    {
        public static bool IsInitialized(this IEntity entity)
        {
            return entity?.Components != null;
        }
    }
}