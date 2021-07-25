using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoutonQuestion : MonoBehaviour, IPointerEnterHandler
{
    Dialogue dialogue;
    public int num ;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GameObject.FindObjectOfType<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        dialogue.selectQuestion(num);
    }

}
