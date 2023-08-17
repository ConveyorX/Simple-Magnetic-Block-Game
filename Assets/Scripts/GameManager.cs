using ConveyorX.MagneticBricks.BlockManagement;
using ConveyorX.MagneticBricks.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ConveyorX.MagneticBricks
{
    public class GameManager : MonoBehaviour
    {
        public RectTransform dragBlock;
        public Text scoreText, bestText, levelText;
        public float scoreMultiplier = 10f;
        public Image imageBG;
        public Color[] levelColors;

        public GameObject winUI, loseUI, center;

        public bool IsDragging { get; set; }
        public int scores { get; set; }
        public bool SoundOn { get; private set; }
        public UIManager ui { get; set; }
        BlockManager blockManager;
        public int refreshLeft { get; set; }
        public Vector3 originalPos { get; set; }
        public Block currentOnBlock { get; set; }
        public bool IsHammerActive { get; set; }
        public int bestScores { get; private set; }
        bool doneCenter;

        public int GetLevelIndex(int level) 
        {
            if (level < 11) 
                return level - 1;
            else 
            {
                //e.g. level = 11 so 11 - (10 * (11 / 10)) = 11 - (10 * 1) = 1
                //e.g. level = 22 so 22 - (10 * (22 / 10)) = 22 - (10 * 2) = 2 and so on a infinite loop!
                return level - (10 * (level / 10));
            }
        }

        private void Start()
        {
            IsHammerActive = false;
            doneCenter = false;
            refreshLeft = 5;
            scores = 0;
            originalPos = dragBlock.position;
            int level = PlayerPrefs.GetInt("LEVEL", 1);
            levelText.text = level + "";
            bestScores = PlayerPrefs.GetInt("BestScores", 0);

            blockManager = GetComponent<BlockManager>();
            ui = FindObjectOfType<UIManager>();
            Application.targetFrameRate = 60;
            bestText.text = "" + bestScores;
            scoreText.text = "0";
            ui.redoButton.interactable = true;

            int i = PlayerPrefs.GetInt("SFX", 1);
            if (i == 1)
                SoundOn = true;
            else 
                SoundOn = false;
            ui.iconSound.sprite = SoundOn ? ui.onSpr : ui.noSpr;
            AudioManager.instance.Toggle(SoundOn);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Exit();
                AudioManager.instance.Play(AssetsManager.Instance.ui);
            }
        }

        public void Redo()
        {
            if (refreshLeft > 0) 
            {
                blockManager.RefreshNewBlock();
                refreshLeft--;

                if (refreshLeft <= 0)
                    ui.redoButton.interactable = false;
                AudioManager.instance.Play(AssetsManager.Instance.ui);
            }
        }

        public void Restart() 
        {
            //simply start anew!
            blockManager.Restart(true);
            AudioManager.instance.Play(AssetsManager.Instance.ui);
        }

        public void Home() 
        {
            SceneManager.LoadScene("TITLE");
            AudioManager.instance.Play(AssetsManager.Instance.ui);
        }

        public void Exit()
        {
            Debug.Log("Quitting...");
            Application.Quit();
        }

        public void AddScores(BlockVar var)
        {
            scores += (int)(var.value * scoreMultiplier);
            scoreText.text = scores.ToString();

            if (scores > bestScores && !doneCenter) //level won 
            {
                //Invoke("OnLevelComplete", 1.0f);
                center.SetActive(true);
                Invoke("CenterOff", 1.5f);
                doneCenter = true;
                PlayerPrefs.SetInt("LEVEL", PlayerPrefs.GetInt("LEVEL", 1) + 1);
            }

            if (scores >= bestScores) //update level/scores alongside!
            {
                bestText.text = scores.ToString();
                levelText.text = PlayerPrefs.GetInt("LEVEL",1).ToString();
            }
        }

        public void ToggleSfx()
        {
            SoundOn = !SoundOn;
            ui.iconSound.sprite = SoundOn ? ui.onSpr : ui.noSpr;
            PlayerPrefs.SetInt("SFX", SoundOn ? 1 : 0);

            AudioManager.instance.Play(AssetsManager.Instance.ui);
            AudioManager.instance.Toggle(SoundOn);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetInt("SFX", SoundOn ? 1 : 0);

            if (scores > bestScores)
            {
                PlayerPrefs.SetInt("BestScores", scores);
            }
        }

        public void NoAds() 
        {
            Debug.Log("no ads!");
            AudioManager.instance.Play(AssetsManager.Instance.ui);
        }

        public void OnLevelComplete() 
        {
            winUI.SetActive(true);
            AudioManager.instance.Play(AssetsManager.Instance.win);
        }

        public void OnLevelNotComplete() 
        {
            Invoke("Lose", 1.0f);
        }

        private void CenterOff() 
        {
            center.SetActive(false);
        }

        public void Lose() 
        {
            loseUI.SetActive(true);
            AudioManager.instance.Play(AssetsManager.Instance.lose);
        }

        public void OkComplete() 
        {
            PlayerPrefs.SetInt("LEVEL", PlayerPrefs.GetInt("LEVEL", 1) + 1);
            PlayerPrefs.SetInt("BestScores", scores);
            SceneManager.LoadScene("TITLE");
            AudioManager.instance.Play(AssetsManager.Instance.ui);
        }

        public void OkLose() 
        {
            blockManager.Restart(true);
            loseUI.SetActive(false);
            AudioManager.instance.Play(AssetsManager.Instance.ui);
        }
    }
}