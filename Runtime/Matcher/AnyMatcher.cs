using System.Linq;

namespace DesertImage.ECS
{
    public partial class AnyMatcher : MatcherBase
    {
        public AnyMatcher(ushort componentId) : base(componentId)
        {
        }

        public AnyMatcher(ushort[] componentIds) : base(componentIds)
        {
        }

        public override bool IsMatch(IEntity entity)
        {
            return ComponentIds.Any(entity.HasComponent);
        }

        protected override bool IsEqualTypes(IMatcher other)
        {
            return other is AnyMatcher;
        }

        public static AnyMatcher GetMatcher(params ushort[] componentIds)
        {
            return new AnyMatcher(componentIds);
        }
    }
}