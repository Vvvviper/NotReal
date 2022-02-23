using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingController : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().material);
    }

    [SerializeField] private float _fadeInDuration = 0.2f;
    [SerializeField] private float _touchShowDuration = 2f;
    [SerializeField] private float _fadeOutDuration = 0.5f;
    private bool _inAnimation = false;
    private Material _material;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_inAnimation)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            _inAnimation = true;
            StartCoroutine(CollisionAnim());
        }
    }

    IEnumerator CollisionAnim()
    {
        float timeElapsed = 0f;
        while (timeElapsed <= _fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / _fadeInDuration);
            _material.SetFloat("_alpha", alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _material.SetFloat("_alpha", 1f);
        yield return new WaitForSeconds(_touchShowDuration);
        timeElapsed = 0f;

        while (timeElapsed <= _fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / _fadeOutDuration);
            _material.SetFloat("_alpha", alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _material.SetFloat("_alpha", 0f);
        _inAnimation = false;
    }
}
