using ConveyorX.MagneticBricks.BlockManagement;
using UnityEngine;

namespace ConveyorX.MagneticBricks
{
    [DefaultExecutionOrder(0)]
    public class AssetsManager : MonoBehaviour
    {
        #region Singleton

        public static AssetsManager Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("Cannot run multiple instances of AssetManager...");
                Destroy(gameObject);
            }
        }

        #endregion

        public BlockVar[] blockVars = new BlockVar[9];
        public Color emptyColor = Color.grey;
        public AudioClip ui, block, win, lose;
        public AudioClip music;
        [Range(0,1)] public float musicVol = 0.5f;

        public Color GetColorVal(int val) 
        {
            for (int i = 0; i < blockVars.Length; i++) 
            {
                if (blockVars[i].value == val)
                    return blockVars[i].colorValue; 
            }
            return Color.white;
        }
    }
}
