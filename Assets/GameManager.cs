using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public infosGameManager infos;
    
    private GestionPnjs gestionPnj;
    private SouvenirBox souvenirBox;
    private GestionPensees gestionPensees;

    void Awake() {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
            string pathLastSave = Application.persistentDataPath + "/saveGeneralLast.json";
            if (File.Exists(pathLastSave)){
                string fileContent = File.ReadAllText(pathLastSave);
                infos = JsonUtility.FromJson<infosGameManager>(fileContent);
                infos.scene = SceneManager.GetActiveScene().name;
            }
            else {
                initialiseOptions();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toTete()
    {
        if (infos.scene == "enferMain")
        {
            gestionPnj = GameObject.FindObjectOfType<GestionPnjs>();
            gestionPnj.savePnjs();
        }
        if (infos.scene == "tete_main" || infos.scene == "enferMain")
        {
            souvenirBox = GameObject.FindObjectOfType<SouvenirBox>();
            souvenirBox.saveSouvenirs();
        }

        if (infos.hasBeenToTete)
        {
            infos.scene = "tete_main";
            SceneManager.LoadScene("tete_main");
        }
        else
        {
            infos.scene = "entreDansTete";
            infos.hasBeenToTete = true;
            SceneManager.LoadScene("entreDansTete");
        }
    }

    public void toEnfer()
    {
        if (infos.scene == "tete_main")
        {
            souvenirBox = GameObject.FindObjectOfType<SouvenirBox>();
            souvenirBox.saveSouvenirs();
        }
        infos.scene = "enferMain";
        SceneManager.LoadScene("enferMain");
    }

    public void save()
    {
        if (infos.scene == "enferMain")
        {
            gestionPnj = GameObject.FindObjectOfType<GestionPnjs>();
            gestionPnj.savePnjs();
        }
        if (infos.scene == "tete_main")
        {
            gestionPensees = GameObject.FindObjectOfType<GestionPensees>();
            gestionPensees.savePensees();
        }
        if (infos.scene == "tete_main" || infos.scene == "enferMain")
        {
            souvenirBox = GameObject.FindObjectOfType<SouvenirBox>();
            souvenirBox.saveSouvenirs();
        }
        saveOptions();
    }

    private void saveOptions()
    {
        string path = Application.persistentDataPath + "/saveGeneral" + infos.numSauvegarde + ".json";
        infosGameManager saveObject = infos;
        File.WriteAllText(path, JsonUtility.ToJson(saveObject), Encoding.UTF8);

        string pathLastSave = Application.persistentDataPath + "/saveGeneralLast.json";
        File.WriteAllText(pathLastSave, JsonUtility.ToJson(saveObject), Encoding.UTF8);
    }

    public void saveGeneralOptions()
    {
        infosGameManager saveObject = infos;
        string pathLastSave = Application.persistentDataPath + "/saveGeneralLast.json";
        File.WriteAllText(pathLastSave, JsonUtility.ToJson(saveObject), Encoding.UTF8);
    }

    public void loadOptions()
    {
        string path = Application.persistentDataPath + "/saveGeneral" + infos.numSauvegarde + ".json";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            infos = JsonUtility.FromJson<infosGameManager>(fileContent);
        } 
    }

    public void initialiseOptions()
    {
        infos.langue = "francais";
        infos.numSauvegarde = "1";
        infos.clavier = "Azerty";
        infos.tailleUI = "Moyenne";
        infos.scene = SceneManager.GetActiveScene().name;
        infos.hasBeenToTete = false;
    }

    [Serializable]
    public struct infosGameManager
    {
        public string langue;
        public string numSauvegarde ;
        public string clavier;
        public string tailleUI;
        public string scene;
        public bool hasBeenToTete;
    }

}
