using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    private Vector2 positionOuvert;
    private Vector2 positionFerme = new Vector2 (371,336);
    private bool ouvert = true;
    private GameObject menuOptions;

    BoutonMenu currentButton;
    RectTransform fleche;
    BoutonMenu[] BoutonsMenuCourant;
    int numBouton = 0;
    RectTransform boutonOuvreFerme;

    // Start is called before the first frame update
    void Start()
    {
        positionOuvert = GetComponent<RectTransform>().anchoredPosition;
        boutonOuvreFerme = transform.GetChild(5).GetComponent<RectTransform>();
        ouvrirFermer();
        menuOptions = transform.GetChild(6).gameObject;
        fleche = transform.GetChild(0).GetComponent<RectTransform>();
        BoutonsMenuCourant = transform.GetComponentsInChildren<BoutonMenu>();
        selectBouton(BoutonsMenuCourant[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (ouvert)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("bas") || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetButtonDown("droite"))
            {
                numBouton += 1;
                if (numBouton >= BoutonsMenuCourant.Length)
                {
                    numBouton = 0;
                }
                selectBouton(BoutonsMenuCourant[numBouton]);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("haut") || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetButtonDown("gauche"))
            {
                numBouton -= 1;
                if (numBouton < 0)
                {
                    numBouton = BoutonsMenuCourant.Length - 1;
                }
                selectBouton(BoutonsMenuCourant[numBouton]);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                print(currentButton.name);
                currentButton.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void ouvrirFermer()
    {
        if (ouvert) //le ferme
        {
            GetComponent<RectTransform>().anchoredPosition = positionFerme;
            ouvert = false;
            fleche = transform.GetChild(0).GetComponent<RectTransform>();
            fleche.GetComponent<Image>().enabled = false;
            boutonOuvreFerme.localRotation = Quaternion.Euler(Vector3.zero);
            // enleve la pause, et si un truc d'ui est ouvert, remet le focus dessus
        }
        else // l'ouvre
        {
            GetComponent<RectTransform>().anchoredPosition = positionOuvert;
            ouvert = true;
            fleche = transform.GetChild(0).GetComponent<RectTransform>();
            fleche.GetComponent<Image>().enabled = true;
            BoutonsMenuCourant = transform.GetComponentsInChildren<BoutonMenu>();
            selectBouton(BoutonsMenuCourant[0]);
            boutonOuvreFerme.localRotation = Quaternion.Euler(new Vector3(0,0,180));
            // met pause
            // met le focus sur le premier bouton
        }
    }

    public void save()
    {
        GameManager.instance.save();
    }

    public void saveAndQuit()
    {
        GameManager.instance.save();
        Application.Quit();
    }

    public void saveAndMenu()
    {
        GameManager.instance.save();
        GameManager.instance.infos.scene = "menuDepart";
        SceneManager.LoadScene("menuDepart");
    }

    public void openOptions()
    {
        menuOptions.SetActive(true);
        fleche = menuOptions.transform.GetChild(0).GetComponent<RectTransform>();
        BoutonsMenuCourant = menuOptions.transform.GetComponentsInChildren<BoutonMenu>();
        selectBouton(BoutonsMenuCourant[0]);
    }

    public void backFromOptions()
    {
        menuOptions.SetActive(false);
        fleche = transform.GetChild(0).GetComponent<RectTransform>();
        BoutonsMenuCourant = transform.GetComponentsInChildren<BoutonMenu>();
        selectBouton(BoutonsMenuCourant[0]);
    }

    public void changeLangue ()
    {
        string newLangue;
        if (GameManager.instance.infos.langue == "francais")
        {
            newLangue = "english";
        }
        else
        {
            newLangue = "francais";
        }
        GameManager.instance.infos.langue = newLangue;

        // change langue du menu : recup code menu intro{

        // change langue selon la scène
        switch (GameManager.instance.infos.scene)
        {
            case ("intro") :
                GameObject.FindObjectOfType<Intro>().changeLangue(newLangue);
                break;
            case ("entreDansTete"):
            case ("fin"):
                GameObject.FindObjectOfType<PlayerIntro>().changeLangue(newLangue);
                break;
        }
    }

    public void changeClavier()
    {
        Text texteClavier = currentButton.transform.GetChild(2).GetComponent<Text>();
        if (texteClavier.text == "Azerty")
        {
            texteClavier.text = "Qwerty";
        }
        else
        {
            texteClavier.text = "Azerty";
        }
        GameManager.instance.infos.clavier = texteClavier.text;
        // sauvegarde les options à chaque fois qu'on change
        GameManager.instance.saveGeneralOptions();
    }


    public void changeTailleUI()
    {
        Text texteTaille = currentButton.transform.GetChild(2).GetComponent<Text>();
        if (texteTaille.text == "Moyenne" || texteTaille.text == "Normal")
        {
            if (texteTaille.text == "Moyenne")
            {
                texteTaille.text = "Grande";
            }
            else
            {
                texteTaille.text = "Big";
            }
            GameManager.instance.infos.tailleUI = "Moyenne";
        }
        else
        {
            if (texteTaille.text == "Grande")
            {
                texteTaille.text = "Moyenne";
            }
            else
            {
                texteTaille.text = "Normal";
            }
            GameManager.instance.infos.tailleUI = "Grande";
        }

    }

    // navigation dans le menu

    public void selectBouton(BoutonMenu newBouton)  // en passant la souris dessus ou en naviguant avec clavier
    {
        currentButton = newBouton;
        numBouton = currentButton.id;
        fleche.anchoredPosition = currentButton.GetComponent<RectTransform>().anchoredPosition + currentButton.decalage;
        if (currentButton.reverseFleche)
        {
            fleche.localScale = new Vector2(-1.0f, 1.0f);
        }
        else
        {
            fleche.localScale = new Vector2(1.0f, 1.0f);
        }
    }

}
