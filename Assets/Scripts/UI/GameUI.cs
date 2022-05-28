using System;
using TMPro;
using UnityEngine;


public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private TextMeshProUGUI rotationText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI laserChargesText;
    [SerializeField] private TextMeshProUGUI laserCooldownText;


    public void OnScoreChanged(int value)
    {
        scoreText.text = value.ToString();
    }

    public void OnPositionChanged(Vector2 value)
    {
        positionText.text = string.Concat(Math.Round(value.x, 1), " / ", Math.Round(value.y, 1));
    }

    public void OnRotationChanged(float value)
    {
        rotationText.text = string.Concat(Math.Round(value), "°");
    }

    public void OnSpeedChanged(float value)
    {
        speedText.text = string.Concat(Math.Round(value, 1));
    }

    public void OnLaserChargesChanged(int value)
    {
        laserChargesText.text = value.ToString();
    }

    public void OnLaserCooldownChanged(float value)
    {
        laserCooldownText.text = Math.Round(value, 1).ToString();
    }
}
