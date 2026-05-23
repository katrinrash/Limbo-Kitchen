using System.Collections;
using UnityEngine;

public class BoilingAnimationManager : MonoBehaviour
{
    [SerializeField] private BaseStepManager stepManager;
    [SerializeField] private SpriteRenderer animationObject1;
    [SerializeField] private SpriteRenderer animationObject2;
    [SerializeField] private Sprite[] animations1;
    [SerializeField] private Sprite[] animations2;
    [SerializeField] private float speed;

    #region UnityMethods

    private void Awake()
    {
        stepManager.OnStepCompleted += StopAndReset;
    }

    private void OnDestroy()
    {
        stepManager.OnStepCompleted -= StopAndReset;
    }

    #endregion

    public void StartBoiling()
    {
        StartCoroutine(Boil());
        AudioManager.Instance.PlayBoilingDumplingSound();
    }

    private IEnumerator Boil()
    {
        int index = 0;

        animationObject1.sprite = animations1[index];
        animationObject2.sprite = animations2[index++];

        while (true)
        {
            yield return new WaitForSeconds(speed);

            animationObject1.sprite = animations1[index];
            animationObject2.sprite = animations2[index++];

            if (index == animations1.Length) index = animations1.Length - 2;
        }

    }

    public void StopAndReset()
    {
        StopAllCoroutines();

        AudioManager.Instance.StopBoilingDumplingSound();
        animationObject1.sprite = animations1[0];
        animationObject2.sprite = null;
    }

}
