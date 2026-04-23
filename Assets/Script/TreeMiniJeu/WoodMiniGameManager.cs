using UnityEngine;

/// <summary>Orchestre la transition entre le plateau et le mini-jeu de bûcheron.</summary>
public class WoodMiniGameManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Player _boardPlayer;

    [Header("Cameras")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _woodCamera;

    [Header("Board UI")]
    [SerializeField] private GameObject _diceButton;

    [Header("Mini-game UI")]
    [SerializeField] private WoodMiniGameUI _miniGameUI;

    [Header("Mini-game Settings")]
    [SerializeField] private WoodClickable _tree;
    [SerializeField] private int _logTarget = 20;
    [SerializeField] private float _timerDuration = 30f;

    private WoodMiniGameTrigger _trigger;
    private int _logsCollected;
    private float _timeRemaining;
    private bool _isRunning;

    private const string LogFormat = "<color=green>[WoodMiniGame] {0}</color>";

    private void Start()
    {
        _woodCamera.gameObject.SetActive(false);
        _tree.gameObject.SetActive(false);
        _miniGameUI?.Hide();
    }

    private void Update()
    {
        if (!_isRunning) return;

        _timeRemaining -= Time.deltaTime;
        _miniGameUI?.UpdateTimer(_timeRemaining);

        if (_timeRemaining <= 0f)
            ResetMiniGame();
    }

    /// <summary>Bascule dans le mini-jeu : masque le plateau, active la caméra arbre.</summary>
    public void EnterMiniGame(Player boardPlayer, WoodMiniGameTrigger trigger)
    {
        _trigger = trigger;
        StartMiniGame();

        _boardPlayer.gameObject.SetActive(false);
        _diceButton?.SetActive(false);

        _mainCamera.gameObject.SetActive(false);
        _woodCamera.gameObject.SetActive(true);

        _tree.gameObject.SetActive(true);
        _miniGameUI?.Show(_logTarget, _timerDuration);

        Debug.Log(string.Format(LogFormat, "Mini-jeu bûcheron démarré."));
    }

    /// <summary>Appelé par WoodClickable à chaque clic valide sur l'arbre.</summary>
    public void OnLogCollected()
    {
        _logsCollected++;
        _miniGameUI?.UpdateCount(_logsCollected, _logTarget);
        Debug.Log(string.Format(LogFormat, $"Bûche {_logsCollected}/{_logTarget} collectée."));

        if (_logsCollected >= _logTarget)
            CompleteMiniGame();
    }

    private void StartMiniGame()
    {
        _logsCollected = 0;
        _timeRemaining = _timerDuration;
        _isRunning = true;
    }

    private void ResetMiniGame()
    {
        Debug.Log(string.Format(LogFormat, "Temps écoulé ! Reset du mini-jeu."));
        StartMiniGame();
        _miniGameUI?.Show(_logTarget, _timerDuration);
    }

    private void CompleteMiniGame()
    {
        _isRunning = false;

        _tree.gameObject.SetActive(false);
        _woodCamera.gameObject.SetActive(false);
        _mainCamera.gameObject.SetActive(true);

        _boardPlayer.gameObject.SetActive(true);
        _diceButton?.SetActive(true);

        _miniGameUI?.Hide();

        Debug.Log(string.Format(LogFormat, "Mini-jeu terminé ! Lancement du dialogue."));
        _trigger.OnMiniGameComplete();
    }
}
