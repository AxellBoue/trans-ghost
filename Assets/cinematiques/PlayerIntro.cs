using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerIntro : MonoBehaviour
{

    // mouvement
    private Rigidbody2D rb;
    public float speed = 25;
    private Vector2 mouvement;
    private float inputX = 0;
    private float inputY = 0;

    private GameObject feedBackInteraction;
    bool interactionProche = false;
    public string scene = "debut";

    public bool bloque = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        feedBackInteraction = GameObject.Find("Interagir");
        feedBackInteraction.SetActive(false);

        if (GameManager.instance.infos.langue == "english")
        {
            changeLangue("english");
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (interactionProche && Input.GetKeyDown(KeyCode.Space))
        {
            if (scene == "debut")
            {
                setBloque(true);
                StartCoroutine("entreDansTeteLater");
            }
            else
            {
                StartCoroutine("finLater");
                setBloque(true);
                GameObject.Find("Alex").GetComponent<Animator>().Play("anim resurection");
                feedBackInteraction.SetActive(false);
            }
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

    public void setInteraction(bool b)
    {
        interactionProche = b;
        feedBackInteraction.SetActive(b);
    }


    private IEnumerator entreDansTeteLater()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.instance.infos.scene = "tete_main";
        SceneManager.LoadScene("tete_main");
    }

    private IEnumerator finLater()
    {
        yield return new WaitForSeconds(5.0f);
        GameManager.instance.infos.scene = "menuDepart";
        SceneManager.LoadScene("menuDepart");
    }


    // bloque
    public void setBloque(bool b)
    {
        bloque = b;
        if (b && rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void changeLangue(string newLangue)
    {
        if (newLangue == "francais")
        {
            feedBackInteraction.transform.GetChild(0).gameObject.SetActive(true);
            feedBackInteraction.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            feedBackInteraction.transform.GetChild(0).gameObject.SetActive(false);
            feedBackInteraction.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
