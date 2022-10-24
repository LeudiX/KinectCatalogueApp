using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Facilita el cambio de escenas en la aplicación al interactuar con LevelSelector. Tambien maneja
/// el cambio de nombre de escena y el efecto de Fade que se lleva a cabo durante la transición de estas.
/// </summary>
public class LevelManager : MonoBehaviour
{
	/// Singleton
	public static LevelManager Instance { get; private set; }		

	
	//[Header("Prefabs")]
	//public GameObject menuPrefab ;
	
	
	
	[Space(10)]
	[Header("Intro and Outro durations")]
	/// duration of the initial fade in
	public float IntroFadeDuration=1f;
	/// duration of the fade to black at the end of the level
	public float OutroFadeDuration=1f;

    // private stuff
    
    
	
	/// <summary>
	/// On awake, instantiates the catalogue
	/// </summary>
	public virtual void Awake()
	{
		Instance=this;
       
    }

	/// <summary>
	/// Initialization
	/// </summary>
	public virtual void Start()
	{
		if (GUIManager.Instance != null)
		{

			// set the level name in the GUI
			GUIManager.Instance.SetLevelName(SceneManager.GetActiveScene().name);

			// fade in
			GUIManager.Instance.FaderOn(false, IntroFadeDuration);
		}
	}

	/// <summary>
	/// Gets the player to the specified level
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public virtual void GotoLevel(string levelName)
	{		
        if (GUIManager.Instance!= null)
        { 
    		GUIManager.Instance.FaderOn(true,OutroFadeDuration);
        }
        StartCoroutine(GotoLevelCo(levelName));
    }

    /// <summary>
    /// Espera un breve periodo de tiempo anes de cargar la escena deseada
    /// </summary>
    /// <returns>The scene co.</returns>
    /// <param name="levelName">Scene name.</param>
    protected virtual IEnumerator GotoLevelCo(string levelName)
	{
      if (Time.timeScale > 0.0f)
        { 
            yield return new WaitForSeconds(OutroFadeDuration);
        }
        AppManager.Instance.UnPause();

        if (string.IsNullOrEmpty(levelName))
		
			throw new Exception ("No se ha encontrado el nivel al que se desea acceder. Por favor, indíquelo en el Inspector");	
			
		else
	
			SceneManager.LoadScene(levelName);
		
	}

}

