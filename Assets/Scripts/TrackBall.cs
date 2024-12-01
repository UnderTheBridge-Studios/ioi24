using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class TrackBall : MonoBehaviour
{
    [SerializeField] private float m_lerpVelocity = 0.2f;
    [SerializeField] private bool m_isPlayer1;
    
    //HamsterRun
    [SerializeField] private float m_movementAmount;
    private float m_velocity;

    private Transform m_ball;
    private Vector2 m_input;
    private float m_angle;

    //Material instance properties
    private MaterialPropertyBlock m_materialBlock;
    private MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_materialBlock = new MaterialPropertyBlock();
        m_meshRenderer = GetComponent<MeshRenderer>();

        if (m_isPlayer1)
            m_ball = GameManager.Instance.Player1.transform;
        else
            m_ball = GameManager.Instance.Player2.transform;
    }

    void Update()
    {
        //if (m_isPlayer1){
        //    m_velocity = Mathf.Min((GameManager.Instance.velocityPlayer1) / 8, 1);
        //    Debug.Log("m_velocity: " + m_velocity);
        //}
        //else
        //    m_velocity = Mathf.Min((GameManager.Instance.velocityPlayer2) / 8, 1);

        //m_velocity *= 0.1f;
        //m_velocity = 1f;


        m_materialBlock.SetFloat("_SSpeed", m_velocity);
        m_materialBlock.SetFloat("_SAmount", m_movementAmount);
        m_meshRenderer.SetPropertyBlock(m_materialBlock);

        transform.position = m_ball.position;
        if (m_input.magnitude > 0.1)
            transform.DOLocalRotate(new Vector3(0, m_angle, 0), 0.3f);
    }

    public void Rotate(InputAction.CallbackContext context)
    {

        if (context.performed)
            m_velocity = 0.8f;
        if (context.canceled)
            m_velocity = 0f;


        m_input = context.ReadValue<Vector2>();
        m_angle = Vector2.SignedAngle(m_input, Vector2.up);
    }




}
