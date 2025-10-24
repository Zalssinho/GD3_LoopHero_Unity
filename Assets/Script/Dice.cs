using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Player _player;

    public void RollTheDice()
    {
        int value = Random.Range(1,7);
        Debug.Log($"Le d� a fait {value}");
        _player.TryMouving(value);
    }

}
