using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFManager : MonoBehaviour
{

    public string URL;
    public void OpenPDFURL()
    {
    
    Application.OpenURL(URL);
   
    }

}
