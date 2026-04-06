using UnityEngine;

public class KeyFragment : MonoBehaviour
{
    [SerializeField] private KeyFragmentTracker _tracker;

    [Header("Idle Animation")]
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatFrequency = 1f;

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        float offsetY = Mathf.Sin(Time.time * floatFrequency * Mathf.PI * 2f) * floatAmplitude;
        transform.position = _initialPosition + Vector3.up * offsetY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LabyrinthPlayer")) return;

        LabyrinthPlayer labyrinthPlayer = other.GetComponent<LabyrinthPlayer>();
        if (labyrinthPlayer == null) return;

        // Désactive le trigger pour éviter un double déclenchement
        GetComponent<Collider>().enabled = false;

        labyrinthPlayer.PlayPickUp(() =>
        {
            _tracker.CollectFragment();
            gameObject.SetActive(false);
        });
    }
}
