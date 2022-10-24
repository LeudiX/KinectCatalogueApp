using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


/// <summary>
/// Actualiza la información en la sección de “Libros”. Una vez que es seleccionado un elemento de este tipo, se  muestra la misma en el panel informativo correspondiente.
/// </summary>
public class BookSelector : MonoBehaviour
{
	public Text text;
		
	public void SelectedTitle ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(0).GetChild(0).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}

	public void SelectedPages ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(1).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}


	public void SelectedISBN ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(2).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}

	public void SelectedShortDescription ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(3).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
	public void SelectedEditorial ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(4).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}

	public void SelectedPublishedDate ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(5).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
	public void SelectedAuthors ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(6).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
	public void SelectedCategories ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(7).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(7).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}

	public void SelectedIdiom ()
	{
		GameObject infopanel = GameObject.Find("InfoPanel");

			infopanel.transform.GetChild(9).GetChild(1).name = this.GetComponent<Text>().text;
			infopanel.transform.GetChild(9).GetChild(1).GetComponent<Text>().text = this.GetComponent<Text>().text;		
	}
	
}
