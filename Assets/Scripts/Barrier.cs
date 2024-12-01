using UnityEngine;
using DG.Tweening;


public class Barrier : MonoBehaviour
{
    private int nPlayers = 0;
    [SerializeField] private GameObject m_barrierMesh;
    [SerializeField] private GameObject m_barrierMesh2;

    private void OnTriggerEnter(Collider other)
    {
        nPlayers++;
        m_barrierMesh2.transform.DOLocalMoveY(0, 0.2f);
        //m_barrierMesh.transform.DOLocalRotate(new Vector3(40, 45, 0), 0.2f);
    }

    private void OnTriggerExit(Collider other)
    {
        nPlayers--;
        if(!(nPlayers>0))
            m_barrierMesh2.transform.DOLocalMoveY(0, 0.2f);
            //m_barrierMesh.transform.DOLocalRotate(new Vector3(0, 45, 0), 0.2f);
    }
}
