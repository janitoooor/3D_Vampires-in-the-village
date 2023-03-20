using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacterFarmerPoison : NpcCharacterFarmer
{
    private protected override void OnDisable()
    {
        PlayerVampire.Instance.PoisonPlayer();
    }
}
