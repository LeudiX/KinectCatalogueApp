﻿using UnityEngine;

/// <summary>
/// Persistent singleton.
/// </summary>
public class PersistentSingleton<T> : MonoBehaviour	where T : Component
{
	private static T _instance;
	
	/// <summary>
	/// Singleton design pattern
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance 
	{
		get 
		{
			if (_instance == null) 
			{
				_instance = FindObjectOfType<T> ();
				if (_instance == null) 
				{
					GameObject obj = new GameObject ();
					//obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent<T> ();
				}
			}
			return _instance;
		}
	}

    /// <summary>
    /// On awake, we check if there's already a copy of the object in the scene. If there's one, we destroy it.
    /// </summary>
    protected virtual void Awake ()
	{
		
		
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this as T;
			//DontDestroyOnLoad(this);
			DontDestroyOnLoad (transform.gameObject);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
