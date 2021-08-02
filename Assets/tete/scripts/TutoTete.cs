using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoTete : MonoBehaviour
{

    Text tutoText;
    GameObject tutoBox;
    public GameObject flecheSouvenirs;
    GameObject flecheNextDialogue;
    int compteur = 0;
    public bool isInTuto = false;
    public bool waitForMemorie = false;
    public bool waitForWord = false;
    public bool waitForUnfold = false;
    string[] tutoFr = new string[] {"Pour faire réfléchir ton père et qu'il change d'avis, tu vas partager des souvenirs avec lui.",
    "Les souvenirs lui évoqueront des pensées, et tu pourras les mélanger avec ses pensées actuelles. Essaie de déposer un souvenir dans son esprit.",
    "Maintenant tu peux déplacer un des mots jaunes en cliquant dessus, et le déposer en cliquant à nouveau.",
    "Voilà. Bon, si tu met un mot qui n'a aucun rapport ou que tu vas trop vite et que tu en mets un qui est trop éloigné de son mode de pensée actuel, il le rejettera. Tu vas devoir être patient et y aller progressiement.",
    "La plupart des pensées sont basées sur d'autres pensées ou croyaces. Pour réveler le pbases d'une pensée, reste cliqué dessu quelques instants",
    "Voilà, une fois que tu auras changé ces nouelles pensées, la pensée initiale devrait être modifiabe",
    "Pour te déplacer dans l'esprit, click et bouge en restant appuyé.",
    "Essaie par exemple de partager le souvenir XXX, et de remplacex XXX mot",
    "Et si tu veux retourner en enfer pour demander conseils aux autres, La machine t'ouvres un passage à l'extérieur de ces pensées, en bas à gauche"};
    string[] tutoEn = new string[] { "To make your father think and change his mind, you'll have to share memories with him.",
    "Memories evoke thoughts, and you'll have to mix them up with his actual thoughts. Try to drag a memory into his mind",
    "Now you can drag a word by clicking on it, and drop it by clicking again",
    "Well, if you put words that are unrelated, or go to fast and put words that are too far away from what he actually thinks, he will reject it. You'll have to be patient and go step by step.",
    "Most of the thoughts are based on other thoughts. To unfold one, hold the click on it.",
    "Once the new thought will be changed, you might be able to change the first one",
    "To move in the mind, hold click and move",
    "Now try to share the memory about XXX, and to replae the word XXX",
    "And if you want to go back to hell to ask the others for advices, the machine opens a way just outside the thoughts, on the bottom left."};


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.infos.hasBeenToTete = false;
        if (!GameManager.instance.infos.hasBeenToTete)
        {
            GameManager.instance.infos.hasBeenToTete = true;
            StartCoroutine("startTutoLater");
            tutoBox = GameObject.Find("Tuto");
            tutoText = tutoBox.GetComponentInChildren<Text>();
            isInTuto = true;
            flecheNextDialogue = tutoBox.transform.GetChild(3).gameObject;
        }
        tutoBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Space))
        {
            conditionAfficheText();
        }   
    }

    IEnumerator startTutoLater()
    {
        yield return new WaitForSeconds(2.0f);
        startTuto();
    }

    private void startTuto()
    {
        tutoBox.gameObject.SetActive(true);
        compteur = 0;
        afficheText();
    }

    public void conditionAfficheText()
    {
        if (isInTuto && !waitForMemorie && !waitForWord && !waitForUnfold)
        {
            if (compteur < tutoFr.Length)
            {
                afficheText();
            }
            else
            {
                tutoBox.SetActive(false);
                isInTuto = false;
            }
        }
    }

    private void afficheText()
    {
        if (GameManager.instance.infos.langue == "francais")
        {
            tutoText.text = tutoFr[compteur];
        }
        else
        {
            tutoText.text = tutoEn[compteur];
        }
        switch (compteur)
        {
            case 1:
                waitForMemorie = true;
                flecheSouvenirs.SetActive(true);
                flecheNextDialogue.SetActive(false);
                break;
            case 2:
                flecheSouvenirs.SetActive(false);
                waitForWord = true;
                flecheNextDialogue.SetActive(false);
                break;
            case 4:
                waitForUnfold = true;
                flecheNextDialogue.SetActive(false);
                break;
        }
        compteur += 1;
    }

    public void doTheThingWaited()
    {
        StartCoroutine("afficheTexteLater");
    }

    IEnumerator afficheTexteLater()
    {
        yield return new WaitForSeconds(1.0f);
        waitForMemorie = false;
        waitForWord = false;
        waitForUnfold = false;
        flecheNextDialogue.SetActive(true);
        afficheText();
    }

    public void changeLangue(string newLangue)
    {
        if (newLangue == "francais")
        {
            tutoText.text = tutoFr[compteur];
        }
        else
        {
            tutoText.text = tutoEn[compteur];
        }
    }

}
