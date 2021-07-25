using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class GestionPensees : MonoBehaviour
{
    List<PenseeOuvrable> tableauPensees;

    // Start is called before the first frame update
    void Start()
    {
        tableauPensees = new List<PenseeOuvrable>(GameObject.FindObjectsOfType<PenseeOuvrable>());
        tableauPensees.Sort(comparePenseesParId);
        loadPensees();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int comparePenseesParId(PenseeOuvrable x, PenseeOuvrable y)
    {
        if (x.infos.id > y.infos.id)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void loadPensees()
    {
        string path = Application.persistentDataPath + "/savePensees" + GameManager.instance.infos.numSauvegarde + ".json";
        if (!File.Exists(path)) // début : cache toutes les pensées sauf la première
        {
            foreach (PenseeOuvrable p in tableauPensees)
            {
                if (p.infos.id != 0)
                {
                    p.infos.actif = false;
                }
                p.infos.isOuverte = false;
                p.initialise();
            }
        }
        else
        {
            string fileContent = File.ReadAllText(path);
            AllPensees fileJson = JsonUtility.FromJson<AllPensees>(fileContent);
            int i = 0;
            foreach (PenseeOuvrable p in tableauPensees)
            {
                p.infos = fileJson.penseeArray[i];
                p.initialise();
                i++;
            }
        }
    }

    public void savePensees()
    {
        AllPensees penseeBox = new AllPensees();
        penseeBox.penseeArray = new AllPensees.PenseeInfos[tableauPensees.Count];
        int i = 0;
        //foreach (AllPensees.PenseeInfos p in penseeBox.penseeArray)
        foreach(PenseeOuvrable p in tableauPensees)
        {
            p.infos.actif = tableauPensees[i].gameObject.activeInHierarchy;
            penseeBox.penseeArray[i] = p.infos;
            i++;
        }
        string infosJson = JsonUtility.ToJson(penseeBox);
        string path = Application.persistentDataPath + "/savePensees" + GameManager.instance.infos.numSauvegarde + ".json";
        File.WriteAllText(path, infosJson, Encoding.UTF8);
    }


    [Serializable]
    public struct AllPensees
    {
        public PenseeInfos[] penseeArray;

        [Serializable]
        public struct PenseeInfos
        {
            public bool actif;
            public int id;
            public bool isOuverte;
        }
    }

}
