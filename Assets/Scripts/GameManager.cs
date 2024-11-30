using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Vector4 buildingPuntiation;

    [SerializeField] private GameObject m_Player1;
    [SerializeField] private GameObject m_Player2;

    private int pointsPlayer1;
    private int pointsPlayer2;

    public GameObject Player1 => m_Player1;
    public GameObject Player2 => m_Player2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void AddPoints(bool player, int height)
    {
        if (player)
            pointsPlayer1 += ((int)buildingPuntiation[height-1]);
        else if (player)
            pointsPlayer2 += ((int)buildingPuntiation[height-1]);


        Debug.Log(pointsPlayer1 + " : " + pointsPlayer2);
    }

    public int GetPoints(bool player)
    {
        if (player)
            return pointsPlayer1;
        else
            return pointsPlayer2;   
    }
}
