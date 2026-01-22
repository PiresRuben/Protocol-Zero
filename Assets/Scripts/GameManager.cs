using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ExtractArea extractArea;

    private Ennemie[] ennemies;
    private Survivor[] survivors;

    private int nbrEnnemiAlive = 0;
    private int nbrSurvivantAlive = 0;

    public static GameManager Instance;

    public int score = 0;

    public PassValue passeur;
    [Space(10)]
    public GameObject blood;

    private int nbrKill;

    private float nbrSurvivorSave = 0;
    public static GameManager GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) // Securité pour si empecher plusieurs GameManager de coexister
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        ennemies = FindObjectsByType<Ennemie>(FindObjectsSortMode.None);
        survivors = FindObjectsByType<Survivor>(FindObjectsSortMode.None);

        nbrEnnemiAlive = ennemies.Length;
        nbrSurvivantAlive = survivors.Length;
    }

    public void EnnemieDying()
    {
        nbrEnnemiAlive--;
        score += 10;
        if (nbrEnnemiAlive < 0)
        {
            Debug.Log("Tout les ennemie sont mort");
            extractArea.canExtract = true;
            nbrKill++;
        }
    }
    public void SurvivorDying()
    {
        nbrSurvivantAlive--;
        score -= 100;
    }

    public void PassValueBewtwenScene(int nbrSurvivor)
    {
        passeur.nbrSurvivant = nbrSurvivor;
        passeur.score = score;
        passeur.nbrKill = nbrKill;
    }
}
