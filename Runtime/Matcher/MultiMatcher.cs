using System.Linq;

namespace DesertImage.ECS
{
    public class MultiMatcher : MatcherBase, IMultiMatcher
    {
        public MultiMatcher(IMatcher[] matchers) : base(matchers.SelectMany(x => x.ComponentIds).OrderBy(x => x).ToArray())
        {
            Matchers = matchers;
        }

        public IMatcher[] Matchers { get; }

        public override bool IsMatch(IEntity entity)
        {
            foreach (var matcher in Matchers)
            {
                if (!matcher.IsMatch(entity)) return false;
            }

            return true;
        }

        public override bool IsContainsComponent(ushort componentId)
        {
            foreach (var matcher in Matchers)
            {
                if (matcher.IsContainsComponent(componentId)) return true;
            }

            return false;
        }

        public override bool Equals(IMatcher other)
        {
            if (!(other is MultiMatcher multiMatcher)) return false;

            if (Matchers.Length != multiMatcher.Matchers.Length) return false;

            var matchesCount = 0;

            foreach (var matcher in Matchers)
            {
                foreach (var targetMatcher in multiMatcher.Matchers)
                {
                    if (!matcher.Equals(targetMatcher)) continue;

                    matchesCount++;
                }
            }

            return matchesCount == Matchers.Length;
        }

        public static MultiMatcher GetMatcher(params IMatcher[] matchers)
        {
            return new MultiMatcher(matchers);
        }
    }
}