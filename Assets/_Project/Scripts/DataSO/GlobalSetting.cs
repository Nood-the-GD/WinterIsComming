using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

[CreateAssetMenu(fileName = "GlobalSetting", menuName = "Game/GlobalSetting", order = 0)]
public class GlobalSetting : ScriptableObject
{
    [SerializeField] private GameSetting _easy, _normal, _hard;
    public Difficulty CurrentDifficulty = Difficulty.Easy;
    public GameSetting CurrentDifficultySetting;
    public bool IsMusic;
    public bool IsSound;

    public void ChangeDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        switch (difficulty)
        {
            case Difficulty.Easy:
                CurrentDifficultySetting = _easy;
                break;
            case Difficulty.Normal:
                CurrentDifficultySetting = _normal;
                break;
            case Difficulty.Hard:
                CurrentDifficultySetting = _hard;
                break;
        }
    }
}