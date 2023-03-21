using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        PlayerVampire.Instance.OnGameOver += PlayerVampire_OnGameOver;
        _canvasGroup.alpha = 0f;
    }

    private void PlayerVampire_OnGameOver(object sender, System.EventArgs e)
    {
        StartCoroutine(ShowGameOver());
    }

    private IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(0.1f);
        _canvasGroup.alpha -= 0.1f;
        if (_canvasGroup.alpha > 0)
            StartCoroutine(ShowGameOver());
    }
}
