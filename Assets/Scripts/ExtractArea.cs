using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtractArea : MonoBehaviour
{
    public bool questComplete = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && questComplete)
        {
            Debug.Log("Level Complete!");
            SceneManager.LoadScene(1);
        }

    }
}
