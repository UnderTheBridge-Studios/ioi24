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

    //HitStop
    [SerializeField] private float m_HitStopTime = 0.1f;
    [SerializeField] private float m_SpeedTimeHitStopTime = 0.2f;
    private bool m_IsInHitStop;
    private bool m_HasBounceInThisFrame;
    private Vector3 m_saveVelocity;

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

    private void LateUpdate()
    {
        m_HasBounceInThisFrame = false;
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
        if (m_HasBounceInThisFrame)
            return;
        m_HasBounceInThisFrame = true;

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

        m_ball.linearVelocity = Vector3.Reflect(m_ball.linearVelocity + Vector3.one, normal.normalized); 
    }

    public void SpeedBost(float height)
    {
        m_saveVelocity = m_ball.linearVelocity.normalized * GameManager.Instance.speedBoostMuliplier;
        m_IsInHitStop = true;
        m_ball.linearVelocity = Vector3.zero;
        Invoke("SetVelocity", m_SpeedTimeHitStopTime);
        m_WwiseSpeedBoost.Post(gameObject);
    }

    private void SetVelocity()
    {
        m_IsInHitStop = false;
        m_ball.linearVelocity = m_saveVelocity;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(m_isPlayer1 && collision.gameObject.TryGetComponent<PlayerController>(out var other))
        {
            m_WwiseCollisionPlayers.Post(gameObject);
        }
    }

    public void SmallBounce(float height)
    {
        if (transform.position.y < m_smallBounceThreshold)
            m_ball.AddForce(0, 100, 0);

        m_saveVelocity = m_ball.linearVelocity;
        m_IsInHitStop = true;
        m_ball.linearVelocity = Vector3.zero;
        Invoke("SetVelocity", m_HitStopTime /*+ (height * 0.01f)*/);
    }
}
