using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public sealed class BuildMaterial : EditorWindow
{
    private static Texture2D selectedTexture;
    private static TextureImporter importer;

    private static BuildMaterial window;
    public static string bumpMap = "/art/Resources/Textures/NormalMap";
    public static string outPath = "Assets/art/Resources/Material/dungeon/";

    [MenuItem("Assets/Build Material")]
    static void buildMaterial()
    {
        window = (BuildMaterial)EditorWindow.GetWindow(typeof (BuildMaterial), false, "BuildMaterial", false);
        window.Show();
    }

    void OnGUI()
    {

        bumpMap = EditorGUILayout.TextField("bumpMap path:", bumpMap);
        outPath = EditorGUILayout.TextField("out path:", outPath);
        if (GUILayout.Button("generate"))
        {
            generate();
        }
    }

    static void generate()
    {
        Debug.Log("generate");
        var path = Application.dataPath + bumpMap;
        DirectoryInfo dictorys = new DirectoryInfo(path);
        FileInfo[] images = dictorys.GetFiles("*.png");

        for (int i = 0; i < images.Length ; i++)
        {
            var name = images[i].Name.Substring(0, images[i].Name.Length - 4);
            var tex = Resources.Load<Texture>("Textures/NormalMap/"+name);
            var mat = new Material(Shader.Find("Standard"));

            if (name.Substring(0, 5) == "water")
                mat.SetFloat("_Glossiness", 0.8f);
            else
                mat.SetFloat("_Glossiness", 0.1f);

            mat.SetTexture("_BumpMap", tex);
            mat.EnableKeyword("_NORMALMAP");
            AssetDatabase.CreateAsset(mat, outPath + name + ".mat");
        }
        Debug.Log("finish!!!");
    }


}

