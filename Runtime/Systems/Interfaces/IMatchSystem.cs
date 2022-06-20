namespace DesertImage.ECS
{
    public interface IMatchSystem : ISystem
    {
        IMatcher Matcher { get; }
    }
}