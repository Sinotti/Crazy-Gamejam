using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed; // Velocidade do projétil
    private int damage; // Dano causado

    private Transform target; // Referência ao alvo

    // Inicializa o projétil com um alvo
    public void Initialize(Transform target, int damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
    }
    
    private void Update()
    {
        if (target != null)
        {
            // Calcula a direção para o alvo e move o projétil nessa direção
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Verifica se o projétil alcançou o alvo
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Aplica dano ao alvo
                if (target.TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
                }
                Destroy(gameObject); // Destrói o projétil ao atingir o alvo
            }
        }
        else
        {
            Destroy(gameObject); // Destrói se o alvo não estiver mais disponível
        }
    }
}
