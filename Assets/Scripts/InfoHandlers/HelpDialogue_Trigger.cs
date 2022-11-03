using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger para manejar la información y las imágenes en el panel de Ayuda
/// </summary>
/// <returns>Da inicio al evento de muestra de información en el panel de Ayuda</returns>
public class HelpDialogue_Trigger : MonoBehaviour {
    public HelpDialogue dialogue;

/// <summary>
/// Lanza el trigger para el manejo de la información en el panel de Ayuda
/// </summary>
/// <returns>Inicia el evento</returns>
    public void TriggerDialogue () {
        FindObjectOfType<HelpManager> ().StartHelpDialogue (dialogue);
    }

}