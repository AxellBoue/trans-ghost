using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuvreSouvenirBox : MonoBehaviour
{
    public bool dragueSouvenir = false;
    private SouvenirBox boxSouvenirs;

    // Start is called before the first frame update
    void Start()
    {
        boxSouvenirs = GameObject.FindObjectOfType<SouvenirBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        if (dragueSouvenir)
        {
            boxSouvenirs.show();
        }
    }

}
