using UnityEngine;
using System.Collections;

public class Letterbox : MonoBehaviour
{
    [SerializeField] private RectTransform _topBar;
    [SerializeField] private RectTransform _bottomBar;
    [Tooltip("Bar height in pixels at the Canvas reference resolution")]
    [SerializeField] private float _barHeight = 120f;
    [SerializeField] private float _duration = 0.4f;

    private Coroutine _routine;

    private void Awake()
    {
        SetHeight(0f);
    }

    public void Show() => AnimateTo(_barHeight);
    public void Hide() => AnimateTo(0f);

    private void AnimateTo(float target)
    {
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(Animate(target));
    }

    private IEnumerator Animate(float target)
    {
        float start = _topBar.sizeDelta.y;
        float t = 0f;
        while (t < _duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / _duration);
            SetHeight(Mathf.Lerp(start, target, k));
            yield return null;
        }
        SetHeight(target);
        _routine = null;
    }

    private void SetHeight(float h)
    {
        _topBar.sizeDelta = new Vector2(_topBar.sizeDelta.x, h);
        _bottomBar.sizeDelta = new Vector2(_bottomBar.sizeDelta.x, h);
    }
}