using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _textComponent;
    [SerializeField] private float _textSpeed;
    [SerializeField] private List<string> _dialogueStrings;
    [SerializeField] private GameObject DialogueBox;

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
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        }
    }

    public void ShowLine()
    {
        if (_dialogueIndex < _dialogueStrings.Count)
        {
            DialogueBox.SetActive(true);
            StartCoroutine(PopText());
            _dialogueIndex++;
        }
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
        CurrentDialogueState = DialogueState.ShowingDia;
        string t = "";
        foreach (char c in _dialogueStrings[_dialogueIndex].ToCharArray())
        {
            t += c;
            _textComponent.text = t;
            yield return new WaitForSeconds(_textSpeed);
        }
        CurrentDialogueState = DialogueState.InPlace;
    }

    IEnumerator PopText(string content)
    {
        CurrentDialogueState = DialogueState.ShowingDia;
        string t = "";
        foreach (char c in content.ToCharArray())
        {
            t += c;
            _textComponent.text = t;
            yield return new WaitForSeconds(_textSpeed);
        }
        CurrentDialogueState = DialogueState.Interact;
    }
}
