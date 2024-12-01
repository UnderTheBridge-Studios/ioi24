using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_ball;

    [Tooltip("True Player1, False Player2")]
    [SerializeField] private bool m_isPlayer1;
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_smallBounceThreshold = 0.8f;

    private float m_velocity;

    [Header("Wwise")]
    [SerializeField] private AK.Wwise.Event m_WwiseCollisionPlayers;
    [SerializeField] private AK.Wwise.Event m_WwiseSpeedBoost;

    private Vector3 m_movement;
    private Vector2 m_input;

    private bool IsInHitStop;

    public bool IsPlayer1 => m_isPlayer1;

    private void Awake()
    {
        m_ball = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_ball.AddForce(m_movement * m_acceleration);
        GameManager.Instance.SaveBallVelocity(m_isPlayer1, m_ball.linearVelocity.magnitude);
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

    public void SpeedBost(float height)
    {
        m_ball.linearVelocity = m_ball.linearVelocity.normalized * GameManager.Instance.speedBoostMuliplier;
        IsInHitStop = true;
        m_WwiseSpeedBoost.Post(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(m_isPlayer1 && collision.gameObject.TryGetComponent<PlayerController>(out var other))
        {
            m_WwiseCollisionPlayers.Post(gameObject);
        }
    }

    public void SmallBounce()
    {
        if(transform.position.y < m_smallBounceThreshold)
            m_ball.AddForce(0, 100, 0);
    }
}
