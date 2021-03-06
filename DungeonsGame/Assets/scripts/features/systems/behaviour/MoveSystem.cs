﻿using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;

public sealed class MoveSystem:IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Move.OnEntityAdded(); } }
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            move(e);
        }
    }

    void move(Entity entity)
    {
        entity.view.controller.gameObject.transform
            .DOMove(entity.position.value, Res.moveTime)
            .SetDelay(entity.move.delay)
            .SetEase(Ease.InOutSine);
        if (entity.isControlable)
            moveCamera(entity);
        entity.RemoveMove();  
    }

    void moveCamera(Entity entity)
    {
        var camera = _pool.cameraEntity;
        camera.position.value.x = entity.position.value.x;
        camera.position.value.y = entity.position.value.y;
        _pool.cameraEntity.AddMove(0.1f);
    }


}
