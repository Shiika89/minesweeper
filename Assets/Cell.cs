using UnityEngine;
using UnityEngine.UI;

public enum CellState
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
public class Cell : MonoBehaviour
{
    [SerializeField] Text m_view = null;
    [SerializeField] public GameObject m_button;
    [SerializeField] private CellState m_cellState = CellState.None;
    GameObject mine;
    Minesweeper minesweeper;
    private Cell[,] cubes;
    public Vector2Int positionCell;
    public bool Open;

    private void Start()
    {
        mine = GameObject.Find("Minesweeper");
        minesweeper = mine.GetComponent<Minesweeper>();
        cubes = new Cell[minesweeper.m_indexNumX,minesweeper.m_indexNumY];
    }

    public CellState CellState
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
        if (m_cellState == CellState.None)
        {
            m_view.text = "";
        }
        else if (m_cellState == CellState.Mine)
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
    }
    public void OnClickThis()
    {
        if (CellState == CellState.None)
        {
            m_button.SetActive(false);
            Open = true;
            minesweeper.OpenCell(this);
        }
        else if (CellState != CellState.Mine)
        {
            m_button.SetActive(false);
            Open = true;
        }
        else
        {
            m_button.SetActive(false);
            Open = true;
        }
    }
}
