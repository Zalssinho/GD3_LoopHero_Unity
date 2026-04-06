using UnityEngine;

public class LabyrinthManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private LabyrinthPlayer _labyrinthPlayer;
    [SerializeField] private Player _boardPlayer;

    [Header("Key Fragment Tracker")]
    [SerializeField] private KeyFragmentTracker _tracker;

    [Header("Cameras")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _labyrinthCamera;

    [Header("Board UI")]
    [SerializeField] private GameObject _diceButton;
    [SerializeField] private KeyCounterUI _keyCounterUI;

    [Header("Spawn Point")]
    [SerializeField] private Transform _labyrinthSpawnPoint;

    [Header("Key Fragments to Reset")]
    [SerializeField] private KeyFragment[] _keyFragments;

    [Header("AI to Reset")]
    [SerializeField] private KnightAIController[] _knights;
    [SerializeField] private Transform[] _knightStartPositions;

    private void Start()
    {
        // Reset du tracker au démarrage : les ScriptableObjects persistent entre les sessions
        _tracker.Reset();

        _labyrinthPlayer.gameObject.SetActive(false);
        _labyrinthCamera.gameObject.SetActive(false);
    }


    /// <summary>Bascule dans le labyrinthe : téléporte le joueur, met le plateau en pause.</summary>
    public void EnterLabyrinth()
    {
        _tracker.Reset();
        ResetFragments();
        ResetAI();

        TeleportLabyrinthPlayer(_labyrinthSpawnPoint.position); // ← remplace l'ancienne ligne
        _labyrinthPlayer.gameObject.SetActive(true);

        _boardPlayer.gameObject.SetActive(false);

        if (_diceButton != null) _diceButton.SetActive(false);
        _keyCounterUI?.Show();

        _mainCamera.gameObject.SetActive(false);
        _labyrinthCamera.gameObject.SetActive(true);
    }


    /// <summary>Retour au plateau : désactive le joueur labyrinthe, réactive le pion et les contrôles.</summary>
    public void ExitLabyrinth()
    {
        _labyrinthPlayer.gameObject.SetActive(false);

        _boardPlayer.gameObject.SetActive(true);

        _labyrinthCamera.gameObject.SetActive(false);
        _mainCamera.gameObject.SetActive(true);

        if (_diceButton != null) _diceButton.SetActive(true);
        _keyCounterUI?.Hide();
    }

    /// <summary>Appelé par KnightAIController quand le joueur est attrapé : reset complet du labyrinthe.</summary>
    public void OnPlayerCaught()
    {
        Debug.Log("<color=red>Joueur attrapé ! Reset du labyrinthe.</color>");

        _tracker.Reset();
        ResetFragments();

        TeleportLabyrinthPlayer(_labyrinthSpawnPoint.position);
    }


    private void ResetFragments()
    {
        foreach (KeyFragment fragment in _keyFragments)
            fragment.gameObject.SetActive(true);
    }

    private void ResetAI()
    {
        for (int i = 0; i < _knights.Length; i++)
        {
            if (i < _knightStartPositions.Length)
                _knights[i].transform.position = _knightStartPositions[i].position;

            _knights[i].ResetToPatrol();
        }
    }

    /// <summary>Téléporte le LabyrinthPlayer en désactivant temporairement le CharacterController.</summary>
    private void TeleportLabyrinthPlayer(Vector3 destination)
    {
        CharacterController cc = _labyrinthPlayer.GetComponent<CharacterController>();
        cc.enabled = false;
        _labyrinthPlayer.transform.position = destination;
        cc.enabled = true;
    }

}
