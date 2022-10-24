using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Handles all GUI effects and changes
/// </summary>
public class GUIManager : MonoBehaviour 
{

	public GameObject PauseScreen;	
	

	public Image Fader;

	public Text LevelText;
	/// the screen used for all fades

    protected static GUIManager _instance;
	
	// Singleton pattern
	public static GUIManager Instance
	{
		get
		{
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GUIManager>();
			return _instance;
		}
	}
 
    /// <summary>
    /// Sets the HUD active or inactive
    /// </summary>
    /// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
    public virtual void SetHUDActive(bool state)
    {
       
         if (LevelText!= null)
        { 
            LevelText.enabled = state;
        }
    }
	/// <summary>
	/// Sets the pause.
	/// </summary>
	/// <param name="state">If set to <c>true</c>, sets the pause.</param>
	public virtual void SetPause(bool state)
	{
        if (PauseScreen!= null)
        { 
    		PauseScreen.SetActive(state);
        }
    }
	
	/// <summary>
	/// Sets the level name in the HUD
	/// </summary>
	public virtual void SetLevelName(string name)
	{
        if (LevelText!= null)
        { 
    		LevelText.text=name;
        }
    }
	
	/// <summary>
	/// Fades the fader in or out depending on the state
	/// </summary>
	/// <param name="state">If set to <c>true</c> fades the fader in, otherwise out if <c>false</c>.</param>
	public virtual void FaderOn(bool state,float duration)
	{
        if (Fader!= null)
        { 
		    Fader.gameObject.SetActive(true);
		    if (state)
			    StartCoroutine(AppTools.FadeImage(Fader,duration, new Color(0,0,0,1f)));
		    else
			    StartCoroutine(AppTools.FadeImage(Fader,duration,new Color(0,0,0,0f)));
			
	    }
		
    }
	

}
