using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignCard : MonoBehaviour
{
    public List<Sprite> deckOfCard = new List<Sprite>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite =  deckOfCard[UnityEngine.Random.Range(0, deckOfCard.Count)];

        }
          iTween.ScaleTo(this.gameObject,iTween.Hash("scale",new Vector3(1,1,1),"time",5f));

    }


}
