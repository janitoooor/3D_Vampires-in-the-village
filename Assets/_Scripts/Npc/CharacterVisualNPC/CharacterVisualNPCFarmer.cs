using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualNPCFarmer : MonoBehaviour
{
    [SerializeField] private NpcCharacter _characterNpc;

    public void DesActiveNpcObject()
    {
        _characterNpc.gameObject.SetActive(false);
    }
}
