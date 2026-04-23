using TMPro;
using UnityEngine;

/// <summary>Affiche le compteur de bûches et le timer pendant le mini-jeu.</summary>
public class WoodMiniGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _timerText;

    private const string CountFormat = "Bûches : {0} / {1}";
    private const string TimerFormat = "Timer : {0:0}s";

    /// <summary>Affiche le HUD et initialise le compteur et le timer.</summary>
    public void Show(int target, float timerDuration)
    {
        gameObject.SetActive(true);
        _countText.text = string.Format(CountFormat, 0, target);
        _timerText.text = string.Format(TimerFormat, timerDuration);
    }

    /// <summary>Met à jour le compteur affiché.</summary>
    public void UpdateCount(int current, int target)
    {
        _countText.text = string.Format(CountFormat, current, target);
    }

    /// <summary>Met à jour le timer affiché. Passe en rouge sous 5 secondes.</summary>
    public void UpdateTimer(float timeRemaining)
    {
        _timerText.text = string.Format(TimerFormat, Mathf.Max(0f, timeRemaining));
        _timerText.color = timeRemaining <= 5f ? Color.red : Color.black;
    }

    /// <summary>Masque le HUD.</summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
