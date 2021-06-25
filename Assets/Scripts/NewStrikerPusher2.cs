using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.RamboGameStudio.CarromBoard
{



    public class NewStrikerPusher2 : Controller
    {
        public static NewStrikerPusher2 instance { get; set; }

        #region variables

        #region Images
        [Header("Image")]
        //Player turn's timer 
        public Image playerTurnTimer;

        #endregion

        #region Floats
        [Space(10, order = 0)]
        [Header("Float variables", order = 1)]

        [Tooltip("rotationSpeed used for playerTurnTimer.")]
        [Range(0, 0.5f)]
        [SerializeField] private float rotationSpeed = 0.2f;
        #endregion

        #region Line Renderers
        [Header("LineRenderer")]
        [Tooltip("Upper LineRenderer")]

        [SerializeField]
        public LineRenderer line;

        [Tooltip("Down LineRenderer")]
        [SerializeField]
        public LineRenderer DownwardLineRenderer;

        LineRenderer launchingLine;
        #endregion

        #region RigidBody2D
        public static Rigidbody2D rb;
        #endregion

        #region RectTransform
        //  RectTransform _rectTransform;
        #endregion
        #endregion

        public Vector2 InitialStrikerPosition { get; private set; }




        bool isCollectedWhite;//private field

        //Property of isCollectedWhite to encapsulate the value of isCollected boolean variable.
        public bool IsCollectedWhite
        {
            get
            {
                return isCollectedWhite;
            }
            set
            {
                isCollectedWhite = value;
            }
        }


        ///<summary>
        /// Monobehaviour Awake method called on GameObject by unity during early initialization phase.
        ///</summary>
        void Awake()
        {

            InitialStrikerPosition = transform.position;



        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            instance = this;
            rb = GetComponent<Rigidbody2D>();
            // _rectTransform = GetComponent<RectTransform>();
            launchingLine = line.GetComponent<LineRenderer>();
            DiskCollected.turnActivatorStriker = this.gameObject;
            DiskCollected.WhiteStrikerHoled = false;
        }

        public void Update()
        {
            storeSpeed = rb.velocity.magnitude;

            if (storeSpeed == 0 && StoreValue.instance.strikerController == true)
            {

                //Check whether the isQueenCollected is true.
                GetComponent<CircleCollider2D>().isTrigger = true;
                Triggggger.Instance.enabled = true;
                NewStrikerPusher2.instance.enabled = true;



                if (DiskCollected.Instance.IsQueenCollected)
                    isCollectedWhite = true;



                if (arrowHeadController)
                {
                    ArrowHeadController(line, DownwardLineRenderer);
                }

                if (stopTimerP2)
                {
                    PlayerTurnTimer(playerTurnTimer, rotationSpeed);

                }



            }
            else if (storeSpeed <= 0.3 && storeSpeed != 0 && this.GetComponent<CircleCollider2D>().isTrigger == false)
            {
                dist = 0;
                rb.velocity = Vector2.zero;
                stopTimerP2 = false;
                arrowHeadController = false;

                // wait for 2 second to reset striker.
                StartCoroutine(ResetStriker());


            }





        }





    }
}
