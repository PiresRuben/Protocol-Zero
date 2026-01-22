using UnityEngine;

[CreateAssetMenu(fileName = "PassValue", menuName = "Scriptable Objects/PassValue")]
public class PassValue : ScriptableObject
{

    public float nbrSurvivant = 0;
    public int score = 0;
    public int nbrKill;
}
