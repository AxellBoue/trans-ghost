using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenseeOuvrable : MonoBehaviour
{

    public GestionPensees.AllPensees.PenseeInfos infos;
    public GameObject[] goAActiver;
    private GameObject image;
    private bool enCoursDeDepliage = false;
    private bool appuie = false;
    private float compteur = 0;
    private Animator anim;
    TutoTete tutoTete;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }
        tutoTete = GameObject.FindObjectOfType<TutoTete>();
    }

    // Update is called once per frame
    void Update()
    {
        if (appuie)
        {
            compteur += 1 * Time.deltaTime;
        }
        if (appuie && compteur > 2.0f){
            depliePensee();
            appuie = false;
        }
    }

    public void OnMouseDown()
    {
        if (!infos.isOuverte && !enCoursDeDepliage)
        {
            appuie = true;
            StartCoroutine("lanceAnimLater");
        }

    }

    public void OnMouseUp()
    {
        if (appuie){
            stopCompteur();
        }
    }

    public void OnMouseExit()
    {
        if (appuie)
        {
            stopCompteur();
        }
    }


    private IEnumerator lanceAnimLater()
    {
        yield return new WaitForSeconds(0.6f);
        if (appuie)
        {
            anim.enabled = true;
            anim.Play("deplie");
        }
    }

    private void stopCompteur()
    {
        appuie = false;
        compteur = 0;
        anim.enabled = false;
    }

    public void initialise()
    {
        image = transform.GetChild(0).gameObject;
        gameObject.SetActive(infos.actif);
        image.SetActive(!infos.isOuverte);
        if (!infos.isOuverte)
        {
            foreach (GameObject go in goAActiver)
            {
                if (go.GetComponent<PenseeOuvrable>() == null) // desactive les traits, les pensées se desactivent individuellement
                {
                    go.SetActive(false);
                }
            }
        }
    }

    public void depliePensee()
    {
        enCoursDeDepliage = true;
        StartCoroutine("depliePenseeLater");
    }

    private IEnumerator depliePenseeLater()
    {
        image.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject go in goAActiver)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        infos.isOuverte = true;
        if (tutoTete.waitForUnfold)
        {
            tutoTete.doTheThingWaited();
        }
    }
}
