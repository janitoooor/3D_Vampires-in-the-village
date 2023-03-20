using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealhPlayerUI : MonoBehaviour
{
    [SerializeField] private List<Image> _singleHealthImageUIList;

    private int _currentHealth;

    private void Start()
    {
        PlayerVampire.Instance.OnPlayerIsTakeDamage += PlayerVampire_OnPlayerIsTakeDamage;
        PlayerVampire.Instance.OnPlayerIsTakeHealth += PlayerVampire_OnPlayerIsTakeHealth;

        _currentHealth = PlayerVampire.Instance.MaxHealthPlayer;

        foreach (var item in _singleHealthImageUIList)
            item.fillAmount = 1;
    }

    private void PlayerVampire_OnPlayerIsTakeHealth(object sender, System.EventArgs e)
    {
        _currentHealth = PlayerVampire.Instance.CurrentHealthPlayer;

        if (_currentHealth > 4)
            _singleHealthImageUIList[^1].fillAmount += 0.5f;
        else if (_currentHealth > 2 && _currentHealth < 4)
            _singleHealthImageUIList[^2].fillAmount += 0.5f;
        else if (_currentHealth <= 2)
            _singleHealthImageUIList[^3].fillAmount += 0.5f;
    }

    private void PlayerVampire_OnPlayerIsTakeDamage(object sender, System.EventArgs e)
    {
        _currentHealth = PlayerVampire.Instance.CurrentHealthPlayer;

        if (_currentHealth >= 4)
            _singleHealthImageUIList[^1].fillAmount -= 0.5f;
        else if (_currentHealth >= 2 && _currentHealth < 4)
            _singleHealthImageUIList[^2].fillAmount -= 0.5f;
        else if (_currentHealth < 2)
            _singleHealthImageUIList[^3].fillAmount -= 0.5f;
    }

    private void OnDestroy()
    {
        PlayerVampire.Instance.OnPlayerIsTakeDamage -= PlayerVampire_OnPlayerIsTakeDamage;
        PlayerVampire.Instance.OnPlayerIsTakeHealth -= PlayerVampire_OnPlayerIsTakeHealth;
    }
}
