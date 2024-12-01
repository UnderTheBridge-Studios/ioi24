using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private int[] m_buildingPuntuation;
    [SerializeField] private GameObject m_Player1;
    [SerializeField] private GameObject m_Player2;
    [SerializeField] private float m_maxTime;
    [SerializeField] private float m_colorChangeTime;

    [Header("References")]
    public UIManager m_UIManager;
	public WwiseManager m_WwiseManager;
    [SerializeField] private GameObject m_buildingsGO;
    [SerializeField] private GameObject m_barriersGO;

    [Header("Wwise")]
    [SerializeField] private AK.Wwise.State m_WwiseMainMenu;
    [SerializeField] private AK.Wwise.State m_WwiseGameplay;
    [SerializeField] private AK.Wwise.State m_WwiseVictory;
    [SerializeField] private AK.Wwise.RTPC m_WwiseTempo;

    public enum GameState { MainMenu, Gameplay, VictoryScreen}

    private int m_pointsPlayer1;
    private int m_pointsPlayer2;
    private float m_timer;
    private float m_colorTimer;
    private GameState m_gameState;

    private List<BuildingController> m_buildings;
    private Transform[] m_barriers;

    public GameObject Player1 => m_Player1;
    public GameObject Player2 => m_Player2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        m_buildings = m_buildingsGO.GetComponentsInChildren<BuildingController>().ToList();
        m_barriers = m_barriersGO.GetComponentsInChildren<Transform>();
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
                m_WwiseMainMenu.SetValue();
                m_WwiseTempo.SetValue(null, 0f);
                break;

            case GameState.Gameplay:
                m_pointsPlayer1 = 0;
                m_pointsPlayer2 = 0;
                m_timer = m_maxTime;

                m_Player1.SetActive(true);
                m_Player2?.SetActive(true);

                m_UIManager.InitializeHUD(m_pointsPlayer1, m_pointsPlayer2, m_timer);
                m_UIManager.ShowHUD();
                m_WwiseGameplay.SetValue();
                break;

            case GameState.VictoryScreen:
                m_Player1.SetActive(false);
                m_Player2?.SetActive(false);
                int result = m_pointsPlayer1 == m_pointsPlayer2 ? -1 : m_pointsPlayer1 > m_pointsPlayer2 ? 1 : 2;
                m_UIManager.ShowVictoryScreen(result);
                m_WwiseVictory.SetValue();
                break;
        }
    }

    private void Update()
    {
        if(m_gameState == GameState.Gameplay)
        {
            m_timer -= Time.deltaTime;
            m_colorTimer += Time.deltaTime;

            if(m_timer < 0f)
            {
                m_timer = 0f;
                ChangeGameState(GameState.VictoryScreen);
            }
            m_UIManager.UpdateTimer(m_timer);
            m_WwiseTempo.SetValue(null, (m_maxTime - m_timer) / m_maxTime);

            if (m_colorTimer >= m_colorChangeTime)
            {
                int randomIndex = Random.Range(0, m_buildings.Count);
                BuildingController randomBuilding = m_buildings[randomIndex];

                if (randomBuilding.CurrentState == BuildingController.BuildingState.Default)
                {
                    ChangeBuildingColor(randomBuilding);
                    m_colorTimer = 0f;
                    m_colorChangeTime = Random.Range(3f, 6f);
                }
            }
        }
    }

    public void AddPoints(bool isPlayer1, BuildingController.BuildingColor buildingColor, int height)
    {
        int pointsMultiplier = 1;
        
        switch(buildingColor)
        {
            case BuildingController.BuildingColor.Gold:
                pointsMultiplier = 2;
                break;
            case BuildingController.BuildingColor.Black:
                pointsMultiplier = -1;
                break;
            default:
                pointsMultiplier = 1;
                break;
        }

        if (isPlayer1)
        {
            m_pointsPlayer1 += m_buildingPuntuation[height - 1] * pointsMultiplier;
            m_UIManager.UpdatePointsPlayer1(m_pointsPlayer1);
            m_WwiseManager.UpdatePointsPlayer1(height);
        }
        else
        {
            m_pointsPlayer2 += m_buildingPuntuation[height - 1] * pointsMultiplier;
            m_UIManager.UpdatePointsPlayer2(m_pointsPlayer2);
            m_WwiseManager.UpdatePointsPlayer2(height);
        }
    }

    private void ChangeBuildingColor(BuildingController building)
    {
        float chances = Random.Range(0f, 1f);

        if (chances <= 0.5f)
            building.SetBuildingColor(BuildingController.BuildingColor.Blue);
        else if (0.5f < chances && chances <= 0.75f)
            building.SetBuildingColor(BuildingController.BuildingColor.Gold);
        else if (0.75f < chances && chances <= 1f)
            building.SetBuildingColor(BuildingController.BuildingColor.Black);
    }

    public void RemoveBuilding(BuildingController building)
    {
        m_buildings.Remove(building);
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