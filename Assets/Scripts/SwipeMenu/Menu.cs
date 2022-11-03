using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeMenu {
	/// <summary>
	/// La clase del menú principal. Maneja la actualización de la posición de los menús
	/// </summary>	[CreateAssetMenu(fileName = "Menu")]
	public class Menu : MonoBehaviour {

		public TextAsset textJSON;

		/// <summary>
		///El elemento del menú de inicio.
		/// </summary>
		public int startingMenuItem = 1;

		/// <summary>
		/// El ángulo de los elementos del menú que no están centrados.
		/// </summary>
		public float menuItemAngle = 50.0f;

		/// <summary>
		/// La distancia entre los menús. La distancia entre menús debe ser divisible por 0.5f. Esto está sujeto a la función Awake.
		/// </summary>
		public float distanceBetweenMenus = 1.0f;

		/// <summary>
		/// Mueve el menú centrado más cerca de la cámara. Proporciona una compensación entre
		/// menú centrado y menús de fondo.
		/// </summary>
		public float zOffsetForCentredItem = 0.5f;

		public MenuItem.BooksList booksList = new MenuItem.BooksList ();
		/// <summary>
		/// Los elementos del menú. Los elementos se emplean directamente en la creacion de paneles dinamicos del menú
		/// </summary>
		public MenuItem[] menuItems;

		private float _centreOffset = 1.0f;
		private float _currentMenuPosition = 0.0f;
		private float _maxMenuPosition;
		private SwipeHandler _swipeHandler;

		private static Menu _instance;

		/// <summary>
		/// Devuelve una instancia de Menu. Proporciona acceso centralizado a la clase desde cualquier script.
		/// </summary>
		/// <value>La instancia.</value>
		public static Menu instance {
			get {
				if (!_instance) {
					_instance = FindObjectOfType<Menu> ();
				}
				return _instance;
			}
		}

		void Start () {

			GameObject book = transform.GetChild (0).gameObject;
			GameObject k;

			List<GameObject> listbook = new List<GameObject> ();

			//Recupero informacion del JSON
			booksList = JsonUtility.FromJson<MenuItem.BooksList> (textJSON.text);

			//Iteracion por todos los elementos de mi JSON para convertirlos en Objetos e instanciarlos en la Escena debajo de su padre
			foreach (var item in booksList.books) {
				k = Instantiate (book, transform) as GameObject;
				k.name = (item.title);

				/*Fragmento para añadir a la información de los libros a cada uno de los hijos de mi objeto Libro */

				item.portada = Resources.Load<Sprite> (item.portadaURL);

				k.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = (item.portada);
				k.transform.GetChild (1).GetComponent<Text> ().text = (item.title);
				k.transform.GetChild (2).GetComponent<Text> ().text = (item.pageCount.ToString ());
				k.transform.GetChild (3).GetComponent<Text> ().text = (item.isbn.ToString ());
				k.transform.GetChild (4).GetComponent<Text> ().text = (item.shortDescription);
				k.transform.GetChild (5).GetComponent<Text> ().text = (item.editorial);
				k.transform.GetChild (6).GetComponent<Text> ().text = (item.publishedDate);
				k.transform.GetChild (9).GetComponent<Text> ().text = (item.idiom);
				k.transform.GetChild (10).GetComponent<Text> ().text = (item.pdfURL);

				/*Fragmento para instanciar los nombres de los autores provenientes del arreglo en el gameobject authors */
				GameObject[] authors = new GameObject[item.authors.Length];

				for (int a = 0; a < item.authors.Length; a++) {
					authors[a] = Instantiate (k.transform.GetChild (7).gameObject, k.transform.GetChild (7)) as GameObject;
					authors[a].name = (item.authors[a]);
					authors[a].GetComponent<Text> ().text = (item.authors[a]);

					k.transform.GetChild (7).GetComponent<Text> ().text += authors[a].GetComponent<Text> ().text + "," + " ";
				}

				/*Fragmento para instanciar los nombres de las categorías provenientes del arreglo en el gameobject categories*/
				GameObject[] categories = new GameObject[item.categories.Length];
				for (int c = 0; c < categories.Length; c++) {
					categories[c] = Instantiate (k.transform.GetChild (8).gameObject, k.transform.GetChild (8)) as GameObject;
					categories[c].name = (item.categories[c]);
					categories[c].GetComponent<Text> ().text = (item.categories[c]);

					k.transform.GetChild (8).GetComponent<Text> ().text += categories[c].GetComponent<Text> ().text + "," + " ";
				}

				//Adiciono todos los gameObjects que obtengo de mi Json
				listbook.Add (k);

			}
			//Convierto la lista anterior en array para aprovechar sus funciones de trabajo al crear mi lista de objetos dinamicamente
			var items = listbook.ToArray ();

			//Defino el tamaño inicial de los Items del Menu con un valor igual al arreglo anterior.
			menuItems = new MenuItem[items.Length];

			//Recorro cada uno de los elementos de mi arreglo de Items del Menu  y le añado al componente MenuItem del Inspector
			//Cada uno de los objectos que recupero de mi JSON
			for (int i = 0; i < menuItems.Length; i++) {
				menuItems[i] = items[i].GetComponent<MenuItem> ();

			}
			//Finalmente destruyo el objeto (Dummy) que tomo de referencia a la hora de instanciar el resto de objetos de mi JSON en la escena
			Destroy (book);

			if (!_instance) {
				_instance = this;
			} else {

				_instance = gameObject.GetComponent<Menu> ();
			}

			distanceBetweenMenus -= IsDivisble (distanceBetweenMenus, 0.5f);

			_maxMenuPosition = (menuItems.Length + 1) * distanceBetweenMenus;

			startingMenuItem = Mathf.Clamp (startingMenuItem, 1, menuItems.Length);

			_currentMenuPosition = ((1) * distanceBetweenMenus) * startingMenuItem;

			ParentMenuItems ();
			UpdateMenuItemsPositionInWorldSpace ();

			if (GetComponent<TouchHandler> () == null) {
				gameObject.AddComponent<TouchHandler> ();
			}

			_swipeHandler = GetComponent<SwipeHandler> ();

			if (_swipeHandler == null) {
				_swipeHandler = gameObject.AddComponent<SwipeHandler> ();
			}

		}

		void Update () {
			_instance = gameObject.GetComponent<Menu> ();
			UpdateMenuItemsPositionInWorldSpace ();

		}

		/// <summary>
		/// Mueve todo el menú a la izquierda/derecha en función del parámetro de la cantidad. 
		/// </summary>
		/// <param name="amount">Amount.</param>
		public void MoveLeftRightByAmount (int amount) {
			int currentIndex = GetClosestMenuItemIndex ();

			if (currentIndex != -1) {
				currentIndex = Mathf.Clamp (currentIndex + amount, 0, menuItems.Length - 1);
				AnimateToTargetItem (menuItems[currentIndex]);
			}
		}

		/// <summary>
		/// Se anima para apuntar a MenuItem usando iTween.
		/// </summary>
		/// <param name="item">Item.</param>
		public void AnimateToTargetItem (MenuItem item) {
			float offset = CalcPosXInverse (item.transform.position.x);

			iTween.ValueTo (gameObject, iTween.Hash ("from", _currentMenuPosition, "to", _currentMenuPosition + offset,
				"time", 0.5, "easetype", iTween.EaseType.easeOutCubic, "onupdate", "UpdateCurrentMenuPosition"));
		}

		/// <summary>
		/// Proporciona un movimiento directo/constante por la cantidad especificada. No animado. Se utiliza para deslizamientos que no están clasificados como movimientos rápidos.
		/// </summary>
		/// <param name="amount">Amount.</param>
		public void Constant (float amount) {
			_currentMenuPosition = Mathf.Clamp (_currentMenuPosition + amount, 0, _maxMenuPosition);
		}

		/// <summary>
		/// Mueve la cantidad especificada con inercia usando iTween. Se utiliza para películas
		/// </summary>
		/// <param name="amount">Amount.</param>
		public void Inertia (float amount) {
			var to = Mathf.Clamp (_currentMenuPosition + amount, 0, _maxMenuPosition);

			iTween.ValueTo (gameObject, iTween.Hash ("from", _currentMenuPosition, "to", to,
				"time", 0.5f, "easetype", iTween.EaseType.easeOutCubic,
				"onupdate", "UpdateCurrentMenuPosition", "oncomplete", "AnimationComplete"));
		}

		/// <summary>
		/// Encuentra MenuItem más cercano al centro y anima ese MenuItem al centro
		/// </summary>
		public void LockToClosest () {
			MenuItem item = GetClosestMenuItem ();

			if (item != null)
				AnimateToTargetItem (item);
		}

		/// <summary>
		/// Devuelve verdadero si el elemento de menú especificado está centrado.
		/// </summary>
		/// <returns><c>true</c>, if centred was menued, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		public bool MenuCentred (MenuItem item) {
			return item.transform.position.x == 0;
		}

		/// <summary>
		/// Deshabilita todos los elementos del menú.
		/// </summary>
		public void HideMenus () {
			foreach (var menu in menuItems) {
				menu.gameObject.SetActive (false);
			}
		}

		/// <summary>
		/// Habilita todos los elementos del menú.
		/// </summary>
		public void ShowMenus () {
			foreach (var menu in menuItems) {
				menu.gameObject.SetActive (true);
			}
		}

		/// <summary>
		/// Invoca el evento OnClick para el elemento de menú especificado. Invoca el OnOtherMenuClick para todos los menús
		/// que no están seleccionados.
		/// </summary>
		/// <param name="item">Item.</param>
		public void ActivateSelectedMenuItem (MenuItem item) {
			item.OnClick.Invoke ();

			foreach (var i in menuItems) {
				if (!i.Equals (item)) {
					i.OnOtherMenuClick.Invoke ();
				}
			}
		}

		/// <summary>
		/// Padres de los elementos del menú para transformar el menú
		/// </summary>
		private void ParentMenuItems () {
			foreach (var menu in menuItems) {
				if (menu == null) {
					Debug.LogError ("Los elementos de este menu no están habilitados en el Inspector");
				} else {
					menu.transform.SetParent (transform); //Obtienen la posición del padre.
				}
			}
		}

		/// <summary>
		/// Retorna el elemento del menu que está más centrado
		/// </summary>
		/// <returns>El elemento más cercano</returns>
		private MenuItem GetClosestMenuItem () {
			MenuItem item = null;

			float xOffset = float.MaxValue;

			foreach (var i in menuItems) {
				var x = CalculateOffsetFromX (i.gameObject.transform.position.x, 0);

				if (x == 0)
					return i;

				if (x < xOffset) {
					item = i;
					xOffset = x;
				}
			}

			return item;
		}

		/// <summary>
		/// Retorna el índice del elemento del menu mas cercano al centro
		/// </summary>
		/// <returns>Indice del elemento del menu mas cercano al centro.</returns>
		private int GetClosestMenuItemIndex () {
			int index = -1;

			float xOffset = float.MaxValue;

			for (int i = 0; i < menuItems.Length; i++) {
				var x = CalculateOffsetFromX (menuItems[i].gameObject.transform.position.x, 0);

				if (x == 0)
					return i;

				if (x < xOffset) {
					index = i;
					xOffset = x;
				}
			}

			return index;
		}

		/// <summary>
		/// Calcula la posición X inversa.
		/// </summary>
		/// <returns>La posicion inversa de X</returns>
		/// <param name="realPosx">Real posx.</param>
		private float CalcPosXInverse (float realPosx) {
			if (realPosx < -1.0f) {
				return -(realPosx * realPosx + 1) / 2;
			} else if (realPosx < 1.0f) {
				return realPosx;
			} else {
				return (realPosx * realPosx + 1) / 2;
			}
		}

		/// <summary>
		/// Calcula la rotación necesaria para el menú. Basado en Menu#menuItemAngle.
		/// </summary>
		/// <returns>La rotacion del elemento del menu correspondiente.</returns>
		/// <param name="offsetx">Offsetx.</param>
		private float CalculateMenuItemRotation (float offsetx) {
			//izquierdas cubiertas
			if (offsetx < -_centreOffset) {
				return -menuItemAngle;
			} else if (offsetx > _centreOffset) {
				return menuItemAngle;
			} else {
				return offsetx * (menuItemAngle / _centreOffset);
			}
		}

		/// <summary>
		/// Calcula la posición X del elemento de menú.
		/// </summary>
		/// <returns>La posición en el eje X del elemento del menú.</returns>
		/// <param name="offsetx">Offsetx.</param>
		private float CalculateMenuItemXPosition (float offsetx) {
			if (offsetx >= 1.0f) {
				return Mathf.Sqrt (2 * offsetx - 1);
			} else if (offsetx <= -1.0f) {
				return -Mathf.Sqrt (-2 * offsetx - 1);
			} else
				return offsetx;
		}

		/// <summary>
		/// Calcula la posicion del elemento del menu en el eje Z. Basado en Menu#distanceBetweenSelectedMenuAndOthers.
		/// </summary>
		/// <returns>La posición Z del elemento del menu</returns>
		/// <param name="offsetx">Offsetx.</param>
		private float CalculateMenuItemZPosition (float offsetx) {
			if (offsetx < -_centreOffset) {
				return 0;
			} else if (offsetx < 0) {
				return -zOffsetForCentredItem / _centreOffset * offsetx - zOffsetForCentredItem;
			} else if (offsetx < _centreOffset) {
				return zOffsetForCentredItem / _centreOffset * offsetx - zOffsetForCentredItem;
			} else {
				return 0;
			}
		}

		/// <summary>
		/// Actualiza la posición y la rotación de los elementos del menú en el espacio 3D
		/// </summary>
		private void UpdateMenuItemsPositionInWorldSpace () {
			for (int i = 0; i < menuItems.Length; i++) {
				float offsetx = distanceBetweenMenus * (i + 1) - _currentMenuPosition;
				float posx = CalculateMenuItemXPosition (offsetx);
				float posz = CalculateMenuItemZPosition (offsetx);
				Vector3 pos = new Vector3 (posx, 0, posz);
				menuItems[i].transform.position = pos;
				Vector3 euler = new Vector3 (0, CalculateMenuItemRotation (offsetx), 0);
				menuItems[i].transform.eulerAngles = euler;
			}
		}

		/// <summary>
		/// Actualiza la posición actual del elemento del menú.
		/// </summary>
		/// <param name="pos">Posicion.</param>
		private void UpdateCurrentMenuPosition (float pos) {
			_currentMenuPosition = pos;
		}

		/// <summary>
		/// Se invoca al final de la animación. Se detiene en el elemento de menú más cercano si SwipeHandler#lockToClosest está activado.
		/// </summary>
		private void AnimationComplete () {
			if (_swipeHandler.lockToClosest)
				LockToClosest ();
		}

		/// <summary>
		/// Calcula si x es divisible por n. Devuelve el resto
		/// </summary>
		/// <returns>el resto de la operacion entre los dos parámetros.</returns>
		/// <param name="x">X.</param>
		/// <param name="n">N.</param>
		private float IsDivisble (float x, float n) {
			return (x % n);
		}

		/// <summary>
		/// Calcula el desplazamiento a partir de x.
		/// </summary>
		/// <returns>El desplazamiento a partir de x.</returns>
		/// <param name="start">Start.</param>
		/// <param name="x">Coordenada en el eje x.</param>
		private float CalculateOffsetFromX (float start, float x) {
			return Mathf.Abs (start - x);
		}

	}
}