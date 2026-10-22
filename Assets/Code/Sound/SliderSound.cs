using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSound : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event tickSound;  
    [SerializeField] private float minInterval = 0.08f;    
    private Slider slider;
    private float lastPlayTime = -1f;
    private float enabledTime;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        enabledTime = Time.unscaledTime;
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        if (Time.unscaledTime - enabledTime < 0.2f) return;

        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != gameObject) return;

        if (Time.unscaledTime - lastPlayTime < minInterval) return;

        lastPlayTime = Time.unscaledTime;
        UISound.PlayEvent(tickSound);
    }
}