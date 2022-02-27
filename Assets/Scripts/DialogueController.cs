using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Controller;

/*[System.Serializable]
public class Dialogue
{
    public List<string> DialoguePiece;
}*/


public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TextMeshProUGUI _textComponent;
    [SerializeField] private float _textSpeed;
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private float _disappearTime = 2f;
    [SerializeField] private List<string> _startingDialogue;
    [SerializeField] private FirstPersonController FPC;

    public enum DialogueState
    {
        ShowingDialoue,
        ShowingLine,
        InPlace,
        Hide,
    }

    public DialogueState CurrentDialogueState = DialogueState.Hide;

    private void Start()
    {
        ShowDialogue(_startingDialogue);
    }

    private void Update()
    {
        
    }

    public void ShowDialogue(List<string> dia)
    {
        StartCoroutine(PopDialogue(dia));
    }

    public void ShowLine(string content)
    {
        if (CurrentDialogueState == DialogueState.ShowingDialoue)
            return;
        StartCoroutine(PopLine(content));
    }

    public void HideDialoge()
    {
        if (CurrentDialogueState != DialogueState.InPlace)
            return;
        FPC.playerCanMove = true;
        DialogueBox.SetActive(false);
        CurrentDialogueState = DialogueState.Hide;
    }

    IEnumerator PopDialogue(List<string> dia)
    {
        CurrentDialogueState = DialogueState.ShowingDialoue;
        FPC.playerCanMove = false;
        //int dialogueIndex = 0;
        for (int i = 0; i < dia.Count; i++)
        {
            DialogueBox.SetActive(true);
            float time = 0f;
            string t = "";
            foreach (char c in dia[i].ToCharArray())
            {
                t += c;
                _textComponent.text = t;
                yield return new WaitForSeconds(_textSpeed);
                time += _textSpeed;
            }
            yield return new WaitForSeconds(_disappearTime + time / 4f);
            CurrentDialogueState = DialogueState.InPlace;
            //dialogueIndex++;
        }
        
        HideDialoge();
    }

    IEnumerator PopLine(string content)
    {
        DialogueBox.SetActive(true);
        float time = 0f;
        CurrentDialogueState = DialogueState.ShowingLine;
        string t = "";
        foreach (char c in content.ToCharArray())
        {
            t += c;
            _textComponent.text = t;
            yield return new WaitForSeconds(_textSpeed );
            time += _textSpeed;
        }
        yield return new WaitForSeconds(_disappearTime + time / 4f);
        CurrentDialogueState = DialogueState.InPlace;
        HideDialoge();
    }
}
