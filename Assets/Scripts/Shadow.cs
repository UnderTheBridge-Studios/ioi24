using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] Transform m_ball;
    [SerializeField] float m_height;

    void Update()
    {
        transform.position = new Vector3(m_ball.position.x, m_height, m_ball.position.z);
    }
}
