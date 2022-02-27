using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent _action;
    [SerializeField] private Animation _animation;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _action.Invoke();
    }

    public void PlayHeadmonsterAnim()
    {
        if (_animation == null)
            return;
        _animation.Play();
    }
}
