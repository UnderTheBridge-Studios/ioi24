using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder.MeshOperations;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private bool m_isRandomized;
    [SerializeField]
    private float m_velocity1;
    [SerializeField]
    private float m_velocity2;
    [SerializeField]
    private float m_velocity3;
    [SerializeField]
    private float m_velocity4;

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

    private Collider m_collider;
    private MeshFilter m_meshFilter;

    private int m_height = 0;
    private int m_rotation = 0;
    private float m_breakVelocity;
    private int m_points = 0;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_meshFilter = GetComponent<MeshFilter>();

        if (m_isRandomized)
        {
            m_height = Random.Range(0, 5);
            m_rotation = Random.Range(0, 4);
        }

        SetBuilding();
    }

    private void SetBuilding()
    {
        switch (m_height)
        {
            case 0:
                m_meshFilter.mesh = m_flat_concrete;
                break;
            case 1:
                m_meshFilter.mesh = m_building1;
                m_breakVelocity = m_velocity1;
                break;
            case 2:
                m_meshFilter.sharedMesh = m_building2;
                m_breakVelocity = m_velocity2;
                break;
            case 3:
                m_meshFilter.sharedMesh = m_building3;
                m_breakVelocity = m_velocity3;
                break;
            case 4:
                m_meshFilter.sharedMesh = m_building4;
                m_breakVelocity = m_velocity4;
                break;
        }

        if (m_height!=0)
            m_collider.enabled = true;

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

        if (playerController.GetVelocity().magnitude <= m_breakVelocity)
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
