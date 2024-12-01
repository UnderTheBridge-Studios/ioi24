using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private Transform player1;
    private Transform player2;

    private Vector3 posCamera;
    private float distancePlayersX;
    private float distancePlayersZ;


    [SerializeField] private float y_var1 = 3;
    [SerializeField] private float y_var2 = 5;

    [SerializeField] private float x_var1 = 4;
    [SerializeField] private float x_var2 = 5;


    [SerializeField] private float smothTime = 0.3f;
    [SerializeField] private float yVelocity = 0;


    void Start()
    {
        player1 = GameManager.Instance.Player2.transform;
        player2 = GameManager.Instance.Player1.transform;
    }

    void Update()
    {
        posCamera.x = (player1.position.x + player2.position.x) / 2;
        posCamera.y = 100;
        posCamera.z = ((player1.position.z + player2.position.z) / 2) - posCamera.y;

        distancePlayersX = Mathf.Abs(player1.position.x - player2.position.x);
        distancePlayersZ = Mathf.Abs(player1.position.z - player2.position.z);

        if(distancePlayersX > distancePlayersZ)
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, distancePlayersX / x_var1 + x_var2, ref yVelocity, smothTime);
        else
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, distancePlayersZ / y_var1 + y_var2, ref yVelocity, smothTime);

        Camera.main.transform.position = posCamera;
    }
}
