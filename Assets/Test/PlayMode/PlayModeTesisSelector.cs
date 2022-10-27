using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PlayModeTesisSelector {

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedTitle () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Title";
        menuItem.AddComponent<Text> ().text = "Interactive Virtual Catalogue for the visualization of bibliographic contents using Kinect 2";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedID () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "ID";
        menuItem.AddComponent<Text> ().text = "T1409112022";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedShortDescription () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Description";
        menuItem.AddComponent<Text> ().text = "El catálogo virtual interactivo constituye una herramienta de apoyo a la formación profesional de los estudiantes universitarios de la UCI";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedEditorial () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Editorial";
        menuItem.AddComponent<Text> ().text = "Editorial UCI";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedPublishedDate () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Published";
        menuItem.AddComponent<Text> ().text = "15/11/2022";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedAuthors () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Authors";
        menuItem.AddComponent<Text> ().text = "Leudis R. Estrada González";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedIdiom () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Idiom";
        menuItem.AddComponent<Text> ().text = "Spanish";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedTutors () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Tutors";
        menuItem.AddComponent<Text> ().text = "Dr. C Omar Correa Madrigal";
        var tesisSelector = menuItem.AddComponent<TesisSelector> ();

        tesisSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_TesisSelector_SelectedPDF () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Visualize PDF";
        menuItem.AddComponent<PDFManager> ();
        menuItem.AddComponent<Text>().text = "https://repositorio.uci.cu/jspui/bitstream/123456789/10243/1/TD_09437_19.pdf";

        var tesisSelector = menuItem.AddComponent<TesisSelector> ();
        
        menuItem.GetComponent<PDFManager> ().URL = tesisSelector.GetComponent<Text> ().text;

        // Use yield to skip a frame.
        yield return null;
    }
}