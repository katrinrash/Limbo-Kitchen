using System.Collections;
using UnityEngine;

public class ActionAnimationManager : MonoBehaviour
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private float animationDuration;
    [SerializeField] private Transform endPositionAdd;
    [SerializeField] private Transform endPositionRemove;
    [SerializeField] private AnimationCurve animationCurve;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += StopAllCoroutines;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= StopAllCoroutines;
    }

    #endregion

    public void PlayAddAnimation(InteractableObject obj, Vector3 returnPos, Sprite[] sprites, SpriteRenderer renderer, float speed, bool extended = true)
    {
        StartCoroutine(AnimateAction(endPositionAdd, obj, returnPos, sprites, renderer, speed, extended));
    }

    public void PlayRemoveAnimation(InteractableObject obj, Vector3 returnPos, Sprite[] sprites, SpriteRenderer renderer, float speed, bool extended = true)
    {
        StartCoroutine(AnimateAction(endPositionRemove, obj, returnPos, sprites, renderer, speed, extended));
    }

    private IEnumerator AnimateAction(Transform pos, InteractableObject obj, Vector3 returnPos, Sprite[] sprites, SpriteRenderer renderer, float speed, bool extended)
    {
        obj.transform.GetPositionAndRotation(out var startPosition, out var startRotation);
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            obj.transform.position = Vector3.Lerp(startPosition, pos.position, animationCurve.Evaluate(t));
            yield return null;
        }

        int i = 0;

        renderer.sprite = sprites[i++];

        while (i < sprites.Length)
        {
            yield return new WaitForSeconds(speed);

            renderer.sprite = sprites[i++];
        }

        if (extended)
        {
            AudioManager.Instance.PlayPourOut();

            i = sprites.Length - 1;

            renderer.sprite = sprites[i--];

            while (i >= 0)
            {
                yield return new WaitForSeconds(speed);

                renderer.sprite = sprites[i--];
            }  
        }

        elapsedTime = 0f;
        renderer.sprite = sprites[0];

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            obj.transform.position = Vector3.Lerp(pos.position, returnPos, animationCurve.Evaluate(t));
            yield return null;
        }

        obj.OnCompleted();
    }

}
