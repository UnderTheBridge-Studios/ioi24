using System.Collections;
using UnityEngine;

public class WwiseManager : MonoBehaviour
{
    public static WwiseManager Instance;

    [SerializeField] float m_BeatAnimationSpeed = 1f;

    [Header("Decaying Values")]
    [SerializeField] float decayRateDestruction;
    [SerializeField] float decayRateCombo;

    [SerializeField] float addValueDestruction;
    [SerializeField] float addValueCombo;

    [SerializeField] private AK.Wwise.RTPC m_PeopleScreaming;
    [SerializeField] private AK.Wwise.RTPC m_ComboPlayer1;
    [SerializeField] private AK.Wwise.RTPC m_ComboPlayer2;

    private float levelOfDestruction;
    private float comboPlayer1;
    private float comboPlayer2;

    private Coroutine beatCo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        levelOfDestruction = Mathf.Max(0f, levelOfDestruction - Time.deltaTime*decayRateDestruction);
        comboPlayer1 = Mathf.Max(0f, comboPlayer1 - Time.deltaTime * decayRateCombo);
        comboPlayer2 = Mathf.Max(0f, comboPlayer2 - Time.deltaTime * decayRateCombo);

        m_PeopleScreaming.SetGlobalValue(levelOfDestruction);
        m_ComboPlayer1.SetGlobalValue(comboPlayer1);
        m_ComboPlayer2.SetGlobalValue(comboPlayer2);
    }

    public void UpdatePointsPlayer1(float destroyedBuildingHeight)
    {
        levelOfDestruction = Mathf.Min(1f, levelOfDestruction + destroyedBuildingHeight * addValueDestruction);
        comboPlayer1 = Mathf.Min(1f, comboPlayer1 + destroyedBuildingHeight * addValueDestruction);

        m_PeopleScreaming.SetGlobalValue(levelOfDestruction);
        m_ComboPlayer1.SetGlobalValue(comboPlayer1);
    }

    public void UpdatePointsPlayer2(float destroyedBuildingHeight)
    {
        levelOfDestruction = Mathf.Min(1f, levelOfDestruction + destroyedBuildingHeight * addValueDestruction);
        comboPlayer2 = Mathf.Min(1f, comboPlayer2 + destroyedBuildingHeight * addValueDestruction);

        m_PeopleScreaming.SetGlobalValue(levelOfDestruction);
        m_ComboPlayer2.SetGlobalValue(comboPlayer2);
    }

    public void BeatCallback()
    {
        //Debug.Log("Beat!");
        if(beatCo != null)
        {
            StopCoroutine(beatCo);
        }
        beatCo = StartCoroutine(ExecuteBeatAnimations());
    }

    private IEnumerator ExecuteBeatAnimations()
    {
        float ph = 0.5f * Mathf.PI; // Start at its peak to feel more in sync with the beat
        float val = 0f;
        while(ph < 2*Mathf.PI)
        {
            ph += Time.deltaTime * m_BeatAnimationSpeed;
            val += Mathf.Sin(ph);
            yield return null;
        }
    }
}
