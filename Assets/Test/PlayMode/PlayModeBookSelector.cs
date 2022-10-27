using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PlayModeBookSelector {

    

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayM_BookSelector_SelectedTitle () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Title";
        menuItem.AddComponent<Text> ().text = "Unit Testing";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_BookSelector_SelectedPages () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Pages";
        menuItem.AddComponent<Text> ().text = "512";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_BookSelector_SelectedISBN () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "ISBN";
        menuItem.AddComponent<Text> ().text = "5125871436";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_BookSelector_SelectedShortDescription () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Description";
        menuItem.AddComponent<Text> ().text = "Este libro permite al lector adquirir una nocion rápida de las pruebas unitarias con el framework Nunit del motor de videojuegos Unity";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_BookSelector_Editorial () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Editorial";
        menuItem.AddComponent<Text> ().text = "Manning Publications Co";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_BookSelector_PublishedDate () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Published";
        menuItem.AddComponent<Text> ().text = "27/10/2022";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_BookSelector_SelectedAuthors () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Authors";
        menuItem.AddComponent<Text> ().text = "Pedro Rafael Estrada González, Jesús Miguel María de Mendive";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator PlayM_BookSelector_SelectedCategories () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Categories";
        menuItem.AddComponent<Text> ().text = "Unit Test, UNity";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayM_BookSelector_SelectedIdiom () {
        var menuItem = new GameObject ();
        menuItem.transform.name = "Idiom";
        menuItem.AddComponent<Text> ().text = "English";
        var bookSelector = menuItem.AddComponent<BookSelector> ();

        bookSelector.GetComponent<Text> ().text = menuItem.GetComponent<Text> ().text;
                         
        // Use yield to skip a frame.
        yield return null;
    }
}