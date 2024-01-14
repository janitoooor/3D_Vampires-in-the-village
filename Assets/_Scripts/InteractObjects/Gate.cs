using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _InteractUI;
    private const string TRIGGER_UNLOCK = "Unlock";

    private bool _isUnlocked;

    private void Start()
    {
        PlayerVampire.Instance.OnUnlockGate += PlayerVampire_OnUnlockGate;
        HideInteract();
    }

    private void PlayerVampire_OnUnlockGate(object sender, PlayerVampire.OnUnlockGateEventArgs e)
    {
        if (e.Gate == this)
        {
            _animator.SetTrigger(TRIGGER_UNLOCK);
            _isUnlocked = true;
            HideInteract();
        }
    }

    public void ShowInteract()
    {
        if (!_isUnlocked)
            _InteractUI.SetActive(true);
    }

    public void HideInteract()
    {
        _InteractUI.SetActive(false);
    }
}
