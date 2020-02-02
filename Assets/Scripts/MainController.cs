using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    [SerializeField]
    GameObject cellPrefab;
    [SerializeField]
    Vector2Int size;
    [SerializeField]
    float cellDistance;
    [SerializeField]
    [Range(0,1)]
    float speed;

    Coroutine simulationCoroutine;
    Cell[,] cells;
    List<Cell> cellsToKill, cellsToTouch;

    private void Start()
    {
        var sw = new System.Diagnostics.Stopwatch();

        sw.Start();

        cells = new Cell[size.x, size.y];

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject tempCell = Instantiate(cellPrefab, new Vector2(i * cellDistance, j * cellDistance), Quaternion.identity);
                cells[i, j] = tempCell.GetComponent<Cell>();
            }
        }

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                if (i - 1 > -1)
                {
                    cells[i, j].AddNeighbourCell(cells[i - 1, j]);

                    if (j - 1 > -1)
                    {
                        cells[i, j].AddNeighbourCell(cells[i - 1, j - 1]);
                        cells[i, j].AddNeighbourCell(cells[i, j - 1]);
                    }
                    if (j + 1 < cells.GetLength(1))
                    {
                        cells[i, j].AddNeighbourCell(cells[i - 1, j + 1]);
                        cells[i, j].AddNeighbourCell(cells[i, j + 1]);
                    }
                }
                if (i + 1 < cells.GetLength(0))
                {
                    cells[i, j].AddNeighbourCell(cells[i + 1, j]);

                    if (j - 1 > -1)
                    {
                        cells[i, j].AddNeighbourCell(cells[i + 1, j - 1]);
                    }
                    if (j + 1 < cells.GetLength(1))
                    {
                        cells[i, j].AddNeighbourCell(cells[i + 1, j + 1]);
                    }
                }
            }
        }

        cellsToKill = new List<Cell>();
        cellsToTouch = new List<Cell>();

        camera.transform.position = new Vector2((size.x * cellDistance) / 2, (size.y * cellDistance) / 2);

        speed = 1;

        PrintHint();

        sw.Stop();

        Debug.Log($"created in {sw.ElapsedMilliseconds} ms");
    }

    public void StartSimulation()
    {
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine);
            simulationCoroutine = null;
            return;
        }

        simulationCoroutine = StartCoroutine(DoSimulate());
    }

    private IEnumerator DoSimulate()
    {
        int neighboursCounter = 0;

        while (true)
        {
            cellsToKill.Clear();
            cellsToTouch.Clear();

            foreach (Cell cell in cells)
            {
                neighboursCounter = 0;
                foreach (Cell neighbour in cell.GetNeighbourCells())
                {
                    if (neighbour.IsActive())
                        neighboursCounter++;
                }

                if (cell.IsActive())
                {
                    if (neighboursCounter < 2 || neighboursCounter > 3)
                        cellsToKill.Add(cell);
                }
                else
                {
                    if (neighboursCounter == 3)
                        cellsToTouch.Add(cell);
                }
            }

            foreach (Cell cell in cellsToKill)
                cell.ChangeCell(false);

            foreach (Cell cell in cellsToTouch)
                cell.ChangeCell(true);

            yield return new WaitForSeconds(1.01f - speed);
        }
    }

    private void PrintHint()
    {
        Debug.Log("Define seed by selecting live cells and press space to begin simulation. You can stop simulation by pressing space again.");
    }
}
