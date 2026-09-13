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
    [SerializeField] private Canvas canvas;
    
    public bool isTyping { get; private set; }
    private Coroutine typingCoroutine; // referencia para a corrotina.

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowText(DialogueLine line)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(line.speaker.GetComponentInChildren<DialoguePoint>().transform.position); //pegando posicao
        //aqui, ele pega a camera principal, chama a funcao world to screen point. dentro, temos ele pega a posicao do speaker.
        //ela devolve a posição correspondente do speaker em screenspace.

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        //agora, aqui, pegamos o recttransform do CANVAS. (a UI).

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, canvas.worldCamera, out Vector2 localPosition);
        // Traduz a posição da tela (Screen Space) para uma posição
        // local dentro do RectTransform do Canvas.
        // O resultado é armazenado em localPosition.

        rectTransform.anchoredPosition = localPosition;
        //depois, a local position é atribuida à posição do RectTransform em relação à âncora.

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
