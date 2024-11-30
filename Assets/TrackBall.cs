using UnityEngine;

public class TrackBall : MonoBehaviour
{
    private Transform m_ball;

    private void Start()
    {
        m_ball = GameManager.Instance.Player1.transform;
    }

    void Update()
    {
        transform.position = m_ball.position;
    }
}
