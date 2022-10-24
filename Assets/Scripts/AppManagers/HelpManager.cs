using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;
    public Animator animator;

    // Se llama a Start antes de la actualización del primer fotograma
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartHelpDialogue (HelpDialogue dialogue)
    {
        //Debug.Log("Comenzando la descripcion de la seccion "+ dialogue.name);

        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
        EndDialogue();
        return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; 
        } 
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }



}
