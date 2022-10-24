using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsSelector : MonoBehaviour
{
  public Text text;
 
  public void SelectedTitle ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel 4");

			infopanel.transform.GetChild(0).GetChild(0).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}

  public void SelectedUbicacion()
   {
       GameObject infopanel = GameObject.Find("InfoPanel 4");

        infopanel.transform.GetChild(1).GetChild(1).name = this.GetComponent<Text>().text;
        infopanel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;

   } 

   public void SelectedShortDescription()
   {
       GameObject infopanel = GameObject.Find("InfoPanel 4");

        infopanel.transform.GetChild(2).GetChild(1).name = this.GetComponent<Text>().text;
        infopanel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;

   } 

   public void SelectedDate()
   {
       GameObject infopanel = GameObject.Find("InfoPanel 4");

        infopanel.transform.GetChild(3).GetChild(1).name = this.GetComponent<Text>().text;
        infopanel.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;

   } 

    public void SelectedPlace()
   {
       GameObject infopanel = GameObject.Find("InfoPanel 4");

        infopanel.transform.GetChild(4).GetChild(0).name = this.GetComponent<Text>().text;
        infopanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = this.GetComponent<Text>().text;

   } 

   public void SelectedStatus()
   {
       GameObject infopanel = GameObject.Find("InfoPanel 4");

        infopanel.transform.GetChild(5).GetChild(1).name = this.GetComponent<Text>().text;
        infopanel.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;

   } 

}
