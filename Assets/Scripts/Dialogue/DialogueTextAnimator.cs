using System.Collections;
using System.Data.Common;
using TMPro;
using UnityEngine;



public class DialogueTextAnimator : MonoBehaviour
{
    public bool isTyping { get; private set; }
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;


    

    public void StartTyping(string text)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        dialogText.text = text;
        dialogText.ForceMeshUpdate();
        
        HideAllCharacters();
        typingCoroutine = StartCoroutine(TypeSentence()); 
    }

    private IEnumerator TypeSentence()
    {
        isTyping = true;
        
        for (int i = 0; i < dialogText.textInfo.characterCount; i++)
        {   
            TMP_CharacterInfo infoChar = dialogText.textInfo.characterInfo[i];
            int vertexIndex = infoChar.vertexIndex;

            if(!infoChar.isVisible)
            continue;

            Color32[] colors = GetCharColors(infoChar);
            
            for (int j = 0; j < 4; j++)
            {
                colors[vertexIndex + j].a = 255;
            }

            dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            StartCoroutine(PopChar(infoChar));
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void ForceCompleteText(string line)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogText.text = line;
        isTyping = false;

    }

    private Color32[] GetCharColors(TMP_CharacterInfo info)
    {
        int materialIndex = info.materialReferenceIndex;
        return dialogText.textInfo.meshInfo[materialIndex].colors32;
    }

    public void HideAllCharacters()
    {
        for (int i = 0; i < dialogText.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo infoChar = dialogText.textInfo.characterInfo[i];
            int vertexIndex = infoChar.vertexIndex;

            if(!infoChar.isVisible)
            continue;

            Color32[] colors = GetCharColors(infoChar);
            
            for (int j = 0; j < 4; j++)
            {
                colors[vertexIndex + j].a = 0;
            }
        }

        dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    public Vector2 GetTextSize()
    {
        return dialogText.GetPreferredValues();
    }
    
    private IEnumerator PopChar(TMP_CharacterInfo charInfo)
    {
        int vertexIndex = charInfo.vertexIndex;// pega o index do vertice da letra
        int materialIndex = charInfo.materialReferenceIndex; //index material da letra

        Vector3[] vertices = dialogText.textInfo.meshInfo[materialIndex].vertices; //pega todos os vertices de todas as letras

        Vector3 center = (charInfo.bottomLeft + charInfo.topRight) / 2f;//pega a ref do centro da letra

        // Guarda os vértices originais
        Vector3[] originalVertices = new Vector3[4]; //cria array pra guardar como era a letra originalmente

        for(int i = 0; i < 4; i++)
        {
            originalVertices[i] = vertices[vertexIndex + i]; //guarda os originais
        }

        float duration = 0.05f; //duração letra pop
        float elapsed = 0f; //quanto tempo passou

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration; // aqui forçamos o lerp a acompanhar a duraçãp

            // 0 -> 1.2 -> 1
            float currentScale;


            currentScale = Mathf.Lerp(0f, 1f, t);


            for(int i = 0; i < 4; i++)
            {
                vertices[vertexIndex + i] =
                    center + (originalVertices[i] - center) * currentScale;
                    //atualizamos os vertices
            } 

            dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            yield return null; //espera frame
        }

        // Garante que termine exatamente em 1
        for(int i = 0; i < 4; i++)
        {
            vertices[vertexIndex + i] = originalVertices[i];
        }

        dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

    
}

