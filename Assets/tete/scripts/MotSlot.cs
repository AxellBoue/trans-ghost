using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MotSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GestionMots.AllMots.MotsInfos infos;
    private GestionMots gestionMots;

    // pour virer le mot pas attendu
    private bool redescend;
    private bool remonte;
    private float vitesse = 12.0f;
    private float aBouge = 0.0f;
    private MotSouvenir motInside = null;
    private Vector3 positionInitiale;

    // pour disparaitre si mot attendu
    private bool disparait = false;
    private Text texte;
    private Text texteEn = null;  // à initialiser quand un motsouvenir devient un motslot
    private Color colorDepart;
    private Color colorDisparait = new Color(0.0f,0.0f,0.0f,0.0f);
    private Color newColor;
    private float vitesseDisparition = 0.3f;
    private float iDisparition = 0;


    // Start is called before the first frame update
    void Start()
    {
        gestionMots = GameObject.FindObjectOfType<GestionMots>();
        positionInitiale = transform.position;
        texte = transform.GetChild(0).GetComponent<Text>();
        colorDepart = texte.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (redescend)
        {
            transform.position -= new Vector3(0.0f, vitesse * Time.deltaTime, 0.0f);
            aBouge += vitesse * Time.deltaTime;
            if (aBouge >= 6.5)
            {
                motInside.transform.position -= new Vector3(0.0f, vitesse * Time.deltaTime, 0.0f);
            }
            if (aBouge >= 20)
            {
                redescend = false;
                remonte = true;
                aBouge = 0;
            }
        }
        else if (remonte)
        {
            transform.position += new Vector3(0.0f, vitesse * Time.deltaTime, 0.0f);
            aBouge += vitesse * Time.deltaTime;
            if (aBouge >= 5)
            {
                remonte = false;
                transform.position = positionInitiale;
                motInside.attendReaction = false;
            }
        }

        if (disparait)
        {
            iDisparition += vitesseDisparition * Time.deltaTime;
            newColor = Color.Lerp(colorDepart, colorDisparait, iDisparition);
            texte.color = newColor;
            if (texteEn != null)
            {
                texteEn.color = newColor;
            }
            if (iDisparition >= 1)
            {
                disparait = false;
                gameObject.SetActive(false);
            }
        }
    }


    public void OnPointerEnter(PointerEventData data)
    {
        if (gestionMots.motDragged != null)
        {
            gestionMots.motDragged.motIsOn = this;
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (gestionMots.motDragged != null)
        {
            gestionMots.motDragged.motIsOn = null;
        }
    }

    public void motPose(MotSouvenir newMot)
    {
        transform.position += new Vector3(0.0f, 15.0f, 0.0f);
        motInside = newMot;
        if (newMot.infos.texteFr != infos.motsAttendus)
        {
            StartCoroutine("redescendLater");
        }
        else
        {
            StartCoroutine("disparaitLater");
            motInside.motAccepte();
        }
    }

    private IEnumerator redescendLater()
    {
        yield return new WaitForSeconds(2.0f);
        aBouge = 0.0f;
        redescend = true;
    }

    private IEnumerator disparaitLater()
    {
        yield return new WaitForSeconds(6.0f);
        iDisparition = 0;
        disparait = true;
    }
}
