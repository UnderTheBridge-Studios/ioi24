using DG.Tweening;
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
        GameManager.Instance.ChangeGameState(GameManager.GameState.Gameplay);
    }

    public void OnReplayButtonPressed()
    {
        GameManager.Instance.Replay();
    }

    public void OnQuitButtonPressed()
    {
        GameManager.Instance.Quit();
    }

    public void UpdateTimer(float timeInSeconds)
    {
        int seconds = ((int)timeInSeconds % 60);
        int minutes = ((int)timeInSeconds / 60);
        m_HUDTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdatePointsPlayer1(int currentPoints)
    {
        m_HUDPointsPlayer1.transform.DOComplete();
        m_HUDPointsPlayer1.transform.DOLocalJump(m_HUDPointsPlayer1.transform.localPosition, 10, 1, 0.3f);
        m_HUDPointsPlayer1.text = currentPoints.ToString();
        m_VictoryPointsPlayer1.text = currentPoints.ToString();
    }

    public void UpdatePointsPlayer2(int currentPoints)
    {
        m_HUDPointsPlayer2.transform.DOComplete();
        m_HUDPointsPlayer2.transform.DOLocalJump(m_HUDPointsPlayer2.transform.localPosition, 10, 1, 0.3f);
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

        m_HUD.transform.localPosition = new Vector3(m_HUD.transform.localPosition.x, m_HUD.transform.localPosition.y + 200, m_HUD.transform.localPosition.z);
        m_HUD.transform.DOLocalMoveY(m_HUD.transform.localPosition.y - 200, 1).SetEase(Ease.OutBack);
    }

    public void ShowVictoryScreen(int playerThatWins)
    {
        if(playerThatWins == -1)
        {
            m_PlayerWon.text = "Tie!";
        }
        else
        {
            m_PlayerWon.text = "Player " + playerThatWins + " Wins!";
        }
        
        m_HUD.SetActive(false);
        m_MainMenu.SetActive(false);
        m_VictoryScreen.SetActive(true);
    }

    public void InitializeHUD(int pointsPlayer1, int pointsPlayer2, float timer)
    {
        UpdatePointsPlayer1(pointsPlayer1);
        UpdatePointsPlayer2(pointsPlayer2);
        UpdateTimer(timer);
        m_HUD.SetActive(true);
    }
}
