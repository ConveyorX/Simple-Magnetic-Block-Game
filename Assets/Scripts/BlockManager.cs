using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace ConveyorX.MagneticBricks.BlockManagement
{
    [DefaultExecutionOrder(1)]
    public class BlockManager : MonoBehaviour
    {
        public Transform blockGrid;
        [Range(1, 25)] public int blocksToFill = 10;
        [Range(1, 100)] public int chanceToFillPercent;
        public Block displayBlock;

        public bool HasLost()
        {
            int filled = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].isFilled) 
                {
                    filled++;
                }
            }

            //Debug.Log(filled);

            if (filled >= 25)
                return true;
            else
                return false;
        }

        GameManager gameManager;
        GridLayoutGroup gridLayoutGroup;
        private Block[] blocks;

        private void Start()
        {
            gridLayoutGroup = blockGrid.GetComponent<GridLayoutGroup>();
            blocks = blockGrid.GetComponentsInChildren<Block>();
            gameManager = GetComponent<GameManager>();
            
            //start randomly
            displayBlock.blockManager = this;
            displayBlock.Fill(AssetsManager.Instance.blockVars[0]); //start from 1 always!

            //randomize level
            int done = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].blockManager = this;
                blocks[i].index = i + 1;
                blocks[i].indexY = (int)(i / 5);

                if (done < blocksToFill)
                {
                    if (Random.Range(1, 101) <= chanceToFillPercent) //50/50 to fill current block 
                    {
                        int m = Random.Range(1, 5);
                        blocks[i].Fill(AssetsManager.Instance.blockVars[m - 1]);
                        done += 1;
                    }
                    else
                        blocks[i].Clear();
                }
                else
                {
                    //do not fill current block!
                    blocks[i].Clear();
                }
            }
        }

        public void Restart(bool resetEverything = false) 
        {
            if (resetEverything == true) 
            {
                gameManager.scores = 0;
                gameManager.scoreText.text = "0";
                RefreshNewBlock();
                gameManager.bestText.text = "" + PlayerPrefs.GetInt("BestScores", 0);
                gameManager.refreshLeft = 5;
                gameManager.ui.redoButton.interactable = true;
                gameManager.IsHammerActive = false;
            }

            int done = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                if (done < blocksToFill)
                {
                    if (Random.Range(1, 101) <= chanceToFillPercent) //50/50 to fill current block 
                    {
                        int m = Random.Range(1, 5);
                        blocks[i].Fill(AssetsManager.Instance.blockVars[m - 1]);
                        done += 1;
                    }
                    else
                        blocks[i].Clear();
                }
                else
                {
                    //do not fill current block!
                    blocks[i].Clear();
                }
            }
        }

        public void RefreshNewBlock()
        {
            StartCoroutine(Reload());
        }

        private IEnumerator Reload() 
        {
            bool done = false;
            while (!done) 
            {
                int i = Random.Range(1, 8);
                if (i != displayBlock.GetValue())
                {
                    displayBlock.Fill(AssetsManager.Instance.blockVars[i - 1]);
                    done = true;
                }
                yield return null;
            }
        }

        Block var;
        public void Fill(Block block)
        {
            AudioManager.instance.Play(AssetsManager.Instance.block);
            var = block;
            Invoke("TryFillBlock", 0.2f);
        }

        public void TryFillBlock() 
        {
            if(var == null) return;

            var.Fill(displayBlock.myBlockVar);
            gameManager.AddScores(displayBlock.myBlockVar);
            CalcuateNewTile(var);
            RefreshNewBlock();
        }

        public void CalcuateNewTile(Block current)
        {
            if (gridLayoutGroup.enabled == true) gridLayoutGroup.enabled = false;

            //check up, down, left & right
            Block up = null, down = null, left = null, right = null;

            //check the index doesn't get out of grid!
            if (current.index - 6 > -1)
                up = blocks[current.index - 6];

            if (current.index + 4 < 25)
            {
                down = blocks[current.index + 4];
            }
            //remain on same row regardless of column!
            if (current.index / 5 == current.indexY)
                right = blocks[current.index];

            if ((current.index - 2) / 5 == current.indexY && current.index - 2 > -1)
                left = blocks[current.index - 2];

            BlockVar var = new BlockVar();
            var.value = current.GetValue() + 1;

            //clamp the value for index!
            if (var.value > 9)
                var.value = 9;

            var.colorValue = AssetsManager.Instance.GetColorVal(var.value);
            //Debug.Log(down.name + " - " + down.index);

            //check if value is equal!
            int last = current.GetValue();
            current.transform.SetAsLastSibling();

            //merge blocks and play animations!
            if (up && up.GetValue() == last)
            {
                up.animator.SetTrigger("DOWN");
                StartCoroutine(FillNew(var, current));
            }
            if (down && down.GetValue() == last)
            {
                down.animator.SetTrigger("UP");
                StartCoroutine(FillNew(var, current));
            }
            if (right && right.GetValue() == last)
            {
                right.animator.SetTrigger("LEFT");
                StartCoroutine(FillNew(var, current));
            }
            if (left && left.GetValue() == last)
            {
                left.animator.SetTrigger("RIGHT");
                StartCoroutine(FillNew(var, current));
            }

            Invoke("TryGameOver", 0.5f);
        }

        private void TryGameOver() 
        {
            if (HasLost() == true) 
            {
                gameManager.OnLevelNotComplete();
            }
        }

        private IEnumerator FillNew(BlockVar var, Block current) 
        {
            yield return new WaitForSeconds(0.7f);
            current.Fill(var);
            AudioManager.instance.Play(AssetsManager.Instance.ui);
        }

        #region PowerUps

        public void Rainbow() 
        {
            for (int i = 0; i < blocks.Length; i++) 
            {
                if (blocks[i].GetValue() == displayBlock.GetValue()) 
                {
                    blocks[i].animator.SetTrigger("CLEAR");
                }
            }
        }

        #endregion
    }
}