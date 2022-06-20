namespace DesertImage.ECS
{
    public static class Match
    {
        public static IMatcher AllOf(params ushort[] componentIds)
        {
            return AllOfMatcher.GetMatcher(componentIds);
        }

        public static IMatcher Any(params ushort[] componentIds)
        {
            return AnyMatcher.GetMatcher(componentIds);
        }

        public static IMatcher NoneOf(params ushort[] componentIds)
        {
            return NoneOfMatcher.GetMatcher(componentIds);
        }

        public static IMatcher Multi(params IMatcher[] matchers)
        {
            return MultiMatcher.GetMatcher(matchers);
        }

        public static IMatcher MultiOr(params IMatcher[] matchers)
        {
            return MultiOrMatcher.GetMatcher(matchers);
        }
    }
}