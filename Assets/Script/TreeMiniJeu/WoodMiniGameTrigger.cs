using UnityEngine;

/// <summary>IActionable attaché à la WoodCell : déclenche le dialogue d'intro puis le mini-jeu de bûcheron, une seule fois.</summary>
public class WoodMiniGameTrigger : MonoBehaviour, IActionable
{
    [SerializeField] private WoodMiniGameManager _miniGameManager;
    [SerializeField] private DialogueComponent _introDialogue;
    [SerializeField] private GameObject _treeObject;

    private Player _currentPawn;
    private bool _hasTriggered = false;

    public void Action(Player currentPawn)
    {
        if (_hasTriggered) return;

        _currentPawn = currentPawn;

        if (_introDialogue != null)
            _introDialogue.Action(currentPawn);
        else
            _miniGameManager.EnterMiniGame(currentPawn, this);
    }

    /// <summary>Appelé par onDialogueComplete du DialogueComponent d'intro.</summary>
    public void OnIntroDialogueComplete()
    {
        _miniGameManager.EnterMiniGame(_currentPawn, this);
    }

    /// <summary>Appelé par WoodMiniGameManager quand les 20 bûches sont collectées.</summary>
    public void OnMiniGameComplete()
    {
        _hasTriggered = true;
        QuestManager.Instance.HasWood = true;
        _treeObject?.SetActive(false);

        DialogueComponent dialogue = GetComponent<DialogueComponent>();
        if (dialogue != null)
            dialogue.Action(_currentPawn);
    }
}
