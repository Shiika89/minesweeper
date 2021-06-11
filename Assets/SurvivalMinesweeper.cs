using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalMinesweeper : MonoBehaviour
{

    [SerializeField] private SurvivalCell m_cellPrefab = null; //セルのobjectを取得するための変数
    [SerializeField] public GameObject m_gameOver;
    [SerializeField] private GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] private int m_mineCount = 1; //Mineの数を好きに変更するための変数
    [SerializeField] public int m_indexNumX = 5; //横に何個セルを設置するかの変数
    [SerializeField] public int m_indexNumY = 5; //縦に何個セルを設置するかの変数
    private SurvivalCell[,] cubes; //セルを格納するための2次元配列の変数


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

        //2次元配列の大きさを横と縦の大きさに設定
        cubes = new SurvivalCell[m_indexNumX, m_indexNumY];

        //設定した配列分のセルを生成
        for (int i = 0; i < m_indexNumX; i++)
        {
            for (int x = 0; x < m_indexNumY; x++)
            {
                var cell = Instantiate(m_cellPrefab);
                cell.positionCell = new Vector2Int(i, x); //生成したセルのpositionを記憶
                var parent = m_gridLayoutGroup.gameObject.transform;
                cell.transform.SetParent(parent); //生成場所を指定
                cubes[i, x] = cell;
            }
        }

        //ランダムにMineを生成
        if (m_mineCount <= m_indexNumX * m_indexNumY) //Mineの数がセルより多ければ生成不可能
        {
            for (var i = 0; i < m_mineCount; i++)
            {
                var r = Random.Range(0, m_indexNumX);
                var c = Random.Range(0, m_indexNumY);

                var cell = cubes[r, c];
                if (cell.SurvivalCellState != SurvivalCellState.Mine)
                {
                    cell.SurvivalCellState = SurvivalCellState.Mine; //選ばれた場所にMineがなければ生成
                    AddMine(r, c); //生成したMineの周りにカウントを追加
                }
                else
                {
                    i--; //Mineがあった場合はやり直し
                }
            }
        }
        else
        {
            Debug.Log("Mineの数がセルの配置数より多いので配置できません");
        }
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
            if (left >= 0 && cubes[top, left].SurvivalCellState != SurvivalCellState.Mine) { cubes[top, left].SurvivalCellState++; }
            if (cubes[top, c].SurvivalCellState != SurvivalCellState.Mine) { cubes[top, c].SurvivalCellState++; }
            if (right < m_indexNumY && cubes[top, right].SurvivalCellState != SurvivalCellState.Mine) { cubes[top, right].SurvivalCellState++; }
        }
        if (left >= 0 && cubes[r, left].SurvivalCellState != SurvivalCellState.Mine) { cubes[r, left].SurvivalCellState++; }
        if (right < m_indexNumY && cubes[r, right].SurvivalCellState != SurvivalCellState.Mine) { cubes[r, right].SurvivalCellState++; }
        if (bottom < m_indexNumX)
        {
            if (left >= 0 && cubes[bottom, left].SurvivalCellState != SurvivalCellState.Mine) { cubes[bottom, left].SurvivalCellState++; }
            if (cubes[bottom, c].SurvivalCellState != SurvivalCellState.Mine) { cubes[bottom, c].SurvivalCellState++; }
            if (right < m_indexNumY && cubes[bottom, right].SurvivalCellState != SurvivalCellState.Mine) { cubes[bottom, right].SurvivalCellState++; }
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
            if (left >= 0 && cubes[top, left].SurvivalCellState == SurvivalCellState.Mine) { count++; }
            if (cubes[top, c].SurvivalCellState == SurvivalCellState.Mine) { count++; }
            if (right < m_indexNumY && cubes[top, right].SurvivalCellState == SurvivalCellState.Mine) { count++; }
        }
        if (left >= 0 && cubes[r, left].SurvivalCellState == SurvivalCellState.Mine) { count++; }
        if (right < m_indexNumY && cubes[r, right].SurvivalCellState == SurvivalCellState.Mine) { count++; }
        if (bottom < m_indexNumX)
        {
            if (left >= 0 && cubes[bottom, left].SurvivalCellState == SurvivalCellState.Mine) { count++; }
            if (cubes[bottom, c].SurvivalCellState == SurvivalCellState.Mine) { count++; }
            if (right < m_indexNumY && cubes[bottom, right].SurvivalCellState == SurvivalCellState.Mine) { count++; }
        }
        return count;
    }

    //押した場所から周囲を調べてNoneなら空けるを繰り返す
    public void OpenCell(SurvivalCell target)
    {
        //自分の座標を記憶
        var x = target.positionCell.x;
        var y = target.positionCell.y;

        foreach (var item in SearchCell(x, y))
        {
            if (item.Open) //既にあいてたら次のセルへ
            {
                continue;
            }
            item.CellOpen(); //セルをあけてフラグをtrueにする
            if (item.SurvivalCellState == SurvivalCellState.None)
            {
                OpenCell(item); //Noneなら再帰でNoneを探してあけていく
            }
        }
    }
    //周囲のセルを調べて配列に入れる関数
    public SurvivalCell[] SearchCell(int r, int c)
    {
        var list = new List<SurvivalCell>();

        var left = c - 1;
        var right = c + 1;
        var top = r - 1;
        var bottom = r + 1;

        if (top >= 0)
        {
            if (left >= 0)
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

    public void GameOver()
    {
        m_gameOver.SetActive(true);
    }
}
