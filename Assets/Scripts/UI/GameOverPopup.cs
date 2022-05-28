using System;
using TMPro;
using UnityEngine;


public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public event Action TryAgainClick; 

    public void Show(int score)
    {
        scoreText.text = score.ToString();
        gameObject.SetActive(true);
    }

    public void OnTryAgainClick()
    {
        TryAgainClick?.Invoke();
    }

    private void OnDestroy()
    {
        TryAgainClick = null;
    }
}