using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelloPlayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(() =>
        {
            StartCoroutine(StartGame());
        });
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        _canvasGroup.alpha -= 0.1f;
        if (_canvasGroup.alpha > 0)
            StartCoroutine(StartGame());
    }
}
