using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    private PlayerControlers m_controls;
    private PlayerControlers.GameplayActions m_gamePlay;

    private Rigidbody m_ball;
    [SerializeField] private float m_acceleration;

    Vector3 movement;

    void Awake()
    {
        //Input Map
        m_controls = new PlayerControlers();
        m_gamePlay = m_controls.Gameplay;


        m_gamePlay.Movement.performed += ctx => Move(ctx);

        m_ball = GetComponent<Rigidbody>();
    }


    // Update is called once per fram
    void Update()
    {
        //Debug.Log("V: " + m_ball.angularVelocity);        
    }

    private void FixedUpdate()
    {
        Debug.Log("Move: " + movement);
        m_ball.AddForce(movement * m_acceleration);
    }

    public void Move(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0, input.y);

        //Debug.Log("x: " + input.x + " y: " + input.y);

    }


}
