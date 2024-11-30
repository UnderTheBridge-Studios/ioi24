using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Vector4 buildingPuntuation;

    [SerializeField] private GameObject m_Player1;
    [SerializeField] private GameObject m_Player2;
    [SerializeField] private float m_MaxTime;

    public UIManager m_UIManager;

    public enum GameState { MainMenu, Gameplay, VictoryScreen}

    private int pointsPlayer1;
    private int pointsPlayer2;
    private float timer;
    private GameState gameState;

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
        GameState previousGameState = gameState;
        gameState = newState;
        switch(newState)
        {
            case GameState.MainMenu:
                m_Player1.SetActive(false);
                m_Player2?.SetActive(false);
                m_UIManager.ShowMainMenu();
                break;

            case GameState.Gameplay:
                pointsPlayer1 = 0;
                pointsPlayer2 = 0;
                timer = m_MaxTime;

                m_Player1.SetActive(true);
                m_Player2?.SetActive(true);

                m_UIManager.InitializeHUD(pointsPlayer1, pointsPlayer2, timer);
                m_UIManager.ShowHUD();
                break;

            case GameState.VictoryScreen:
                m_Player1.SetActive(false);
                m_Player2?.SetActive(false);
                int result = pointsPlayer1 == pointsPlayer2 ? -1 : pointsPlayer1 > pointsPlayer2 ? 1 : 2;
                m_UIManager.ShowVictoryScreen(result);
                break;
        }
    }

    private void Update()
    {
        if(gameState == GameState.Gameplay)
        {
            timer -= Time.deltaTime;
            if(timer < 0f)
            {
                timer = 0f;
                ChangeGameState(GameState.VictoryScreen);
            }
            m_UIManager.UpdateTimer(timer);
        }
    }

    public void AddPoints(bool player, int height)
    {
        if (player)
        {
            pointsPlayer1 += ((int)buildingPuntuation[height - 1]);
            m_UIManager.UpdatePointsPlayer1(pointsPlayer1);
        }
        else
        {
            pointsPlayer2 += ((int)buildingPuntuation[height - 1]);
            m_UIManager.UpdatePointsPlayer2(pointsPlayer2);
        }
    }

    public int GetPoints(bool player)
    {
        if (player)
            return pointsPlayer1;
        else
            return pointsPlayer2;   
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
