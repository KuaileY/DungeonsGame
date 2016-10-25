using System.Collections.Generic;
using Entitas;

public sealed class RenderPositionSystem : ISetPools, IEntityCollectorSystem
{
    public EntityCollector entityCollector
    {
        get { return _entityCollector; }
    }

    EntityCollector _entityCollector;

    public void SetPools(Pools pools)
    {
        _entityCollector = new[] {pools.board,pools.core}
            .CreateEntityCollector(Matcher.AllOf(BoardMatcher.View, BoardMatcher.Position));
    }

    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("RenderPositionSystem Execute");
        foreach (var e in entities)
        {
            e.view.controller.position = e.position.value;
        }
    }

}

