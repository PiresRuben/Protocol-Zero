using TMPro;
using UnityEngine;

public class RenderScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreRef;
    [SerializeField] private TextMeshProUGUI survivorNbrRef;
    [SerializeField] private PassValue passeur;


    private void Start()
    {
        scoreRef.text = "Score: " + passeur.score;
        survivorNbrRef.text = "Save Survivor: " + passeur.nbrSurvivant;
    }
}
