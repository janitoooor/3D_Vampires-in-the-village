using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private int _amountCreateTemplate = 4;
    [Space]
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _iconTemplate;
    [SerializeField] private PlayerInventory _playerInventory;

    private List<Image> _iconIngridientList;

    private void Awake()
    {
        _iconIngridientList = new();

        for (int i = 0; i < _amountCreateTemplate; i++)
            InstantiateTemplate();
    }

    private void Start()
    {
        _playerInventory.OnIngridientChanged += PlayerInventory_OnIngridientAdded;

        UpdateVisual();
    }

    private void PlayerInventory_OnIngridientAdded(object sender, System.EventArgs e)
    {
        UpdateVisual();

    }

    private void UpdateVisual()
    {
        foreach (Transform child in _container.transform)
            child.gameObject.SetActive(false);

        for (int i = 0; i < _playerInventory.IngridientSOList.Count; i++)
        {
            _iconIngridientList[i].gameObject.SetActive(true);
            _iconIngridientList[i].sprite = _playerInventory.IngridientSOList[i].Sprite;
        }
    }

    private void InstantiateTemplate()
    {
        Transform recipeTransform = Instantiate(_iconTemplate, _container);
        recipeTransform.gameObject.SetActive(true);
        _iconIngridientList.Add(recipeTransform.gameObject.GetComponent<Image>());
    }
}
