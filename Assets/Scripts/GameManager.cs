using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int[] m_buildingPuntuation;

    [SerializeField] private GameObject m_Player1;
    [SerializeField] private GameObject m_Player2;
    [SerializeField] private float m_maxTime;

    public UIManager m_UIManager;

    public enum GameState { MainMenu, Gameplay, VictoryScreen}

    private int m_pointsPlayer1;
    private int m_pointsPlayer2;
    private float m_timer;
    private GameState m_gameState;

    public GameObject Player1 => m_Player1;
    public GameObject Player2 => m_Player2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
	
    private void Start()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void ChangeGameState(GameState newState)
    {
        GameState previousGameState = m_gameState;
        m_gameState = newState;
        switch(newState)
        {
            case GameState.MainMenu:
                m_Player1.SetActive(false);
                m_Player2?.SetActive(false);
                m_UIManager.ShowMainMenu();
                break;

            case GameState.Gameplay:
                m_pointsPlayer1 = 0;
                m_pointsPlayer2 = 0;
                m_timer = m_maxTime;

                m_Player1.SetActive(true);
                m_Player2?.SetActive(true);

                m_UIManager.InitializeHUD(m_pointsPlayer1, m_pointsPlayer2, m_timer);
                m_UIManager.ShowHUD();
                break;

            case GameState.VictoryScreen:
                m_Player1.SetActive(false);
                m_Player2?.SetActive(false);
                int result = m_pointsPlayer1 == m_pointsPlayer2 ? -1 : m_pointsPlayer1 > m_pointsPlayer2 ? 1 : 2;
                m_UIManager.ShowVictoryScreen(result);
                break;
        }
    }

    private void Update()
    {
        if(m_gameState == GameState.Gameplay)
        {
            m_timer -= Time.deltaTime;
            if(m_timer < 0f)
            {
                m_timer = 0f;
                ChangeGameState(GameState.VictoryScreen);
            }
            m_UIManager.UpdateTimer(m_timer);
        }
    }

    public void AddPoints(bool player, int height)
    {
        if (player)
        {
            m_pointsPlayer1 += m_buildingPuntuation[height - 1];
            m_UIManager.UpdatePointsPlayer1(m_pointsPlayer1);
        }
        else
        {
            m_pointsPlayer2 += m_buildingPuntuation[height - 1];
            m_UIManager.UpdatePointsPlayer2(m_pointsPlayer2);
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
