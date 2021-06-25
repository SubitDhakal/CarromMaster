using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.RamboGameStudio.CarromBoard
{
    public class NewStrikerPusher1 : Controller
    {
        public static NewStrikerPusher1 instance { get; set; }

        #region Variables

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

        [SerializeField] LineRenderer line;

        [Tooltip("Down LineRenderer")]
        [SerializeField] public LineRenderer DownwardLineRenderer;

        public LineRenderer launchingLine;
        #endregion

        #region RigidBody2D
        public static Rigidbody2D rb;
        #endregion

        #region RectTransform
        // RectTransform _rectTransform;

        #endregion

        #endregion

        public Vector2 InitialStrikerPosition { get; set; }



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
        private void OnEnable()
        {
            instance = this;
            launchingLine = line.GetComponent<LineRenderer>();
            DiskCollected.turnActivatorStriker = this.gameObject;

            rb = GetComponent<Rigidbody2D>();
            DiskCollected.BlackStrikerHoled = false;
            DiskCollected.WhiteStrikerHoled = false;



        }
        //Monobehaviour Update method called on GameObject by Unity in every frame.
        void Update()
        {


            storeSpeed = rb.velocity.magnitude;

            if (storeSpeed == 0 && StoreValue.instance.strikerController == false)
            {


                Triggggger.Instance.enabled = true;
                NewStrikerPusher1.instance.enabled = true;
                GetComponent<CircleCollider2D>().isTrigger = true;


                if (arrowHeadController)
                {
                    ArrowHeadController(line, DownwardLineRenderer);
                }

                if (reset)
                {
                    PlayerTurnTimer(playerTurnTimer, rotationSpeed);

                }
            }
            else if (storeSpeed > 0.3)
            {
                this.GetComponent<CircleCollider2D>().isTrigger = false;

            }

            else if (storeSpeed <= 0.3 && storeSpeed != 0 && this.GetComponent<CircleCollider2D>().isTrigger == false)
            {
                reset = false;
                dist = 0;
                rb.velocity = Vector2.zero;
                arrowHeadController = false;
                // wait for 2 second to reset striker.
                StartCoroutine(ResetStriker());


            }
        }
    }
}