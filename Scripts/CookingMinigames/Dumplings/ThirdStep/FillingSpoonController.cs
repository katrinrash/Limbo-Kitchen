/// <summary>
/// Specialised spoon controller used for filling dumplings. Extends base spoon behaviour with simple fill-on-contact logic.
/// </summary>

public class FillingSpoonController : BaseFillingSpoonController
{
    private void Update()
    {
        // Handle spoon movement and call check function to check for valid interaction while mouse button is held down
        if (InputManager.Instance.MouseAction.IsPressed())
        {
            Move();
            Check();
            return;
        }

        // Reset spoon when input is released
        else if (transform.position != _defaultPosition)
        {
            // Move spoon back to default position
            transform.position = _defaultPosition;

            // Reset filled state when spoon returns to default position
            if (IsFilled)
            {
                IsFilled = false;
                spoonSpriteRenderer.sprite = spoonSprites[0];
            }

            enabled = false;
        }
    }

    /// <summary>
    /// Checks whether spoon is placed on a valid target.
    /// </summary>
    protected override void Check()
    {
        if (Raycaster.CheckRaycastResult(targetTag, transform.position) && !IsFilled)
        {
            // Set spoon to filled state when valid target is detected and spoon is not already filled
            IsFilled = true;
            spoonSpriteRenderer.sprite = spoonSprites[1];
        }
    }
}
