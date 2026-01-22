using UnityEngine;
using UnityEngine.Rendering.Universal; // Nécessaire pour accéder au composant Light2D

public class DynamicFlashlight : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Sélectionne les layers qui doivent bloquer la lumière (ex: Default, Walls)")]
    public LayerMask obstacleLayer;

    [Tooltip("Vitesse de transition pour éviter les saccades")]
    public float smoothSpeed = 15f;

    [Tooltip("Distance minimale pour que la lumière ne disparaisse pas totalement")]
    public float minLightDistance = 0.5f;

    private Light2D light2D;
    private float defaultOuterRadius;

    void Awake()
    {
        light2D = GetComponent<Light2D>();

        // On sauvegarde le rayon défini dans l'inspecteur au lancement comme étant le "max"
        if (light2D != null)
        {
            defaultOuterRadius = light2D.pointLightOuterRadius;
        }
        else
        {
            Debug.LogError("Pas de composant Light2D trouvé sur cet objet !");
        }
    }

    void Update()
    {
        if (light2D == null) return;

        AdjustLightRange();
    }

    private void AdjustLightRange()
    {
        // 1. Définir la direction du rayon
        // D'après ton screenshot, ta light a une rotation Z de -90. 
        // Dans Unity 2D, les Spot Lights pointent généralement vers l'axe Y local (transform.up).
        Vector2 origin = transform.position;
        Vector2 direction = transform.up;

        // 2. Lancer le rayon
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, defaultOuterRadius, obstacleLayer);

        float targetRadius;

        // 3. Calculer la nouvelle portée
        if (hit.collider != null)
        {
            // On a touché un mur : la portée devient la distance jusqu'au mur
            // On garde une petite distance minimale pour l'esthétique
            targetRadius = Mathf.Clamp(hit.distance, minLightDistance, defaultOuterRadius);
        }
        else
        {
            // Rien devant : on remet la portée par défaut
            targetRadius = defaultOuterRadius;
        }

        // 4. Appliquer avec un lissage (Lerp) pour que ce soit fluide
        light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, targetRadius, Time.deltaTime * smoothSpeed);
    }

    // Pour visualiser le rayon dans l'éditeur
    private void OnDrawGizmos()
    {
        if (light2D == null) return;

        Gizmos.color = Color.yellow;
        // On dessine la ligne jusqu'à la portée max théorique
        Gizmos.DrawLine(transform.position, transform.position + transform.up * (defaultOuterRadius > 0 ? defaultOuterRadius : 5f));
    }
}