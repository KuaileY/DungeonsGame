using System.Collections.Generic;
using Entitas;
public class DropSystem:IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger { get { return InputMatcher.Destroy.OnEntityAdded(); } }
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            //内容
            _pool.DestroyEntity(entity);
        }
    }

}

