using Entitas;
using UnityEngine;


public sealed class TestInputSystem : ISetPool, IExecuteSystem, ICleanupSystem
{
    Pool _pool;
    Group _group;
    public void SetPool(Pool pool)
    {
        _pool = pool;
        _group = pool.GetGroup(InputMatcher.Input);
    }

    public void Execute()
    {
        var input = Input.GetMouseButtonDown(0);
        if (input)
        {
            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (col.Length > 0)
            {
                foreach (Collider2D c in col)
                {
                    c.name.print();
                }
            }
            Debug.Log("mouse down");
            _pool.CreateEntity().IsTest(true);
        }

    }

    public void Cleanup()
    {
        foreach (var e in _group.GetEntities())
        {
            _pool.DestroyEntity(e);
        }
    }
}

