using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipDebugHUD : MonoBehaviour
{
    [Header("Spaceship movement")]
    [SerializeField] private Image _innerImage;
    [SerializeField] private Image _outerImage;
    [SerializeField] private SpaceshipMovement _spaceshipMovement;
    [SerializeField] private TextMeshProUGUI _velocityText;
    [SerializeField] private TextMeshProUGUI _thrustText;
    
    private RectTransform _innerRect;
    private float radius;

    private void Start()
    {
        _innerRect = _innerImage.GetComponent<RectTransform>();

        RectTransform _outerRect = _outerImage.GetComponent<RectTransform>();
        radius = _outerRect.rect.width * 0.5f - _innerRect.rect.width * 0.5f;
    }

    private void Update()
    {
        if (_spaceshipMovement == null) return;

        Vector2 input = _spaceshipMovement.GetDirectionDelta();
        input.Normalize();

        _innerRect.anchoredPosition = Vector2.Lerp(_innerRect.anchoredPosition, input * radius, 0.1f);

        _velocityText.text = "Velocity: " + _spaceshipMovement.GetVelocity().magnitude.ToString();
        _thrustText.text = "Thrust Velocity: " + _spaceshipMovement.GetThrust().ToString();
    }
}
