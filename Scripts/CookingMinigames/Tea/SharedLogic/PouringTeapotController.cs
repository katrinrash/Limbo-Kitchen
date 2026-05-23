using System.Collections;
using UnityEngine;

public class PouringTeapotController : TeapotBase
{
    [SerializeField] private GaiwanManager gaiwan;
    [SerializeField] private ActionAnimationManager actionAnimationManager;

    public override void OnRaycast()
    {
        if (_isUsed) return;

        enabled = true;
    }

    void Update()
    {
        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            Check();
            return;
        }

        if (transform.position != _startPosition)
        {
            transform.position = _startPosition;
            enabled = false;
        }
    }


    private void Check()
    {
        if (Raycaster.CheckRaycastResult(gaiwan, transform.position))
        {
            _isUsed = true;
            actionAnimationManager.PlayAddAnimation(this, _startPosition, animations, animationObject, speed);
            enabled = false;
        }
    }

    public override void OnCompleted()
    {
        gaiwan.ChangeTheContent();
    }

}
