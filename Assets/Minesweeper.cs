using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour
{
    [SerializeField] private Cell m_cellPrefab = null;
    Cell m_cellScript;
    [SerializeField] private GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] private int m_mineCount = 1;
    [SerializeField] public int m_indexNumX = 5;
    [SerializeField] public int m_indexNumY = 5;
    private Cell[,] cubes;
    public Cell lastOpenCell;
    

    // Start is called before the first frame update
    void Start()
    {
        m_cellScript = m_cellPrefab.GetComponent<Cell>();

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
                cell.positionCell = new Vector2Int(i, x);
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

    //Mineから見て周りのセルにカウントを足す
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
    
    //周りのMineの数を数えて数値を入れる
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

    public void OpenCell()
    {
        var x = lastOpenCell.positionCell.x;
        var y = lastOpenCell.positionCell.y;

        //m_cellScript.m_button.SetActive(false);

        foreach (var cell in SearchCell(x,y))
        {
            if (cell.CellState == CellState.None)
            {
                OpenCell();
            }
        }
    }

    public Cell[] SearchCell(int r, int c)
    {
        var list = new List<Cell>();
        
	    var left = c - 1;
        var right = c + 1;
        var top = r - 1;
        var bottom = r + 1;
        
	if (top >= 0)
        {
            if (left >= 0 && cubes[r,c].CellState == CellState.None) 
            { 
                list.Add(cubes[top, left]); 
            }
            list.Add(cubes[top, c]);
            if (right < m_indexNumX) 
            { 
                list.Add(cubes[top, right]); 
            }
        }
        if (left >= 0) 
        { 
            list.Add(cubes[r, left]); 
        }
        if (right < m_indexNumX) 
        { 
            list.Add(cubes[r, right]); 
        }
        if (bottom < m_indexNumY)
        {
            if (left >= 0) 
            { 
                list.Add(cubes[bottom, left]); 
            }
            list.Add(cubes[bottom, c]);
            if (right < m_indexNumX) 
            { 
                list.Add(cubes[bottom, right]); 
            }
        }
        
	return list.ToArray();
    }
}
