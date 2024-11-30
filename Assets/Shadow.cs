using UnityEngine;

public class Shadow : MonoBehaviour
{

    [SerializeField] private GameObject Ball;
    [SerializeField] private float heigt;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Ball.transform.position.x, heigt, Ball.transform.position.z);  
    }


    [ContextMenu("aaaa")]
    void teste()
    {
        Debug.Log("aaaaa");
    }
}
