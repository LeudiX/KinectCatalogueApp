using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace SwipeMenu
{
	/// <summary>
	/// Adjuntar a cualquier elemento del submenú. Véase la escena de ejemplo del Menú Múltiple para su uso.
	/// </summary>
	public class SubMenuItem : MonoBehaviour
	{
		/// <summary>
		/// El elemento de menú al que pertenece este submenú.
		/// </summary>
		public MenuItem OwnerMenu;

		/// <summary>
		/// El comportamiento que se invoca cuando se selecciona este submenú.
		/// </summary>
		public Button.ButtonClickedEvent OnClick;

		void Update ()
		{
			
			if (!Menu.instance.MenuCentred (OwnerMenu)) {
				return;
			}

			#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
						if (Input.touchCount > 0) {
							if (Input.GetTouch (0).phase == TouchPhase.Ended) {
								CheckTouch (Input.GetTouch (0).position);
							}
						}
			#else
						if (Input.GetMouseButtonUp (0) && MouseAxisGetter.GetMouseAxis(MouseAxis.x) == 0) {
							CheckTouch (Input.mousePosition);
						}
			#endif
		}
		
		private void CheckTouch (Vector3 screenPoint)
		{
			Ray touchRay = Camera.main.ScreenPointToRay (screenPoint);
			RaycastHit hit;
			
			Physics.Raycast (touchRay, out hit);
			
			if (hit.collider != null && hit.collider.gameObject.Equals (gameObject)) {
				
				OnClick.Invoke ();
			}
		}
	}
}
