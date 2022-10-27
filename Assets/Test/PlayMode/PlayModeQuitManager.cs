using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeQuitManager
{
   
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayM_QuitManager()
    {
         var gameObject = new GameObject();
             gameObject.transform.name = "QuitManager";
         var quitManager = gameObject.AddComponent<QuitManager>();
            
         //quitManager.QuitGame();
         Application.Quit();
        // Use yield to skip a frame.
        yield return null;
    }
}
