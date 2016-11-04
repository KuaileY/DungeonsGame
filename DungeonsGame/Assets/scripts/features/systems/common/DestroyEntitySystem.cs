using System.Collections.Generic;
using Entitas;
public sealed class DestroyEntitySystem:ISetPools, IEntityCollectorSystem
{
    public EntityCollector entityCollector { get { return _entityCollector; } }
    Pool[] _pools;
    EntityCollector _entityCollector;
    public void SetPools(Pools pools)
    {
        _pools = new[] { pools.input,pools.core};
        _entityCollector = _pools.CreateEntityCollector(Matcher.AnyOf(InputMatcher.Destroy, CoreMatcher.Destroy));
    }

    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            foreach (var pool in _pools)
            {
                pool.DestroyEntity(e);
                break;
            }
        }
    }

}

