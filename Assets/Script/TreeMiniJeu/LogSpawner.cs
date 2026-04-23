using UnityEngine;

/// <summary>Instancie et anime des bûches tombant de l'arbre à chaque clic.</summary>
public class LogSpawner : MonoBehaviour
{
    [Header("Log Prefab")]
    [SerializeField] private GameObject _logPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnRadiusXZ = 0.3f;
    [SerializeField] private float _destroyDelay = 3f;

    /// <summary>Spawn une bûche près du point de frappe avec une impulsion physique aléatoire.</summary>
    public void SpawnLog(Vector3 hitPoint)
    {
        if (_logPrefab == null) return;

        Vector3 offset = new Vector3(
            Random.Range(-_spawnRadiusXZ, _spawnRadiusXZ),
            0f,
            Random.Range(-_spawnRadiusXZ, _spawnRadiusXZ)
        );

        Vector3 spawnPos = (_spawnPoint != null ? _spawnPoint.position : hitPoint) + offset;
        GameObject log = Instantiate(_logPrefab, spawnPos, Random.rotation);

        Rigidbody rb = log.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 impulse = new Vector3(
                Random.Range(-2f, 2f),
                Random.Range(2f, 5f),
                Random.Range(-2f, 2f)
            );
            rb.AddForce(impulse, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 3f, ForceMode.Impulse);
        }

        Destroy(log, _destroyDelay);
    }
}
