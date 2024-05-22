#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ftBatchSceneBake : ScriptableWizard
{
    public bool render = true;
    public bool renderLightProbes = true;
    public bool renderReflectionProbes = true;
	public Object[] scenes;

    static bool _render, _renderLightProbes, _renderReflectionProbes;
    static string[] sceneList;
    static IEnumerator progressFunc;
    static bool loaded = false;

    static IEnumerator BatchBakeFunc()
    {
        for(int i=0; i<sceneList.Length; i++)
        {
            loaded = false;
            EditorSceneManager.OpenScene(sceneList[i]);
            while(!loaded) yield return null;

            var storage = ftRenderLightmap.FindRenderSettingsStorage();
            var bakery = ftRenderLightmap.instance != null ? ftRenderLightmap.instance : new ftRenderLightmap();
            bakery.LoadRenderSettings();

            if (_render)
            {
                bakery.RenderButton(false);
                while(ftRenderLightmap.bakeInProgress)
                {
                    yield return null;
                }
            }

            if (_renderLightProbes)
            {
                bakery.RenderLightProbesButton(false);
                while(ftRenderLightmap.bakeInProgress)
                {
                    yield return null;
                }
            }

            if (_renderReflectionProbes)
            {
                bakery.RenderReflectionProbesButton(false);
                while(ftRenderLightmap.bakeInProgress)
                {
                    yield return null;
                }
            }

            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
            yield return null;
        }
        Debug.Log("Batch bake finished");
    }

    static void BatchBakeUpdate()
    {
        if (progressFunc.MoveNext()) return;
        EditorApplication.update -= BatchBakeUpdate;
    }

    static void SceneOpened(Scene scene, OpenSceneMode mode)
    {
        loaded = true;
    }

	void OnWizardCreate()
	{
        sceneList = new string[scenes.Length];
        _render = render;
        _renderLightProbes = renderLightProbes;
        _renderReflectionProbes = renderReflectionProbes;
        for(int i=0; i<scenes.Length; i++)
        {
            var path = AssetDatabase.GetAssetPath(scenes[i]);
            sceneList[i] = path;
        }
        loaded = false;
        EditorSceneManager.sceneOpened += SceneOpened;
        progressFunc = BatchBakeFunc();
        EditorApplication.update += BatchBakeUpdate;
	}

	[MenuItem ("Bakery/Utilities/Batch bake")]
	public static void RenderCubemap () {
		ScriptableWizard.DisplayWizard("Batch bake", typeof(ftBatchSceneBake), "Batch bake");
	}
}

#endif
