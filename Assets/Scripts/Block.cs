using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConveyorX.MagneticBricks.BlockManagement
{
    public class Block : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image myImg;
        public Text myText;
        public bool isDisplayBlock = false;

        public int index { get; set; }
        public int indexY { get; set; }
        public bool isFilled { get; private set; }
        public BlockManager blockManager { get; set; }
        public BlockVar myBlockVar { get; private set; }
        public Animator animator { get; private set; }
        public int GetValue() { return myBlockVar.value; }
        
        private GameManager gameManager;
        private Image image;

        bool isPointerOver;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            animator = GetComponent<Animator>();
            image = GetComponent<Image>();

            myBlockVar = new BlockVar();
            myBlockVar.value = -1;
            myBlockVar.colorValue = Color.gray;
        }

        public void Fill(BlockVar var)
        {
            myBlockVar = var;
            myImg.color = myBlockVar.colorValue;
            myText.text = myBlockVar.value.ToString();
            isFilled = true;
        }

        public void Clear()
        {
            myBlockVar = new BlockVar();
            isFilled = false;
            myImg.color = AssetsManager.Instance.emptyColor;
            myText.text = "";
        }

        public void OnClick()
        {
            if (isDisplayBlock == true) 
                return;
            if (gameManager.IsDragging) 
                return;
            if (gameManager.IsHammerActive == true) //is hammer active? lol! 
            {
                animator.SetTrigger("CLEAR");
                gameManager.IsHammerActive = false;
                return;
            }

            animator.SetTrigger("POP");

            if (!isFilled)
            {
                //fill with current block!
                blockManager.Fill(this);
            }
        }

        private void Update()
        {
            if (isDisplayBlock && gameManager.IsDragging)
            {
                if (Input.GetMouseButtonUp(0) == true && isDisplayBlock)
                {
                    //drop
                    gameManager.dragBlock.position = gameManager.originalPos;
                    gameManager.IsDragging = false;
                    gameManager.dragBlock.GetComponent<Image>().raycastTarget = true;
                    
                    if(gameManager.currentOnBlock != null)
                        gameManager.currentOnBlock.OnClick();
                    return;
                }

                gameManager.dragBlock.position = Input.mousePosition;
            }
        }

        #region POINTER_EVENTS

        public void OnPointerDown(PointerEventData eventData)
        {
            //only make drag from display box!
            if (isDisplayBlock && eventData.button == PointerEventData.InputButton.Left && isPointerOver)
            {
                gameManager.IsDragging = true;
                image.raycastTarget = false;
            }
        }

        //normal clicking!
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isPointerOver && !isDisplayBlock && eventData.button == PointerEventData.InputButton.Left && !gameManager.IsDragging)
            {
                OnClick();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerOver = false;
            gameManager.currentOnBlock = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerOver = true;
            gameManager.currentOnBlock = this;
        }

        #endregion
    }
}
