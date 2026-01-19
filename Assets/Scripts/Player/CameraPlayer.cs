using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public static CameraPlayer instance;

    public Transform player;
    public Vector3 offset;

    private float shakeTimeRemaining;
    private float shakePower;
    private float shakeFadeTime;

    [SerializeField] private float rotationMultiplier = 5f;

    private void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(player.position.x + offset.x, player.position.y + offset.y, -10);

        float rotationZ = 0f;

        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            targetPos += new Vector3(xAmount, yAmount, 0f);

            rotationZ = Random.Range(-1f, 1f) * shakePower * rotationMultiplier;

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }

        transform.position = targetPos;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    public void Shake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;
        shakeFadeTime = power / length;
    }
}