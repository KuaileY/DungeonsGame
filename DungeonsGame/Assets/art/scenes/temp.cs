using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour {
    private static string bumpMap = "Sprites/dungeon/dungeontiles_NRM";
    // Use this for initialization
    void Start()
    {
        var tex = Resources.Load<Texture>("Textures/NormalMap/floor_2");
        var com = GetComponent<SpriteRenderer>();
        var mat = new Material(Shader.Find("Standard"));

        mat.SetFloat("_Glossiness", 0.5f);
        mat.SetTexture("_BumpMap", tex);
        mat.EnableKeyword("_NORMALMAP");
        com.material = mat;

    }

}
