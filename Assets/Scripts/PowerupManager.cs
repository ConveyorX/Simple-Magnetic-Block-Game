using ConveyorX.MagneticBricks.BlockManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ConveyorX.MagneticBricks
{
    public class PowerupManager : MonoBehaviour
    {
        public Button rainbowButton, hammerButton;

        GameManager gameManager;
        BlockManager blockManager;

        private void Start()
        {
            gameManager = GetComponent<GameManager>();
            blockManager = GetComponent<BlockManager>();
        }

        public void Use(string type)
        {
            //un-subscribe first
            AdsManager.OnRewardedAdWatchedComplete -= Hammer;
            AdsManager.OnRewardedAdWatchedComplete -= Rainbow;

            Debug.Log(type);

            //show ad then use!
            if (type == "HAMMER")
            {
                AdsManager.OnRewardedAdWatchedComplete += Hammer;
                AdsManager.GetInstance().ShowRewarded();
            }

            if (type == "RAINBOW")
            {
                AdsManager.OnRewardedAdWatchedComplete += Rainbow;
                AdsManager.GetInstance().ShowRewarded();
            }
        }

        public void Rainbow() 
        {
            Debug.Log("Rainbow Rewarded!");
            blockManager.Rainbow();
            int i = PlayerPrefs.GetInt("SFX", 1);
        }
        public void Hammer() 
        { 
            Debug.Log("Hammer Rewarded!");
            gameManager.IsHammerActive = true;
            int i = PlayerPrefs.GetInt("SFX", 1);
        }
    }
}