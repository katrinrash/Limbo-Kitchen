
public class AddContainerController : ContainerBaseController
{
    private bool _isUsed = false;

    public override void OnRaycast()
    {
        if (_isUsed) return;

        enabled = true;
    }

    protected override void Check()
    {
        if (Raycaster.CheckRaycastResult(targetTag, transform.position))
        {
            _isUsed = true;
            enabled = false;
            actionAnimationManager.PlayAddAnimation(this, _startPos, animations, animationObject, speed);
        }
    }

    protected override void ResetContainer()
    {
        actionAnimationManager.StopAllCoroutines();
        _isUsed = false;
        base.ResetContainer();
    }

    public override void OnCompleted()
    {
        stepManager.UpdateAfterUsed();
    }
}
