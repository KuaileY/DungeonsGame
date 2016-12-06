
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportSprites : Editor
{
    private static Texture2D selectedTexture;
    private static TextureImporter importer;
    private static Sprite[] spriteResources;
    private static string path;
    private static string assetPath;

    [MenuItem("Assets/Export Sprites")]
    static void exportSprites()
    {
        UseSelectedTexture();
        var outPath=Application.dataPath+assetPath.Substring(assetPath.IndexOf('/'), assetPath.LastIndexOf('/') - assetPath.IndexOf('/'));
        foreach (var sprite in spriteResources)
        {
            Texture2D tex = sprite.texture;
            Rect r = sprite.textureRect;
            Texture2D subtex = tex.CropTexture((int) r.x, (int) r.y, (int) r.width, (int) r.height);
            byte[] data = subtex.EncodeToPNG();
            File.WriteAllBytes(outPath + "/" + sprite.name + ".png", data);
        }
    }

    private static void UseSelectedTexture()
    {
        if (Selection.objects.Length > 1)
            selectedTexture = null;
        else
            selectedTexture = Selection.activeObject as Texture2D;

        if (selectedTexture != null)
        {
            assetPath = AssetDatabase.GetAssetPath(selectedTexture);
            importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer)
            {
                if (importer.spriteImportMode != SpriteImportMode.Multiple)
                {
                    EditorUtility.DisplayDialog("Error", "SpriteImportMode must be Multiple !", "OK", "");
                    return;
                }

                int startPoint = assetPath.IndexOf("Resources/");
                path = assetPath.Substring(startPoint + 10, assetPath.Length - startPoint - 10);

                spriteResources = Resources.LoadAll<Sprite>(path.Substring(0, path.Length - 4));

            }
        }

    }

}

static class Texture2DExtensions
{
    public static Texture2D CropTexture(this Texture2D pSource, int left, int top, int width, int height)
    {
        if (left < 0)
        {
            width += left;
            left = 0;
        }
        if (top < 0)
        {
            height += top;
            top = 0;
        }
        if (left + width > pSource.width)
        {
            width = pSource.width - left;
        }
        if (top + height > pSource.height)
        {
            height = pSource.height - top;
        }

        if (width <= 0 || height <= 0)
        {
            return null;
        }

        Color[] aSourceColor = pSource.GetPixels(0);

        //*** Make New
        Texture2D oNewTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        //*** Make destination array
        int xLength = width * height;
        Color[] aColor = new Color[xLength];

        int i = 0;
        for (int y = 0; y < height; y++)
        {
            int sourceIndex = (y + top) * pSource.width + left;
            for (int x = 0; x < width; x++)
            {
                aColor[i++] = aSourceColor[sourceIndex++];
            }
        }

        //*** Set Pixels
        oNewTex.SetPixels(aColor);
        oNewTex.Apply();

        //*** Return
        return oNewTex;
    }
}


