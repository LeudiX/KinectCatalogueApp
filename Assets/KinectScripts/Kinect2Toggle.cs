using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Catalogue{


[AddComponentMenu("Scripts/GUI/Kinect2 Toggle")]
public class Kinect2Toggle : MonoBehaviour
{
    [SerializeField] protected Toggle toggle;
    [SerializeField] private GameObject eventSyst;  

     string optionName;
     bool isOn;
    
    private void Reset() 
    {
        toggle = GetComponentInChildren<Toggle>();
        
    }

    public bool Value 
    {
        
        get { return toggle.isOn = Value;}
        set {
            if(toggle.isOn == value)
               OnValueChange(value);

            else
                toggle.isOn = value;

            }
    }

    /// <summary>
	/// Inicializa los valores y suscribe los listeners a los eventos.
	/// </summary>
    private void Awake()
    {
        
        toggle = GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener((bool _) => OnValueChange(_));
        Value = AppSettingsManager.LoadBool(optionName, isOn );
    }

  
    private void OnValueChange(bool isOn)
		{

         Cursor.visible = !isOn;   
         InteractionManager.Instance.enabled= isOn;
         eventSyst.GetComponent<InteractionInputModule>().enabled = isOn; 
         eventSyst.GetComponent<StandaloneInputModule>().enabled = !isOn;  
      
         AppSettingsManager.SaveBool(optionName,isOn);  
        }
    
    }
}