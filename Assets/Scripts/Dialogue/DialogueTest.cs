using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

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
            dialogueBox.ForceCompleteText(lines[currentLine].text);
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
        dialogueBox.ShowText(lines[currentLine].text);
    }

    private void AdvanceDialogue()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            dialogueBox.ShowText(lines[currentLine].text);
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
