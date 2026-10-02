using System.Collections;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

// DialogueBox: A view.
/*
    Esse script é responsável por MOSTRAR as falas. Como se fosse o motor das Textboxes.

    Seu papel é simples: Exibir e ocultar caixa e texto, e aplicar efeitos de digitação,
    controlar a velocidade de digitação e controlar os parâmetro booleanos.

    O texto, atualmente, é provido pelo script DialogueTest.
*/

public class DialogueBox : MonoBehaviour
{   

    [SerializeField] private DialogueTextAnimator textAnimator;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform bubbleImage;
    private RectTransform rectTransform;
    private RectTransform canvasRect;

    private GameObject speaker;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
        //agora, aqui, pegamos o recttransform do CANVAS. (a UI).
    }


    public void ShowText(DialogueLine line)
    {
        textAnimator.StartTyping(line.text);

        SetBubbleSize();

        speaker = line.speaker;
    }


    private void UpdatePosition(GameObject target)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.GetComponentInChildren<DialoguePoint>().transform.position); //pegando posicao
        //aqui, ele pega a camera principal, chama a funcao world to screen point. dentro, temos ele pega a posicao do speaker.
        //ela devolve a posição correspondente do speaker em screenspace.

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, canvas.worldCamera, out Vector2 localPosition);
        // Traduz a posição da tela (Screen Space) para uma posição
        // local dentro do RectTransform do Canvas.
        // O resultado é armazenado em localPosition.

        rectTransform.anchoredPosition = localPosition;
        //depois, a local position é atribuida à posição do RectTransform em relação à âncora.
    }



    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (gameObject.activeSelf == true && speaker != null) UpdatePosition(speaker);
    }

    public void SetBubbleSize()
    {
        Vector2 prefValues = textAnimator.GetTextSize();
        Vector2 padding = new Vector2(20f, 20f);

        bubbleImage.sizeDelta = prefValues + padding;
    }

    public bool IsTyping()
    {
        return textAnimator.isTyping;
    }

    public void ForceCompleteText(string text)
    {
        textAnimator.ForceCompleteText(text);
    }

}




