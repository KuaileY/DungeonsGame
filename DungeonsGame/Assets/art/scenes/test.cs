using System.Collections;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    aa();
	}

    void aa(Vector2 pos=default(Vector2))
    {
        pos.x.ToString().print();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
