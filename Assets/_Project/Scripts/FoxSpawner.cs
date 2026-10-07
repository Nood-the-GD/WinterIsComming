using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    [SerializeField] private GlobalSetting _globalSetting;
    [SerializeField] private Fox _foxPref;
    [SerializeField] private Vector2 _limitX, _limitY;

    private GameSetting _setting => _globalSetting.CurrentDifficultySetting;

    void Start()
    {
        var numberOfFox = _setting.NumberOfFox;
        for(int i = 0; i < numberOfFox; i++)
        {
            var randX = Random.Range(_limitX.x, _limitX.y);
            var randY = Random.Range(_limitY.x, _limitY.y);

            var newFox = Instantiate<Fox>(_foxPref, new Vector3(randX, randY, 0), Quaternion.identity);
        }
    }
}
