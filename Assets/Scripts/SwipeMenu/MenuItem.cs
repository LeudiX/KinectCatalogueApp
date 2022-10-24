using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace SwipeMenu
{

	/// <summary>
	///Adjuntar a cualquier elemento del menú.
	/// </summary>
	public class MenuItem : MonoBehaviour
	{
		
	        
	public GameObject title;

	[System.Serializable]
	public class Books{
			
		

		public Sprite portada;
		public string portadaURL;

		public string pdfURL;
	 	public string title;
	 	public string idiom;
        public string isbn;
        public int pageCount;
        public string publishedDate;
        public string shortDescription;
        public string  editorial;
        public string[] authors;
        public string[] categories;
	}
 	

	[System.Serializable]
    public class BooksList{

        public Books[] books;
    }


 		/// <summary>
		/// El comportamiento que se invocará cuando se seleccione el elemento de menú.
		/// </summary>
		public Button.ButtonClickedEvent OnClick;

		/// <summary>
		/// El comportamiento que se invocará cuando se seleccione otro elemento de menú.
		/// </summary>
		public Button.ButtonClickedEvent OnOtherMenuClick;

	
	void Update ()
	{
		
		//Si este elemento está centrado
		if (Menu.instance.MenuCentred (this.GetComponent<MenuItem>())) 		
			// Muestro el texto sobre el elemento que está centrado
			title.transform.GetComponent<Text>().text = this.transform.GetChild(1).GetComponent<Text>().text;
		else 
			return;
	
	}

	}
}