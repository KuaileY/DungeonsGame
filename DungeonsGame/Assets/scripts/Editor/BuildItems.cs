using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class BuildItems:Editor
{
    private static Texture2D selectedTexture;
    private static TextureImporter importer;
    private static Sprite[] spriteResources;
    private static string[] clipTypes = new string[] { "close", "open" };
    private static string[] prefabTypes = new string[] { "Items" };

    [MenuItem("Assets/Build Items")]
    static void buildItems()
    {
        UseSelectedTexture();
        var spriteGroups = spriteResources.GroupBy(x => x.name.Substring(0, x.name.Length - 5)).ToList();
        foreach (var spriteGroup in spriteGroups)
        {
            List<AnimationClip> clips = new List<AnimationClip>();
            int i = 0;
            foreach (var clip in clipTypes)
            {
                clips.Add(BuildAnimatinoClip(spriteGroup.ToArray<Sprite>(), clip,i));
                i++;
            }
            var controller = BuildAnimationController(clips, spriteGroup.Key);
            BuildPrefab(controller, spriteGroup.First(), prefabTypes[0], spriteGroup.Key);
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
            var assetPath = AssetDatabase.GetAssetPath(selectedTexture);
            importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer)
            {
                if (importer.spriteImportMode != SpriteImportMode.Multiple)
                {
                    EditorUtility.DisplayDialog("Error", "SpriteImportMode must be Multiple !", "OK", "");
                    return;
                }

                int startPoint = assetPath.IndexOf("Resources/");
                var path = assetPath.Substring(startPoint + 10, assetPath.Length - startPoint - 10);

                spriteResources = Resources.LoadAll<Sprite>(path.Substring(0, path.Length - 4));

            }
        }

    }

    private static AnimationClip BuildAnimatinoClip(Sprite[] sprites, string clipType,int i)
    {
        var animationClip = new AnimationClip { frameRate = 12 };
        var animationClipSettings = new AnimationClipSettings();

        var curveBinding = new EditorCurveBinding();
        curveBinding.path = "";
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.propertyName = "m_Sprite";

        animationClipSettings.loopTime = true;
        var objectReferenceKeyframes = new ObjectReferenceKeyframe[1];

        int num = -1;
        if (sprites.Length > 1)
            num = i;
        else
            num = 0;

        objectReferenceKeyframes[0] = new ObjectReferenceKeyframe
        {
            value = sprites[num],
            time = 1.0f,
        };

        AnimationUtility.SetAnimationClipSettings(animationClip, animationClipSettings);
        AnimationUtility.SetObjectReferenceCurve(animationClip, curveBinding, objectReferenceKeyframes);
        var name = sprites[0].name.Substring(0, sprites[0].name.Length - 5);
        AssetDatabase.CreateAsset(animationClip, Res.editorPath + Res.AnimationPath + name + clipType + ".anim");
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(animationClip);
        return animationClip;
    }

    private static AnimatorController BuildAnimationController(List<AnimationClip> clips, string name)
    {
        var animatorController =
            AnimatorController.CreateAnimatorControllerAtPath(Res.editorPath + Res.AnimationControllerPath + name +
                                                              ".controller");
        AnimatorControllerLayer layer = animatorController.layers[0];
        AnimatorStateMachine sm = layer.stateMachine;
        AnimatorState closeState = null;
        AnimatorState openSate = null;
        animatorController.AddParameter("Open", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("Close", AnimatorControllerParameterType.Trigger);

        foreach (var clip in clips)
        {
            var stateName = clip.name.Substring(clip.name.Length - clipTypes[0].Length, clipTypes[0].Length);
            if (stateName == clipTypes[0])
            {
                openSate = sm.AddState(clip.name);
                openSate.motion = clip;
                sm.defaultState = openSate;
            }
            else
            {
                closeState = sm.AddState(clip.name);
                closeState.motion = clip;
            }
        }


        var codition = openSate.AddTransition(closeState);
        var codition2 = closeState.AddTransition(openSate);

        codition2.AddCondition(AnimatorConditionMode.If, 0, "Close");
        codition2.duration = 0;

        codition.AddCondition(AnimatorConditionMode.If, 0, "Open");
        codition.duration = 0;


        AssetDatabase.SaveAssets();
        return animatorController;
    }

    private static void BuildPrefab(AnimatorController controller, Sprite sprite, string parent, string name)
    {
        GameObject go = new GameObject(name);
        SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();
        spriteRender.sprite = sprite;
        spriteRender.sortingLayerID = (int)GameSortLayers.Items;
        spriteRender.sortingOrder = 10;
        Animator animator = go.AddComponent<Animator>();
        animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = true;
        go.AddComponent<ViewController>();
        var box2d = go.AddComponent<BoxCollider2D>();
        box2d.size = new Vector2(1.0f, 1.0f);
        PrefabUtility.CreatePrefab(Res.editorPath + Res.PrefabPath + parent + "/" + name + ".prefab", go);
        DestroyImmediate(go);
    }

}

