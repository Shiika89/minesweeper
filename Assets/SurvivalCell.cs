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
public class SurvivalCell : EventObject
{
    [SerializeField] Text m_view = null;
    [SerializeField] public GameObject m_button;
    [SerializeField] private SurvivalCellState m_cellState = SurvivalCellState.None;
    Image m_buttonImage;
    GameObject m_player;
    Animator m_anim;
    GameObject m_survivalMine;
    SurvivalMinesweeper m_survivalMinesweeper;
    public Vector2Int positionCell;
    public bool Open;

    private void Start()
    {
        m_survivalMine = GameObject.Find("SurvivalMinesweeper");
        m_survivalMinesweeper = m_survivalMine.GetComponent<SurvivalMinesweeper>();
        m_player = GameObject.Find("Player");
        m_anim = m_player.GetComponent<Animator>();
        m_buttonImage = m_button.GetComponent<Image>();
        if (m_survivalMinesweeper.m_trans == true)
        {
            m_buttonImage.color = new Color(0, 0, 0, 0.5f);
        }
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
        m_survivalMinesweeper.m_healCount += (int)SurvivalCellState;
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
    void Conditions()
    {
        if (SurvivalCellState == SurvivalCellState.None)
        {
            m_anim.SetTrigger("Attack");

            if (Input.GetMouseButtonUp(0))
            {
                m_button.SetActive(false);
                Open = true;
                m_survivalMinesweeper.OpenCell(this);
                m_survivalMinesweeper.Heal();
            }
            if (Input.GetMouseButtonUp(1))
            {
                m_button.SetActive(false);
                Open = true;
                m_survivalMinesweeper.Defense();
                m_survivalMinesweeper.OpenCell(this);
                Debug.Log("Mine???????????????E?N???b?N?I?A5?_???[?W?I");
                m_anim.SetTrigger("DefenseDamage");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_defenseDamage;
                if (m_survivalMinesweeper.m_playerHP <= 0)
                {
                    m_anim.SetTrigger("Die");
                    m_survivalMinesweeper.GameOver();
                }
            }
            if (m_survivalMinesweeper.m_invincible == true)
            {
                m_survivalMinesweeper.m_playerHP += 50;
            }
        }
        else if (SurvivalCellState != SurvivalCellState.Mine)
        {
            m_button.SetActive(false);
            Open = true;
            m_anim.SetTrigger("Attack");
            if (Input.GetMouseButtonUp(0))
            {
                m_survivalMinesweeper.m_healCount += (int)SurvivalCellState;
                m_survivalMinesweeper.Heal();
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
                m_survivalMinesweeper.Defense();
                Debug.Log("Mine???????????????E?N???b?N?I?A5?_???[?W?I");
                m_anim.SetTrigger("DefenseDamage");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_defenseDamage;
                if (m_survivalMinesweeper.m_playerHP <= 0)
                {
                    m_anim.SetTrigger("Die");
                    m_survivalMinesweeper.GameOver();
                }
                if (m_survivalMinesweeper.m_invincible == true)
                {
                    m_survivalMinesweeper.m_playerHP += 50;
                }
            }
        }
        else
        {
            m_button.SetActive(false);
            Open = true;
            m_anim.SetTrigger("Attack");
            
            m_survivalMinesweeper.m_mineCount--;
            if (Input.GetMouseButtonUp(0))
            {
                m_anim.SetTrigger("Damage");
                m_survivalMinesweeper.Damage();
                Debug.Log("Mine?????????????????I?A20?_???[?W?I");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_MineDamage;
            }
            if (Input.GetMouseButtonUp(1))
            {
                m_survivalMinesweeper.Defense();
                m_anim.SetTrigger("DefenseDamage");
                Debug.Log("Mine?????j?????_???[?W???y???????I?A5?_???[?W?I");
                m_survivalMinesweeper.m_playerHP -= m_survivalMinesweeper.m_defenseDamage;
            }
            if (m_survivalMinesweeper.m_invincible == true)
            {
                m_survivalMinesweeper.m_playerHP += 50;
            }
            if (m_survivalMinesweeper.m_playerHP <= 0)
            {
                m_anim.SetTrigger("Die");
                m_survivalMinesweeper.GameOver();
            }
            if (m_survivalMinesweeper.m_mineCount == 0)
            {
                m_survivalMinesweeper.NextStage();
            }
        }
    }

    public void OnClickThis()
    {
        if (m_survivalMinesweeper.m_gameStop == false)
        {
            Conditions();
        }
    }

    public override void StageClear()
    {
        Destroy(this.gameObject);
    }
}
