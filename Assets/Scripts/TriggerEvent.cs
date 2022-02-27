using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent _action;
    [SerializeField] private Animation _animation;
    [SerializeField] private bool _onlyTriggerOnce = true;

    private bool _triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;
        if (other.CompareTag("Player"))
        {
            _action.Invoke();
            if (_onlyTriggerOnce)
            {
                _triggered = true;
            }
        }
    }

    public void PlayHeadmonsterAnim()
    {
        if (_animation == null)
            return;
        _animation.Play();
    }
}
