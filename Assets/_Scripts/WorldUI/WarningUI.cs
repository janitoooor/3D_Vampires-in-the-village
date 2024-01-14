using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUI : MonoBehaviour
{
    [SerializeField] private NpcCharacter _npcCharacter;
    [SerializeField] private GameObject _warningImage;

    private void Start()
    {
        _npcCharacter.OnNpcWarningStateEnter += NpcFarmer_OnNpcWarningStateEnter;
        _npcCharacter.OnNpcWarningStateExit += NpcFarmer_OnNpcWarningStateExit;

        Hide();
    }

    private void NpcFarmer_OnNpcWarningStateExit(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void NpcFarmer_OnNpcWarningStateEnter(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Hide()
    {
        _warningImage.SetActive(false);
    }

    private void Show()
    {
        _warningImage.SetActive(true);
    }
}
