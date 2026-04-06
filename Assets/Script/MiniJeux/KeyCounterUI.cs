using TMPro;
using UnityEngine;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private KeyFragmentTracker _tracker;
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private GameObject _keyCounterBloc;

    private void Start()
    {
        _tracker.OnFragmentCollected += UpdateCounter;
        _tracker.OnAllFragmentsCollected += UpdateCounter;

        UpdateCounter();
        _keyCounterBloc.SetActive(false);
    }

    private void OnDestroy()
    {
        _tracker.OnFragmentCollected -= UpdateCounter;
        _tracker.OnAllFragmentsCollected -= UpdateCounter;
    }

    /// <summary>Affiche le bloc compteur de clés.</summary>
    public void Show()
    {
        UpdateCounter();
        _keyCounterBloc.SetActive(true);
    }

    /// <summary>Cache le bloc compteur de clés.</summary>
    public void Hide()
    {
        _keyCounterBloc.SetActive(false);
    }

    private void UpdateCounter()
    {
        _counterText.text = $"Clé : {_tracker.CollectedFragments} / {_tracker.TotalFragments}";
    }
}
