using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;
    //private List<Cell> _cellsList; Cela correspond à une liste.

    public Cell GetCellByNumber(int number)
    {
        return _cells[number];
    }

    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }
}   