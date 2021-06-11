using UnityEngine;
using UnityEngine.UI;

public enum SurvivalCellState
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,

    Mine = -1,
}
public class SurvivalCell : MonoBehaviour
{
    [SerializeField] Text m_view = null;
    [SerializeField] public GameObject m_button;
    [SerializeField] private SurvivalCellState m_cellState = SurvivalCellState.None;
    GameObject m_survivalMine;
    SurvivalMinesweeper m_survivalMinesweeper;
    public Vector2Int positionCell;
    public bool Open;

    private void Start()
    {
        m_survivalMine = GameObject.Find("SurvivalMinesweeper");
        m_survivalMinesweeper = m_survivalMine.GetComponent<SurvivalMinesweeper>();
    }

    public SurvivalCellState SurvivalCellState
    {
        get => m_cellState;
        set
        {
            m_cellState = value;
            OnCellStateChanged();
        }
    }

    private void OnValidate()
    {
        OnCellStateChanged();
    }

    private void OnCellStateChanged()
    {
        if (m_cellState == SurvivalCellState.None)
        {
            m_view.text = "";
        }
        else if (m_cellState == SurvivalCellState.Mine)
        {
            m_view.text = "X";
            m_view.color = Color.red;
        }
        else
        {
            m_view.text = ((int)m_cellState).ToString();
            m_view.color = Color.blue;
        }
    }
    public void CellOpen()
    {
        m_button.SetActive(false);
        Open = true;
        if (SurvivalCellState != SurvivalCellState.None)
        {
            if (m_survivalMinesweeper.m_playerHP < m_survivalMinesweeper.m_maxPlayerHP)
            {
                if (m_survivalMinesweeper.m_playerHP + (int)SurvivalCellState > m_survivalMinesweeper.m_maxPlayerHP)
                {
                    m_survivalMinesweeper.m_playerHP = m_survivalMinesweeper.m_maxPlayerHP;
                }
                else
                {
                    m_survivalMinesweeper.m_playerHP += (int)SurvivalCellState;
                }
            }
        }
    }
    public void OnClickThis()
    {
        if (SurvivalCellState == SurvivalCellState.None)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_button.SetActive(false);
                Open = true;
                m_survivalMinesweeper.OpenCell(this);            
            }
            if (Input.GetMouseButtonUp(1))
            {
                m_button.SetActive(false);
                Open = true;
                m_survivalMinesweeper.OpenCell(this);
                Debug.Log("Mineじゃない場所で右クリック！、5ダメージ！");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_defenseDamage;
            }
        }
        else if (SurvivalCellState != SurvivalCellState.Mine)
        {
            m_button.SetActive(false);
            Open = true;
            if (Input.GetMouseButtonUp(0))
            {
                if (m_survivalMinesweeper.m_playerHP < m_survivalMinesweeper.m_maxPlayerHP)
                {
                    if (m_survivalMinesweeper.m_playerHP + (int)SurvivalCellState > m_survivalMinesweeper.m_maxPlayerHP)
                    {
                        m_survivalMinesweeper.m_playerHP = m_survivalMinesweeper.m_maxPlayerHP;
                    }
                    else
                    {
                        m_survivalMinesweeper.m_playerHP += (int)SurvivalCellState;
                    }
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                m_button.SetActive(false);
                Open = true;
                Debug.Log("Mineじゃない場所で右クリック！、5ダメージ！");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_defenseDamage;
            }
        }
        else
        {
            m_button.SetActive(false);
            Open = true;
            m_survivalMinesweeper.m_mineCount--;
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Mineを踏んでしまった！、10ダメージ！");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_MineDamage;
            }
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Mineを看破してダメージを軽減した！、5ダメージ！");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_defenseDamage;
            }
            if (m_survivalMinesweeper.m_playerHP <= 0)
            {
                m_survivalMinesweeper.GameOver();
            }
        }
    }
}
