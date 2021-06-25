using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Com.RamboGameStudio.CarromBoard;

public class StoreValue : MonoBehaviour
{
    #region variables
    public int valueStoredWhite;
    public int valueStoreBlack;
    public bool storeBool;
    [SerializeField] GameObject Queen;
    DiskCollected disk;
    public List<GameObject> blackDiskCollected = new List<GameObject>();
    public List<GameObject> whiteDiskCollected = new List<GameObject>();
    public bool controller = false;


    public bool strikerController;
    public bool checkFirst = true;
    [SerializeField] private TextMeshProUGUI txt;
    [Header("Confetti")]
    [SerializeField] private GameObject confetti;
    #endregion
    public static StoreValue instance { get; private set; }
    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    /// #Critical: First Coin come then only remove from the list.
    /// BlackCoin_Move_Remove is for black collected coin.
    /// </summary>
    public void BlackCoin_Move_Remove()
    {
        MoveRemove(blackDiskCollected[0]);
        blackDiskCollected.Remove(blackDiskCollected[0]);

    }
    //This one is for white collected coin.
    public void WhiteCoin_Move_Remove()
    {
        MoveRemove(whiteDiskCollected[0]);
        whiteDiskCollected.Remove(whiteDiskCollected[0]);

    }
    ///<summary>
    ///Move the gameObject in the parameter to the Center of the carrom board.
    ///</summary>
    ///<param name = "moverGameObject"> This is the GameObject passes by BlackCoin_Move_Remove() and WhiteCoin_Move_Remove(). </param>
    public void MoveRemove(GameObject moverGameObject)
    {
        moverGameObject.GetComponent<CircleCollider2D>().enabled = true;
        iTween.MoveTo(moverGameObject, iTween.Hash("position", Queen.transform.position, "time", 1f));

    }

    //This function is called in DiskCollected to add the collected coins.
    ///<param name="disk"> disk is passes from DisckCollected script's OnTriggerStay2D.</param>
    public void CoinCollected(GameObject disk)
    {
        if (disk.tag == "BlackCoin")
        {
            blackDiskCollected.Add(disk);
            if (blackDiskCollected.Count == 9)
            {
                txt.text = "<b><color=black>Player1</color></b> won the <color=red>game</color>";
                LoadScene();
            }

        }

        else if (disk.tag == "WhiteCoin")
        {
            whiteDiskCollected.Add(disk);
            if (whiteDiskCollected.Count == 9)
            {
                txt.text = "<b><color=black>Player2</color></b> won the <color=red>game</color>";
                LoadScene();
            }

        }
        else if (disk.tag == "BlackCoin" && disk.tag == "WhiteCoin")
        {
            Debug.Log("Hello");
        }




    }

    public void LoadScene()
    {
        confetti.SetActive(true);
        Controller.Instance.enabled = false;
    }



}
