
using System.Collections.Generic;
using UnityEngine;

public static class SpriteExtension
{
    static Dictionary<Sprite[], Dictionary<string, Sprite>> _sprites =
        new Dictionary<Sprite[], Dictionary<string, Sprite>>();

    public static Sprite GetSprite(this Sprite[] sprites, int index)
    {
        if (index >= 0 && index < sprites.Length)
            return sprites[index];
        return null;
    }

    public static Sprite GetSprite(this Sprite[] sprites, string spriteName)
    {
        Dictionary<string, Sprite> spriteDic = null;
        if (_sprites.Count != 0)
        {
            foreach (KeyValuePair<Sprite[], Dictionary<string, Sprite>> keyValuePair in _sprites)
            {
                if (keyValuePair.Key == sprites)
                    spriteDic = keyValuePair.Value;
                else
                    spriteDic = AddSpritesToDic(sprites);
            }
        }
        else
        {
            spriteDic = AddSpritesToDic(sprites);
        }

        return spriteDic[spriteName];
    }

    static Dictionary<string, Sprite> AddSpritesToDic(Sprite[] sprites)
    {
        var spriteDic = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Length; i++)
            spriteDic.Add(sprites[i].name, sprites[i]);
        _sprites.Add(sprites, spriteDic);
        return spriteDic;
    }

    public static T GetAsset<T>(string assetName) where T : Object
    {
        return Resources.Load<T>(assetName);
    }

    public static T Clone<T>(T obj) where T : Object
    {
        return Object.Instantiate(obj);
    }

    public static T Instantiate<T>(string assetName) where T : Object
    {
        return Clone(GetAsset<T>(assetName));
    }
}