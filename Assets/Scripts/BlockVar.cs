using UnityEngine;

namespace ConveyorX.MagneticBricks.BlockManagement
{
    [System.Serializable]
    public class BlockVar
    {
        [Range(0, 9)] public int value = 0;
        public Color colorValue = Color.white;
    }
}
