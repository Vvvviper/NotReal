using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Controller;
using UnityEngine.Events;

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
    [Header("Fade in and out")]
    [SerializeField] private GameObject _fadePanel;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _inBetweenTime = 0.5f;
    [SerializeField] private UnityEvent _fadeAction;

    private CanvasGroup _canvasGroup;

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
        _canvasGroup = _fadePanel.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            FadeInAndOut();
        }
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

    public void FadeInAndOut()
    {
        StartCoroutine(FadeAnim(_fadeDuration, _inBetweenTime, _fadeAction));
    }

    IEnumerator FadeAnim(float duration, float inBetweenTime, UnityEvent e)
    {
        FPC.playerCanMove = false;

        float timeElapsed = 0f;
        while( timeElapsed <= duration)
        {
            float a = Mathf.Lerp(0f, 1f, timeElapsed / duration);
            _canvasGroup.alpha = a;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 1f;
        e.Invoke();
        timeElapsed = 0f;
        yield return new WaitForSeconds(inBetweenTime);
        while (timeElapsed <= duration)
        {
            float a = Mathf.Lerp(1f, 0f, timeElapsed / duration);
            _canvasGroup.alpha = a;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 0f;
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
