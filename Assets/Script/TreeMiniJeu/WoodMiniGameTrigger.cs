using UnityEngine;

/// <summary>IActionable attaché à la WoodCell : déclenche le mini-jeu de bûcheron.</summary>
public class WoodMiniGameTrigger : MonoBehaviour, IActionable
{
    [SerializeField] private WoodMiniGameManager _miniGameManager;

    private Player _currentPawn;

    public void Action(Player currentPawn)
    {
        _currentPawn = currentPawn;
        _miniGameManager.EnterMiniGame(currentPawn, this);
    }

    /// <summary>Appelé par WoodMiniGameManager quand les 20 bûches sont collectées.</summary>
    public void OnMiniGameComplete()
    {
        DialogueComponent dialogue = GetComponent<DialogueComponent>();
        if (dialogue != null)
            dialogue.Action(_currentPawn);
    }
}
