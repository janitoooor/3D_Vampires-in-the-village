using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public event EventHandler OnIngridientChanged;

    private List<IngridientSO> _ingridientSOList;
    public List<IngridientSO> IngridientSOList => _ingridientSOList;

    private InteractBoiler _interactBoiler;

    private void Awake()
    {
        _ingridientSOList = new List<IngridientSO>();
    }

    private void Start()
    {
        PlayerVampire.Instance.OnAddIngridientToInventory += PlayerVampire_OnAddIngridientToInventory;
        PlayerVampire.Instance.OnAddIngridientToBoiler += PlayerVampire_OnAddIngridientToBoiler;
    }

    private void PlayerVampire_OnAddIngridientToBoiler(object sender, PlayerVampire.OnAddIngridientToBoilerEventArgs e)
    {
        _interactBoiler = e.InteractBoiler;

        _ingridientSOList = _interactBoiler.AddIngridient(_ingridientSOList);
        OnIngridientChanged?.Invoke(this, EventArgs.Empty);

        print("Message in InventoryPlayer");
    }

    private void PlayerVampire_OnAddIngridientToInventory(object sender, PlayerVampire.OnAddIngridientToInventoryEventArgs e)
    {
        _ingridientSOList.Add(e.IngridientSO);
        OnIngridientChanged?.Invoke(this, EventArgs.Empty);
    }
}
