using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder.MeshOperations;

public class BuildingController : MonoBehaviour
{
    [Header("Debug Colors")]
    [SerializeField] private bool m_isGold;
    [SerializeField] private bool m_isBlack;
    [SerializeField] private bool m_isBlue;

	[Header("Randomizers")]
    [SerializeField] private bool m_randomizeHeight;
    [SerializeField] private bool m_randomizeRotation;

    [Header("Default Values")]
    [Tooltip("0 to 4")]
    [SerializeField] private int m_buildingHeight;
    [Tooltip("0 to 3")]
    [SerializeField] private int m_buildingRotation;

    [Header("Breaking velocities")]
    [SerializeField] private float[] m_velocities;

    [Header("Meshes")]
    [SerializeField] private Mesh[] m_buildings;

    private Collider m_collider;
    private MeshFilter m_meshFilter;

    private int m_height = 0;
    private int m_rotation = 0;
    private float m_breakVelocity = 0.1f;

    //Material instance properties
    public MaterialPropertyBlock m_materialBlock;
    private MeshRenderer m_meshRenderer;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_meshFilter = GetComponent<MeshFilter>();

        m_materialBlock = new MaterialPropertyBlock();
        m_meshRenderer = GetComponent<MeshRenderer>();

        if (m_randomizeHeight)
            m_height = Random.Range(0, 5);
        else
            m_height = m_buildingHeight;

        if (m_randomizeRotation)
            m_rotation = Random.Range(0, 4);
        else
            m_rotation = m_buildingRotation;

        SetBuilding();

        //Debug option
        if (m_isGold)
            SetColor(Color.yellow);
        if (m_isBlack)
            SetColor(Color.black);
        if (m_isBlue)
            SetColor(Color.blue);
    }

    private void SetBuilding()
    {
        if (m_height == 0)
        {
            m_meshFilter.mesh = m_buildings[0];
            m_collider.enabled = false;
        }
        else
        {
            m_breakVelocity = m_velocities[m_height-1];
            m_meshFilter.mesh = m_buildings[m_height];
            m_collider.enabled = true;
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

        if (playerController.GetVelocity().magnitude <= m_breakVelocity)
        {
            playerController.Bounce(transform.position);
        }
        else
        {
            playerController.SmallBounce();
            GameManager.Instance.AddPoints(playerController.IsPlayer1, m_height);
            m_collider.enabled = false;
            m_meshFilter.mesh = m_buildings[5];
        }
    }

    public void SetColor(Color color)
    {
        m_materialBlock.SetColor("_BaseColor", color);
        m_meshRenderer.SetPropertyBlock(m_materialBlock);
    }
}
