using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder.MeshOperations;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private bool m_randomizeHeight;
    [SerializeField]
    private bool m_randomizeRotation;

    [Tooltip("0 to 5")]
    [SerializeField]
    private int m_buildingHeight;

    [Tooltip("0 to 4")]
    [SerializeField]
    private int m_buildingRotation;


    [Header("Meshes")]
    [SerializeField]
    private Mesh m_flat_concrete;
    [SerializeField]
    private Mesh m_building1;
    [SerializeField]
    private Mesh m_building2;
    [SerializeField]
    private Mesh m_building3;
    [SerializeField]
    private Mesh m_building4;
    [SerializeField]
    private Mesh m_buildingDestroyed;

    [SerializeField]
    private LayerMask m_layerMask;
    private Collider m_collider;
    private MeshFilter m_meshFilter;

    private int m_height = 0;
    private int m_rotation = 0;
    private int m_points = 0;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_meshFilter = GetComponent<MeshFilter>();

        if (m_randomizeHeight)
            m_height = Random.Range(0, 5);
        else
            m_height = m_buildingHeight;

        if (m_randomizeRotation)
            m_rotation = Random.Range(0, 4);
        else
            m_rotation = m_buildingRotation;

        SetMesh();
    }

    private void SetMesh()
    {
        switch (m_height)
        {
            case 0:
                m_meshFilter.mesh = m_flat_concrete;
                m_collider.enabled = false;
                break;
            case 1:
                m_meshFilter.mesh = m_building1;
                break;
            case 2:
                m_meshFilter.sharedMesh = m_building2;
                break;
            case 3:
                m_meshFilter.sharedMesh = m_building3;
                break;
            case 4:
                m_meshFilter.sharedMesh = m_building4;
                break;
        }

        switch (m_rotation)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0f ,0f ,0f );
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();

        if (playerController.GetVelocity().magnitude <= m_height)
        {
            playerController.Bounce(transform.position);
        }
        else
        {
            GameManager.Instance.AddPoints(playerController.playerNumber, m_height);
            m_collider.enabled = false;
            m_meshFilter.mesh = m_buildingDestroyed;
        }
    }
}
