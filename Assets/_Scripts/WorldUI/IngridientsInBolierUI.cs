using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngridientsInBolierUI : MonoBehaviour
{
    [SerializeField] private InteractBoiler _interactBoiler;

    [SerializeField] private int _amountCreateTemplate = 4;
    [Space]
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _iconTemplate;

    private List<Image> _iconIngridientList;
    private Sprite _defaultSprite;

    private void Awake()
    {
        _iconIngridientList = new();

        for (int i = 0; i < _amountCreateTemplate; i++)
            InstantiateTemplate();
    }

    private void Start()
    {
        _interactBoiler.OnIngridientChanged += PlayerInventory_OnIngridientAdded;

        _defaultSprite = _iconTemplate.GetComponent<SingleIconIngridientUI>().IconImage.sprite;
        UpdateVisual();
    }

    private void PlayerInventory_OnIngridientAdded(object sender, System.EventArgs e)
    {
        UpdateVisual();

    }

    private void UpdateVisual()
    {
        if (_interactBoiler.IngridientsInBoilerList.Count <= 0)
        {
            for (int i = 0; i < _iconIngridientList.Count; i++)
                _iconIngridientList[i].sprite = _defaultSprite;

            return;
        }

        for (int i = 0; i < _interactBoiler.IngridientsInBoilerList.Count; i++)
        {
            _iconIngridientList[i].sprite = _interactBoiler.IngridientsInBoilerList[i].Sprite;
        }
    }

    private void InstantiateTemplate()
    {
        Transform recipeTransform = Instantiate(_iconTemplate, _container);
        recipeTransform.gameObject.SetActive(true);
        SingleIconIngridientUI iconIngridientUI = recipeTransform.gameObject.GetComponent<SingleIconIngridientUI>();
        _iconIngridientList.Add(iconIngridientUI.IconImage);
    }
}
