using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WarmthController : MonoBehaviour
{
    [SerializeField] private float _maxWarmth = 100;
    [SerializeField] private float _currentWarmth;
    [SerializeField] private float _warmthDecreaseSpeed;
    [SerializeField] private Color _coldVolumeColor, _warmVolumeColor, _normalVolumeColor;
    [SerializeField] private float _minIntend = 0, _maxIntend = 0.5f;
    [SerializeField] private Volume _globalVolume;
    private bool _isEndGame;
    private bool _isOutSide;

    void Start()
    {
        _isEndGame = false;
        _currentWarmth = _maxWarmth;
        _isOutSide = false;
    }

    void Update()
    {
        if (_isEndGame) return;

        UpdateVolume();
        if (_isOutSide == false) return;

        _currentWarmth -= _warmthDecreaseSpeed * Time.deltaTime;
        if(_currentWarmth <= 0)
        {
            MessageBus.Publish<OnOutOfWarmth>(new());
            _isEndGame = true;
            return;
        }
    }

    public void SetIsOutSide(bool isOutSide)
    {
        _isOutSide = isOutSide;
        if (isOutSide == false) _currentWarmth = _maxWarmth;
    }

    private void UpdateVolume()
    {
        var currentIntend = Mathf.Lerp(_minIntend, _maxIntend, 1 - (_currentWarmth / _maxWarmth));
        var vin = _globalVolume.profile.components[0] as Vignette;
        if(_currentWarmth >= _maxWarmth)
        {
            vin.color.value = _warmVolumeColor;
            vin.intensity.value = _maxIntend;
        }
        else
        {
            vin.intensity.value = currentIntend;
            vin.color.value = _coldVolumeColor;
        }
    }
}
