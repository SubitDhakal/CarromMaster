using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCard : MonoBehaviour
{
    #region Transform
    [Header("Transform")]
    [Tooltip("Destination Throw of cards")]
    [SerializeField] private Transform[] destination;
    #endregion

    #region Floats
    [Header("Floats")]
    float time = 0f;
    [SerializeField] private float waitingTime = 1;
    #endregion
    bool stop=true;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Card badne kaam garxa yesle. Ramrari vako xaina future ma garne xu.
    /// </summary>

    void Update()
    {
        
       
        for (int i = 0; i < transform.childCount - 1; i++)
        {

            time += Time.deltaTime;

            if (time >= waitingTime)
            {
                if (i % 2 == 0)//even gameobject
                {
                    //transform.GetChild(i)
                    time = 0;
                    PushingCards(i, 0);
                }

                else
                {
                    time = 0;
                    PushingCards(i, 1);
                }

            }

            void PushingCards(int x, int y)
            {
                iTween.MoveTo(transform.GetChild(x).gameObject, iTween.Hash("position", destination[y].position, "time", 1f));
                StartCoroutine(StopRotation());
              
                iTween.RotateTo(transform.GetChild(x).gameObject, iTween.Hash("rotation", new Vector3(0, 0, Random.Range(-20, 50))));
                
            }
           
           IEnumerator StopRotation()
           {
               yield return new WaitForSecondsRealtime(5);
               enabled=false;
               
           }
           


        }
        
       



    }




}

