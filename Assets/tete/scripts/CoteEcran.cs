using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoteEcran : MonoBehaviour
{

    public Vector3 direction;
    private tete_move cameraMouvement;

    // Start is called before the first frame update
    void Start()
    {
        cameraMouvement = GameObject.FindObjectOfType<tete_move>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        cameraMouvement.EntreSurUnCote(this);
    }

    public void OnMouseExit()
    {
        cameraMouvement.SortDunCote(this);
    }

}
