using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backToEarth : MonoBehaviour
{
    bool playerIn = false;
    GameObject interactionBackEarth;

    // Start is called before the first frame update
    void Start()
    {
        interactionBackEarth = GameObject.Find("InteragirToEarth");
        interactionBackEarth.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn && Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.toTete();
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "player")
        {
            playerIn = true;
            interactionBackEarth.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name == "player")
        {
            playerIn = false;
            interactionBackEarth.SetActive(false);
        }
    }

}
