using System;

namespace DesertImage.ECS
{
    public interface IMatcher : IEquatable<IMatcher>, IPoolable, IDisposable
    {
        ushort Id { get; }
        
        ushort[] ComponentIds { get; }

        bool IsMatch(IEntity entity);

        bool IsContainsComponent(ushort componentId);
    }
}