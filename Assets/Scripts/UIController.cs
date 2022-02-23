using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    
    [SerializeField] private GameObject DialogueBox;

    private DialogueController DiaCon;

    private void Start()
    {
        DiaCon = DialogueBox.GetComponent<DialogueController>();
    }

    public void ShowDialogue()
    {

    }
}
