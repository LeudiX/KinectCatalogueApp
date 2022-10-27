using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeLevelManager {

    GameObject gameObject = new GameObject ();
   
    [UnityTest]
    public IEnumerator PlayM_LevelManager_Awake () {

        gameObject.transform.name = "LevelManager";
        var levelManager = gameObject.AddComponent<LevelManager> ();

        levelManager.Awake ();

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_LevelManager_Start () {

        gameObject.transform.name = "LevelManager";
        var levelManager = gameObject.AddComponent<LevelManager> ();

        levelManager.Start ();

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_LevelManager_GotoLevel () {

        gameObject.transform.name = "LevelManager";
        var levelManager = gameObject.AddComponent<LevelManager> ();

        levelManager.GotoLevel ("Contenidos bibliográficos");

        // Use yield to skip a frame.
        yield return null;
    }

}