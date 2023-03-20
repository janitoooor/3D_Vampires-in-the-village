using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualNPCHunter : MonoBehaviour
{
    [SerializeField] private NpcCharacter _characterNpc;

    public void DesActiveNpcObject()
    {
        _characterNpc.gameObject.SetActive(false);
    }

    public void HitPlayer()
    {
        PlayerVampire.Instance.TakeDamage();
    }
}
