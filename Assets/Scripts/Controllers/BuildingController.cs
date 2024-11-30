using UnityEngine;
using DG.Tweening;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_layerMask;

    private Collider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();

        if (playerController.GetVelocity().magnitude <=500)
        {
            playerController.Bounce(transform.position);
        }
        else
                {
            m_collider.enabled = false;
            transform.DOMoveY(-2f, 0.6f);
        }
    }
}
