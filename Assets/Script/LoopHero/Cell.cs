using UnityEngine;

public class Cell : MonoBehaviour, ICellActivable
{
    public virtual void Activate(Player CurrentPawn)
    {
        IActionable[] actionables = GetComponents<IActionable>();

        if (actionables.Length == 0) return;

        // Si plusieurs IActionable, on priorise celui qui n'est pas un DialogueComponent
        // (le DialogueComponent est déclenché par les autres IActionable en fin de séquence)
        IActionable primary = null;
        foreach (IActionable actionable in actionables)
        {
            if (actionable is not DialogueComponent)
            {
                primary = actionable;
                break;
            }
        }

        // Fallback : uniquement un DialogueComponent sur la cellule
        if (primary == null)
            primary = actionables[0];

        primary.Action(CurrentPawn);
    }
}
