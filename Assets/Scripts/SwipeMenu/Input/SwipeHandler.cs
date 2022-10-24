using UnityEngine;
using UnityEngine.UI;


namespace SwipeMenu
{
	/// <summary>
	/// Maneja el deslizamiento y el desplazamiento. Incluye soporte para el ratón y el móvil.
	/// </summary>
	public class SwipeHandler : MonoBehaviour
	{
		/// <summary>
		/// Si es verdadero, se manejarán los swipes.
		/// </summary>
		public bool handleSwipes = true;

		/// <summary>
		/// Los gestos se clasifican como deslizamientos pero con una fuerza mayor que SwipeHandler#requiredForFlick.
		/// </summary>
		public bool handleFlicks = true;

		/// <summary>
		/// La fuerza necesaria para que un barrido sea una clase de barrido.
		/// </summary>
		public float requiredForceForFlick = 7f; 
	
		public enum FlickType
		{
			Inertia,
			MoveOne
		}
		/// <summary>
		/// El tipo de desplazamiento. La inercia se desplaza cinemáticamente, MoveOne desplaza el menú en la dirección x en uno por cada flick.
		/// </summary>
		public FlickType flickType = FlickType.Inertia;

		/// <summary>
		/// Una vez que se haya realizado un barrido o un deslizamiento, se moverá el menú más cercano del centro, hacia el centro.
		/// </summary>
		public bool lockToClosest = true;

        /// <summary>
        /// Limita la fuerza máxima aplicada al deslizar.
        /// </summary>
        public float maxForce = 15f;

		private Vector3 finalPosition, startpos, endpos, oldpos;
		private float length, startTime, mouseMove, force;
		private bool SW;

		/// <summary>
		/// Obtiene un valor que indica si este <see cref="SwipeMenu.SwipeHandler"/> se está deslizando.
		/// </summary>
		/// <value><c>true</c> si se está deslizando; en otro caso devuelve, <c>false</c>.</value>
		public bool isSwiping {
			get {return SW || length != 0;}
		}

		void Update ()
		{

			#if (!UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL)
						HandleMobileSwipe ();

			#else
						HandleMouseSwipe();
			#endif


        }

		private void HandleMobileSwipe ()
		{

            if (Input.touchCount > 0) {

				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					startTime = Time.time;
					finalPosition = Vector3.zero;
					length = 0;
					SW = false;
					Vector2 touchDeltaPosition = Input.GetTouch (0).position;
					startpos = new Vector3 (touchDeltaPosition.x, 0, touchDeltaPosition.y);
					oldpos = startpos;
				}   

				if (Input.GetTouch (0).phase == TouchPhase.Moved) {
					SW = true;

					Vector2 touchDeltaPosition = Input.GetTouch (0).position;
					Vector3 pos = new Vector3 (touchDeltaPosition.x, 0, touchDeltaPosition.y);

					if (handleSwipes && pos.x != oldpos.x) {
						var f = pos - oldpos;

						var l = f.x < 0 ? (f.magnitude * Time.deltaTime) : -(f.magnitude * Time.deltaTime);
					
						l *= .2f;

						Menu.instance.Constant (l);
					}

					oldpos = pos;
				}
			
				if (Input.GetTouch (0).phase == TouchPhase.Canceled) {
					SW = false;
				}
			
				if (Input.GetTouch (0).phase == TouchPhase.Stationary) {
					SW = false;
				}

				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					if (SW && handleFlicks) {
						Vector2 touchPosition = Input.GetTouch (0).position;
						endpos = new Vector3 (touchPosition.x, 0, touchPosition.y);
						finalPosition = endpos - startpos;
						length = finalPosition.x < 0 ? -(finalPosition.magnitude * Time.deltaTime) : (finalPosition.magnitude * Time.deltaTime);

						length *= .35f;

						var force = length / (Time.time - startTime);

                        force = Mathf.Clamp(force, -maxForce, maxForce);

                        if (handleFlicks && Mathf.Abs (force) > requiredForceForFlick) {
							Menu.instance.Inertia (-length);
						}  
					}

					if (lockToClosest) {
						Menu.instance.LockToClosest ();
					}
				}

			}


		
		}

		private void HandleMouseSwipe ()
		{

            if (Input.GetMouseButtonDown (0)) {
				startTime = Time.time;
				finalPosition = Vector3.zero;
				length = 0;
				Vector2 touchDeltaPosition = Input.mousePosition;
				startpos = new Vector3 (touchDeltaPosition.x, 0, touchDeltaPosition.y);
			}

			if (Input.GetMouseButtonUp (0)) {
                Vector2 touchPosition = Input.mousePosition;
				endpos = new Vector3 (touchPosition.x, 0, touchPosition.y);
				finalPosition = endpos - startpos;
				length = finalPosition.x < 0 ? (finalPosition.magnitude * Time.deltaTime) : -(finalPosition.magnitude * Time.deltaTime);
				length *= .5f;

				force = length / (Time.time - startTime);

                force = Mathf.Clamp(force, -maxForce, maxForce);

				if (handleFlicks && Mathf.Abs (force) > requiredForceForFlick) {

					if (flickType == FlickType.Inertia) {
                        Menu.instance.Inertia (length);
					} else {
						if (length > 0) {
							Menu.instance.MoveLeftRightByAmount (1);
						} else {
							Menu.instance.MoveLeftRightByAmount (-1);
						}
					}
				} else if (lockToClosest && force != 0) {
					Menu.instance.LockToClosest ();
				}

			}

            mouseMove = Helper.GetMouseAxis(MouseAxis.x); 
        
            if (handleSwipes && Input.GetMouseButton (0) && mouseMove != 0) {
         
                Menu.instance.Constant (-(mouseMove * .1f));
			}  

		
		}
		
	}
}