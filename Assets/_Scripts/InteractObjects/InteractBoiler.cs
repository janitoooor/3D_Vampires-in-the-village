using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBoiler : MonoBehaviour, IHoverObject
{
    public event EventHandler OnIngridientChanged;

    [SerializeField] private Transform _topPoint;
    [Space]
    [SerializeField] private RecipeIngridientSO _recipeIngridients;
    [SerializeField] private IngridientsInBolierUI _ingridientsInBolierUI;
    [SerializeField] private SingleIconIngridientUI _finishIconRecipeUI;
    [Space]
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private GameObject _selectionIndicator;
    [SerializeField] private float _timeToSpawnRecipeFinish = 2f;

    private List<IngridientSO> _ingridientsInBoilerList;
    public List<IngridientSO> IngridientsInBoilerList => _ingridientsInBoilerList;

    private GameObject _finishRecipeObject;

    public bool IsRecipeFinish => _ingridientsInBoilerList.Count == _recipeIngridients.IngridientSOList.Count;

    private void Awake()
    {
        _ingridientsInBoilerList = new();
    }

    private void Start()
    {
        HideInteract();
        HideSelectionIndicator();
        HideFinishRecipe();
    }

    public void ShowInteract()
    {
        _interactUI.SetActive(true);
        ShowIngridientsInBoiler();
    }

    public void HideInteract()
    {
        _interactUI.SetActive(false);
        HideIngridientsInBoiler();
    }

    public void ShowSelectionIndicator()
    {
        _selectionIndicator.SetActive(true);
    }

    public void HideSelectionIndicator()
    {
        _selectionIndicator.SetActive(false);
    }

    public void ShowIngridientsInBoiler()
    {
        if (!IsRecipeFinish)
            _ingridientsInBolierUI.gameObject.SetActive(true);
    }

    public void HideIngridientsInBoiler()
    {
        _ingridientsInBolierUI.gameObject.SetActive(false);
    }

    public IngridientSO GiveFinishRecipeSO()
    {
        if (IsRecipeFinish)
        {
            HideFinishRecipe();
            HideInteract();

            Destroy(_finishRecipeObject);
            _finishRecipeObject = null;
            _ingridientsInBoilerList.Clear();
            OnIngridientChanged?.Invoke(this, EventArgs.Empty);

            return _recipeIngridients;
        }

        return null;
    }

    public List<IngridientSO> AddIngridient(List<IngridientSO> listIngridients)
    {
        if (IsRecipeFinish)
            return listIngridients;

        ShowInteract();

        List<IngridientSO> ingridientsInInventory = listIngridients;

        for (int i = 0; i < _recipeIngridients.IngridientSOList.Count; i++)
        {
            for (int j = 0; j < listIngridients.Count; j++)
            {
                if (listIngridients[j] == _recipeIngridients.IngridientSOList[i] && !_ingridientsInBoilerList.Contains(listIngridients[j]))
                {
                    _ingridientsInBoilerList.Add(listIngridients[j]);
                    ingridientsInInventory.Remove(listIngridients[j]);
                    break;
                }
            }
        }

        _interactUI.SetActive(false);
        OnIngridientChanged?.Invoke(this, EventArgs.Empty);

        if (IsRecipeFinish)
            StartCoroutine(WaitToShowFinishRecipe());

        return ingridientsInInventory;
    }

    private IEnumerator WaitToShowFinishRecipe()
    {
        yield return new WaitForSeconds(_timeToSpawnRecipeFinish);
        HideIngridientsInBoiler();

        _finishIconRecipeUI.IconImage.sprite = _recipeIngridients.Sprite;
        ShowFinishRecipe();
        _interactUI.SetActive(true);

        _finishRecipeObject = Instantiate(_recipeIngridients.Prefab, _topPoint);
        _finishRecipeObject.transform.localPosition = Vector3.zero;
    }

    private void ShowFinishRecipe()
    {
        _finishIconRecipeUI.gameObject.SetActive(true);
    }

    private void HideFinishRecipe()
    {
        _finishIconRecipeUI.gameObject.SetActive(false);
    }
}
