using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
        var input = Input.GetMouseButtonDown(0);
	    if (input)
	    {
	        //AstarExtension.PathFind(new Vector2(0, 0), new Vector2(31, 31));

	    }

    }
}
