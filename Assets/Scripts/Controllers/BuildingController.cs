using System.Collections;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Time to restore original building color")]
    [SerializeField] private float m_colorTime = 10f;
    [SerializeField] private bool m_randomizeHeight;
    [SerializeField] private bool m_randomizeRotation;
    [Tooltip("Breaking velocities")]
    [SerializeField] private float[] m_velocities;
    [Tooltip("Height chances of appearing")]
    [SerializeField] private float[] m_heightChances;

    [Header("Default Values")]
    [Tooltip("0 to 4")]
    [SerializeField] private int m_buildingHeight;
    [Tooltip("0 to 3")]
    [SerializeField] private int m_buildingRotation;

    [Header("Meshes")]
    [SerializeField] private Mesh[] m_buildings;

    private Collider m_collider;
    private MeshFilter m_meshFilter;

    private int m_height = 0;
    private int m_rotation = 0;
    private float m_breakVelocity = 0.1f;

    //Material instance properties
    private MaterialPropertyBlock m_materialBlock;
    private MeshRenderer m_meshRenderer;

    public enum BuildingState { Default, Colored, Destroyed }
    public enum BuildingColor { Default, Gold, Black, Blue}

    private BuildingState m_state = BuildingState.Default;
    private BuildingColor m_color = BuildingColor.Default;
    
    public BuildingState CurrentState => m_state;
    public BuildingColor CurrentColor => m_color;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_meshFilter = GetComponent<MeshFilter>();

        m_materialBlock = new MaterialPropertyBlock();
        m_meshRenderer = GetComponent<MeshRenderer>();

        if (m_randomizeHeight)
        {
            float chances = Random.Range(0f ,1f);

            if(chances <= m_heightChances[0])
                m_height = 0;
            else if (m_heightChances[0] > chances && chances <= m_heightChances[0])
                m_height = 1;
            else if (m_heightChances[1] > chances && chances <= m_heightChances[2])
                m_height = 2;
            else if (m_heightChances[2] > chances && chances <= m_heightChances[3])
                m_height = 3;
            else if (m_heightChances[3] > chances && chances <= 1f)
                m_height = 4;
        }
        else
        {
            m_height = m_buildingHeight;
        }

        if (m_randomizeRotation)
            m_rotation = Random.Range(0, 4);
        else
            m_rotation = m_buildingRotation;

        SetBuilding();
    }

    private void SetBuilding()
    {
        if (m_height == 0)
        {
            m_meshFilter.mesh = m_buildings[0];
            m_collider.enabled = false;
            GameManager.Instance.RemoveBuilding(this);
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
                transform.localRotation = Quaternion.Euler(0f ,0f ,0f );
                break;
            case 1:
                transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 2:
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 3:
                transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
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
            m_collider.enabled = false;
            m_meshFilter.mesh = m_buildings[5];
            m_state = BuildingState.Destroyed;

            GameManager.Instance.AddPoints(playerController.IsPlayer1, m_color, m_height);
            GameManager.Instance.RemoveBuilding(this);
        }
    }

    public void SetBuildingColor(BuildingColor color)
    {
        m_color = color;
        switch (m_color)
        {
            case BuildingColor.Default:
                m_materialBlock.SetColor("_BaseColor", Color.white);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);
                m_state = BuildingState.Default;
                break;
            case BuildingColor.Gold:
                m_materialBlock.SetColor("_BaseColor", Color.yellow);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);
                m_state = BuildingState.Colored;
                StartCoroutine(colorTimer());
                break;
            case BuildingColor.Black:
                m_materialBlock.SetColor("_BaseColor", Color.black);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);
                m_state = BuildingState.Colored;
                StartCoroutine(colorTimer());
                break;
            case BuildingColor.Blue:
                m_materialBlock.SetColor("_BaseColor", Color.blue);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);
                m_state = BuildingState.Colored;
                StartCoroutine(colorTimer());
                break;
        }
    }

    private IEnumerator colorTimer()
    {
        yield return new WaitForSeconds(m_colorTime);
        SetBuildingColor(BuildingColor.Default);
    }
}
