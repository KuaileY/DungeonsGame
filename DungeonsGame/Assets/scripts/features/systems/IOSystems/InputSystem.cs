﻿using Entitas;
using UnityEngine;

public sealed class InputSystem : ISetPool, IExecuteSystem
{
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }

    public void Execute()
    {
        var input = Input.GetMouseButtonDown(0);
        if (input)
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100);
            if (hit.collider != null)
            {
                var pos = hit.collider.transform.position;
                string name = hit.collider.gameObject.name;
                int roomID = int.Parse(name.Substring(0, name.IndexOf(',')));
                _pool.CreateEntity()
                    .AddInput(roomID,(int) pos.x, (int) pos.y);
            }
            Debug.Log("mouseDown");
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

}

