using System;
using Entitas;
using UnityEngine;

public static class CreateObjectPoolExtension
{
    public static MyObjectPool<GameObject> createObjectPool()
    {
        var indeies = ObjectsIndeies.indexNames;
        var objectPool = new MyObjectPool<GameObject>();
        Transform parent = new GameObject("ObjectsPool").transform;
        for (int i = 0; i < indeies.Length; i++)
        {
            string path = String.Empty;
            if (indeies[i][0] == 'I')
                path = Res.PrefabPath + "Items/" + indeies[i];
            if (indeies[i][0] == 'M')
                path = Res.PrefabPath + "Monster/" + indeies[i];

            if (path == String.Empty)
                throw new Exception("objectPoolName is wrong!");

            var go = AssetsExtension.Instantiate<GameObject>(path);
            go.SetActive(false);
            go.transform.SetParent(parent);
            objectPool.Pop(go, indeies[i]);
        }
        return objectPool;
    }

}

