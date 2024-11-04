using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed; // Velocidade do proj�til
    private int damage; // Dano causado

    private Transform target; // Refer�ncia ao alvo

    // Inicializa o proj�til com um alvo
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
            // Calcula a dire��o para o alvo e move o proj�til nessa dire��o
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Verifica se o proj�til alcan�ou o alvo
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Aplica dano ao alvo
                if (target.TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
                }
                Destroy(gameObject); // Destr�i o proj�til ao atingir o alvo
            }
        }
        else
        {
            Destroy(gameObject); // Destr�i se o alvo n�o estiver mais dispon�vel
        }
    }
}
