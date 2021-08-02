using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    GameObject menuBase;
    GameObject menuNewGame;
    GameObject menuCharger;
    GameObject menuOptions;

    BoutonMenu currentButton;
    RectTransform fleche;
    BoutonMenu[] BoutonsMenuCourant;
    int numBouton = 0;

    // Start is called before the first frame update
    void Start()
    {
        menuBase = GameObject.Find("menu base");
        menuNewGame = transform.GetChild(3).gameObject;
        menuCharger = transform.GetChild(4).gameObject;
        menuOptions = transform.GetChild(5).gameObject;
        fleche = menuBase.transform.GetChild(0).GetComponent<RectTransform>();
        selectBouton(menuBase.transform.GetChild(1).GetComponent<BoutonMenu>());
        BoutonsMenuCourant = menuBase.GetComponentsInChildren<BoutonMenu>();

        changeLangue(GameManager.instance.infos.langue);
    }

    // Update is called once per frame
    void Update()
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
            currentButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void open( GameObject to )
    {
        menuBase.SetActive(false);
        to.SetActive(true);
        fleche = to.transform.GetChild(0).GetComponent<RectTransform>();
        BoutonsMenuCourant = to.GetComponentsInChildren<BoutonMenu>();
        selectBouton(to.transform.GetChild(1).GetComponent<BoutonMenu>());

    }

    public void retour(GameObject from)
    {
        from.SetActive(false);
        menuBase.SetActive(true);
        fleche = menuBase.transform.GetChild(0).GetComponent<RectTransform>();
        BoutonsMenuCourant = menuBase.GetComponentsInChildren<BoutonMenu>();
        selectBouton(menuBase.transform.GetChild(1).GetComponent<BoutonMenu>());
    }

    public void newGame(string numPartie)
    {
        GameManager.instance.infos.numSauvegarde = numPartie;
        GameManager.instance.infos.hasBeenToTete = false;
        string path = Application.persistentDataPath + "/saveSouvenirsBox" + GameManager.instance.infos.numSauvegarde + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        string path2 = Application.persistentDataPath + "/saveSouvenirsTete" + GameManager.instance.infos.numSauvegarde + ".json";
        if (File.Exists(path2))
        {
            File.Delete(path2);
        }
        string path3 = Application.persistentDataPath + "/savePnj" + GameManager.instance.infos.numSauvegarde + ".json";
        if (File.Exists(path3))
        {
            File.Delete(path3);
        }
        string path4 = Application.persistentDataPath + "/saveGeneral" + GameManager.instance.infos.numSauvegarde + ".json";
        if (File.Exists(path4))
        {
            File.Delete(path4);
        }
        string path5 = Application.persistentDataPath + "/savePensees" + GameManager.instance.infos.numSauvegarde + ".json";
        if (File.Exists(path5))
        {
            File.Delete(path5);
        }
        GameManager.instance.infos.scene = "intro";
        SceneManager.LoadScene("intro");
    }

    public void chargeGame(string numPartie)
    {
        GameManager.instance.infos.numSauvegarde = numPartie;
        GameManager.instance.loadOptions();
        SceneManager.LoadScene(GameManager.instance.infos.scene);
    }

    public void changeLangue( string newLangue = "" )
    {
        // récupère tous les boutons
        List<Button> textesBoutons = new List<Button>();
        Button[] boutonsMenu = menuBase.transform.GetComponentsInChildren<Button>();
        foreach (Button b in boutonsMenu)
        {
            textesBoutons.Add(b);
        }
        boutonsMenu = menuNewGame.transform.GetComponentsInChildren<Button>();
        foreach (Button b in boutonsMenu)
        {
            textesBoutons.Add(b);
        }
        boutonsMenu = menuCharger.transform.GetComponentsInChildren<Button>();
        foreach (Button b in boutonsMenu)
        {
            textesBoutons.Add(b);
        }
        boutonsMenu = menuOptions.transform.GetComponentsInChildren<Button>();
        foreach (Button b in boutonsMenu)
        {
            textesBoutons.Add(b);
        }
       
        if ((newLangue == "" && GameManager.instance.infos.langue == "francais") || newLangue == "english")
        {
            GameManager.instance.infos.langue = "english";
            // change l'affichage du choix de langue
            menuOptions.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "English";
            // changer le texte de tous les boutons
            foreach (Button b in textesBoutons)
            {
                b.transform.GetChild(0).gameObject.SetActive(false);
                b.transform.GetChild(1).gameObject.SetActive(true);
            }
            // changer les titres
            menuNewGame.transform.GetChild(5).GetComponent<Text>().text = "New game";
            menuCharger.transform.GetChild(5).GetComponent<Text>().text = "Load game";
            // changer le texte de taille de l'ui
            Text texteUiAChanger = menuOptions.transform.GetChild(3).GetChild(2).GetComponent<Text>();
            if (texteUiAChanger.text == "Moyenne")
            {
                texteUiAChanger.text = "Normal";
            }
            else
            {
                texteUiAChanger.text = "Big";
            }
        }
        else
        {
            GameManager.instance.infos.langue = "francais";
            // change le choix de langue
            menuOptions.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "Français";
            // change tous les boutons
            foreach (Button b in textesBoutons)
            {
                b.transform.GetChild(0).gameObject.SetActive(true);
                b.transform.GetChild(1).gameObject.SetActive(false);
            }
            // changer les titres
            menuNewGame.transform.GetChild(5).GetComponent<Text>().text = "Nouvelle partie";
            menuCharger.transform.GetChild(5).GetComponent<Text>().text = "Charger partie";
            // changer le texte de taille de l'ui
            Text texteUiAChanger = menuOptions.transform.GetChild(3).GetChild(2).GetComponent<Text>();
            if (texteUiAChanger.text == "Normal")
            {
                texteUiAChanger.text = "Moyenne";
            }
            else
            {
                texteUiAChanger.text = "Grande";
            }
        }
        // sauvegarde les options à chaque fois qu'on change
        GameManager.instance.saveGeneralOptions();
    }

    public void changeClavier(Text texteClavier) 
    {
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


    public void changeTailleUI(Text texteTaille)
    {
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


    public void quitter()
    {
        Application.Quit();
    }

    // navigation dans le menu

    public void selectBouton(BoutonMenu newBouton)  // en passant la souris dessus ou en naviguant avec clavier
    {
        currentButton = newBouton;
        numBouton = currentButton.id;
        fleche.anchoredPosition = currentButton.GetComponent<RectTransform>().anchoredPosition + currentButton.decalage;
    }

}
