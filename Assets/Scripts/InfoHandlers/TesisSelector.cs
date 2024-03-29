﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TesisSelector : MonoBehaviour
{
    public Text text;

    public void SelectedTitle ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(0).GetChild(0).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
      public void SelectedID(){
        GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(1).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
    }
      public void SelectedShortDescription ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(2).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
     public void SelectedEditorial ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(3).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
     public void SelectedPublishedDate ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(4).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
    public void SelectedAuthors ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(5).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
     public void SelectedIdiom ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(6).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
     public void SelectedTutors ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(7).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(7).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}

	public void SelectedPDFURL(){

			GameObject infopanel = GameObject.Find("InfoPanel 3");

			infopanel.transform.GetChild(9).GetComponent<PDFManager>().URL = this.GetComponent<Text>().text;			
	}	

}
