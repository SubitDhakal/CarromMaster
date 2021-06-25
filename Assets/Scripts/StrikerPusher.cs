using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StrikerPusher : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    Vector2 speed;
    RectTransform _rectTransform;
    float storeAnchorMinX;
    public float magnetForce = 10f;
    bool doAgain = true;
    List<Rigidbody2D> caughtRigidbodies = new List<Rigidbody2D>();
    Vector2 startPosition;

    private void Start()
    {


        _rectTransform = GetComponent<RectTransform>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        //if (doAgain)
        {
            for (int i = 0; i < caughtRigidbodies.Count; i++)
            {
                speed = ((Vector2)transform.position - ((Vector2)caughtRigidbodies[i].transform.position + caughtRigidbodies[i].centerOfMass)) * magnetForce * Time.deltaTime;
               // transform.Translate(new Vector2(speed.x, 0) * Time.deltaTime);//
                                                                              //storeAnchorMinX = _rectTransform.anchorMin.x;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(speed.x, 0),ForceMode2D.Impulse);
            }
        }



        /*if (_rectTransform.anchorMin.x > storeAnchorMinX || _rectTransform.anchorMin.x < storeAnchorMinX)
        {
            _rectTransform.anchoredPosition = (Vector2)transform.position + offset;
        }*/




    }
    public void DragStriker()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Vector2 startingPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            transform.position = new Vector2(Mathf.Clamp(startingPoint.x, -144, 144), startPosition.y);

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D r = other.GetComponent<Rigidbody2D>();

            if (!caughtRigidbodies.Contains(r))
            {
                //Add Rigidbody
                caughtRigidbodies.Add(r);
            }
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        doAgain=false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (other.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D r = other.GetComponent<Rigidbody2D>();

            if (caughtRigidbodies.Contains(r))
            {
                //Remove Rigidbody
                caughtRigidbodies.Remove(r);

            }
        }
    }
}
