#region  HeaderFiles
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion

namespace Com.RamboGameStudio.CarromBoard
{
    public class Triggggger : MonoBehaviour
    {
        public static Triggggger Instance { get; set; }
        #region Fields

        #region Vectors 
        [SerializeField] private Vector2 offset;
        Vector2 startPosition;
        Vector2 touchPoint;
        #endregion
        #region  2D_Objects
        CircleCollider2D stikerCircleCollider2D;
        Rigidbody2D strikerRigidBody2D;
        #endregion
        #region  Images
        Image striker;
        [SerializeField] Transform[] clampPosition;
        #endregion

        #region color
        [SerializeField] private Color newColor;
        #endregion
        #region bool
        bool isStay2D;



        #endregion
        #region Int
        [SerializeField] int factor;
        #endregion
        #region  GameObject
        [SerializeField] private GameObject[] P1_P2;
        [SerializeField] GameObject two_Players;
        [SerializeField] GameObject four_Players;

        #endregion
        #endregion

        #region  Property
        public bool IsStay2D
        {
            get
            {
                return isStay2D;
            }

            set
            {
                isStay2D = value;
            }
        }
        public Rigidbody2D StrikerRigidbody2D { get => strikerRigidBody2D; set => strikerRigidBody2D = value; }
        //Manages the striker drag system.
        public static bool DragStrikerManager { get; set; } = true;
        //Property of InitialStriker
        public Vector2 InitialStrikerPosition { get => startPosition; set => startPosition = value; }
        #endregion


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {

            Instance = this;
        }



        // Start is called before the first frame update
        void Start()
        {

            StrikerRigidbody2D = GetComponent<Rigidbody2D>();
            stikerCircleCollider2D = GetComponent<CircleCollider2D>();
            striker = GetComponent<Image>();
            // transform.position= clampPosition[2].position;
            InitialStrikerPosition = this.transform.position;

        }

        /// <summary>
        /// This function is called if the EventTrigger drag is occuring.
        /// </summary>

        public void DragStriker()
        {
            if (DragStrikerManager)
            {
                for (int i = 0; i < Input.touchCount; ++i)
                {
                    touchPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    if (two_Players.activeInHierarchy)
                    {
                        transform.position = clampX();
                    }
                    else if (four_Players.activeInHierarchy)
                    {
                        if (P1_P2[0].activeInHierarchy && P1_P2[0] != null || P1_P2[1].activeInHierarchy && P1_P2[1] != null)
                        {
                            transform.position = clampX();
                            //transform.position = new Vector2(Mathf.Clamp(touchPoint.x, clampPosition[0].position.x, clampPosition[1].position.x), InitialStrikerPosition.y);
                        }

                        else if (P1_P2[2].activeInHierarchy && P1_P2[2] != null || P1_P2[3].activeInHierarchy && P1_P2[3] != null)
                        {
                            transform.position = new Vector2(InitialStrikerPosition.x, Mathf.Clamp(touchPoint.y, clampPosition[0].position.y, clampPosition[1].position.y));
                        }
                    }
                    Vector2 clampX()
                    {
                        return new Vector2(Mathf.Clamp(touchPoint.x, clampPosition[0].position.x, clampPosition[1].position.x), InitialStrikerPosition.y);

                    }


                }
            }


        }
        #region Commented-

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this
        /// object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Hole")// || other.tag == "BoxCollider")
            {

                striker.color = newColor;
            }

        }

        /// <summary>
        /// Sent when Stiker's touchEnded occurs.You understand right XD 
        /// </summary>
        /// <param name="data">The data PointerEventData involved in this PointerUp.</param>

        public void FlaseTrigger()
        {
            stikerCircleCollider2D.isTrigger = false;
            isStay2D = true;
        }



        /// <summary>
        /// Sent each frame where another object is within a trigger collider
        /// attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        public void OnTriggerStay2D(Collider2D other)
        {



            if (other.tag == "Hole" || other.tag == "BoxCollider")
                return;

            else if (other.tag != "Hole" || other.tag != "BoxCollider")
            {
                other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                StrikerPusher_EnableDisable(false);
                striker.color = newColor;
                Vector2 dir = (Vector2)other.transform.position - (Vector2)transform.position;
                if (two_Players.activeInHierarchy)
                    NewStrikerPusher1Work();
                else if (four_Players.activeInHierarchy)
                {
                    if (NewStrikerPusher1.instance.gameObject.activeInHierarchy)
                        NewStrikerPusher1Work();
                    else if (NewStrikerPusher2.instance.gameObject.activeInHierarchy)
                    {
                        StrikerRigidbody2D.AddForce(new Vector2(-dir.x, -dir.normalized.y) * factor * Time.deltaTime);
                        StrikerRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                    }

                }
                void NewStrikerPusher1Work()
                {
                    StrikerRigidbody2D.AddForce(new Vector2(-dir.normalized.x, -dir.y) * factor * Time.deltaTime);
                    StrikerRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
                }




                // if (IsStay2D)
                {
                    //stikerCircleCollider2D.isTrigger = false;



                }
            }
        }


        /// <summary>
        /// Sent when another object leaves a trigger collider attached to
        /// this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        void OnTriggerExit2D(Collider2D other)
        {

            if (other.tag == "Hole" || other.tag == "BoxCollider")
                return;

            else if (other.tag != "Hole" || other.tag != "BoxCollider")
            {

                StrikerPusher_EnableDisable(true);
                other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                StrikerRigidbody2D.velocity = new Vector2(0, 0);
                ColorRotation();
            }





        }
        /// <summary>
        /// on and off the NewStrikerPusher script.
        /// </summary>
        /// <param name="onOff">onOff manipulate the NewStrikerPusher script.</param>
        void StrikerPusher_EnableDisable(bool onOff)
        {
            if (two_Players.activeInHierarchy)
            {
                if (P1_P2[0].activeInHierarchy)
                    NewStrikerPusher1.instance.enabled = onOff;
                else if (P1_P2[1].activeInHierarchy)
                    NewStrikerPusher2.instance.enabled = onOff;
            }
            else if (four_Players.activeInHierarchy)
            {
                if (P1_P2[0].activeInHierarchy || P1_P2[1].activeInHierarchy)
                    NewStrikerPusher1.instance.enabled = onOff;
                else if (P1_P2[2].activeInHierarchy || P1_P2[3].activeInHierarchy)
                    NewStrikerPusher2.instance.enabled = onOff;
            }

        }

        void ColorRotation()
        {
            striker.color = Color.white;
            StrikerRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
        #endregion

    }


}

