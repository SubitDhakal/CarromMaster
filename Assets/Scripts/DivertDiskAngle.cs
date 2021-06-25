using UnityEngine;
using UnityEngine.UI;

public class DivertDiskAngle : MonoBehaviour
{
    Rigidbody2D rb;
    CheckOnLine check;
    public bool isTriggered = false;
    float speed = 7f;
    public float preserveTransform;

    RectTransform _rectTransform;



    public GameObject Striker;
    public GameObject WhiteCoin;
    public GameObject BlackCoin;
    public GameObject Queen;





    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<BoxCollider2D>())
        {
            Vector2 dir = col.contacts[0].point - (Vector2)transform.position;
            dir = -dir.normalized;
            rb.AddForce(dir*speed);
          
        }
    }


   /*  private void OnTriggerStay2D(Collider2D col)
    {



        
        if (col.gameObject.tag == "Striker")
        {
            Striker = col.gameObject;
            Striker.GetComponent<Image>().color = new Color(1, 0, 0, 0.8f);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (Striker != null)
        {
            Striker.GetComponent<Image>().color = Color.white;
           // Striker.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }    
            

    } */

}
