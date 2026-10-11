using System;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquirrelUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro _acornText;

    [SerializeField] private Squirrel _squirrel;
    [SerializeField] private Transform _squirrelSprite;

    [SerializeField] private WarmthController _warmController;
    [SerializeField] private Slider _warmSlider;
    [SerializeField] private Color _warmColor, _coldColor;
    [SerializeField] private Animator _anim;

    void Start()
    {
        _squirrel.OnCollectAcorn += Handle_OnAcornCollect;
        _acornText.text = $"{_squirrel.CurrentAcorn}/{_squirrel.MaxAcorn}";
    }

    void Update()
    {
        if (_squirrel.MoveDirection.x > 0)
        {
            _squirrelSprite.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_squirrel.MoveDirection.x < 0)
        {
            _squirrelSprite.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (_squirrel.MoveDirection != Vector3.zero)
        {
            _anim.SetBool("IsRun", true);
        }
        else
        {
            _anim.SetBool("IsRun", false);
        }

        var currentWarmColor = Color.Lerp(_coldColor, _warmColor, (_warmController.CurrentWarmth / _warmController.MaxWarmth));
        _warmSlider.image.color = currentWarmColor;
        _warmSlider.value = _warmController.CurrentWarmth / _warmController.MaxWarmth;
    }

    private void Handle_OnAcornCollect()
    {
        _acornText.text = $"{_squirrel.CurrentAcorn}/{_squirrel.MaxAcorn}";
    }
}
