using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager para el manejo de la información en el panel de Ayuda
/// </summary>
/// <returns>Maneja  parámetros y funciones relativas al trabajo con la información que se muestra en el panel de Ayuda</returns>
public class HelpManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;

    public Image image;
    private Queue<string> sentences;
    private Queue<Sprite> sprites;
    public Animator animator;

    ///<summary> Se llama a Start antes de la actualización del primer fotograma </summary>
    void Start () {
        sentences = new Queue<string> ();
        sprites = new Queue<Sprite> ();
    }

    ///<summary> Maneja el flujo de información y la muestra de imágenes </summary>
    public void StartHelpDialogue (HelpDialogue dialogue) {
        //Debug.Log("Comenzando la descripcion de la seccion "+ dialogue.name);

        animator.SetBool ("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear ();
        sprites.Clear ();

        //información
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue (sentence);
        }
        //imágenes
        foreach (Sprite sprite in dialogue.sprites) {
            sprites.Enqueue (sprite);
        }
        //Concatena las secuencias de información 
        DisplayNextSentence ();
    }
    /// <summary>Muestra las siguientes secuencias de información</summary>    
    public void DisplayNextSentence () {
        if (sentences.Count == 0) {
            EndDialogue ();
            return;
        }
        string sentence = sentences.Dequeue ();
        Sprite sprite = sprites.Dequeue ();
        StopAllCoroutines ();
        StartCoroutine (TypeSentence (sentence));
        StartCoroutine (ShowSprite (sprite));
    }
    
    
    /// <summary>Muestra la información definida caractér por caractér</summary>    
    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray ()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    ///<summary>Muestra las imágenes definidas</summary>
    IEnumerator ShowSprite (Sprite sprite) {
        image.GetComponent<Image> ().sprite = sprite;
        yield return null;

    }

    ///<summary>Fin del diálogo. Desencadeno animación para cerrar el panel</summary>   
   public void EndDialogue () {
        animator.SetBool ("isOpen", false);
    }

}