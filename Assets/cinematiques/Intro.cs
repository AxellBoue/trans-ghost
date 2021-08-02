using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    PlayerIntro player;
    GameObject texteTuto;
    GameObject texteIntro;
    bool canMove = false;
    bool aBouge = false;
    public bool finAnim = false;
    Animator anim;

    // pour mettre le player au bon endroit
    Vector3 cinematicPosition;
    GameObject soulPlayer;
    private bool goesToPosition = false;

    // pour faire poper les traits
    public bool doPopTrait = false;
    public GameObject[] traitsPrefabs;
    private Transform parentTraits;
    private float compteur = 0;
    private float timeNextTrait = 0.05f ;

     // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerIntro>();
        anim = player.GetComponent<Animator>();
        player.setBloque(true);
        texteTuto = GameObject.Find("texteTuto");
        texteTuto.SetActive(false);
        texteIntro = GameObject.Find("texte intro");
        texteIntro.SetActive(false);

        soulPlayer = transform.parent.GetChild(3).gameObject;
        cinematicPosition = new Vector3(soulPlayer.transform.position.x,soulPlayer.transform.position.y,player.transform.position.z);

        parentTraits = transform.GetChild(0).GetComponent<Transform>();

        if (GameManager.instance.infos.langue == "english")
        {
            changeLangue("english");
        }

        anim.Play("perso appear");
        StartCoroutine("afficheTutoLater");
    }

    // Update is called once per frame
    void Update()
    {
        if (!aBouge && canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                aBouge = true;
                StartCoroutine("cacheTutoLater");
            }
        }

        if (finAnim)
        {
            finAnim = false;
            GameManager.instance.toEnfer();
        }

        if (doPopTrait)
        {
            compteur += Time.deltaTime;
            if (compteur >= timeNextTrait)
            {
                popTrait();
                compteur = 0;
                timeNextTrait = Random.Range(0.05f, 0.6f);
            }
        }

        if (goesToPosition)
        {
            Vector3 mouvement = cinematicPosition - player.transform.position;
            mouvement = mouvement.normalized;
            player.transform.position += mouvement * 20 * Time.deltaTime;
            if (Vector3.Distance(player.transform.position,cinematicPosition) < 0.2)
            {
                changementPersoFini();
                goesToPosition = false;
            }
        }
    }

    IEnumerator afficheTutoLater()
    {
        yield return new WaitForSeconds(5.0f);
        texteIntro.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        player.setBloque(false);
        texteTuto.SetActive(true);
        StartCoroutine("cacheTexteLater");
        canMove = true;
    }

    IEnumerator cacheTexteLater()
    {
        yield return new WaitForSeconds(5.0f);
        texteIntro.SetActive(false);
    }

    IEnumerator cacheTutoLater()
    {
        yield return new WaitForSeconds(2.0f);
        texteTuto.SetActive(false);
        StartCoroutine("changePersoLater");
        //StartCoroutine("lanceAnimLater");
    }

    IEnumerator changePersoLater()
    {
        yield return new WaitForSeconds(5.0f);
        player.setBloque(true);
        anim.Play("transforme en ame");
        StartCoroutine("deplacePersoLater");
    }

    IEnumerator deplacePersoLater()
    {
        yield return new WaitForSeconds(2.0f);
        goesToPosition = true;
    }

    private void changementPersoFini()
    {
        player.gameObject.SetActive(false);
        soulPlayer.SetActive(true);
        StartCoroutine("lanceAnimLater");
    }

    IEnumerator lanceAnimLater()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("anim").GetComponent<Animator>().enabled = true;
        GameObject.Find("anim").GetComponent<Animator>().Play("animIntro");
    }

    public void changeLangue(string newLangue)
    {
        if (newLangue == "english")
        {
            texteTuto.transform.GetChild(0).gameObject.SetActive(false);
            texteTuto.transform.GetChild(1).gameObject.SetActive(true);

            texteIntro.transform.GetChild(0).gameObject.SetActive(false);
            texteIntro.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            texteTuto.transform.GetChild(0).gameObject.SetActive(true);
            texteTuto.transform.GetChild(1).gameObject.SetActive(false);

            texteIntro.transform.GetChild(0).gameObject.SetActive(true);
            texteIntro.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // pour pop les traits

    private void popTrait()
    {
        GameObject newTrait = Instantiate(traitsPrefabs[Random.Range(0, traitsPrefabs.Length)]) as GameObject;
        newTrait.transform.SetParent(parentTraits);
        newTrait.transform.localPosition = new Vector3(Random.Range(-40.0f,40.0f),0.0f,0.0f);
    }



}
