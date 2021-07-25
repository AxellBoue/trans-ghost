using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tete_move : MonoBehaviour
{

    Camera cam ;
    bool drag = false;
    Vector3 mouseMovement;
    Vector3 lastMousePos;
    // pour le raycast world canvas
    GraphicRaycaster m_raycaster;
    EventSystem m_eventSystem;
    PointerEventData m_pointerEventData;
    // pour le raycast camera Canvas
    GraphicRaycaster m_raycaster2;
    PointerEventData m_pointerEventData2;
    // pour bouger quand on eset sur les bords en draguant qqchose
    public bool isDraggingQqchose = false;
    private List<GameObject> cotes = new List<GameObject>();
    private Vector3 mouvementCote;
    private float vitesseCote = 80;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<Camera>();
        cam.transform.position = new Vector3(-47.0f, -69.0f, -10.0f);
        m_raycaster = GetComponent<GraphicRaycaster>();
        m_eventSystem = GameObject.FindObjectOfType<EventSystem>();
        m_raycaster2 = GameObject.Find("CanvasCamera").GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // vérifie qu'on est pas au dessus d'un objet interactif (avec lequel le clic fait autre chose)
            // canvas world
            m_pointerEventData = new PointerEventData(m_eventSystem);
            m_pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_raycaster.Raycast(m_pointerEventData, results);
            // canvas camera
            m_pointerEventData2 = new PointerEventData(m_eventSystem);
            m_pointerEventData2.position = Input.mousePosition;
            List<RaycastResult> results2 = new List<RaycastResult>();
            m_raycaster2.Raycast(m_pointerEventData2, results2);

            if (results.Count == 0 && results2.Count == 0)
            {
                // variables pour commencer le drag and drop de la camera
                drag = true;
                lastMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (drag)
            {
                drag = false;
            }
            if (isDraggingQqchose)
            {
                //isDraggingQqchose = false;
            }
        }

        if (drag)
        {
            mouseMovement = lastMousePos - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += mouseMovement;
            lastMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (isDraggingQqchose && cotes.Count > 0)
        {
            cam.transform.position += mouvementCote.normalized * vitesseCote*Time.deltaTime;
        }

    }

    public void EntreSurUnCote(CoteEcran newCote)
    {
        cotes.Add(newCote.gameObject);
        mouvementCote += newCote.direction;
    }

    public void SortDunCote(CoteEcran coteAVirer)
    {
        cotes.Remove(coteAVirer.gameObject);
        mouvementCote -= coteAVirer.direction;
    }

}
