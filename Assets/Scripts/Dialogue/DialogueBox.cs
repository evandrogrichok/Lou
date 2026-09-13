using System.Collections;
using TMPro;
using UnityEngine;

// DialogueBox: A view.
/*
    Esse script é responsável por MOSTRAR as falas. Como se fosse o motor das Textboxes.

    Seu papel é simples: Exibir e ocultar caixa e texto, e aplicar efeitos de digitação,
    controlar a velocidade de digitação e controlar os parâmetro booleanos.

    O texto, atualmente, é provido pelo script DialogueTest.
*/

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText; //a referencia para o componente do gmobj
    [SerializeField] private float typingSpeed = 0.05f;
    public bool isTyping { get; private set; }
    private Coroutine typingCoroutine; // referencia para a corrotina.

    public void ShowText(DialogueLine line)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        
        typingCoroutine = StartCoroutine(TypeSentence(line.text));
        // a corrotina é 100% responsável pela digitação do texto
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";
        
        foreach(char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        
    }

    public void ForceCompleteText(DialogueLine line)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogText.text = line.text;
        isTyping = false;

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
