using UnityEngine;
using System.Collections;
using SwipeMenu;

/// <summary>
/// El gestor de la app es un singleton persistente que maneja el tiempo
/// </summary>
public class AppManager : PersistentSingleton<AppManager>
{		

	public float TimeScale { get; private set; }
	/// true si la app está en pausa
	public bool Paused { get; set; } 

	
    // almacenamiento
    protected float _savedTimeScale=1f;

			
	/// <summary>
	/// establece la escala de tiempo a la de los parámetros
	/// </summary>
	/// <param name="newTimeScale">Nueva escala de tiempo.</param>
	public virtual void SetTimeScale(float newTimeScale)
	{
		_savedTimeScale = Time.timeScale;
		Time.timeScale = newTimeScale;
	}
	
	/// <summary>
	/// Restablece la escala de tiempo a la última escala de tiempo guardada.
	/// </summary>
	public virtual void ResetTimeScale()
	{
		Time.timeScale = _savedTimeScale;
	}
	
	/// <summary>
	/// Pauses the app or unpauses it depending on the current state
	/// </summary>
	public virtual void Pause()
	{	
		// if time is not already stopped		
		if (Time.timeScale>0.0f)
		{
			Instance.SetTimeScale(0.0f);
			Instance.Paused=true;
			GUIManager.Instance.SetPause(true);
		}
		else
		{
            UnPause();
		}		
	}

    /// <summary>
    /// Unpauses the app
    /// </summary>
    public virtual void UnPause()
    {
        Instance.ResetTimeScale();
        Instance.Paused = false;
        if (GUIManager.Instance!= null)
        { 
            GUIManager.Instance.SetPause(false);
        }
    }

	
	
		
}
