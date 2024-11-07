using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private ParticleSystem waterParticles;

    private void Update()
    {
        Ray ray = new Ray(raycastOrigin.position, -raycastOrigin.up);

        RaycastHit raycastHit;

        bool hit = Physics.Raycast(ray, out  raycastHit, 200f, hitLayers);

        if (hit)
        {
            if (raycastHit.collider.gameObject.layer == 3)
            {
                var emi = dustParticles.emission;
                emi.enabled = true;
                emi = waterParticles.emission;
                emi.enabled = false;

            }
            else if(raycastHit.collider.gameObject.layer == 4)
            {
                var emi = dustParticles.emission;
                emi.enabled = false;
                emi = waterParticles.emission;
                emi.enabled = true;
            }

            Debug.Log(raycastHit.collider.gameObject.layer);
            Debug.DrawLine(ray.origin, raycastHit.point);
        }
    }
}
