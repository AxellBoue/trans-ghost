using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicTuto : MonoBehaviour
{
    TutoTete tutoTete;

    // Start is called before the first frame update
    void Start()
    {
        tutoTete = GameObject.FindObjectOfType<TutoTete>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        tutoTete.conditionAfficheText();
    }

}
