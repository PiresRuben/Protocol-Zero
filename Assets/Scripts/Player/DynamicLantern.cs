using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DynamicLantern : MonoBehaviour
{
    [Header("Configuration")]
    public LayerMask obstacleLayer; // Assure-toi de mettre "Default" ou "Wall"
    public float smoothSpeed = 10f;
    public float minRadius = 1.5f; // Rayon minimum pour ne jamais être dans le noir total

    private Light2D light2D;
    private float targetRadius;
    private float defaultRadius;

    // Tampon pour éviter d'allouer de la mémoire à chaque frame
    private Collider2D[] results = new Collider2D[10];

    void Awake()
    {
        light2D = GetComponent<Light2D>();
        if (light2D != null)
        {
            defaultRadius = light2D.pointLightOuterRadius;
        }
    }

    void Update()
    {
        if (light2D == null) return;
        DetectNearestWall();
        ApplyLightRadius();
    }

    private void DetectNearestWall()
    {
        // On cherche tous les obstacles dans le rayon max de la lumière
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, defaultRadius, results, obstacleLayer);

        if (hitCount > 0)
        {
            float closestDistance = defaultRadius;

            // On parcourt tous les murs touchés pour trouver le point le plus proche
            for (int i = 0; i < hitCount; i++)
            {
                // ClosestPoint trouve le point exact sur la surface du collider le plus proche du joueur
                Vector2 closestPoint = results[i].ClosestPoint(transform.position);
                float distance = Vector2.Distance(transform.position, closestPoint);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }

            // On garde une taille minimale (minRadius) pour que le joueur voie ses pieds
            targetRadius = Mathf.Max(closestDistance, minRadius);
        }
        else
        {
            // Aucun mur à proximité, rayon maximum
            targetRadius = defaultRadius;
        }
    }

    private void ApplyLightRadius()
    {
        light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, targetRadius, Time.deltaTime * smoothSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        if (light2D != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, light2D.pointLightOuterRadius);
        }
    }
}