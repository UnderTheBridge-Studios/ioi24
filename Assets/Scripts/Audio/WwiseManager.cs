using System.Collections;
using UnityEngine;

public class WwiseManager : MonoBehaviour
{
    [SerializeField] float m_BeatAnimationSpeed = 1f;

    private static WwiseManager instance;
    private Coroutine beatCo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        float ph = 0f;
        float val = 0f;
        while(ph < 2*Mathf.PI)
        {
            ph += Time.deltaTime * m_BeatAnimationSpeed;
            val += Mathf.Cos(ph); // I think cos is better than sin here
            yield return null;
        }
    }
}
