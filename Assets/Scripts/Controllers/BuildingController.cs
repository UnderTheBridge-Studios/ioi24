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

    [Header("Colors")]
    [SerializeField] private Color m_gold;
    [SerializeField] private Color m_black;
    [SerializeField] private Color m_blue;
    [SerializeField] private Color m_player1;
    [SerializeField] private Color m_player2;

    [Header("Default Values")]
    [Tooltip("0 to 4")]
    [SerializeField] private int m_buildingHeight;
    [Tooltip("0 to 3")]
    [SerializeField] private int m_buildingRotation;

    [Header("Meshes")]
    [SerializeField] private Mesh[] m_buildings;

    [Header("Wwise")]
    [SerializeField] private AK.Wwise.Event m_WwiseBuildingDestroyedPlayer1;
    [SerializeField] private AK.Wwise.Event m_WwiseBuildingDestroyedPlayer2;
    [SerializeField] private AK.Wwise.Event m_WwiseBuildingCollision;

    private Collider m_collider;
    private MeshFilter m_meshFilter;
    private ParticleSystem m_particleSystem;

    private int m_height = 0;
    private int m_rotation = 0;
    private float m_breakVelocity = 0.1f;
    private Coroutine m_coroutine;

    private MaterialPropertyBlock m_materialBlock;
    private MeshRenderer m_meshRenderer;

    public enum BuildingColor { Default, Gold, Black, Blue, Player1, Player2}

    private BuildingColor m_color = BuildingColor.Default;
    public BuildingColor CurrentColor => m_color;

    private void Start()
    {
        m_collider = GetComponent<Collider>();
        m_meshFilter = GetComponent<MeshFilter>();
        m_particleSystem = GetComponentInChildren<ParticleSystem>();

        m_materialBlock = new MaterialPropertyBlock();
        m_meshRenderer = GetComponent<MeshRenderer>();

        if (m_randomizeHeight)
        {
            float chances = Random.Range(0f ,1f);

            if(chances <= m_heightChances[0])
                m_height = 0;
            else if (chances > m_heightChances[0] && chances <= m_heightChances[1])
                m_height = 1;
            else if (chances > m_heightChances[1] && chances <= m_heightChances[2])
                m_height = 2;
            else if (chances > m_heightChances[2] && chances <= m_heightChances[3])
                m_height = 3;
            else if (chances > m_heightChances[3] && chances <= 1f)
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
                transform.localRotation = Quaternion.Euler(0f ,45f ,0f );
                break;
            case 1:
                transform.localRotation = Quaternion.Euler(0f, 135f, 0f);
                break;
            case 2:
                transform.localRotation = Quaternion.Euler(0f, 225f, 0f);
                break;
            case 3:
                transform.localRotation = Quaternion.Euler(0f, 315f, 0f);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();

        if (playerController.GetVelocity().magnitude <= m_breakVelocity)
        {
            playerController.Bounce(transform.position);
            m_WwiseBuildingCollision.Post(gameObject);
        }
        else
        {
            DestroyBuilding(playerController);
        }
    }

    private void DestroyBuilding(PlayerController player)
    {
        m_collider.enabled = false;
        m_meshFilter.mesh = m_buildings[5];

        GameManager.Instance.AddPoints(player.IsPlayer1, m_color, m_height);
        GameManager.Instance.RemoveBuilding(this);

        if (m_color == BuildingColor.Blue)
            player.SpeedBost(m_height);
        else
            player.SmallBounce(m_height);

        if (player.IsPlayer1)
            m_WwiseBuildingDestroyedPlayer1.Post(gameObject);
        else
            m_WwiseBuildingDestroyedPlayer2.Post(gameObject);

        if (player.IsPlayer1)
            SetBuildingColor(BuildingColor.Player1);
        else
            SetBuildingColor(BuildingColor.Player2);

        m_particleSystem.Play();
    }

    public void SetBuildingColor(BuildingColor color)
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        m_color = color;

        switch (m_color)
        {
            case BuildingColor.Default:
                m_materialBlock.SetColor("_Tint", Color.white);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);

                break;
            case BuildingColor.Gold:
                m_materialBlock.SetColor("_Tint", m_gold);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);

                m_coroutine = StartCoroutine(colorTimer());
                break;
            case BuildingColor.Black:
                m_materialBlock.SetColor("_Tint", m_black);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);

                m_coroutine = StartCoroutine(colorTimer());
                break;
            case BuildingColor.Blue:
                m_materialBlock.SetColor("_Tint", m_blue);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);

                m_coroutine = StartCoroutine(colorTimer());
                break;
            case BuildingColor.Player1:
                m_materialBlock.SetColor("_Tint", m_player1);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);

                break;
            case BuildingColor.Player2:
                m_materialBlock.SetColor("_Tint", m_player2);
                m_meshRenderer.SetPropertyBlock(m_materialBlock);
                break;
        }
    }

    private IEnumerator colorTimer()
    {
        yield return new WaitForSeconds(m_colorTime);
        SetBuildingColor(BuildingColor.Default);
    }
}
