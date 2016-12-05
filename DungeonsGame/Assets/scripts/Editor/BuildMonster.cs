using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class BuildMonster : Editor {
    private static Texture2D selectedTexture;
    private static TextureImporter importer;
    private static Sprite[] spriteResources;
    private static string[] clipTypes = new string[] {"Idle","Attack","Hit"};
    private static string[] prefabTypes = new string[] { "Monster" };

    [MenuItem("Assets/Build Monster")]
    static void buildMonster()
    {
        UseSelectedTexture();
        var spriteGroups = spriteResources.GroupBy(x => x.name.Substring(0, x.name.Length - 5)).ToList();
        foreach (var spriteGroup in spriteGroups)
        {
            List<AnimationClip> clips = new List<AnimationClip>();
            foreach (var clip in clipTypes)
            {
                clips.Add(BuildAnimatinoClip(spriteGroup.ToArray<Sprite>(),clip));
            }
            var controller = BuildAnimationController(clips, spriteGroup.Key);
            BuildPrefab(controller,spriteGroup.First(), prefabTypes[0],spriteGroup.Key);
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

    private static AnimationClip BuildAnimatinoClip(Sprite[] sprites, string clipType)
    {
        var animationClip = new AnimationClip {frameRate = 12};
        var animationClipSettings = new AnimationClipSettings();


        var curveBinding = new EditorCurveBinding();
        curveBinding.path = "";
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] objectReferenceKeyframes = null;

        if (clipType == clipTypes[0])
        {
            animationClipSettings.loopTime = true;

            objectReferenceKeyframes = new ObjectReferenceKeyframe[sprites.Length + 1];
            for (var i = 0; i < objectReferenceKeyframes.Length; i++)
            {
                objectReferenceKeyframes[i] = new ObjectReferenceKeyframe
                {
                    value = sprites[i%sprites.Length],
                    time = i*0.5f,
                };
            }
        }

        if (clipType == clipTypes[1])
        {
            animationClipSettings.loopTime = false;
            objectReferenceKeyframes = new ObjectReferenceKeyframe[sprites.Length];
            for (var i = 0; i < objectReferenceKeyframes.Length; i++)
            {
                objectReferenceKeyframes[i] = new ObjectReferenceKeyframe
                {
                    value = sprites[0],
                    time = i * 0.3f,
                };
            }
            AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 0.3f, 0.0f);
            curve.AddKey(0.1f, -0.1f);
            animationClip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        }

        if (clipType == clipTypes[2])
        {
            animationClipSettings.loopTime = false;
            objectReferenceKeyframes = new ObjectReferenceKeyframe[sprites.Length];
            for (var i = 0; i < objectReferenceKeyframes.Length; i++)
            {
                objectReferenceKeyframes[i] = new ObjectReferenceKeyframe
                {
                    value = sprites[sprites.Length-1],
                    time = i * 0.3f,
                };
            }
            AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.1f, 0.3f, 0.0f);
            animationClip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        }
        AnimationUtility.SetAnimationClipSettings(animationClip, animationClipSettings);
        AnimationUtility.SetObjectReferenceCurve(animationClip, curveBinding, objectReferenceKeyframes);
        var name = sprites[0].name.Substring(0, sprites[0].name.Length - 5);
        AssetDatabase.CreateAsset(animationClip,Res.editorPath+ Res.AnimationPath + name + clipType + ".anim");
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(animationClip);
        return animationClip;
    }
    
    private static AnimatorController BuildAnimationController(List<AnimationClip> clips, string name)
    {
        var animatorController =
            AnimatorController.CreateAnimatorControllerAtPath(Res.editorPath+Res.AnimationControllerPath + name + ".controller");
        AnimatorControllerLayer layer = animatorController.layers[0];
        AnimatorStateMachine sm = layer.stateMachine;
        List<AnimatorState> states=new List<AnimatorState>();
        AnimatorState IdleSate = null;
        animatorController.AddParameter("Attack", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("Hit", AnimatorControllerParameterType.Trigger);
        foreach (var clip in clips)
        {
            var stateName = clip.name.Substring(clip.name.Length - clipTypes[0].Length, clipTypes[0].Length);
            if (stateName == clipTypes[0])
            {
                IdleSate = sm.AddState(clip.name);
                IdleSate.motion = clip;
                sm.defaultState = IdleSate;
            }
            else
            {
                states.Add(sm.AddState(clip.name));
                states[states.Count - 1].motion = clip;
            }
        }

        foreach (var state in states)
        {
            var codition=IdleSate.AddTransition(state);
            if (state.name.Substring(state.name.Length-clipTypes[1].Length,clipTypes[1].Length) == clipTypes[1])
            {
                codition.AddCondition(AnimatorConditionMode.If, 0, "Attack");
                codition.duration = 0;
            }
            if (state.name.Substring(state.name.Length - clipTypes[2].Length, clipTypes[2].Length) == clipTypes[2])
            {
                codition.AddCondition(AnimatorConditionMode.If, 0, "Hit");
                codition.duration = 0;
            }
            var codition2 = state.AddTransition(IdleSate);
            codition2.hasExitTime = true;
        }

        AssetDatabase.SaveAssets();
        return animatorController;
    }

    private static void BuildPrefab(AnimatorController controller,Sprite sprite,string parent,string name)
    {
        GameObject go = new GameObject(name);
        SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();
        spriteRender.sprite = sprite;
        spriteRender.sortingLayerID = (int)GameSortLayers.Units;
        spriteRender.sortingOrder = 0;
        Animator animator = go.AddComponent<Animator>();
        animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = true;
        var box2d = go.AddComponent<BoxCollider2D>();
        box2d.size = new Vector2(1.0f, 1.0f);
        go.AddComponent<ViewController>();
        PrefabUtility.CreatePrefab(Res.editorPath + Res.PrefabPath + parent + "/" + name + ".prefab", go);
        DestroyImmediate(go);
    }

}
