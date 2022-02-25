using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueScripts
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
    [SerializeField] private List<DialogueScripts> _dialogueStrings;
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private float _disappearTime = 2f;

    private int _dialoguePieceIndex = 0;
    private int _dialogueIndex = 0;

    public enum DialogueState
    {
        ShowingDia,
        InPlace,
        Hide,
        Interact
    }

    public DialogueState CurrentDialogueState = DialogueState.Hide;

    private void Start()
    {
        ShowLine();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (CurrentDialogueState)
            {
                case DialogueState.ShowingDia:
                    break;
                case DialogueState.InPlace:
                    if (_dialogueIndex < _dialogueStrings.Count)
                    {
                        ShowLine();
                    }
                    else
                    {
                        HideDialoge();
                    }
                    break;
                case DialogueState.Hide:
                    break;
                case DialogueState.Interact:
                    HideDialoge();
                    break;
                default:
                    break;
            }

        }*/
    }

    public void ShowLine()
    {
        if (_dialoguePieceIndex >= _dialogueStrings.Count)
            return;
        if (_dialogueIndex >= _dialogueStrings[_dialoguePieceIndex].DialoguePiece.Count)
            return;
        DialogueBox.SetActive(true);
        StartCoroutine(PopText());
    }

    public void ShowLine(string content)
    {
        DialogueBox.SetActive(true);
        StartCoroutine(PopText(content));
    }

    public void HideDialoge()
    {
        DialogueBox.SetActive(false);
    }

    IEnumerator PopText()
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
            
    }

    IEnumerator PopText(string content)
    {
        float time = 0f;
        CurrentDialogueState = DialogueState.ShowingDia;
        string t = "";
        foreach (char c in content.ToCharArray())
        {
            t += c;
            _textComponent.text = t;
            yield return new WaitForSeconds(_textSpeed );
            time += _textSpeed;
        }
        CurrentDialogueState = DialogueState.Interact;
        yield return new WaitForSeconds(_disappearTime + time / 5f);
        HideDialoge();
    }
}
