using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class LoadSpritesSystem:IInitializeSystem,ISetPool
{
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Initialize()
    {
        Dictionary<string, Sprite[]> spDic = new Dictionary<string, Sprite[]>();
        for (int i = 0; i < Res.maps.Length; i++)
        {
            var sprites = Resources.LoadAll<Sprite>(Res.mapsTexturePath + Res.maps[i]);
            spDic.Add(Res.maps[i], sprites);
        }
        _pool.CreateEntity().AddSpriteList(spDic);
    }


}

