using System.Collections.Generic;
using Entitas;
using UniRx;
using UnityEngine;

public sealed class TurnSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.ActiveTurn.OnEntityAdded(); } }
    Pools _pools;
    //跨房间
    int _oldRoomId = -1, _roomId = -1;
    //玩家对象
    Entity _player;
    //行动方向
    Vector2 _dir;
    //行动位置
    Vector2 _pos;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Execute(List<Entity> entities)
    {
        init();
        //超出边界，直接返回

        //判断是否跨房间
        checkOutRoom(_pos);
        //玩家行为
        RoleBehaviour(_pos);
        _pools.input.DestroyEntity(entities.SingleEntity());
    }

    void init()
    {
        _player = _pools.core.controlableEntity;
        dirGet();
        _player.ReplaceDir(_dir);
        _pos = (Vector2)_player.view.controller.position + _dir;
    }

    void checkOutRoom(Vector2 pos)
    {
        _roomId = _player.room.roomId;
        _oldRoomId = _roomId;
        if (TurnExtension.outsideRoom((int)pos.x, (int)pos.y, _pools.core.itemBoard.roomList.rooms[_oldRoomId]))
        {
            Debug.Log("input roomid:" + _pools.input.input.roomId);
            _roomId = _pools.input.input.roomId - 1;
        }
        _player.ReplaceRoom(_roomId, _oldRoomId);
    }

    void RoleBehaviour(Vector2 pos)
    {
        move(pos);
    }

    void move(Vector2 pos)
    {
        _player.ReplacePosition(0,0,pos).AddMove(0);
    }

    void dirGet()
    {
        var input = _pools.input.input;
        int _roomId=-1;
        _pools.core.GetEntities()
            .ToObservable()
            .Where(x => x.hasAsset)
            .Where(x => x.asset.name == Res.Prefabs.player)
            .Subscribe(xx =>
            {
                _dir = InputExtension.getDir(new Vector3(input.x, input.y, 0) - xx.position.value);
            });
    }

}