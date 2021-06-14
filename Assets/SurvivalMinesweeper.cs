using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalMinesweeper : MonoBehaviour
{

    [SerializeField] private SurvivalCell m_cellPrefab = null; //�Z����object���擾���邽�߂̕ϐ�
    [SerializeField] GameObject m_gameOver;
    [SerializeField] GameObject m_stageClear;
    [SerializeField] private GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] public int m_maxMineCount = 1; //Mine�̐����D���ɕύX���邽�߂̕ϐ�
    [SerializeField] public int m_indexNumX = 5; //���ɉ��Z����ݒu���邩�̕ϐ�
    [SerializeField] public int m_indexNumY = 5; //�c�ɉ��Z����ݒu���邩�̕ϐ�
    private SurvivalCell[,] cubes; //�Z�����i�[���邽�߂�2�����z��̕ϐ�
    public int m_mineCount;
    public int m_playerHP;
    public int m_maxPlayerHP = 20;
    public int m_MineDamage = 10;
    public int m_defenseDamage = 5;
    public int m_heel;
    public bool m_invincible;
    int m_stageCount = 1;
    [SerializeField] GameObject m_hp_object;
    [SerializeField] GameObject m_mineText_object;
    [SerializeField] GameObject m_stage_object;
    [SerializeField] GameObject m_clear_object;
    Text m_hp;
    Text m_mineText;
    Text m_stage;
    Text m_clearText;
    public bool m_gameStop = false;


    // Start is called before the first frame update
    void Start()
    {
        m_hp = m_hp_object.GetComponent<Text>();
        m_mineText = m_mineText_object.GetComponent<Text>();
        m_stage = m_stage_object.GetComponent<Text>();
        m_clearText = m_clear_object.GetComponent<Text>();
        m_playerHP = m_maxPlayerHP;

        GameStart();
    }

    private void Update()
    {
        m_hp.text = "�c��HP�@�F" + m_playerHP + "/" + m_maxPlayerHP;
        m_mineText.text = "�c��MINE�@�F" + m_mineCount + "/" + m_maxMineCount;
        m_stage.text = "�X�e�[�W�@" + m_stageCount;
        if (m_stageCount < 3)
        {
            m_clearText.text = "STAGE�@" + m_indexNumX + "�~" + m_indexNumY + "��" + (m_indexNumX + 1) + "�~" + (m_indexNumY + 1) + "\n" +
                           "MINE�@" + m_maxMineCount + "��" + (m_maxMineCount + 4) + "\n" +
                           "HP�@" + m_playerHP + "/" + m_maxPlayerHP + "��" + (m_playerHP + 20) + "/" + (m_maxPlayerHP + 20);
        }
        else
        {
            m_clearText.text = "MINE�@" + m_maxMineCount + "��" + (m_maxMineCount + 2) + "\n" +
                               "HP�@" + m_playerHP + "/" + m_maxPlayerHP;
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
        m_gameStop = true;
        m_gameOver.SetActive(true);
    }

    public void NextStage()
    {
        m_gameStop = true;
        EventManager.StageClear();
        m_stageClear.SetActive(true);
    }

    public void StageClear()
    {
        m_stageClear.SetActive(false);
        EventManager.StageClear();
        m_stageCount += 1;
        if (m_indexNumX < 10 && m_indexNumY < 10)
        {
            m_indexNumX++;
            m_indexNumY++;
            m_maxMineCount += 4;
            m_maxPlayerHP += 20;
            m_playerHP += 20;
        }
        else
        {
            m_maxMineCount += 2;
        }
        GameStart();
    }

    void GameStart()
    {
        m_gameStop = false;

        m_mineCount = m_maxMineCount;

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
        if (m_maxMineCount <= m_indexNumX * m_indexNumY) //Mine�̐����Z����葽����ΐ����s�\
        {
            for (var i = 0; i < m_maxMineCount; i++)
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
}
