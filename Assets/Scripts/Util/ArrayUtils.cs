using System;

public static class ArrayUtils
{
    private static int _rowSize = Inventory.ROW_SIZE;

    public static void ShrinkArrayByRow<T>(T[] array, int elemIndex)
    {
        CopyValuesToRow(array, elemIndex);
        // resize array
        Array.Resize(ref array, array.Length - _rowSize);
    }

    public static void CopyValuesToRow<T>(T[] array, int elemIndex)
    {
        // remove empty row
        int rowStartIndex = (elemIndex / _rowSize) * _rowSize;
        int nextRowIndex = rowStartIndex + _rowSize;
        // if not last row
        if (nextRowIndex < array.Length)
        {
            // copy remaining contents
            Array.Copy(array, nextRowIndex, array, rowStartIndex, array.Length - nextRowIndex);
        }
    }
}
