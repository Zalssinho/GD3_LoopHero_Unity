using UnityEngine;

public class WallTriggerCell : MonoBehaviour, IActionable
{
    [Header("References")]
    [SerializeField] private KeyFragmentTracker _tracker;
    [SerializeField] private LabyrinthManager _labyrinthManager;

    [Header("Door / Wall Settings")]
    [SerializeField] private GameObject _wall;
    [SerializeField] private float _moveDistance = 5f;
    [SerializeField] private float _moveDuration = 2f;
    [SerializeField] private WallController.MoveDirection _direction = WallController.MoveDirection.Left;

    private Vector3 _wallStartPosition;
    private Vector3 _wallTargetPosition;
    private bool _doorOpened = false;

    private void Start()
    {
        if (_wall != null)
        {
            _wallStartPosition = _wall.transform.position;
            _wallTargetPosition = _wallStartPosition + GetDirectionVector() * _moveDistance;
        }

        _tracker.OnAllFragmentsCollected += OnAllFragmentsCollected;
    }

    private void OnDestroy()
    {
        _tracker.OnAllFragmentsCollected -= OnAllFragmentsCollected;
    }

    public void Action(Player currentPawn)
    {
        if (_doorOpened) return;

        if (_tracker.AllFragmentsCollected)
        {
            OpenDoor();
        }
        else
        {
            _labyrinthManager.EnterLabyrinth();
        }
    }

    private void OnAllFragmentsCollected()
    {
        // Retour plateau immédiat, puis ouverture de la porte
        _labyrinthManager.ExitLabyrinth();
        OpenDoor();
    }

    private void OpenDoor()
    {
        if (_doorOpened || _wall == null) return;
        _doorOpened = true;
        StartCoroutine(MoveWall());
    }

    private System.Collections.IEnumerator MoveWall()
    {
        float elapsed = 0f;
        while (elapsed < _moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _moveDuration);
            _wall.transform.position = Vector3.Lerp(_wallStartPosition, _wallTargetPosition, t);
            yield return null;
        }
        _wall.transform.position = _wallTargetPosition;
    }

    private Vector3 GetDirectionVector()
    {
        return _direction switch
        {
            WallController.MoveDirection.Left    => Vector3.left,
            WallController.MoveDirection.Right   => Vector3.right,
            WallController.MoveDirection.Up      => Vector3.up,
            WallController.MoveDirection.Down    => Vector3.down,
            WallController.MoveDirection.Forward => Vector3.forward,
            WallController.MoveDirection.Back    => Vector3.back,
            _                                    => Vector3.left,
        };
    }
}

