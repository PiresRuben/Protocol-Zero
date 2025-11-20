using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI infectionText;
    public TextMeshProUGUI fatigueText;


    public void UpdateStats(int health, int maxhealth, int attack, float infection, int fatigue)
    {
        healthText.text = "Vie: " + health + "/" + maxhealth;
        attackText.text = "Attaque: " + attack;
        infectionText.text = "Infection: " + infection + "%";
        fatigueText.text = "Fatigue: " + fatigue;
    }
}
