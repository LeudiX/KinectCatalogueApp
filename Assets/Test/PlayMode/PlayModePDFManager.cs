using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModePDFManager {

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayM_PDFManager () {
       
        var gameObject = new GameObject ();
        gameObject.transform.name = "PDFManager";
        var pdfManager = gameObject.AddComponent<PDFManager> ();

        string url = "https://repositorio.uci.cu/jspui/bitstream/123456789/10243/1/TD_09437_19.pdf";

        Application.OpenURL (url);
        // pdfManager.OpenPDFURL();
        // Use yield to skip a frame.
        yield return null;
    }
}