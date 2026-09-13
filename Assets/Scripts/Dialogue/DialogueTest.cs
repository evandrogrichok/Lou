using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

// DialogueTest: O comandante.
/*
    Esse script é responsável por MANDAR no motor da textbox.
    Ele diz quando começar, qual fala exibir, quando avançar, quando pular a animação...
*/


public class DialogueTest : MonoBehaviour
{
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private DialogueLine[] lines; //aqui colocamos o tipo para o objeto DialogueLine. 

    private int currentLine = 0;
    private bool isTalking = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnInteract(InputValue value)
    {
        if (!isTalking)
        {
            StartDialogue();
        }
        else
        {
            ResolveIsTyping();   
        }
    }

    private void ResolveIsTyping()
    {
        if (dialogueBox.isTyping)
        {
            dialogueBox.ForceCompleteText(lines[currentLine]);
        }
        else
        {
            AdvanceDialogue();
        }
    }

    private void StartDialogue()
    {   
        if (lines.Length ==  0) return;
        
        isTalking = true;
        currentLine = 0;

        dialogueBox.Show();
        dialogueBox.ShowText(lines[currentLine]);
    }

    private void AdvanceDialogue()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            dialogueBox.ShowText(lines[currentLine]);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isTalking = false;
        dialogueBox.Hide();
    }
    

    // Update is called once per frame
    void Update()
    {


    }
}
