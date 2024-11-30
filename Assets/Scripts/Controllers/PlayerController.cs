using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 m_input;
    private Rigidbody m_characterController;

    [SerializeField]
    private float m_speed;
    private Vector3 m_direction;

    private void Awake()
    {
        m_characterController = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        m_characterController.AddForce(m_direction * m_speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        m_input = context.ReadValue<Vector2>();
        m_direction = new Vector3(m_input.x, 0.0f, m_input.y);
    }

    public Vector3 GetVelocity()
    {
        return m_characterController.linearVelocity;
    }
}
