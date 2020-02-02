using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer image;
    [SerializeField]
    Color activeCol, disabledCol;
    [SerializeField]
    bool isActive;
    [SerializeField]
    List<Cell> neighbourCells;

    private void OnEnable()
    {
        isActive = false;
        SetCellColor(isActive);
        neighbourCells = new List<Cell>();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void AddNeighbourCell(Cell neighbourCell)
    {
        neighbourCells.Add(neighbourCell);
    }

    public List<Cell> GetNeighbourCells()
    {
        return neighbourCells;
    }

    public void ChangeCell(bool active)
    {
        isActive = active;

        SetCellColor(isActive);
    }

    public void ChangeCell()
    {
        isActive = !isActive;

        SetCellColor(isActive);
    }

    public void SetCellColor(bool _isActive)
    {
        image.color = _isActive ? activeCol : disabledCol;
    }
}
