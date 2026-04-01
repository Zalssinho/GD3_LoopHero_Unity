using System;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyFragmentTracker", menuName = "Scriptable Objects/KeyFragmentTracker")]
public class KeyFragmentTracker : ScriptableObject
{
    private const int RequiredFragments = 3;

    [SerializeField] private int _collectedFragments;

    public int CollectedFragments => _collectedFragments;
    public bool AllFragmentsCollected => _collectedFragments >= RequiredFragments;

    public event Action OnFragmentCollected;
    public event Action OnAllFragmentsCollected;

    /// <summary>Réinitialise le compteur de fragments.</summary>
    public void Reset()
    {
        _collectedFragments = 0;
    }

    /// <summary>Enregistre la collecte d'un fragment et notifie les abonnés.</summary>
    public void CollectFragment()
    {
        if (_collectedFragments >= RequiredFragments) return;

        _collectedFragments++;
        Debug.Log($"<color=yellow>Fragment {_collectedFragments} / {RequiredFragments} collecté !</color>");
        OnFragmentCollected?.Invoke();

        if (AllFragmentsCollected)
        {
            Debug.Log("<color=green>Tous les fragments collectés !</color>");
            OnAllFragmentsCollected?.Invoke();
        }
    }
}
