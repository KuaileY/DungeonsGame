using Entitas;
using UnityEngine;

public sealed class InputSystem : ISetPool, IExecuteSystem,ICleanupSystem
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
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100);
            if (hit.collider != null)
            {
                var pos = hit.collider.transform.position;
                string name = hit.collider.gameObject.name;
                _pool.CreateEntity()
                    .AddInput(name,(int) pos.x, (int) pos.y);
            }
            else
            {
                _pool.CreateEntity()
                    .AddInput("NULL", 0, 0);
            }
        }
        if (Input.GetKeyDown("l"))
        {
            _pool.CreateEntity().IsLoadGame(true);
        }
        if (Input.GetKeyDown("s"))
        {
            Debug.Log("save");
        }
        if (Input.GetKeyDown("n"))
        {
            _pool.CreateEntity().IsNewGame(true);
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

