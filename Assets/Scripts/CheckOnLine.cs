using UnityEngine;
using UnityEngine.UI;

public class CheckOnLine : MonoBehaviour
{
   public GameObject Striker;
   public GameObject WhiteCoin;
   public GameObject BlackCoin;
   public GameObject Queen;


    private void Update()
    {
        if(WhiteCoin!=null && WhiteCoin.GetComponent<Rigidbody2D>().velocity.magnitude >0  
            || BlackCoin!=null && BlackCoin.GetComponent<Rigidbody2D>().velocity.magnitude >0
           ||Queen != null && Queen.GetComponent<Rigidbody2D>().velocity.magnitude > 0 )
        {
            ActivateCollision();
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
   
      
        if (col.gameObject.tag == "Striker")
            Striker = col.gameObject;
        
        if (col.gameObject.tag == "WhiteCoin")
            WhiteCoin = col.gameObject;
        
        if (col.gameObject.tag == "BlackCoin")
            BlackCoin = col.gameObject;

        if (col.gameObject.tag == "Queen")
            Queen = col.gameObject;

        ActivateTrigger();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        ActivateCollision();
    }

    void ActivateTrigger()
    {
        if (WhiteCoin != null)
        {
            WhiteCoin.GetComponent<CircleCollider2D>().isTrigger = true;

        }

        if (BlackCoin != null)
            BlackCoin.GetComponent<CircleCollider2D>().isTrigger = true;
        if (Queen != null)
            Queen.GetComponent<CircleCollider2D>().isTrigger = true;
    }

    public void  ActivateCollision()
    {
        if (WhiteCoin != null)
            WhiteCoin.GetComponent<CircleCollider2D>().isTrigger = false;
        if (BlackCoin != null)
            BlackCoin.GetComponent<CircleCollider2D>().isTrigger = false;
        if (Queen != null)
            Queen.GetComponent<CircleCollider2D>().isTrigger = false;
    }
}

