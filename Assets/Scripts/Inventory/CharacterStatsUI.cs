using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    public Text healthText;
    public Text attackText;
    public Text infectionText;
    public Text fatigueText;

    public void UpdateStats(int health, int attack, float infection, int fatigue)
    {
        healthText.text = "Vie: " + health;
        attackText.text = "Attaque: " + attack;
        infectionText.text = "Infection: " + infection + "%";
        fatigueText.text = "Fatigue: " + fatigue;
    }
}
