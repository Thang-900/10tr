using UnityEngine;
using UnityEngine.UI;

public class RTSSelectionManager : MonoBehaviour
{
    public RectTransform selectionBox;
    private Vector2 startPos;
    private Vector2 endPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            selectionBox.gameObject.SetActive(true);

            TrySelectUnitUnderMouse();
        }

        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            UpdateSelectionBox();
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);
            SelectUnitsInBox();
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveSelectedUnitsToClickPosition();
        }
    }

    void UpdateSelectionBox()
    {
        Vector2 boxStart = startPos;
        Vector2 boxSize = endPos - startPos;

        if (boxSize.x < 0)
        {
            boxStart.x += boxSize.x;
            boxSize.x = Mathf.Abs(boxSize.x);
        }

        if (boxSize.y < 0)
        {
            boxStart.y += boxSize.y;
            boxSize.y = Mathf.Abs(boxSize.y);
        }

        selectionBox.anchoredPosition = boxStart;
        selectionBox.sizeDelta = boxSize;
    }

    void TrySelectUnitUnderMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            SelectableUnit clickedUnit = hit.collider.GetComponent<SelectableUnit>();
            if (clickedUnit != null)
            {
                DeselectAllUnits();
                clickedUnit.Select();
            }
        }
        else
        {
            DeselectAllUnits();
        }
    }

    void SelectUnitsInBox()
    {
        foreach (SelectableUnit unit in FindObjectsOfType<SelectableUnit>())
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

            float xMin = Mathf.Min(startPos.x, endPos.x);
            float xMax = Mathf.Max(startPos.x, endPos.x);
            float yMin = Mathf.Min(startPos.y, endPos.y);
            float yMax = Mathf.Max(startPos.y, endPos.y);

            if (screenPos.x >= xMin && screenPos.x <= xMax && screenPos.y >= yMin && screenPos.y <= yMax)
            {
                unit.Select();
            }
            else
            {
                unit.Deselect();
            }
        }
    }

    void DeselectAllUnits()
    {
        foreach (SelectableUnit unit in FindObjectsOfType<SelectableUnit>())
        {
            unit.Deselect();
        }
    }

    void MoveSelectedUnitsToClickPosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (SelectableUnit unit in FindObjectsOfType<SelectableUnit>())
        {
            if (unit.isSelected)
            {
                unit.MoveTo(mousePos);
            }
        }
    }
}
