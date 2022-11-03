using UnityEngine;
using UnityEngine.UI;
//using Windows.Kinect;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.IO;


/// <summary>
/// Esta interfaz debe ser implementada por todos los oyentes de interacción
/// </summary>
public interface InteractionListenerInterface
{
	/// <summary>
	/// Se invoca cuando se detecta el agarre de la mano.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="isRightHand">Si es la mano derecha o no</param>
	/// <param name="isHandInteracting">Si esta mano es la que interactúa o no</param>
	/// <param name="handScreenPos">Posición de la pantalla de mano, incluida la profundidad (Z)</param>
	void HandGripDetected(long userId, int userIndex, bool isRightHand, bool isHandInteracting, Vector3 handScreenPos);

	/// <summary>
	/// Se invoca cuando se detecta la liberación de la mano.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="isRightHand">Si es la mano derecha o no</param>
	/// <param name="isHandInteracting">Si esta mano es la que interactúa o no</param>
	/// <param name="handScreenPos">Posición de la pantalla de mano, incluida la profundidad (Z)</param>
	void HandReleaseDetected(long userId, int userIndex, bool isRightHand, bool isHandInteracting, Vector3 handScreenPos);

	/// <summary>
	/// Se invoca cuando se detecta el clic de la mano.
	/// </summary>
	/// <returns><c>true</c>, si hay que reiniciar la detección de clics, <c>false</c> en cualquier otro caso.</returns>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="isRightHand">Si es la mano derecha o no</param>
	/// <param name="handScreenPos">Posición de la pantalla de mano, incluida la profundidad (Z)</param>
	bool HandClickDetected(long userId, int userIndex, bool isRightHand, Vector3 handScreenPos);
}


/// <summary>
/// El gestor de interacciones es el componente que se encarga de las interacciones manuales.
/// </summary>
public class InteractionManager : MonoBehaviour 
{
	/// <summary>
	/// Los tipos de eventos manuales.
	/// </summary>
	public enum HandEventType : int
    {
        None = 0,
        Grip = 1,
        Release = 2
    }

	[Tooltip("Índice del jugador, seguido por este componente. 0 significa el primer jugador, 1 - el segundo, 2 - el tercero, etc.")]
	public int playerIndex = 0;
	
	[Tooltip("Mostrar o no el cursor movido por la mano en la pantalla. También es necesario establecer las siguientes texturas.")]
	public bool showHandCursor = true;
	
	[Tooltip("Textura del cursor de la mano para el estado de agarre de la mano.")]
	public Texture gripHandTexture;
	[Tooltip("Textura del cursor de la mano para el estado de liberación de la mano.")]
	public Texture releaseHandTexture;
	[Tooltip("Textura del cursor de mano para el estado no rastreado.")]
	public Texture normalHandTexture;

	[Tooltip("Factor de suavidad para el movimiento del cursor.")]
	public float smoothFactor = 10f;
	
	[Tooltip("Si los clics de la mano (la mano no se mueve durante ~2 segundos) están activados o no.")]
	public bool allowHandClicks = true;
	
	[Tooltip("Si el cursor de la mano y las interacciones controlan el cursor del ratón o no.")]
	public bool controlMouseCursor = false;

	[Tooltip("Si los agarres y liberaciones de la mano controlan el arrastre del ratón o no.")]
	public bool controlMouseDrag = false;

	// Bool para especificar si se convierten las coordenadas de la pantalla de Unity en coordenadas del ratón a pantalla completa
	//public bool convertMouseToFullScreen = false;
	
	[Tooltip("Lista de los escuchadores de interacción disponibles. Deben implementar InteractionListenerInterface. Si la lista está vacía, los escuchadores de interacción disponibles se detectarán al inicio.")]
	public List<MonoBehaviour> interactionListeners;

	[Tooltip("GUI-Texto para mostrar los mensajes de depuración del gestor de interacciones.")]
	public Text debugText;
	
	private long playerUserID = 0;
	private long lastUserID = 0;
	
	private bool isLeftHandPrimary = false;
	private bool isRightHandPrimary = false;
	
	private bool isLeftHandPress = false;
	private bool isRightHandPress = false;
	
	private Vector3 cursorScreenPos = Vector3.zero;
	private bool dragInProgress = false;
	
	private KinectInterop.HandState leftHandState = KinectInterop.HandState.Unknown;
	private KinectInterop.HandState rightHandState = KinectInterop.HandState.Unknown;
	
	private HandEventType leftHandEvent = HandEventType.None;
	private HandEventType lastLeftHandEvent = HandEventType.Release;

	private Vector3 leftHandPos = Vector3.zero;
	private Vector3 leftHandScreenPos = Vector3.zero;
	private Vector3 leftIboxLeftBotBack = Vector3.zero;
	private Vector3 leftIboxRightTopFront = Vector3.zero;
	private bool isleftIboxValid = false;
	private bool isLeftHandInteracting = false;
	private float leftHandInteractingSince = 0f;
	
	private Vector3 lastLeftHandPos = Vector3.zero;
	private float lastLeftHandTime = 0f;
	private bool isLeftHandClick = false;
	private float leftHandClickProgress = 0f;
	
	private HandEventType rightHandEvent = HandEventType.None;
	private HandEventType lastRightHandEvent = HandEventType.Release;

	private Vector3 rightHandPos = Vector3.zero;
	private Vector3 rightHandScreenPos = Vector3.zero;
	private Vector3 rightIboxLeftBotBack = Vector3.zero;
	private Vector3 rightIboxRightTopFront = Vector3.zero;
	private bool isRightIboxValid = false;
	private bool isRightHandInteracting = false;
	private float rightHandInteractingSince = 0f;
	
	private Vector3 lastRightHandPos = Vector3.zero;
	private float lastRightHandTime = 0f;
	private bool isRightHandClick = false;
	private float rightHandClickProgress = 0f;
	
	// Bool para controlar si Kinect y la biblioteca de interacción han sido inicializados
	private bool interactionInited = false;
	
	// La única instancia de FacetrackingManager
	private static InteractionManager instance;

	
	/// <summary>
	/// Obtiene la instancia única de InteractionManager.
	/// </summary>
	/// <value>La instancia del InteractionManager.</value>
    public static InteractionManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	/// <summary>
	/// Determina si el InteractionManager fue inicializado con éxito.
	/// </summary>
	/// <returns><c>true</c> si InteractionManager fue inicializado con éxito; en cualquier otro caso, <c>false</c>.</returns>
	public bool IsInteractionInited()
	{
		return interactionInited;
	}
	
	/// <summary>
	/// Obtiene el ID del usuario actual, o 0 si no hay ningún usuario rastreado.
	/// </summary>
	/// <returns>The ID del usuario</returns>
	public long GetUserID()
	{
		return playerUserID;
	}
	
	/// <summary>
	/// Obtiene el evento actual de la mano izquierda (ninguno, agarre o liberación).
	/// </summary>
	/// <returns>El evento actual de la mano izquierda.</returns>
	public HandEventType GetLeftHandEvent()
	{
		return leftHandEvent;
	}
	
	/// <summary>
	/// Obtiene el último evento detectado de la mano izquierda (agarre o liberación).
	/// </summary>
	/// <returns>El último evento de generado por la mano izquierda.</returns>
	public HandEventType GetLastLeftHandEvent()
	{
		return lastLeftHandEvent;
	}
	
	/// <summary>
	/// Obtiene la posición actual normalizada de la vista de la mano izquierda, en el rango [0, 1].
	/// </summary>
	/// <returns>La posición de la mano izquierda respecto a la ventana gráfica.</returns>
	public Vector3 GetLeftHandScreenPos()
	{
		return leftHandScreenPos;
	}
	
	/// <summary>
	/// Determina si la mano izquierda es la principal para el usuario.
	/// </summary>
	/// <returns><c>true</c> si la mano izquuierda es la principal para el usuario; en cualquie otro caso, <c>false</c>.</returns>
	public bool IsLeftHandPrimary()
	{
		return isLeftHandPrimary;
	}
	
	/// <summary>
	/// Determina si la mano izquierda está presionando.
	/// </summary>
	/// <returns><c>true</c> si la mano izquierda esta presionada; en cualquier otro caso, <c>false</c>.</returns>
	public bool IsLeftHandPress()
	{
		return isLeftHandPress;
	}
	
	/// <summary>
	/// Determina si se detecta un clic con la mano izquierda izquierda, falso en caso contrario.
	/// </summary>
	/// <returns><c>true</c> si el clic con la mano izquierda fue detectado; en cualquier otro caso, <c>false</c>.</returns>
	public bool IsLeftHandClickDetected()
	{
		if(isLeftHandClick)
		{
			isLeftHandClick = false;
			leftHandClickProgress = 0f;
			lastLeftHandPos = Vector3.zero;
			lastLeftHandTime = Time.realtimeSinceStartup;
			
			return true;
		}
		
		return false;
	}

	/// <summary>
	/// Obtiene el progreso del clic proveniente de la mano izquierda, en el rango [0, 1].
	/// </summary>
	/// <returns>El progreso del clic realizado con la mano izquierda.</returns>
	public float GetLeftHandClickProgress()
	{
		return leftHandClickProgress;
	}
	
	/// <summary>
	/// Obtiene el evento actual de la mano derecha (ninguno, agarre o liberación).
	/// </summary>
	/// <returns>El evento actual de la mano derecha.</returns>
	public HandEventType GetRightHandEvent()
	{
		return rightHandEvent;
	}
	
	/// <summary>
	/// Obtiene el último evento detectado de la mano derecha (agarre o liberación).
	/// </summary>
	/// <returns>El último evento generado por la mano derecha.</returns>
	public HandEventType GetLastRightHandEvent()
	{
		return lastRightHandEvent;
	}
	
	/// <summary>
	/// Obtiene la posición actual normalizada de la vista de la mano derecha, en el rango [0, 1].
	/// </summary>
	/// <returns>La posición de la mano derecha respecto a la ventana gráfica.</returns>
	public Vector3 GetRightHandScreenPos()
	{
		return rightHandScreenPos;
	}
	
	/// <summary>
	/// Determines whether the right hand is primary for the user.
	/// </summary>
	/// <returns><c>true</c> if the right hand is primary for the user; otherwise, <c>false</c>.</returns>
	public bool IsRightHandPrimary()
	{
		return isRightHandPrimary;
	}
	
	/// <summary>
	/// (): Determina si la mano derecha está presionada.
	/// </summary>
	/// <returns><c>true</c> si la mano derecha está cerrada; en cualquier otro caso, <c>false</c>.</returns>
	public bool IsRightHandPress()
	{
		return isRightHandPress;
	}
	
	/// <summary>
	///Determina si se detecta un clic con la mano derecha o no en caso contrario.
	/// </summary>
	/// <returns><c>true</c> si se ha detectado un clic con la mano derecha; en cualquie otro caso, <c>false</c>.</returns>
	public bool IsRightHandClickDetected()
	{
		if(isRightHandClick)
		{
			isRightHandClick = false;
			rightHandClickProgress = 0f;
			lastRightHandPos = Vector3.zero;
			lastRightHandTime = Time.realtimeSinceStartup;
			
			return true;
		}
		
		return false;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>El progreso del ciic realizado con la mano derecha.</returns>
	public float GetRightHandClickProgress()
	{
		return rightHandClickProgress;
	}
	
	/// <summary>
	/// Obtiene la posición actual del cursor normalizado de la ventana gráfica.
	/// </summary>
	/// <returns>La posicion del cursor respecto a la ventana gráfica.</returns>
	public Vector3 GetCursorPosition()
	{
		return cursorScreenPos;
	}


	//----------------------------------- fin de las funciones públicas --------------------------------------//

	void Awake()
	{
		instance = this;
	}


	void Start() 
	{
		interactionInited = true;

		// intentar detectar automáticamente los oyentes de interacción disponibles en la escena
		if(interactionListeners.Count == 0)
		{
			MonoBehaviour[] monoScripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];

			foreach(MonoBehaviour monoScript in monoScripts)
			{
//				if(typeof(InteractionListenerInterface).IsAssignableFrom(monoScript.GetType()) &&
//					monoScript.enabled)
				if((monoScript is InteractionListenerInterface) && monoScript.enabled)
				{
					interactionListeners.Add(monoScript);
				}
			}
		}

	}
	
	void OnDestroy()
	{
		interactionInited = false;
		instance = null;
	}
	
	void Update () 
	{
		KinectManager kinectManager = KinectManager.Instance;
		
		// mantiene actualizada la interacción con Kinect 2
		if(kinectManager && kinectManager.IsInitialized())
		{
			playerUserID = kinectManager.GetUserIdByIndex(playerIndex);
			
			if(playerUserID != 0)
			{
				lastUserID = playerUserID;
				HandEventType handEvent = HandEventType.None;
				
				// obtener el estado de la mano izquierda
				leftHandState = kinectManager.GetLeftHandState(playerUserID);
				
				// verifica si la mano izquierda está interactuando
				isleftIboxValid = kinectManager.GetLeftHandInteractionBox(playerUserID, ref leftIboxLeftBotBack, ref leftIboxRightTopFront, isleftIboxValid);
				//bool bLeftHandPrimaryNow = false;

				// was the left hand interacting till now
				bool wasLeftHandInteracting = isLeftHandInteracting;

				if(isleftIboxValid && //bLeftHandPrimaryNow &&
				   kinectManager.GetJointTrackingState(playerUserID, (int)KinectInterop.JointType.HandLeft) != KinectInterop.TrackingState.NotTracked)
				{
					leftHandPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.HandLeft);

					leftHandScreenPos.x = Mathf.Clamp01((leftHandPos.x - leftIboxLeftBotBack.x) / (leftIboxRightTopFront.x - leftIboxLeftBotBack.x));
					leftHandScreenPos.y = Mathf.Clamp01((leftHandPos.y - leftIboxLeftBotBack.y) / (leftIboxRightTopFront.y - leftIboxLeftBotBack.y));
					leftHandScreenPos.z = Mathf.Clamp01((leftIboxLeftBotBack.z - leftHandPos.z) / (leftIboxLeftBotBack.z - leftIboxRightTopFront.z));

					isLeftHandInteracting = (leftHandPos.x >= (leftIboxLeftBotBack.x - 1.0f)) && (leftHandPos.x <= (leftIboxRightTopFront.x + 0.5f)) &&
						(leftHandPos.y >= (leftIboxLeftBotBack.y - 0.1f)) && (leftHandPos.y <= (leftIboxRightTopFront.y + 0.7f)) &&
						(leftIboxLeftBotBack.z >= leftHandPos.z) && (leftIboxRightTopFront.z * 0.8f <= leftHandPos.z);
					//bLeftHandPrimaryNow = isLeftHandInteracting;

					// start interacting?
					if(!wasLeftHandInteracting && isLeftHandInteracting)
					{
						leftHandInteractingSince = Time.realtimeSinceStartup;
					}

					// check for left press
					isLeftHandPress = ((leftIboxRightTopFront.z - 0.1f) >= leftHandPos.z);
					
					// check for left hand click
					if(allowHandClicks && !dragInProgress && isLeftHandInteracting && 
						((leftHandPos - lastLeftHandPos).magnitude < KinectInterop.Constants.ClickMaxDistance))
					{
						if((Time.realtimeSinceStartup - lastLeftHandTime) >= KinectInterop.Constants.ClickStayDuration)
						{
							if(!isLeftHandClick)
							{
								isLeftHandClick = true;
								leftHandClickProgress = 1f;

								foreach(InteractionListenerInterface listener in interactionListeners)
								{
									if (listener.HandClickDetected (playerUserID, playerIndex, false, leftHandScreenPos)) 
									{
										isLeftHandClick = false;
										leftHandClickProgress = 0f;
										lastLeftHandPos = Vector3.zero;
										lastLeftHandTime = Time.realtimeSinceStartup;
									}
								}

								if(controlMouseCursor)
								{
									MouseControl.MouseClick();
								

									isLeftHandClick = false;
									leftHandClickProgress = 0f;
									lastLeftHandPos = Vector3.zero;
									lastLeftHandTime = Time.realtimeSinceStartup;
								}
							}
						}
						else
						{
							leftHandClickProgress = (Time.realtimeSinceStartup - lastLeftHandTime) / KinectInterop.Constants.ClickStayDuration;
						}
					}
					else
					{
						isLeftHandClick = false;
						leftHandClickProgress = 0f;
						lastLeftHandPos = leftHandPos;
						lastLeftHandTime = Time.realtimeSinceStartup;
					}
				}
				else
				{
					isLeftHandInteracting = false;
					isLeftHandPress = false;
				}
				
				// get the right hand state
				rightHandState = kinectManager.GetRightHandState(playerUserID);

				// check if the right hand is interacting
				isRightIboxValid = kinectManager.GetRightHandInteractionBox(playerUserID, ref rightIboxLeftBotBack, ref rightIboxRightTopFront, isRightIboxValid);
				//bool bRightHandPrimaryNow = false;

				// was the right hand interacting till now
				bool wasRightHandInteracting = isRightHandInteracting;

				if(isRightIboxValid && //bRightHandPrimaryNow &&
				   kinectManager.GetJointTrackingState(playerUserID, (int)KinectInterop.JointType.HandRight) != KinectInterop.TrackingState.NotTracked)
				{
					rightHandPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.HandRight);

					rightHandScreenPos.x = Mathf.Clamp01((rightHandPos.x - rightIboxLeftBotBack.x) / (rightIboxRightTopFront.x - rightIboxLeftBotBack.x));
					rightHandScreenPos.y = Mathf.Clamp01((rightHandPos.y - rightIboxLeftBotBack.y) / (rightIboxRightTopFront.y - rightIboxLeftBotBack.y));
					rightHandScreenPos.z = Mathf.Clamp01((rightIboxLeftBotBack.z - rightHandPos.z) / (rightIboxLeftBotBack.z - rightIboxRightTopFront.z));

					isRightHandInteracting = (rightHandPos.x >= (rightIboxLeftBotBack.x - 0.5f)) && (rightHandPos.x <= (rightIboxRightTopFront.x + 1.0f)) &&
						(rightHandPos.y >= (rightIboxLeftBotBack.y - 0.1f)) && (rightHandPos.y <= (rightIboxRightTopFront.y + 0.7f)) &&
						(rightIboxLeftBotBack.z >= rightHandPos.z) && (rightIboxRightTopFront.z * 0.8f <= rightHandPos.z);
					//bRightHandPrimaryNow = isRightHandInteracting;
					
					if(!wasRightHandInteracting && isRightHandInteracting)
					{
						rightHandInteractingSince = Time.realtimeSinceStartup;
					}
					
					// check for right press
					isRightHandPress = ((rightIboxRightTopFront.z - 0.1f) >= rightHandPos.z);
					
					// check for right hand click
					if(allowHandClicks && !dragInProgress && isRightHandInteracting && 
						((rightHandPos - lastRightHandPos).magnitude < KinectInterop.Constants.ClickMaxDistance))
					{
						if((Time.realtimeSinceStartup - lastRightHandTime) >= KinectInterop.Constants.ClickStayDuration)
						{
							if(!isRightHandClick)
							{
								isRightHandClick = true;
								rightHandClickProgress = 1f;
								
								foreach(InteractionListenerInterface listener in interactionListeners)
								{
									if (listener.HandClickDetected (playerUserID, playerIndex, true, rightHandScreenPos)) 
									{
										isRightHandClick = false;
										rightHandClickProgress = 0f;
										lastRightHandPos = Vector3.zero;
										lastRightHandTime = Time.realtimeSinceStartup;
									}
								}

								if(controlMouseCursor)
								{
									MouseControl.MouseClick();
		
									isRightHandClick = false;
									rightHandClickProgress = 0f;
									lastRightHandPos = Vector3.zero;
									lastRightHandTime = Time.realtimeSinceStartup;
								}
							}
						}
						else
						{
							rightHandClickProgress = (Time.realtimeSinceStartup - lastRightHandTime) / KinectInterop.Constants.ClickStayDuration;
						}
					}
					else
					{
						isRightHandClick = false;
						rightHandClickProgress = 0f;
						lastRightHandPos = rightHandPos;
						lastRightHandTime = Time.realtimeSinceStartup;
					}
				}
				else
				{
					isRightHandInteracting = false;
					isRightHandPress = false;
				}

				// if both hands are interacting, check which one interacts longer than the other
				if(isLeftHandInteracting && isRightHandInteracting)
				{
					if(rightHandInteractingSince <= leftHandInteractingSince)
						isLeftHandInteracting = false;
					else
						isRightHandInteracting = false;
				}

				// if left hand just stopped interacting, send extra non-interaction event
				if (wasLeftHandInteracting && !isLeftHandInteracting) 
				{
					foreach(InteractionListenerInterface listener in interactionListeners)
					{
//						if(lastLeftHandEvent == HandEventType.Grip)
//							listener.HandGripDetected (playerUserID, playerIndex, false, isLeftHandInteracting, leftHandScreenPos);
//						else //if(lastLeftHandEvent == HandEventType.Release)
//							listener.HandReleaseDetected (playerUserID, playerIndex, false, isLeftHandInteracting, leftHandScreenPos);
						if(lastLeftHandEvent == HandEventType.Grip)
							listener.HandReleaseDetected (playerUserID, playerIndex, false, true, leftHandScreenPos);
					}
				}


				// if right hand just stopped interacting, send extra non-interaction event
				if (wasRightHandInteracting && !isRightHandInteracting) 
				{
					foreach(InteractionListenerInterface listener in interactionListeners)
					{
//						if(lastRightHandEvent == HandEventType.Grip)
//							listener.HandGripDetected (playerUserID, playerIndex, true, isRightHandInteracting, rightHandScreenPos);
//						else //if(lastRightHandEvent == HandEventType.Release)
//							listener.HandReleaseDetected (playerUserID, playerIndex, true, isRightHandInteracting, rightHandScreenPos);
						if(lastRightHandEvent == HandEventType.Grip)
							listener.HandReleaseDetected (playerUserID, playerIndex, true, true, rightHandScreenPos);
					}
				}


				// process left hand
				handEvent = HandStateToEvent(leftHandState, lastLeftHandEvent);

				if((isLeftHandInteracting != isLeftHandPrimary) || (isRightHandInteracting != isRightHandPrimary))
				{
					if(controlMouseCursor && dragInProgress)
					{
						MouseControl.MouseRelease();
						dragInProgress = false;
					}
					
					lastLeftHandEvent = HandEventType.Release;
					lastRightHandEvent = HandEventType.Release;
				}
				
				if(controlMouseCursor && (handEvent != lastLeftHandEvent))
				{
					if(controlMouseDrag && !dragInProgress && (handEvent == HandEventType.Grip))
					{
						dragInProgress = true;
						MouseControl.MouseDrag();
					}
					else if(dragInProgress && (handEvent == HandEventType.Release))
					{
						MouseControl.MouseRelease();
						dragInProgress = false;
					}
				}
				
				leftHandEvent = handEvent;
				if(handEvent != HandEventType.None)
				{
					if (leftHandEvent != lastLeftHandEvent) 
					{
						foreach(InteractionListenerInterface listener in interactionListeners)
						{
							if(leftHandEvent == HandEventType.Grip)
								listener.HandGripDetected (playerUserID, playerIndex, false, isLeftHandInteracting, leftHandScreenPos);
							else if(leftHandEvent == HandEventType.Release)
								listener.HandReleaseDetected (playerUserID, playerIndex, false, isLeftHandInteracting, leftHandScreenPos);
						}
					}

					lastLeftHandEvent = handEvent;
				}
				
				// if the hand is primary, set the cursor position
				if(isLeftHandInteracting)
				{
					isLeftHandPrimary = true;

					if((leftHandClickProgress < 0.8f) /**&& !isLeftHandPress*/)
					{
						float smooth = smoothFactor * Time.deltaTime;
						if(smooth == 0f) smooth = 1f;
						
						cursorScreenPos = Vector3.Lerp(cursorScreenPos, leftHandScreenPos, smooth);
					}

					// move mouse-only if there is no cursor texture
					if(controlMouseCursor && 
					   (!showHandCursor || (!gripHandTexture && !releaseHandTexture && !normalHandTexture)))
					{
						MouseControl.MouseMove(cursorScreenPos, debugText);
					}
				}
				else
				{
					isLeftHandPrimary = false;
				}


				// process right hand
				handEvent = HandStateToEvent(rightHandState, lastRightHandEvent);

				if(controlMouseCursor && (handEvent != lastRightHandEvent))
				{
					if(controlMouseDrag && !dragInProgress && (handEvent == HandEventType.Grip))
					{
						dragInProgress = true;
						MouseControl.MouseDrag();
					}
					else if(dragInProgress && (handEvent == HandEventType.Release))
					{
						MouseControl.MouseRelease();
						dragInProgress = false;
					}
				}
				
				rightHandEvent = handEvent;
				if(handEvent != HandEventType.None)
				{
					if (rightHandEvent != lastRightHandEvent) 
					{
						foreach(InteractionListenerInterface listener in interactionListeners)
						{
							if(rightHandEvent == HandEventType.Grip)
								listener.HandGripDetected (playerUserID, playerIndex, true, isRightHandInteracting, rightHandScreenPos);
							else if(rightHandEvent == HandEventType.Release)
								listener.HandReleaseDetected (playerUserID, playerIndex, true, isRightHandInteracting, rightHandScreenPos);
						}
					}

					lastRightHandEvent = handEvent;
				}	
				
				// if the hand is primary, set the cursor position
				if(isRightHandInteracting)
				{
					isRightHandPrimary = true;

					if((rightHandClickProgress < 0.8f) /**&& !isRightHandPress*/)
					{
						float smooth = smoothFactor * Time.deltaTime;
						if(smooth == 0f) smooth = 1f;
						
						cursorScreenPos = Vector3.Lerp(cursorScreenPos, rightHandScreenPos, smooth);
					}

					// move mouse-only if there is no cursor texture
					if(controlMouseCursor && 
					   (!showHandCursor || (!gripHandTexture && !releaseHandTexture && !normalHandTexture)))
					{
						MouseControl.MouseMove(cursorScreenPos, debugText);
					}
				}
				else
				{
					isRightHandPrimary = false;
				}

			}
			else
			{
				// send release events
				if (lastLeftHandEvent == HandEventType.Grip || lastRightHandEvent == HandEventType.Grip) 
				{
					foreach(InteractionListenerInterface listener in interactionListeners)
					{
						if(lastLeftHandEvent == HandEventType.Grip)
							listener.HandReleaseDetected (lastUserID, playerIndex, false, true, leftHandScreenPos);
						if(lastRightHandEvent == HandEventType.Grip)
							listener.HandReleaseDetected (lastUserID, playerIndex, true, true, leftHandScreenPos);
					}
				}

				leftHandState = KinectInterop.HandState.NotTracked;
				rightHandState = KinectInterop.HandState.NotTracked;
				
				isLeftHandPrimary = isRightHandPrimary = false;
				isLeftHandInteracting = isRightHandInteracting = false;
				leftHandInteractingSince = rightHandInteractingSince = 0f;

				isLeftHandClick = isRightHandClick = false;
				leftHandClickProgress = rightHandClickProgress = 0f;
				lastLeftHandTime = lastRightHandTime = Time.realtimeSinceStartup;

				isLeftHandPress = false;
				isRightHandPress = false;
				
				leftHandEvent = HandEventType.None;
				rightHandEvent = HandEventType.None;
				
				lastLeftHandEvent = HandEventType.Release;
				lastRightHandEvent = HandEventType.Release;

				if(controlMouseCursor && dragInProgress)
				{
					MouseControl.MouseRelease();
					dragInProgress = false;
				}
			}
		}
		
	}

	// converts hand state to hand event type
	public static HandEventType HandStateToEvent(KinectInterop.HandState handState, HandEventType lastEventType)
	{
		switch(handState)
		{
			case KinectInterop.HandState.Open:
				return HandEventType.Release;

			case KinectInterop.HandState.Closed:
			case KinectInterop.HandState.Lasso:
				return HandEventType.Grip;
			
			case KinectInterop.HandState.Unknown:
				return lastEventType;
		}

		return HandEventType.None;
	}
	

	void OnGUI()
	{
		if(!interactionInited)
			return;
		
		// display debug information
		if(debugText)
		{
			string sGuiText = string.Empty;

			//if(isLeftHandPrimary)
			{
				sGuiText += "L.Hand" + (isLeftHandInteracting ? "*: " : " : ") + leftHandScreenPos.ToString();
				
				if(lastLeftHandEvent == HandEventType.Grip)
				{
					sGuiText += "  LeftGrip";
				}
				else if(lastLeftHandEvent == HandEventType.Release)
				{
					sGuiText += "  LeftRelease";
				}
				
				if(isLeftHandClick)
				{
					sGuiText += "  LeftClick";
				}
//				else if(leftHandClickProgress > 0.5f)
//				{
//					sGuiText += String.Format("  {0:F0}%", leftHandClickProgress * 100);
//				}
				
				if(isLeftHandPress)
				{
					sGuiText += "  LeftPress";
				}
			}
			
			//if(isRightHandPrimary)
			{
				sGuiText += "\nR.Hand" + (isRightHandInteracting ? "*: " : " : ") + rightHandScreenPos.ToString();
				
				if(lastRightHandEvent == HandEventType.Grip)
				{
					sGuiText += "  RightGrip";
				}
				else if(lastRightHandEvent == HandEventType.Release)
				{
					sGuiText += "  RightRelease";
				}
				
				if(isRightHandClick)
				{
					sGuiText += "  RightClick";
				}
//				else if(rightHandClickProgress > 0.5f)
//				{
//					sGuiText += String.Format("  {0:F0}%", rightHandClickProgress * 100);
//				}

				if(isRightHandPress)
				{
					sGuiText += "  RightPress";
				}
			}
			
			debugText.text = sGuiText;
		}
		
		// display the cursor status and position
		if(showHandCursor)
		{
			Texture texture = null;
			
			if(isLeftHandPrimary)
			{
				if(lastLeftHandEvent == HandEventType.Grip)
					texture = gripHandTexture;
				else if(lastLeftHandEvent == HandEventType.Release)
					texture = releaseHandTexture;
			}
			else if(isRightHandPrimary)
			{
				if(lastRightHandEvent == HandEventType.Grip)
					texture = gripHandTexture;
				else if(lastRightHandEvent == HandEventType.Release)
					texture = releaseHandTexture;
			}
			
			if(texture == null)
			{
				texture = normalHandTexture;
			}
			
			//if(useHandCursor)
			{
				if((texture != null) && (isLeftHandPrimary || isRightHandPrimary))
				{
					Rect rectTexture; 

					if(controlMouseCursor)
					{
						MouseControl.MouseMove(cursorScreenPos, debugText);
						rectTexture = new Rect(Input.mousePosition.x - texture.width / 2, Screen.height - Input.mousePosition.y - texture.height / 2, 
						                       texture.width, texture.height);
					}
					else 
					{
						rectTexture = new Rect(cursorScreenPos.x * Screen.width - texture.width / 2, (1f - cursorScreenPos.y) * Screen.height - texture.height / 2, 
						                       texture.width, texture.height);
					}

					GUI.DrawTexture(rectTexture, texture);
				}
			}
		}
	}

}
