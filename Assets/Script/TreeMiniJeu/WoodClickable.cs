using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Reçoit les clics souris sur l'arbre et notifie le WoodMiniGameManager.</summary>
[RequireComponent(typeof(Collider))]
public class WoodClickable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WoodMiniGameManager _manager;
    [SerializeField] private LogSpawner _logSpawner;

    [Header("Click Feedback")]
    [SerializeField] private Animator _treeAnimator;

    private Camera _woodCamera;

    private static readonly int HitHash = Animator.StringToHash("Hit");

    private void OnEnable()
    {
        // Récupère la caméra active au moment où l'arbre s'active
        _woodCamera = FindActiveCamera();
    }

    private void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Ray ray = _woodCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            OnTreeHit(hit.point);
    }

    private void OnTreeHit(Vector3 hitPoint)
    {
        if (_treeAnimator != null)
            _treeAnimator.SetTrigger(HitHash);

        _logSpawner?.SpawnLog(hitPoint);
        _manager.OnLogCollected();
    }

    private Camera FindActiveCamera()
    {
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.gameObject.activeInHierarchy)
                return cam;
        }
        return Camera.main;
    }
}
