using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Controller;

[System.Serializable]
public class Dialogue
{
    public List<string> DialoguePiece;
}


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
    [SerializeField] private Dialogue _startingDialogue;
    [SerializeField] private FirstPersonController FPC;

    public enum DialogueState
    {
        ShowingDialoue,
        ShowingLine,
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

    public void ShowDialogue(Dialogue dia)
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
        FPC.playerCanMove = true;
        DialogueBox.SetActive(false);
        CurrentDialogueState = DialogueState.Hide;
    }

    IEnumerator PopDialogue(Dialogue dia)
    {
        CurrentDialogueState = DialogueState.ShowingDialoue;
        FPC.playerCanMove = false;
        //int dialogueIndex = 0;
        for (int i = 0; i < dia.DialoguePiece.Count; i++)
        {
            DialogueBox.SetActive(true);
            float time = 0f;
            string t = "";
            foreach (char c in dia.DialoguePiece[i].ToCharArray())
            {
                t += c;
                _textComponent.text = t;
                yield return new WaitForSeconds(_textSpeed);
                time += _textSpeed;
            }
            yield return new WaitForSeconds(_disappearTime + time / 4f);
            //dialogueIndex++;
            Debug.Log(i);
        }
        
        HideDialoge();
    }
    /*IEnumerator PopText()
    {
        float time = 0f;
        CurrentDialogueState = DialogueState.ShowingDia;
        string t = "";
        foreach (char c in _dialogueStrings[_dialoguePieceIndex].DialoguePiece[_dialogueIndex].ToCharArray())
        {
            t += c;
            _textComponent.text = t;
            yield return new WaitForSeconds(_textSpeed);
            time += _textSpeed;
        }
        CurrentDialogueState = DialogueState.InPlace;
        yield return new WaitForSeconds(_disappearTime + time / 5f);
        _dialogueIndex++;
        if (_dialogueIndex < _dialogueStrings[_dialoguePieceIndex].DialoguePiece.Count)
            ShowLine();
        else
        {
            HideDialoge();
            _dialoguePieceIndex++;
            _dialogueIndex = 0;
        }
            
    }*/

    IEnumerator PopLine(string content)
    {
        DialogueBox.SetActive(true);
        FPC.playerCanMove = false;
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
        HideDialoge();
    }
}
