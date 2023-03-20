using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_VisualPlayer : MonoBehaviour
{
    public void EndAttack()
    {
        PlayerVampire.Instance.EndStateAttack();
    }
}
