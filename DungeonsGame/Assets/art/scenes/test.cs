using System.Collections;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GameObject go = Resources.Load<GameObject>(Res.PrefabPath+Res.Prefabs.food);
	    Object.Instantiate(go, new Vector2(0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
