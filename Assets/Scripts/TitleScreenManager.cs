using ConveyorX.MagneticBricks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public Text levelText;
    public GameObject loadingScreen;

    private void Start()
    {
        levelText.text = "Level " + PlayerPrefs.GetInt("LEVEL", 1) + "\nBest Scores: " + PlayerPrefs.GetInt("BestScores", 0);

        //show ad while returning to title after playing certain amount of levels!
        if (PlayerPrefs.GetInt("LEVEL", 1) >= PlayerPrefs.GetInt("LAST", 3))
        {
            PlayerPrefs.SetInt("LAST", PlayerPrefs.GetInt("LEVEL", 1) + 3);
            Debug.Log("Showing level type ad!");
            AdsManager.GetInstance().ShowInterstitial();
        }

        Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("Inter");
            AdsManager.GetInstance().ShowInterstitial();
        }
    }

    public void Play() 
    {
        AudioManager.instance.Play(AssetsManager.Instance.ui);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Game");
    }
}
