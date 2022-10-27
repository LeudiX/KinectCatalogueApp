using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PlayModePortraitSelector {
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayM_PortraitSelectedBookPortrait () {

        var menuItem = new GameObject ();

        menuItem.transform.name = "Book Portrait";

        Sprite portada = Resources.Load<Sprite> ("Images/Books/AI Game Programming Wisdom 2");

        menuItem.AddComponent<SpriteRenderer> ().sprite = portada;
        menuItem.AddComponent<Image> ();

        var portraitSelector = menuItem.AddComponent<PortraitSelector> ();

        menuItem.GetComponent<Image> ().sprite = portraitSelector.GetComponent<SpriteRenderer> ().sprite;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_PortraitSelectedSerieCPortrait () {

        var menuItem = new GameObject ();

        menuItem.transform.name = "Scientific Series Portrait";

        Sprite portada = Resources.Load<Sprite> ("Images/SerieCientifica/Serie");

        menuItem.AddComponent<SpriteRenderer> ().sprite = portada;
        menuItem.AddComponent<Image> ();

        var portraitSelector = menuItem.AddComponent<PortraitSelector> ();

        menuItem.GetComponent<Image> ().sprite = portraitSelector.GetComponent<SpriteRenderer> ().sprite;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_PortraitSelectedTesisPortrait () {

        var menuItem = new GameObject ();

        menuItem.transform.name = "Tesis Portrait";

        Sprite portada = Resources.Load<Sprite> ("Images/Tesis/tesis");

        menuItem.AddComponent<SpriteRenderer> ().sprite = portada;
        menuItem.AddComponent<Image> ();

        var portraitSelector = menuItem.AddComponent<PortraitSelector> ();

        menuItem.GetComponent<Image> ().sprite = portraitSelector.GetComponent<SpriteRenderer> ().sprite;

        // Use yield to skip a frame.
        yield return null;
    }


    [UnityTest]
    public IEnumerator PlayM_PortraitSelectedEventPortrait () {

        var menuItem = new GameObject ();

        menuItem.transform.name = "Event Portrait";

        Sprite portada = Resources.Load<Sprite> ("Images/ScientificEvents/Global Game Jam 2023");

        menuItem.AddComponent<SpriteRenderer> ().sprite = portada;
        menuItem.AddComponent<Image> ();

        var portraitSelector = menuItem.AddComponent<PortraitSelector> ();

        menuItem.GetComponent<Image> ().sprite = portraitSelector.GetComponent<SpriteRenderer> ().sprite;

        // Use yield to skip a frame.
        yield return null;
    }

}