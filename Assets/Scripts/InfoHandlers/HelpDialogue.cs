using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

/// <summary>
/// Contiene las bases necesaria para el manejo de la información en el panel de Ayuda
 /// </summary>
/// <returns>ampos asociados al manejo de la información</returns>
public class HelpDialogue
{

    public string name;

    [TextArea(5,10)]
    public string[] sentences;
    public Sprite[] sprites;

}
