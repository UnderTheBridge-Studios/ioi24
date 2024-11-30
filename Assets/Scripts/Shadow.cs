using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] float m_height;

    private Transform m_ball;


    private void Start()
    {
        m_ball = GameManager.Instance.Player1.transform;
    }

    void Update()
    {
        transform.position = new Vector3(m_ball.position.x, m_height, m_ball.position.z);
    }
}
