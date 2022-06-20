using DesertImage.Extensions;

namespace DesertImage.ECS
{
    public class NoneOfMatcher : MatcherBase
    {
        public NoneOfMatcher(ushort componentId) : base(componentId)
        {
        }

        public NoneOfMatcher(ushort[] componentIds) : base(componentIds)
        {
        }

        public override bool IsMatch(IEntity entity)
        {
            for (var i = 0; i < ComponentIds.Length; i++)
            {
                if (!entity.HasComponent(ComponentIds[i])) continue;

                return false;
            }

            return true;
        }

        public override bool IsContainsComponent(ushort componentId)
        {
            return ComponentIds.Contains(componentId, (arg1, arg2) => arg1 == arg2);
        }

        protected override bool IsEqualTypes(IMatcher other)
        {
            return other is NoneOfMatcher;
        }

        public static NoneOfMatcher GetMatcher(params ushort[] componentIds)
        {
            return new NoneOfMatcher(componentIds);
        }
    }
}