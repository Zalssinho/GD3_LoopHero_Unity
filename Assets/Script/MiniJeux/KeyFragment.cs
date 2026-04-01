using UnityEngine;

public class KeyFragment : MonoBehaviour
{
    [SerializeField] private KeyFragmentTracker _tracker;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LabyrinthPlayer")) return;

        _tracker.CollectFragment();
        gameObject.SetActive(false);
    }
}
