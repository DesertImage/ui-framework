namespace DesertImage.ECS
{
    public interface IMultiMatcher : IMatcher
    {
        IMatcher[] Matchers { get; }
    }
}