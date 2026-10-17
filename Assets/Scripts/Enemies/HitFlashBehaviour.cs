using UnityEngine;
using System.Collections;

public class HitFlashBehaviour : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.softRed;
    [SerializeField] private float flashDuration = .2f;
    [SerializeField] private int flashTimes = 3; // how many times it will flash

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // this only works because we don't have anything else that manipulates this color. Otherwise we should have a stacking effects approach or something
        _originalColor = _spriteRenderer.color;
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < flashTimes; i++)
        {
            _spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            _spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        _flashCoroutine = null;
    }

    public void Flash()
    {
        if (_flashCoroutine != null)
        {
            // stops previous coroutine run
            StopCoroutine(_flashCoroutine);
        }

        _flashCoroutine = StartCoroutine(FlashCoroutine());
    }
}
