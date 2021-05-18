using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour
{
    [SerializeField] private Cell m_cellPrefab = null;
    [SerializeField] private GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] private int m_mineCount = 1;
    [SerializeField] int m_indexNumX = 5;
    [SerializeField] int m_indexNumY = 5;
    //private GameObject[,] cubes;

    // Start is called before the first frame update
    void Start()
    {
        if (m_indexNumX < m_indexNumY)
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            m_gridLayoutGroup.constraintCount = m_indexNumX;
        }
        else
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            m_gridLayoutGroup.constraintCount = m_indexNumY;
        }

        //cubes = new GameObject[m_indexNumX, m_indexNumY];
        var cubes = new Cell[m_indexNumX, m_indexNumY];
        for (int i = 0; i < m_indexNumX; i++)
        {
            for (int x = 0; x < m_indexNumY; x++)
            {
                var cell = Instantiate(m_cellPrefab);
                var parent = m_gridLayoutGroup.gameObject.transform;
                cell.transform.SetParent(parent);
                //cell.transform.position = new Vector3(-4 + i * 2, -3 + x * 2, 0);
                cubes[i, x] = cell;
            }
        }

        if (m_mineCount <= m_indexNumX * m_indexNumY)
        {
            for (var i = 0; i < m_mineCount; i++)
            {
                var r = Random.Range(0, m_indexNumX);
                var c = Random.Range(0, m_indexNumY);

                var cell = cubes[r, c];
                if (cell.CellState != CellState.Mine)
                {
                    cell.CellState = CellState.Mine;
                }
                else
                {
                    i--;
                }
            }
        }
        else
        {
            Debug.Log("Mineの数がセルの配置数より多いので配置できません");
        }


        for (int i = 0; i < m_indexNumX; i++)
        {
            for (int x = 0; x < m_indexNumY; x++)
            {
                var count = 0;
                var cell = cubes[i, x];
                if (cell.CellState != CellState.Mine)
                {
                    if (cubes[i - 1, x -1].CellState == CellState.Mine || i >= 0 || x >= 0)
                    {
                        count++;
                    }
                    Debug.Log($"cell[{i}, {x}] = {cell.CellState}");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
