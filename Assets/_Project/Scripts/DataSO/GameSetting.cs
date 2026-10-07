using UnityEngine;

[CreateAssetMenu(fileName = "GameSetting", menuName = "Scriptable Objects/GameSetting")]
public class GameSetting : ScriptableObject
{
    public int MaxAcornCanCarry;
    public int RequireAcorn;
    public float MaxTimeInSecond;
    public int NumberOfFox;
}
