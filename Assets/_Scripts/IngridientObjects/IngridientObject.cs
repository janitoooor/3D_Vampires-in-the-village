using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngridientObject : MonoBehaviour, IHoverObject
{
    [SerializeField] private IngridientSO _ingridientSO;
    [SerializeField] private Image _interactImage;
    [SerializeField] private GameObject _selectionIndicator;

    public IngridientSO IngridientSO => _ingridientSO;

    private void Start()
    {
        HideInteract();
        HideSelectionIndicator();
    }

    public void ShowInteract()
    {
        _interactImage.gameObject.SetActive(true);
    }

    public void HideInteract()
    {
        _interactImage.gameObject.SetActive(false);
    }

    public void ShowSelectionIndicator()
    {
        _selectionIndicator.gameObject.SetActive(true);
    }

    public void HideSelectionIndicator()
    {
        _selectionIndicator.gameObject.SetActive(false);
    }
}




