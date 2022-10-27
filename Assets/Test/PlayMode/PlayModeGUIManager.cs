using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeGUIManager {

    GameObject gameObject = new GameObject ();

     // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.

    [UnityTest]
    public IEnumerator PlayM_GUIManager_SetHUDActive () {
        gameObject.transform.name = "GUIManager";
        var guiManager = gameObject.AddComponent<GUIManager> ();

        guiManager.SetHUDActive (true);

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_GUIManager_SetPause () {
        gameObject.transform.name = "GUIManager";
        var guiManager = gameObject.AddComponent<GUIManager> ();

        guiManager.SetPause (true);

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_GUIManager_SetLevelName () {
        gameObject.transform.name = "GUIManager";
        var guiManager = gameObject.AddComponent<GUIManager> ();

        guiManager.SetLevelName ("TestGUIManager");

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_GUIManager_FaderOn () {
        gameObject.transform.name = "GUIManager";
        var guiManager = gameObject.AddComponent<GUIManager> ();

        guiManager.FaderOn (true, 0.75f);

        // Use yield to skip a frame.
        yield return null;
    }

}