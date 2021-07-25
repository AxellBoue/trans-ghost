using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Souvenir : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // infos du souvenir
    public SouvenirBox.SouvenirsAllList.souvenirInfo infos;
    // pour drag and drop
    bool drag = false;
    Vector3 decalage;
    Vector3 positionBox;
    public bool dansTete = false;
    public bool sceneIsTete = false;
    public int numDansListe = 0;
    SouvenirBox souvenirBox;
    // pour changement de parents
    Transform souvenirListe;
    Transform canvasCameraT;
    Transform boxSouvenirsTete;
    // pour activer le mouvement de caméra si drag
    tete_move teteMove;
    // pour ouvrir la boite à ouvenirs si on drag sur le bouton d'ouverture
    OuvreSouvenirBox ouvreBox;
    // pour la phrase
    GameObject phrase;

    // Start is called before the first frame update
    void Start()
    {
        souvenirBox = GameObject.FindObjectOfType<SouvenirBox>();
        souvenirListe = GameObject.Find("ListeSouvenirs").transform;
        canvasCameraT = GameObject.Find("CanvasCamera").GetComponent<Transform>();
        teteMove = GameObject.FindObjectOfType<tete_move>();
        ouvreBox = GameObject.FindObjectOfType<OuvreSouvenirBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - decalage;
        }
    }

    public void initialise(bool isInTete)
    {
        GetComponentInChildren<Text>().text = infos.texte_fr;
        // charger image en fonction du nom du souvenir
        if (sceneIsTete)
        {
            boxSouvenirsTete = GameObject.Find("BoxSouvenirsTete").GetComponent<Transform>();
            instanciePhrase();
            phrase.SetActive(isInTete);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (sceneIsTete)
        {
            decalage = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GetComponent<RectTransform>().position;
            positionBox = souvenirListe.position;
            if (!dansTete)
            {
                GetComponent<Transform>().SetParent(canvasCameraT);
            }
            drag = true;
            teteMove.isDraggingQqchose = true;
            ouvreBox.dragueSouvenir = true;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (sceneIsTete)
        {
            drag = false;
            teteMove.isDraggingQqchose = false;
            ouvreBox.dragueSouvenir = false;
            if (!souvenirBox.hidden && Mathf.Abs(GetComponent<Transform>().position.y - positionBox.y) < 50) // si on le lache proche de la boite a souvenirs
            {
                GetComponent<Transform>().SetParent(souvenirListe);
                GetComponent<RectTransform>().anchoredPosition = new Vector3(souvenirBox.position1erSouvenir + souvenirBox.espacementSouvenir * numDansListe, 0.0f, 0.0f);
                // ajoute dans la liste et réorganise tous les souvenirs si il vient de la tête
                if (dansTete)
                {
                    souvenirBox.rerangeSouvenir(gameObject);
                    phrase.SetActive(false);
                }
                dansTete = false;
            }
            else  // si on le lache ailleurs
            {
                if (!dansTete) // si il était pas dans la tete avant
                {
                    dansTete = true;
                    GetComponent<Transform>().SetParent(boxSouvenirsTete);
                    souvenirBox.removeSouvenir(gameObject);
                    souvenirBox.hide();
                    StartCoroutine("affichePhraseLater");
                    StartCoroutine("affichePhraseLater");
                }
                infos.positionX = GetComponent<Transform>().position.x;
                infos.positionY = GetComponent<Transform>().position.y;
            }
            // + fait apparaitre sa phrase en déroulée, active les mots déplacables et tout et tout
        }
    }

    public void instanciePhrase()
    {

        phrase = Instantiate(Resources.Load("phrasesSouvenirs/" + infos.phraseName) as GameObject);
        phrase.transform.SetParent(boxSouvenirsTete);
        phrase.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
        phrase.transform.SetParent(this.transform);
        phrase.transform.localPosition = new Vector3(0.0f, 120.0f, 0.0f);

    }

    private IEnumerator affichePhraseLater()
    {
        yield return new WaitForSeconds(0.5f);
        phrase.SetActive(true);
    }

}
