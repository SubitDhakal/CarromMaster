using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.RamboGameStudio.CarromBoard
{



    public class Controller : MonoBehaviour
    {
        public static Controller Instance;
        #region variables

        #region Images
        [Header("Image")]
        //Player turn's timer 
        //[SerializeField] private Image playerTurnTimer;
        [SerializeField] private Image[] StrikerTimer;
        #endregion

        #region Colors
        [Header("Color")]
        // Image use for toggle
        [Tooltip("This is the array of 2 color which is use for toggling when the timer is less than 3 sec")]
        public Color[] TogglingColor;
        #endregion

        #region Floats
        [Space(10, order = 0)]
        [Header("Float variables", order = 1)]

        /*[Tooltip("rotationSpeed used for playerTurnTimer.")]
        [Range(0, 0.5f)]
        [SerializeField] private float rotationSpeed = 0.2f;*/

        [Tooltip("togglingSpeed used for playerTurnTimer.")]
        [Range(1, 10f)]
        [SerializeField] private float togglingSpeed = 5;
        //dist for lineRenderer length.
        public static float dist;
        //storeSpeed stores the speed of the stiker.
        public static float storeSpeed;
        //Impulse speed while launching striker.
        [Tooltip("Impulse speed while launching striker.")]
        public float speed;
        #endregion

        #region Booleans
        //Booleans
        [Header("Boolean")]
        [Tooltip("redo the task of striker from beginning")]
        public static bool reset = true;

        //For lineRenderer controller
        bool enabledLineRenderer = false;

        //Locking unlocking the striker
        public bool locked = true;
        //Control the ArrowHeadFunc
        public static bool arrowHeadController = true;
        public static bool stopTimerP2 = true;
        #endregion

        #region Vectors
        //Vectors
        Vector2 startingPoint;
        Vector2 direction;
        Vector2 end;
        #endregion

        /* #region Line Renderers
         [Header("LineRenderer")]
         [Tooltip("Upper LineRenderer")]

         [SerializeField] LineRenderer line;

         [Tooltip("Down LineRenderer")]
         [SerializeField] LineRenderer DownwardLineRenderer;

         LineRenderer launchingLine;
         #endregion*/

        #region GameObjects
        [Header("GameObject")]
        public List<GameObject> strikers = new List<GameObject>();
        [SerializeField] GameObject queen;
        [Tooltip("This active the two_Players or four_Players game as per set string value")]
        [SerializeField] GameObject two_Players;
        [SerializeField] GameObject four_Players;

        #endregion

        #region RigidBody2D
        Rigidbody2D rb;
        #endregion

        #region RectTransform
        // RectTransform _rectTransform;
        [SerializeField] Transform center;
        #endregion

        #endregion


        ///<summary>
        /// Monobehaviour Awake method called on GameObject by unity during early initialization phase.
        ///</summary>

        NewStrikerPusher1 x;
        void Awake()
        {
            Instance = this;
            x = NewStrikerPusher1.instance;
            if (PlayerPrefs.HasKey(LoadScene.key))
            {
                string name = PlayerPrefs.GetString(LoadScene.key, "Subit");
                if (name == "2_Players")
                {
                    two_Players.SetActive(true);
                }
                else if (name == "4_Players")
                {
                    four_Players.SetActive(true);
                }


            }


        }




        ///<summary>
        ///ArrowHeadController() handles all the touch event.
        ///</summary>
        protected void ArrowHeadController(LineRenderer line, LineRenderer DownwardLineRenderer)
        {

            if (Input.touchCount > 0)
            {

                startingPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.Raycast(startingPoint, Vector2.zero);

                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        break;
                    case TouchPhase.Moved:

                        if (enabledLineRenderer)
                        {

                            #region LineRenderer Length
                            line.enabled = true;
                            Vector2 dir = startingPoint - (Vector2)transform.position;
                            dist = Mathf.Clamp(Vector2.Distance(transform.position, startingPoint), 0, .8f);
                            end = (Vector2)transform.position + (dir.normalized * dist * -1);

                            var downwardEnd = (Vector2)transform.position + (dir.normalized * dist);
                            line.SetPosition(0, transform.position);
                            line.SetPosition(1, end);

                            DownwardLineRenderer.enabled = true;
                            DownwardLineRenderer.SetPosition(0, transform.position);
                            DownwardLineRenderer.SetPosition(1, downwardEnd);
                            #endregion
                        }
                        break;

                    case TouchPhase.Ended:
                        //lock and unlock the striker depend on boolean value
                        if (touch.tapCount == 2)
                        {

                            if (locked)
                            {



                                enabledLineRenderer = true;

                                if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
                                {
                                    // #Critical stopBlack bool is active true for only move the black coin from target position to center.
                                    DiskCollected.stopBlack = true;
                                }

                                else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                                {
                                    DiskCollected.stopWhite = true;
                                }
                                Triggggger.DragStrikerManager = false;
                                Triggggger.Instance.StrikerRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                                GetComponent<Image>().color = Color.green;
                                locked = false;
                            }

                            else if (!locked)
                            {
                                enabledLineRenderer = false;
                                if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
                                {



                                }
                                else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                                {

                                }

                                Triggggger.DragStrikerManager = true;
                                GetComponent<Image>().color = Color.white;
                                locked = true;
                            }
                        }

                        if (dist >= .2f)
                        {
                            GetComponent<CircleCollider2D>().isTrigger = false;
                            LaunchStriker();

                        }

                        DownwardLineRenderer.enabled = false;
                        line.enabled = false;
                        break;
                }

            }
        }
        ///<summary>
        //LauchStriker() launch the striker. Called when the dist >= .2f 
        ///</summary> 
        void LaunchStriker()
        {


            if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
            {

                Common(NewStrikerPusher1.instance.gameObject);


            }



            else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
            {

                Common(NewStrikerPusher2.instance.gameObject);
                //                Debug.Log(NewStrikerPusher2.instance.gameObject.transform.parent.name);


            }

            void Common(GameObject striker)
            {
                striker.GetComponent<CircleCollider2D>().isTrigger = false;
                direction = (Vector2)(startingPoint - (Vector2)striker.transform.position);
                direction.Normalize();
                striker.GetComponent<Rigidbody2D>().AddForce(-direction * speed * dist, ForceMode2D.Impulse);
                Triggggger.Instance.enabled = false;
            }


        }
        //Reset the striker
        protected IEnumerator ResetStriker()
        {
            yield return new WaitForSecondsRealtime(1);
            if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
            {
                //                Debug.Log("Hello");
                //This works as default. Like if boolean is false then condition match and executes the given command.
                if (StoreValue.instance.strikerController == false)
                {
                    StrikerTurn(NewStrikerPusher1.instance.playerTurnTimer, strikers, true, false, true);
                    NewStrikerPusher2.instance.transform.position = NewStrikerPusher2.instance.InitialStrikerPosition;
                }
                //This one is different. This below means, black coin is just collected so we change the boolean condition to get the turn again of P1 striker.
                //You can check on DiskCollected script OnTriggerStay2D() at black coin code.
                else if (StoreValue.instance.strikerController == true)
                {
                    // #Important these two lines of code is essential for timer fillamount when the disk is collected.
                    reset = true;
                    NewStrikerPusher1.instance.playerTurnTimer.fillAmount = 1;
                    NewStrikerPusher1.instance.transform.position = NewStrikerPusher1.instance.InitialStrikerPosition;
                    DiskCollected.BlackStrikerHoled = false;
                    TopCommon(false, true, false);
                }
                MiddleCommon();//strikers[0])
                               // Debug.Log(strikers[0].name);


            }

            else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
            {
                //This works as default. Like if boolean is true then condition match and executes the given command.
                if (StoreValue.instance.strikerController == true)
                {
                    StrikerTurn(NewStrikerPusher2.instance.playerTurnTimer, strikers, false, false, true);
                }
                //This one is different. This below means, white coin is just collected so we change the boolean condition to get the turn again of P2 striker.
                //You can check on DiskCollected script OnTriggerStay2D() at white coin code.
                else if (StoreValue.instance.strikerController == false)
                {
                    // #Important these two lines of code stopTimerP2 & playerTurnTimer.fillAmount is essential for timer fillamount when the disk is collected.
                    stopTimerP2 = true;
                    NewStrikerPusher2.instance.playerTurnTimer.fillAmount = 1;
                    NewStrikerPusher2.instance.transform.position = NewStrikerPusher2.instance.InitialStrikerPosition;
                    DiskCollected.WhiteStrikerHoled = false;
                    //NewStrikerPusher1.instance.transform.position = NewStrikerPusher1.instance.InitialStrikerPosition;
                    TopCommon(true, true, false);

                }

                MiddleCommon();//strikers[1]
                NewStrikerPusher1.instance.transform.position = NewStrikerPusher1.instance.InitialStrikerPosition;
            }

            void TopCommon(bool x, bool y, bool z)
            {

                Triggggger.DragStrikerManager = true;
                StoreValue.instance.strikerController = x;
                strikers[0].gameObject.SetActive(y);
                strikers[1].gameObject.SetActive(z);

            }
            void MiddleCommon()//GameObject striker
            {

                locked = true;
                enabledLineRenderer = false;
                if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
                    NewStrikerPusher1.arrowHeadController = true;
                else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                    NewStrikerPusher2.arrowHeadController = true;
                GetComponent<Image>().color = Color.white;
            }

        }

        #region Public Functions
        //Divert the striker in angle after the collision
        public void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.GetComponent<BoxCollider2D>())
            {
                direction = Vector2.Reflect(direction, collision.contacts[0].normal);

                if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)

                    NewStrikerPusher1.rb.velocity = -direction * NewStrikerPusher1.storeSpeed;
                else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                    NewStrikerPusher2.rb.velocity = -direction * NewStrikerPusher2.storeSpeed;
            }
        }
        public void StrikerTurn(Image StrikerTimer, List<GameObject> strikers, bool x, bool y, bool z)
        {


            //reset the fill amount after getting turn.
            StrikerTimer.fillAmount = 1;
            StoreValue.instance.strikerController = x;
            strikers[0].gameObject.SetActive(y);
            strikers[1].gameObject.SetActive(z);
            GetComponent<Image>().color = Color.white;
            Triggggger.DragStrikerManager = true;
            stopTimerP2 = true;
            reset = true;


            // FindObjectOfType<NewStrikerPusher2>().stopTimerP2 = true;
            if (DiskCollected.Instance.IsQueenCollected)
            {
                queen.GetComponent<CircleCollider2D>().enabled = true;
                iTween.MoveTo(queen, iTween.Hash("position", center.transform.position, "time", 1f));
                DiskCollected.Instance.IsQueenCollected = false;
            }
        }
        public void PlayerTurnTimer(Image playerTurnTimer, float rotationSpeed)
        {
            playerTurnTimer.fillAmount -= rotationSpeed * Time.deltaTime;

            if (playerTurnTimer.fillAmount <= 0.3f)
            {
                //This helps to toggle the between the two color and notices the player that their turn timer is getting over.
                playerTurnTimer.color = Color.Lerp(TogglingColor[1], TogglingColor[0], Mathf.PingPong(Time.time, 0.5f));
                if (playerTurnTimer.fillAmount <= 0)
                {
                    //  Debug.Log(NewStrikerPusher1.instance.gameObject.transform.parent.name);
                    if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
                    {
                        StrikerTurn(StrikerTimer[0], strikers, true, false, true);
                        NewStrikerPusher1.instance.launchingLine.enabled = false;
                        NewStrikerPusher1.instance.DownwardLineRenderer.enabled = false;
                        NewStrikerPusher2.instance.transform.position = NewStrikerPusher2.instance.InitialStrikerPosition;
                    }

                    else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                    {
                        // Debug.Log("Hello");
                        StrikerTurn(StrikerTimer[0], strikers, false, false, true);
                        NewStrikerPusher2.instance.line.enabled = false;
                        NewStrikerPusher2.instance.DownwardLineRenderer.enabled = false;
                        NewStrikerPusher1.instance.transform.position = NewStrikerPusher1.instance.InitialStrikerPosition;
                    }


                    locked = true;
                    enabledLineRenderer = false;


                    dist = 0;





                }
            }
        }
        #endregion
    }
}