using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private NpcCharacter _npcFarmer;
    [SerializeField] private GameObject _interactImage;

    private void Start()
    {
        _npcFarmer.OnNpcTriggerEnter += NpcFarmer_OnNPCTryAttack;
        _npcFarmer.OnNpcTriggerExit += NpcFarmer_OnNpcTriggerExit;

        Hide();
    }

    private void NpcFarmer_OnNpcTriggerExit(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void NpcFarmer_OnNPCTryAttack(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Hide()
    {
        _interactImage.SetActive(false);
    }

    private void Show()
    {
        _interactImage.SetActive(true);
    }
}
