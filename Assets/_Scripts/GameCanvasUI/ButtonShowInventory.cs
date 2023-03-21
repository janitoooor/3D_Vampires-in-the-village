using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShowInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventoryUI _inventoryUI;
    [SerializeField] private Button _buttonInventory;

    private bool _isShowInventory;

    private void Awake()
    {
        _buttonInventory.onClick.AddListener(() =>
        {
            print("Click");
            if (!_isShowInventory)
            {
                _inventoryUI.gameObject.SetActive(true);
                _isShowInventory = true;
            }
            else
            {
                _inventoryUI.gameObject.SetActive(false);
                _isShowInventory = false;
            }
        });
    }

    private void Start()
    {
        //_inventoryUI.gameObject.SetActive(false);
    }
}
