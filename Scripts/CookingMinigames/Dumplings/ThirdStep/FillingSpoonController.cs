
// Specialised spoon controller used for filling dumplings. Extends base spoon behaviour with simple fill-on-contact logic.
public class FillingSpoonController : BaseFillingSpoonController
{
    private void Update()
    {
        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            Check();
            return;
        }

        else if (transform.position != _defaultPosition)
        {
            transform.position = _defaultPosition;

            if (IsFilled)
            {
                IsFilled = false;
                spoonSpriteRenderer.sprite = spoonSprites[0];
            }

            enabled = false;
        }
    }

    protected override void Check()
    {
        if (Raycaster.CheckRaycastResult(targetTag, transform.position) && !IsFilled)
        {
            IsFilled = true;
            spoonSpriteRenderer.sprite = spoonSprites[1];
        }
    }
}
