using TMPro;
using UnityEngine;

public class RenderScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreRef;
    [SerializeField] private TextMeshProUGUI survivorNbrRef;
    [SerializeField] private TextMeshProUGUI killNbrRef;
    [SerializeField] private PassValue passeur;


    private void Start()
    {
        scoreRef.text = "Score: " + passeur.score;
        survivorNbrRef.text = "Save Survivor: " + passeur.nbrSurvivant;
        killNbrRef.text = "Zombies Kill : " + passeur.nbrKill;

        passeur.score = 0;
        passeur.nbrSurvivant = 0;
        passeur.nbrKill = 0;
    }
}
