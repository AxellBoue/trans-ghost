using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MotSouvenir : MonoBehaviour, IPointerDownHandler
{
    public GestionMots.AllMots.MotsInfos infos;

    private tete_move teteMove;
    private Image image;
    private GestionMots gestionMots;
    private Transform parentMots;

    private bool drag;
    private bool isJustDragged = false;
    private Vector3 decalage;

    public bool attendReaction = false;

    public MotSlot motIsOn = null;

    // pour changement de couleur
    private bool changeCouleur = false;
    private Text texteFr;
    private Text texteEn;
    private Color colorDepart;
    private Color colorChange = new Color(0.203f, 0.557f, 0.186f, 1.0f);
    private Color newColor;
    private float vitesseChangement = 0.4f;
    private float iChangement = 0;

    // pour le tuto
    TutoTete tutoTete;

    // Start is called before the first frame update
    void Start()
    {
        gestionMots = GameObject.FindObjectOfType<GestionMots>();
        teteMove = GameObject.FindObjectOfType<tete_move>();
        parentMots = GameObject.Find("MotsSouvenirsBox").transform;
        image = GetComponent<Image>();
        texteFr = transform.GetChild(0).GetComponent<Text>();
        texteEn = transform.GetChild(1).GetComponent<Text>();
        infos.texteFr = texteFr.text;
        colorDepart = texteFr.color;
        tutoTete = GameObject.FindObjectOfType<TutoTete>();
    }

    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + decalage;
            if (Input.GetMouseButtonDown(0) && !attendReaction)
            {
                if (isJustDragged) // pour pas que ça le drop au même clic que le debut du drag
                {
                    isJustDragged = false;
                }
                else
                {
                    drop();
                }
            }
        }

        if (changeCouleur)
        {
            iChangement += vitesseChangement * Time.deltaTime;
            newColor = Color.Lerp(colorDepart, colorChange, iChangement);
            texteFr.color = newColor;
            texteEn.color = newColor;
            if (iChangement >= 1)
            {
                changeCouleur = false;

                MotSlot motSlot = GetComponent<MotSlot>();
                motSlot.enabled = true;
                motSlot.infos.motsAttendus = infos.motsAttendus;

                enabled = false;
            }
        }

    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!attendReaction)
        {
            isJustDragged = true;
            drag = true;
            gestionMots.motDragged = this;
            decalage = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
            teteMove.isDraggingQqchose = true;
            image.raycastTarget = false;
            transform.SetParent(parentMots);

            // if (infos.nombreDUtilisation > souvenir.utilisationsMot) {
            // instancie un nouveau mot identique dans la phrase
            // }
        }
    }

    private void drop()
    {
        gestionMots.motDragged = null;
        drag = false;
        teteMove.isDraggingQqchose = false;
        image.raycastTarget = true;
        if (motIsOn != null)
        {
            attendReaction = true;
            transform.position = motIsOn.transform.position;
            motIsOn.motPose(this);
            if (tutoTete.waitForWord)
            {
                tutoTete.doTheThingWaited(5.0f);
            }
        }
        motIsOn = null;
    }


    public void motAccepte()
    {
        if (infos.texteTurnIntoFr != "")
        {
            texteFr.text = infos.texteTurnIntoFr;
            texteEn.text = infos.texteTurnIntoEn;
        }
        StartCoroutine("changeCouleurLater");
    }


    private IEnumerator changeCouleurLater()
    {
        yield return new WaitForSeconds(1.0f);
        iChangement = 0;
        changeCouleur = true;
    }


    private void retourDansLeSouvenir()
    {
        // delete
        // re instancie un mot dans le souvenir si il y est pas déjà
        // variable utilisationsMot dans le souvenir : augmente d'un
    }

}
