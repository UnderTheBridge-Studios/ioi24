using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_ball;

    [Tooltip("True Player1, False Player2")]
    [SerializeField] private bool m_isPlayer1;

    [SerializeField] private float m_acceleration;

    private Vector3 m_movement;
    private Vector2 m_input;

    public bool IsPlayer1 => m_isPlayer1;

    private void Awake()
    {
        m_ball = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_ball.AddForce(m_movement * m_acceleration);
    }

    public void Move(InputAction.CallbackContext context)
    {
        m_input = context.ReadValue<Vector2>();
        m_movement = new Vector3(m_input.x, 0.0f, m_input.y);
    }

    public Vector3 GetVelocity()
    {
        return m_ball.linearVelocity;
    }

    public void Bounce(Vector3 buildPos)
    {
        Vector3 normal;
        if (transform.position.x < buildPos.x)
        {
            if (transform.position.z < buildPos.z)
                normal = new Vector3(-1, 0, -1);
            else
                normal = new Vector3(-1, 0, 1);
        }
        else
        {
            if (transform.position.z < buildPos.z)
                normal = new Vector3(1, 0, -1);
            else
                normal = new Vector3(1, 0, 1);
        }

        m_ball.linearVelocity = Vector3.Reflect(m_ball.linearVelocity, normal.normalized); 
    }
    public void SmallBounce()
    {
        m_ball.AddForce(0, 1, 0);
    }

}
