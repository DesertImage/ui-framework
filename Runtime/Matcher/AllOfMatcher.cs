using DesertImage.Extensions;

namespace DesertImage.ECS
{
    public partial class AllOfMatcher : MatcherBase
    {
        public AllOfMatcher(ushort componentId) : base(componentId)
        {
        }

        public AllOfMatcher(ushort[] componentIds) : base(componentIds)
        {
        }

        protected override bool IsEqualTypes(IMatcher other)
        {
            return other is AllOfMatcher;
        }

        public static AllOfMatcher GetMatcher(params ushort[] componentIds)
        {
            return new AllOfMatcher(componentIds);
        }
    }
}