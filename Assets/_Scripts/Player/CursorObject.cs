using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    public const string GROUND_TAG = "Ground";
    public const string INTERACT_POINT_TAG = "InteractPoint";

    [SerializeField] private Camera _camera;
    [SerializeField] private CursorPointObject _cursorPointObjectPrefab;

    private CursorPointObject _cursorPointObject;

    private SelectableObject _hovered;
    private SelectableObject _currentSelectableObject;

    private void Start()
    {
        _cursorPointObject = Instantiate(_cursorPointObjectPrefab);
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            HoverObject(hit);
        else
            UnHoverCurrent();

        ClickOnObject(hit);
    }

    private void HoverObject(RaycastHit hit)
    {
        if (hit.collider.GetComponent<SelectableCollider>())
        {
            SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
            if (_hovered)
            {
                if (_hovered != hitSelectable)
                {
                    _hovered.UnOnHover();
                    _hovered = hitSelectable;
                    _hovered.OnHover();
                }
            }
            else
            {
                _hovered = hitSelectable;
                _hovered.OnHover();
            }
        }
        else
        {
            UnHoverCurrent();
        }
    }

    private void ClickOnObject(RaycastHit hit)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_hovered)
            {
                _currentSelectableObject = _hovered;
                _hovered.Select();
            }

            if (_currentSelectableObject != null && hit.collider.TryGetComponent(out IHoverObject hoverObject))
            {
                _currentSelectableObject.ClickOnHoverObject(hoverObject);
            }
            else if (_currentSelectableObject != null && hit.collider.CompareTag(GROUND_TAG) || hit.collider.CompareTag(INTERACT_POINT_TAG))
            {
                _currentSelectableObject.ClickOnGround(hit.point);

                _cursorPointObject.ClickOnPoint(hit.point);
            }
        }
    }
    private void UnHoverCurrent()
    {
        if (_hovered)
        {
            _hovered.UnOnHover();
            _hovered = null;
        }
    }
}
