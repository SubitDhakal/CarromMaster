using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikerController : MonoBehaviour
{
    #region variables
    float dist;
   public float storeSpeed;
    
    bool enabledLineRenderer = false;
    bool locked = true;
    public  bool p1_turn=false;
    Rigidbody2D rb;
    RectTransform _rectTransform;

    [SerializeField] float speed;

    [Header("Vector2")]
    Vector2 startingPoint;
    Vector2 direction;
    Vector2 end;
    
    [Header("Slider")]
    [SerializeField] Slider striker;


    [Header ("LineRenderer")]
    [SerializeField] LineRenderer line;
    [SerializeField] LineRenderer DownwardLineRenderer;
    LineRenderer launchingLine;

    [Header("GameObject")]
    [SerializeField] GameObject Striker;
    public GameObject P1_Striker;
    public GameObject P2_Striker;
    StrikerPusher push;
    #endregion

    public float timer, intervals = 2f;


    //Player turn's timer 
    public Image playerTurnTimer;
   
    // Image use for toggle
    [Tooltip("This is the array of 2 color which is use for toggling when the timer is less than 3 sec")]
    public Color[] TogglingColor;

    public float rotationSpeed = 0.2f;
    public float togglingSpeed = 5;


   


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _rectTransform = GetComponent<RectTransform>();
        launchingLine = line.GetComponent<LineRenderer>();
        push = GetComponent<StrikerPusher>();
    }

    
    //Update is called once per frame
    void Update()
    {
        //Debug.Log(FindObjectOfType<DiskCollected>().playAgain());
       
        storeSpeed = rb.velocity.magnitude;
        if (storeSpeed == 0 )
        {
            StartCoroutine(P1_StrikerTurn());
            
           // P1_StrikerTurn();
             //Invoke("P1_StrikerTurn", 2f);
            //StoreValue.instance.stopBlack = true;
            playerTurnTimer.fillAmount -= rotationSpeed*Time.deltaTime;
            if(playerTurnTimer.fillAmount<=0.3f)
            {
                //This helps to toggle the between the two color and notices the player that their turn timer is getting over.
               playerTurnTimer.color = Color.Lerp(TogglingColor[0], TogglingColor[1], Mathf.PingPong(Time.time,0.5f));
              //if the fill amount of image is <= 0 then the turn will be assign as per the boolean condition
                if (playerTurnTimer.fillAmount <= 0)
                {
                    //if p1 is false then turn will given to p1 
                    if (!p1_turn)
                    {
                        P1_StrikerTurn();
                        
                    }
                      
                    // else if p1 is true then turn will given to p2
                    else if (p1_turn)
                        P2_StrikerTurn();
                }
                    

            }
            GetComponent<CircleCollider2D>().isTrigger = true;
            ArrowHeadController();
          
        }
        else if (storeSpeed <= 0.3f  && storeSpeed != 0 )//storeSpeed != 0)
        {
           
            StartCoroutine(P2_StrikerTurn());
            ResetStriker();
           // StoreValue.instance.stopWhite = true;
        }
    }
    
    public   IEnumerator P1_StrikerTurn()
    {
        yield return new WaitForSecondsRealtime(2);
        //change the player's turn
        if (!p1_turn)// if(!StoreValue.instance.controller)      
        {    
             ResetStriker();
            P2_Striker.SetActive(false);
            P1_Striker.SetActive(true);
          
          
            p1_turn = true;
            playerTurnTimer.fillAmount = 1;
        }
    }

    public IEnumerator P2_StrikerTurn()
    {

        yield return new WaitForSecondsRealtime(2);
       
        //change the player's turn
        if (p1_turn) //if(StoreValue.instance.controller)      //
        {
             ResetStriker();
            // iTween.FadeTo(P1_Striker.gameObject,0,1);//iTween.Hash("alpha",0,"delay",2f)
            P1_Striker.SetActive(false);
            P2_Striker.SetActive(true);

            // Invoke("ResetStriker", 0.5f);
            //p1_turn = false;
          
            playerTurnTimer.fillAmount = 1;
            if (FindObjectOfType<StoreValue>().storeBool)
            {
                p1_turn = true;
                FindObjectOfType<StoreValue>().storeBool = false;
            }

          
            p1_turn = false;



        }
       
    }
    // Touch Phase
    private void ArrowHeadController()
    {

        if (Input.touchCount >0)
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
                        launchingLine.enabled = true;
                        Vector2 dir = startingPoint - (Vector2)transform.position;
                        dist = Mathf.Clamp(Vector2.Distance(transform.position, startingPoint), 0, .8f);
                        end = (Vector2)transform.position + (dir.normalized * dist * -1);

                        var downwardEnd = (Vector2)transform.position + (dir.normalized * dist);
                        line.SetPosition(0, transform.position);
                        line.SetPosition(1, end);

                        DownwardLineRenderer.enabled = true;
                        DownwardLineRenderer.SetPosition(0, transform.position);
                        DownwardLineRenderer.SetPosition(1,downwardEnd);
                        #endregion
                    }
                    break;
                    
                case TouchPhase.Ended:
                    if(touch.tapCount ==2)
                    {
                        if(locked)
                        {
                           
                            enabledLineRenderer = true;
                            push.enabled = false;
                            Striker.GetComponent<Slider>().enabled = false;
                            GetComponent<Image>().color = Color.green;
                            locked = false;
                        }
                        
                        else  if (!locked)
                        {
                            //transform.parent.parent.GetComponent<CheckOnLine>().enabled = true;
                            enabledLineRenderer = false;
                            push.enabled = true;
                            Striker.GetComponent<Slider>().enabled = true;
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
                    launchingLine.enabled = false;
                    break;
            }

        }
    }
     
    void LaunchStriker()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        direction = (Vector2)(startingPoint - (Vector2)transform.position);
        direction.Normalize();
        rb.AddForce(-direction * speed * dist, ForceMode2D.Impulse);
      
    }
    
    //Divert the striker in angle after the collision
    public  void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.GetComponent<BoxCollider2D>())
        {
            direction = Vector2.Reflect(direction, collision.contacts[0].normal);
            rb.velocity = -direction * storeSpeed;
        }


    }
    //Reset the striker
    void ResetStriker()
    {
   
        locked = true;
        dist = 0;
       
        enabledLineRenderer = false;
        rb.velocity = Vector2.zero;
     
        transform.position = Vector2.zero;
        _rectTransform.anchoredPosition = transform.position;
        striker.GetComponent<Slider>().value = 0.5f;
        Striker.GetComponent<Slider>().enabled = true;
     
        GetComponent<Image>().color = Color.white;
        push.enabled = true;
    }
}