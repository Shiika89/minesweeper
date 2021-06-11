using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalMinesweeper : MonoBehaviour
{

    [SerializeField] private SurvivalCell m_cellPrefab = null; //�Z����object���擾���邽�߂̕ϐ�
    [SerializeField] public GameObject m_gameOver;
    [SerializeField] private GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] private int m_mineCount = 1; //Mine�̐����D���ɕύX���邽�߂̕ϐ�
    [SerializeField] public int m_indexNumX = 5; //���ɉ��Z����ݒu���邩�̕ϐ�
    [SerializeField] public int m_indexNumY = 5; //�c�ɉ��Z����ݒu���邩�̕ϐ�
    private SurvivalCell[,] cubes; //�Z�����i�[���邽�߂�2�����z��̕ϐ�


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

        //2�����z��̑傫�������Əc�̑傫���ɐݒ�
        cubes = new SurvivalCell[m_indexNumX, m_indexNumY];

        //�ݒ肵���z�񕪂̃Z���𐶐�
        for (int i = 0; i < m_indexNumX; i++)
        {
            for (int x = 0; x < m_indexNumY; x++)
            {
                var cell = Instantiate(m_cellPrefab);
                cell.positionCell = new Vector2Int(i, x); //���������Z����position���L��
                var parent = m_gridLayoutGroup.gameObject.transform;
                cell.transform.SetParent(parent); //�����ꏊ���w��
                cubes[i, x] = cell;
            }
        }

        //�����_����Mine�𐶐�
        if (m_mineCount <= m_indexNumX * m_indexNumY) //Mine�̐����Z����葽����ΐ����s�\
        {
            for (var i = 0; i < m_mineCount; i++)
            {
                var r = Random.Range(0, m_indexNumX);
                var c = Random.Range(0, m_indexNumY);

                var cell = cubes[r, c];
                if (cell.SurvivalCellState != SurvivalCellState.Mine)
                {
                    cell.SurvivalCellState = SurvivalCellState.Mine; //�I�΂ꂽ�ꏊ��Mine���Ȃ���ΐ���
                    AddMine(r, c); //��������Mine�̎���ɃJ�E���g��ǉ�
                }
                else
                {
                    i--; //Mine���������ꍇ�͂�蒼��
                }
            }
        }
        else
        {
            Debug.Log("Mine�̐����Z���̔z�u����葽���̂Ŕz�u�ł��܂���");
        }
    }

    //Mine���猩�Ď���̃Z���ɃJ�E���g�𑫂�
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

    //�����Mine�̐��𐔂��Đ��l������
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

    //�������ꏊ������͂𒲂ׂ�None�Ȃ�󂯂���J��Ԃ�
    public void OpenCell(SurvivalCell target)
    {
        //�����̍��W���L��
        var x = target.positionCell.x;
        var y = target.positionCell.y;

        foreach (var item in SearchCell(x, y))
        {
            if (item.Open) //���ɂ����Ă��玟�̃Z����
            {
                continue;
            }
            item.CellOpen(); //�Z���������ăt���O��true�ɂ���
            if (item.SurvivalCellState == SurvivalCellState.None)
            {
                OpenCell(item); //None�Ȃ�ċA��None��T���Ă����Ă���
            }
        }
    }
    //���͂̃Z���𒲂ׂĔz��ɓ����֐�
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
