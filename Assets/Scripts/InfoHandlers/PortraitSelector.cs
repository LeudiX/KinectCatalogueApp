using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
    
    public class PortraitSelector : MonoBehaviour {

        public SpriteRenderer portrait;
        
    public void SelectedBookPortrait()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

		infopanel.transform.GetChild(8).GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;		
	}

      public void SelectedSerieCPortrait()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 2");

		infopanel.transform.GetChild(7).GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;		
	} 

	  public void SelectedTesisPortrait()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 3");

		infopanel.transform.GetChild(8).GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;		
	} 
	  public void SelectedEventPortrait()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 4");

		infopanel.transform.GetChild(6).GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;		
	} 

    }

