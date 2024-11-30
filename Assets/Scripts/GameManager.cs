using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Vector4 m_buildingPuntuation;

    [SerializeField]
    private GameObject m_Player1;
    [SerializeField]
    private GameObject m_Player2;

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

    public void AddPoints(bool isPlayer1, int height)
    {
        if (isPlayer1)
            pointsPlayer1 += ((int)m_buildingPuntuation[height-1]);
        else if (!isPlayer1)
            pointsPlayer2 += ((int)m_buildingPuntuation[height-1]);


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
