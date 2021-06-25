using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Com.RamboGameStudio.CarromBoard;


public class DiskCollected : MonoBehaviour
{
    #region varirables
    Text tmB;
    Text tmW;
    public GameObject queenInfo;

    [SerializeField] GameObject BlackCoin;
    [SerializeField] GameObject WhiteCoin;
    [SerializeField] GameObject CircleCollider;
    [SerializeField] GameObject targetPositionBlack;
    [SerializeField] GameObject targetPositionWhite;
    public static GameObject turnActivatorStriker;
    [SerializeField] GameObject P2_Striker;

    public int speed = 30;
    StoreValue store;
    public int numB = 0;
    public int numW = 0;
    int tempWhiteIncrement;
    GameObject whiteCoin;
    GameObject blackCoin;

    StrikerController SC;
    int temp = 0;
    bool isWhiteTriggeredCircle = false;
    bool isBlackTriggeredCircle = false;
    public bool playAgainP1;
    public bool save;
    public static bool stopWhite;
    public static bool stopBlack;
    int tempBlackIncrement;

    // private field
    #endregion
    private static bool isQueenCollected;
    private static bool blackStrikerHoled;






    ///<summary>
    /// Property of isQueenCollected is made to Encapsulate the data of isQueenCollected.
    ///</summary>
    public bool IsQueenCollected
    {
        get { return isQueenCollected; }
        set
        {
            isQueenCollected = value;
        }
    }
    public static DiskCollected Instance { get; set; }
    public static bool BlackStrikerHoled { get => blackStrikerHoled; set => blackStrikerHoled = value; }
    public static bool WhiteStrikerHoled { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SC = FindObjectOfType<StrikerController>();
        tmB = BlackCoin.GetComponent<Text>();
        tmW = WhiteCoin.GetComponent<Text>();
        store = CircleCollider.GetComponent<StoreValue>();

    }

    private void Update()
    {
        // Debug.Log(strikerHoled);
    }

    public void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.GetComponent<DivertDiskAngle>())
        {
            //if (col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude <= 2f)

            if (col.gameObject.tag == "WhiteCoin")//&& SC.P2_Striker.activeSelf
            {


                whiteCoin = col.gameObject;
                Coin();
                //This is the instance that store the collected white coin in a list.
                StoreValue.instance.CoinCollected(whiteCoin);

                //Count the number of disk collected in the list and assign it into tmB.text
                tmW.text = StoreValue.instance.whiteDiskCollected.Count.ToString();
                // StoreValue.instance.stopWhite = true;
                stopWhite = true;
                // #Important donot delete this below code its very important. It helps in striker turn mechanism

                if (turnActivatorStriker.transform.parent.tag == "Team_2")
                    StoreValue.instance.strikerController = false;
                //This moves the collected white coin toward target position
                //  iTween.MoveTo(whiteCoin.gameObject, iTween.Hash("position", targetPositionWhite.transform.position, "delay", 1f, "time", 3f));

                StartCoroutine(MoveWhiteCoin());

                if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                    if (IsQueenCollected)
                        IsQueenCollected = false;



            }
            else if (col.gameObject.tag == "BlackCoin")//&& SC.P1_Striker.activeSelf
            {
                // Debug.Log("At BlackCoin " + StrikerHoled);
                Debug.Log(col.gameObject.name);
                //Assign collided gameobject to blackcoin gameobject
                blackCoin = col.gameObject;
                //Call the function Coin().
                Coin();
                //This is the instance that store the collected black coin in a list.
                if (!blackStrikerHoled)
                {
                    StoreValue.instance.CoinCollected(blackCoin);
                    //Count the number of disk collected in the list and assign it into tmB.text
                    tmB.text = StoreValue.instance.blackDiskCollected.Count.ToString();
                }










                // #Important: This boolean gives  turn again to black disk if black coin is collected.
                // SC.p1_turn = false;

                //This iTween moves the collected black coin toward target position


                StartCoroutine(MoveCoin());
                // Move(blackCoin, targetPositionBlack.transform);

                // Invoke("Move(blackCoin, targetPositionBlack.transform)",2f);
                if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
                    if (IsQueenCollected)
                        IsQueenCollected = false;






                // #Important donot delete this below code its very important. It helps in striker turn mechanism
                if (turnActivatorStriker.transform.parent.tag == "Team_1")
                    StoreValue.instance.strikerController = true;

                stopBlack = true;

            }

            //Queen Manager
            else if (col.gameObject.tag == "Queen")
            {
                if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)//P1_Striker.gameObject.activeInHierarchy
                {
                    QueenCommon(targetPositionBlack.transform);
                    // Assigning false leads to activate the P2 Striker true because see at NewStrikerPusher2's Reset().
                    StoreValue.instance.strikerController = true;
                }
                else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                {
                    QueenCommon(targetPositionWhite.transform);

                    // Assigning false leads to activate the P2 Striker true because see at NewStrikerPusher2's Reset().
                    StoreValue.instance.strikerController = false;

                }
                void QueenCommon(Transform trns)
                {

                    IsQueenCollected = true;

                    // IsQueenCollected(true);

                    //Make the Queen remain in the hole.
                    Coin();
                    //Move the Queen head toward targeted position
                    Move(col.gameObject, trns.transform);
                    //Activate true the Text Component game object
                    iTween.ScaleTo(queenInfo.gameObject, iTween.Hash("x", 1));
                    queenInfo.SetActive(true);
                    //Hide the Text game object after 5 seconds.
                    iTween.ScaleTo(queenInfo.gameObject, iTween.Hash("x", 0, "time", 1f, "delay", 5f));
                }

                /* if(!playAgainP1)
                 //else
                {
                    Debug.Log("Locked");
                   // Debug.Log(P2_Striker.activeSelf);
                    playAgainP1 = true;
                    queenInfo.SetActive(true);
                    Coin();
                    Move(col.gameObject, targetPositionWhite.transform);
                   //Give turn again to white striker.

                }

                else if (P1_Striker.gameObject.activeSelf)
                {
                    Debug.Log(P1_Striker.activeSelf);
                    queenInfo.SetActive(true);
                    Coin();
                    //Queen will travel to BlackTarget position
                    Move(col.gameObject, targetPositionBlack.transform);
                    //Give turn again to black striker.
                    SC.p1_turn = false;
                }*/

                // Debug.Log("Queenasyo");
                // col.gameObject.SetActive(false);
            }



        }
        else//if (col.gameObject.tag == "Striker")
        {
            col.gameObject.transform.position = transform.position;
            // col.gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 0.8f); */
            if (col.gameObject.transform.parent.tag == "Team_1")
            {
                Debug.Log(col.gameObject.name);



                // Debug.Log(StrikerHoled);
                // Coin();
                //Checks the number of black disk collected is greater than zero or not.
                if (StoreValue.instance.blackDiskCollected.Count > 0)
                {
                    if (StoreValue.instance.blackDiskCollected.Count == 1)
                        BlackStrikerHoled = true;
                    // Check the boolean is true and call Store()
                    if (stopBlack)
                    {
                        StoreValue.instance.BlackCoin_Move_Remove();

                        stopBlack = false;
                    }
                    //count the disk collected in the list and assign into text.
                    tmB.text = StoreValue.instance.blackDiskCollected.Count.ToString();
                }
            }
            else if (col.gameObject.transform.parent.tag == "Team_2")
            {
                //Coin();
                //Checks the number of white disk collected is greater than zero or not.
                if (StoreValue.instance.whiteDiskCollected.Count > 0)
                {
                    if (StoreValue.instance.whiteDiskCollected.Count == 1)
                        WhiteStrikerHoled = true;
                    //Check the boolean is true and call Store()
                    if (stopWhite)
                    {
                        StoreValue.instance.WhiteCoin_Move_Remove();
                        stopWhite = false;
                    }

                    //count the disk collected in the list and assign into text.
                    tmW.text = StoreValue.instance.whiteDiskCollected.Count.ToString();
                }


            }
        }
        void Coin()
        {

            //  col.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; ;
            col.gameObject.GetComponent<CircleCollider2D>().enabled = false; ;

            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            col.gameObject.transform.position = transform.position;
        }
        IEnumerator MoveCoin()
        {
            yield return new WaitForSecondsRealtime(1f);
            if (!BlackStrikerHoled || BlackStrikerHoled && StoreValue.instance.blackDiskCollected.Count == 1)
                Move(blackCoin, targetPositionBlack.transform);
            //else if (BlackStrikerHoled && StoreValue.instance.blackDiskCollected.Count == 1)






        }
        IEnumerator MoveWhiteCoin()
        {
            yield return new WaitForSecondsRealtime(1f);
            if (!WhiteStrikerHoled || WhiteStrikerHoled && StoreValue.instance.whiteDiskCollected.Count == 1)

                Move(whiteCoin, targetPositionWhite.transform);
        }



        void Move(GameObject passenger, Transform targetPosition)
        {
            iTween.MoveTo(passenger.gameObject, iTween.Hash("position", targetPosition.transform.position, "delay", 1f, "time", 3f));
        }

    }
}


