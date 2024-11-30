using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject m_HUD;
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_VictoryScreen;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI m_HUDPointsPlayer1;
    [SerializeField] private TextMeshProUGUI m_HUDPointsPlayer2;
    [SerializeField] private TextMeshProUGUI m_HUDTimer;

    [Header("Victory")]
    [SerializeField] private TextMeshProUGUI m_PlayerWon;
    [SerializeField] private TextMeshProUGUI m_VictoryPointsPlayer1;
    [SerializeField] private TextMeshProUGUI m_VictoryPointsPlayer2;

    public void OnStartButtonPressed()
    {
        // Tell Game Manager to start game
    }

    public void OnReplayButtonPressed()
    {
        // Tell Game Manager to replay game, or restart scene here?
    }

    public void OnQuitButtonPressed()
    {
        // Tell Game Manager to quit game, or Application.Quit here?
    }

    public void UpdateTimer(int timeInSeconds)
    {
        int seconds = ((int)timeInSeconds % 60);
        int minutes = ((int)timeInSeconds / 60);
        m_HUDTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdatePointsPlayer1(int currentPoints)
    {
        m_HUDPointsPlayer1.text = currentPoints.ToString();
        m_VictoryPointsPlayer1.text = currentPoints.ToString();
    }

    public void UpdatePointsPlayer2(int currentPoints)
    {
        m_HUDPointsPlayer2.text = currentPoints.ToString();
        m_VictoryPointsPlayer2.text = currentPoints.ToString();
    }

    public void ShowMainMenu()
    {
        m_HUD.SetActive(false);
        m_MainMenu.SetActive(true);
        m_VictoryScreen.SetActive(false);
    }

    public void ShowHUD()
    {
        m_HUD.SetActive(true);
        m_MainMenu.SetActive(false);
        m_VictoryScreen.SetActive(false);
    }

    public void ShowVictoryScreen(int playerThatWins)
    {
        m_PlayerWon.text = "Player " + playerThatWins + " Wins!";
        m_HUD.SetActive(false);
        m_MainMenu.SetActive(false);
        m_VictoryScreen.SetActive(true);
    }

}
