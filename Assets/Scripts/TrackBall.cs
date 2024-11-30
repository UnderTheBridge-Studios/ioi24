using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class TrackBall : MonoBehaviour
{
    private Transform m_ball;
    private Vector2 m_input;
    private float m_angle;

    [SerializeField] private float m_lerpVelocity = 0.2f;

    [SerializeField] private bool m_isPlayer1;

    private void Start()
    {
        if (m_isPlayer1)
            m_ball = GameManager.Instance.Player1.transform;
        else
            m_ball = GameManager.Instance.Player2.transform;
    }

    void Update()
    {
        transform.position = m_ball.position;
        transform.DOLocalRotate(new Vector3(0, m_angle, 0), 0.3f);
        Debug.Log("transform.eulerAngles: " + transform.eulerAngles + " m_angle: " + m_angle);
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        m_input = context.ReadValue<Vector2>();

        m_angle = Vector2.SignedAngle(m_input, Vector2.up);
    }

}
