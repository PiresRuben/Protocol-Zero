using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtractArea : MonoBehaviour
{
    public bool questComplete = false;

    public bool canExtract = false;

    public int nbrSurvivantToExtract = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && questComplete  && canExtract)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10);
            GameManager gameManager = GameManager.GetInstance();


            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<Survivor>(out Survivor script))
                {
                    if (script.isCured && !script.isDead)
                    {
                        Debug.LogWarning("lesd gbdezdzdzdz");
                        nbrSurvivantToExtract++;
                        gameManager.score += 50;
                    }
                }
            }
            gameManager.PassValueBewtwenScene(nbrSurvivantToExtract);
            Debug.Log("Level Complete!");
            SceneManager.LoadScene(1);
        }

    }
}
