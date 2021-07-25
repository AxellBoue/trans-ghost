using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class SouvenirBox : MonoBehaviour
{
    // pour afficher ou cacher la boite à souvenirs
    Vector2 positionBoiteAffichee;
    Vector2 positionBoiteCachee = new Vector2(0.0f,0.0f);
    public bool hidden = false;

    // pour bouger les souvenirs dans la liste avec les fleches droite gauche
    GameObject listeSouvenirsGO; // le game object qui contient les souvenirs dans la boite
    float positionInitialeListe;
    float positionMinListe; // recalculée au début et à chaque ajout/retrait de la liste
    float distanceBouge = 300f;

    // pour afficher les souvenirs à partir du json
    public GameObject souvenirPrefab;
    List<GameObject> souvenirsGO = new List<GameObject>();  // liste de tous les souvenirs actif et dans la boite à souvenirs
    SouvenirsAllList.souvenirInfo[] allSouvenirsInfos;
    SouvenirsAllList.souvenirInfo[] allSouvenirsToLoadInfos;
    public float espacementSouvenir = 175;
    public float position1erSouvenir = -423;

    // pour drag and drop uniquement si dans tête
    public bool isTete = false;

    // pour récupérer les souvenirs posés dans la tête pour les sauvegarder
    private GameObject boxSouvenirsTete;


    // Start is called before the first frame update
    void Start()
    {
        listeSouvenirsGO = GameObject.Find("ListeSouvenirs");

        TextAsset file = Resources.Load<TextAsset>("souvenirs");
        SouvenirBox.SouvenirsAllList bigListe = JsonUtility.FromJson<SouvenirsAllList>(file.text);
        allSouvenirsInfos = bigListe.souvenirs;

        // pour afficher et cacher la liste
        positionBoiteAffichee = GetComponent<RectTransform>().anchoredPosition;
        hide();

        // pour bouger les souvenirs dans la liste
        positionInitialeListe = listeSouvenirsGO.GetComponent<RectTransform>().localPosition.x;

        // pour récupérer les souvenirs dans la tête
        boxSouvenirsTete = GameObject.Find("BoxSouvenirsTete");

        // pour afficher les souvenirs à partir du json
        loadSouvenirs();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hide()
    {
        GetComponent<RectTransform>().anchoredPosition = positionBoiteCachee;
        hidden = true;
    }

    public void show()
    {
        GetComponent<RectTransform>().anchoredPosition = positionBoiteAffichee;
        hidden = false;
    }

    public void buttonHideAndShow()
    {
        if (hidden)
        {
            show();
        }
        else
        {
            hide();
        }
    }

    public void moveRight()
    {
        listeSouvenirsGO.GetComponent<RectTransform>().localPosition = new Vector3( Mathf.Clamp(listeSouvenirsGO.GetComponent<RectTransform>().localPosition.x - distanceBouge, positionMinListe, positionInitialeListe), listeSouvenirsGO.GetComponent<RectTransform>().localPosition.y, listeSouvenirsGO.GetComponent<RectTransform>().localPosition.z);
    }

    public void moveLeft()
    {
        listeSouvenirsGO.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Clamp(listeSouvenirsGO.GetComponent<RectTransform>().localPosition.x + distanceBouge, positionMinListe, positionInitialeListe), listeSouvenirsGO.GetComponent<RectTransform>().localPosition.y, listeSouvenirsGO.GetComponent<RectTransform>().localPosition.z);
    }


    private void loadSouvenirs()
    {
        // chrge le fichier json et met tout dans un struct qui contient un array de souvenirsinfos
        
        if (System.IO.File.Exists(Application.persistentDataPath + "/saveSouvenirsBox" + GameManager.instance.infos.numSauvegarde + ".json") )  // si le fichier de sauvegarde contient quelque chose, ça le charge
        {
            string fileSave = System.IO.File.ReadAllText(Application.persistentDataPath + "/saveSouvenirsBox" + GameManager.instance.infos.numSauvegarde + ".json");
            SouvenirsAllList bigListe = JsonUtility.FromJson<SouvenirsAllList>(fileSave);
            allSouvenirsToLoadInfos = bigListe.souvenirs;
        }
        else  // si le fichier de sauvegarde est vide (c'est le début de la partie) charge le fichier de base
        {
            allSouvenirsToLoadInfos = allSouvenirsInfos;
        }
        
        // cree un prefab pour chaque souvenir actif, le positionne, initialise ses valeurs et l'ajoure à la liste de go de souvenirs
        foreach (SouvenirsAllList.souvenirInfo s in allSouvenirsToLoadInfos)
        {
            if (s.actif)
            {
                GameObject newSouvenir = Instantiate(souvenirPrefab);
                newSouvenir.transform.SetParent(listeSouvenirsGO.transform);
                newSouvenir.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                //newSouvenir.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,160);
                //newSouvenir.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 175);
                newSouvenir.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
                newSouvenir.GetComponent<Souvenir>().infos = s;
                newSouvenir.GetComponent<Souvenir>().sceneIsTete = isTete;
                newSouvenir.GetComponent<Souvenir>().initialise(false);
                souvenirsGO.Add(newSouvenir);
            }
        }
        // positionne les game object de souvenir dans la liste
        organiseSouvenirs();

        // si dans la tete, charge les souvenirs de la tete
        if (isTete)
        {
            loadSouvenirsInTete();
        }
    }

    private void loadSouvenirsInTete()
    {
        SouvenirsAllList bigListe;
        if (System.IO.File.Exists(Application.persistentDataPath + "/saveSouvenirsTete"+GameManager.instance.infos.numSauvegarde+".json"))  // si le fichier de sauvegarde contient quelque chose, ça le charge
        {
            string fileSave = System.IO.File.ReadAllText(Application.persistentDataPath + "/saveSouvenirsTete" + GameManager.instance.infos.numSauvegarde + ".json");
            bigListe = JsonUtility.FromJson<SouvenirsAllList>(fileSave);

            foreach (SouvenirsAllList.souvenirInfo s in bigListe.souvenirs)
            {
                GameObject newSouvenir = Instantiate(souvenirPrefab);
                newSouvenir.transform.SetParent(listeSouvenirsGO.transform); // passe par l'autre canvas pour pas que la taille fasse nimp ??? pas réussi à faire autrement
                newSouvenir.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
                newSouvenir.transform.SetParent(boxSouvenirsTete.transform);
                newSouvenir.GetComponent<Souvenir>().infos = s;
                newSouvenir.transform.position = new Vector3(newSouvenir.GetComponent<Souvenir>().infos.positionX, newSouvenir.GetComponent<Souvenir>().infos.positionY, 0.0f);
                newSouvenir.GetComponent<Souvenir>().sceneIsTete = true;
                newSouvenir.GetComponent<Souvenir>().dansTete = true;
                newSouvenir.GetComponent<Souvenir>().initialise(true);
            }
        }
    }


    // enleve le souvenir de la blo a souvenirs
    public void removeSouvenir(GameObject s)
    {
        souvenirsGO.Remove(s);
        s.GetComponent<Souvenir>().infos.actif = false;
        organiseSouvenirs();
    }

    // remet le souvenir dans la boite
    public void rerangeSouvenir(GameObject s)  
    {
        s.GetComponent<Souvenir>().infos.actif = true;
        souvenirsGO.Add(s);
        rangeSouvenirs();
        organiseSouvenirs();
    }

    public IEnumerator gagneSouvenirLater(int numNewSouvenir)
    {
        show();
        yield return new WaitForSeconds(0.3f);
        focusSurNum(numNewSouvenir);
        yield return new WaitForSeconds(0.5f);
        if (!checkSiOnADejaSouvenir(numNewSouvenir))
        {
            gagneSouvenir(numNewSouvenir);
        }
    }


    private bool checkSiOnADejaSouvenir(int numSouvenir)
    {
        bool retour = false;
        foreach (GameObject go in souvenirsGO)
        {
            if (go.GetComponent<Souvenir>().infos.id == numSouvenir)
            {
                retour = true;
            }
        }
        return retour;
    }


    private void gagneSouvenir(int numSouvenir)
    {
        GameObject newSouvenir = Instantiate(souvenirPrefab);
        newSouvenir.transform.SetParent(listeSouvenirsGO.transform);
        newSouvenir.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        newSouvenir.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
        newSouvenir.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 175);
        newSouvenir.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
        newSouvenir.GetComponent<Souvenir>().infos = allSouvenirsInfos[numSouvenir];
        newSouvenir.GetComponent<Souvenir>().infos.actif = true;
        newSouvenir.GetComponent<Souvenir>().initialise(false);
        souvenirsGO.Add(newSouvenir);
        rangeSouvenirs();
        organiseSouvenirs();
    }


    private void focusSurNum(int num)
    {
        int compteur = 0;
        foreach (GameObject go in souvenirsGO) // compte le nombre de souvenirs qui seront avant dals la liste
        {
            if (go.GetComponent<Souvenir>().infos.id < num)
            {
                compteur ++;
            }
        }
        float newX = Mathf.Clamp(position1erSouvenir + compteur * espacementSouvenir - Camera.main.scaledPixelWidth / 2.0f,positionMinListe,positionInitialeListe);
        listeSouvenirsGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(newX, listeSouvenirsGO.GetComponent<RectTransform>().anchoredPosition.y) ;
    }


    // trier l'ordre de la liste par la propriété info.id
    private void rangeSouvenirs()
    {
        souvenirsGO.Sort(compareSouvenirsById);
    }

    private int compareSouvenirsById(GameObject x, GameObject y)
    {
        if (x.GetComponent<Souvenir>().infos.id > y.GetComponent<Souvenir>().infos.id)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    private void organiseSouvenirs() // met les souvenirs à la bonne position en fonction de leur position dans la liste
    {
        int i = 0;
        foreach (GameObject go in souvenirsGO)
        {
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(position1erSouvenir + espacementSouvenir * i, 0.0f, 0.0f);
            go.GetComponent<Souvenir>().numDansListe = i;
            i++;
        }
        positionMinListe = positionInitialeListe - espacementSouvenir * (Mathf.Clamp(souvenirsGO.Count-2,1,100));
    }


    public void saveSouvenirs()
    {
        // sauvegarde les souvenirs dans la boite a souvenirs
        SouvenirsAllList listeASauverInBox = new SouvenirsAllList();
        SouvenirsAllList.souvenirInfo[] souvenirInfoListe = new SouvenirsAllList.souvenirInfo[souvenirsGO.Count];
        int k = 0;
        foreach (GameObject go in souvenirsGO)
        {
            souvenirInfoListe[k] = go.GetComponent<Souvenir>().infos;
            k++;
        }
        listeASauverInBox.souvenirs = souvenirInfoListe;
        string souvenirsInBox = JsonUtility.ToJson(listeASauverInBox);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/saveSouvenirsBox" + GameManager.instance.infos.numSauvegarde + ".json", souvenirsInBox, Encoding.UTF8);

        // sauvegarde les souvenirs placés dans la tete si on est dans la scene tête
        if (isTete)
        {
            Souvenir[] souvenirsDansTete = boxSouvenirsTete.transform.GetComponentsInChildren<Souvenir>();
            SouvenirsAllList.souvenirInfo[] infosSouvenirsDansTete = new SouvenirsAllList.souvenirInfo[souvenirsDansTete.Length];
            int l = 0;
            foreach (Souvenir s in souvenirsDansTete)
            {
                infosSouvenirsDansTete[l] = s.infos;
                l++;
            }
            SouvenirsAllList listeASauverInTete = new SouvenirsAllList();
            listeASauverInTete.souvenirs = infosSouvenirsDansTete;
            string souvenirsInTete = JsonUtility.ToJson(listeASauverInTete);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/saveSouvenirsTete" + GameManager.instance.infos.numSauvegarde + ".json", souvenirsInTete,Encoding.UTF8);
        }
    }


    [Serializable]
    public struct SouvenirsAllList
    {
        [Serializable]
        public struct souvenirInfo
        {
            public int id;
            public bool actif;
            public float positionX;
            public float positionY;
            public string sName;
            public string texte_fr;
            public string texte_en;
            public string phraseName;
            public int[] numUtilisationMot;
            public int[] numDejaUtilises;
        }
        public souvenirInfo[] souvenirs;
    }


}
