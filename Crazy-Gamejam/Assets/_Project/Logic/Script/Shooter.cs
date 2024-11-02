using UnityEngine;
using Main.Utilities;

namespace Main.Gameplay.Enemies
{
    public class Shooter : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [SerializeField] private GameObject bulletPrefab; // Prefab da bala
        [SerializeField] private Transform firePoint; // Ponto de onde a bala será disparada
        [SerializeField] private float bulletSpeed = 10f; // Velocidade inicial da bala

        [Header("Cooldown Settings")]
        [SerializeField] private float fireRate = 1f; // Tiros por segundo
        [SerializeField] private Cooldown cooldown; // Cooldown para o disparo

        private void Start()
        {
            // Calcula o tempo de cooldown baseado na cadência de tiro
            cooldown = new Cooldown { _cooldownTime = 1f / fireRate };
        }

        private void Update()
        {
            // Verifica se o cooldown terminou e dispara a bala
            if (!cooldown.IsCoolingDown)
            {
                Shoot();
                cooldown.StartCoolDown();
            }
        }

        private void Shoot()
        {
            // Instancia a bala no firePoint
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Adiciona movimento à bala
            if (bullet.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = firePoint.forward * bulletSpeed;
            }
        }
    }
}
