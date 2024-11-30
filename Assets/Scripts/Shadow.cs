using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private float m_height;
    [SerializeField] private bool m_isPlayer1;

    private Transform m_ball;

    private void Start()
    {
        if(m_isPlayer1)
            m_ball = GameManager.Instance.Player1.transform;
        else
            m_ball = GameManager.Instance.Player2.transform;
    }

    void Update()
    {
        transform.position = new Vector3(m_ball.position.x, m_height, m_ball.position.z);
    }
}
