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
    private Cell[,] cubes;

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

        cubes = new Cell[m_indexNumX, m_indexNumY];
        //var cubes = new Cell[m_indexNumX, m_indexNumY];
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
                    AddMine(r, c);
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

        //for (int i = 0; i < m_indexNumX; i++)
        //{
        //    for (int x = 0; x < m_indexNumY; x++)
        //    {
        //        var cell = cubes[i, x];
        //        if (cell.CellState == CellState.Mine)
        //        {
        //            continue;
        //        }

        //        var count = GetMineCount(i, x);
        //        cell.CellState = (CellState)count;
        //    }
        //}
    }

    private void AddMine(int r, int c)
    {
        var left = c - 1;
        var right = c + 1;
        var top = r - 1;
        var bottom = r + 1;

        if (top >= 0)
        {
            if (left >= 0 && cubes[top, left].CellState != CellState.Mine) { cubes[top, left].CellState++; }
            if (cubes[top, c].CellState != CellState.Mine) { cubes[top, c].CellState++; }
            if (right < m_indexNumY && cubes[top, right].CellState != CellState.Mine) { cubes[top, right].CellState++; }
        }
        if (left >= 0 && cubes[r, left].CellState != CellState.Mine) { cubes[r, left].CellState++; }
        if (right < m_indexNumY && cubes[r, right].CellState != CellState.Mine) { cubes[r, right].CellState++; }
        if (bottom < m_indexNumX)
        {
            if (left >= 0 && cubes[bottom, left].CellState != CellState.Mine) { cubes[bottom, left].CellState++; }
            if (cubes[bottom, c].CellState != CellState.Mine) { cubes[bottom, c].CellState++; }
            if (right < m_indexNumY && cubes[bottom, right].CellState != CellState.Mine) { cubes[bottom, right].CellState++; }
        }
    }

    private int GetMineCount(int r, int c)
    {
        var left = c - 1;
        var right = c + 1;
        var top = r - 1;
        var bottom = r + 1;

	var count = 0;
        if (top >= 0)
        {
            if (left >= 0 && cubes[top, left].CellState == CellState.Mine) { count++; }
            if (cubes[top, c].CellState == CellState.Mine) { count++; }
            if (right < m_indexNumY && cubes[top, right].CellState == CellState.Mine) { count++; }
        }
        if (left >= 0 && cubes[r, left].CellState == CellState.Mine) { count++; }
        if (right < m_indexNumY && cubes[r, right].CellState == CellState.Mine) { count++; }
        if (bottom < m_indexNumX)
        {
            if (left >= 0 && cubes[bottom, left].CellState == CellState.Mine) { count++; }
            if (cubes[bottom, c].CellState == CellState.Mine) { count++; }
            if (right < m_indexNumY && cubes[bottom, right].CellState == CellState.Mine) { count++; }
        }
        return count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
