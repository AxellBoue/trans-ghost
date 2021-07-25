using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // mouvement
    private Rigidbody2D rb;
    public float speed = 25;
    private Vector2 mouvement;
    private float inputX = 0;
    private float inputY = 0;
    // pour les pnjs
    List<Pnj> listePnjProches = new List<Pnj>();
    Pnj pnjActif = null;
    GameObject feedBackInteraction;
    Dialogue boxDialogue;
    bool bloque = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        feedBackInteraction = GameObject.Find("Interagir");
        feedBackInteraction.SetActive(false);
        boxDialogue = GameObject.FindObjectOfType<Dialogue>();
        boxDialogue.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (pnjActif != null && Input.GetKeyDown(KeyCode.Space))
        {
            boxDialogue.parler(pnjActif.infos.numDialogue);
        }
    }

    private void FixedUpdate()
    {
        if (!bloque)
        {
            mouvement = new Vector2(inputX, inputY).normalized * speed;
            rb.velocity = mouvement;
        }
    }

    // pour les pnjs
    public void addPnj(Pnj newPnj)
    {
        listePnjProches.Add(newPnj);
        pnjActif = newPnj;
        feedBackInteraction.SetActive (true);
    }

    public void removePnj(Pnj pnjAVirer)
    {
        listePnjProches.Remove(pnjAVirer);
        if (pnjActif == pnjAVirer)
        {
            boxDialogue.closeBox();
        }
        if (listePnjProches.Count > 0)
        {
            pnjActif = listePnjProches[0];
        }
        else
        {
            pnjActif = null;
            feedBackInteraction.SetActive(false);
        }
    }

    // bloque
    public void setBloque(bool b)
    {
        bloque = b;
        if (b)
        {
            rb.velocity = Vector2.zero;
        }
    }

}
