using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/RecipeIngridientSO ")]
public class RecipeIngridientSO : IngridientSO
{
    [SerializeField] private List<IngridientSO> _ingridientSOList;
    public List<IngridientSO> IngridientSOList => _ingridientSOList;
}
