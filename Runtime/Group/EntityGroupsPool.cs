using System.Collections.Generic;
using DesertImage.ECS;
using DesertImage.External;
using DesertImage.Pools;

namespace Group
{
    public class EntityGroupsPool : Pool<EntityGroup>
    {
        private ushort _idsCounter;

        protected override EntityGroup CreateInstance()
        {
            return new EntityGroup
            {
                Id = _idsCounter++,
                Entities = new CustomList<IEntity>()
            };
        }
    }
}