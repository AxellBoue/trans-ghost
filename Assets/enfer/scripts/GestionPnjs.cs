using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class GestionPnjs : MonoBehaviour
{
    List<Pnj> allPnjs;

    // Start is called before the first frame update
    void Start()
    {
        allPnjs = new List<Pnj>(GetComponentsInChildren<Pnj>());
        allPnjs.Sort(comparePnjById);
        chargerPnj();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDialoguePnj(int pnjConcerne, int newNum)
    {
        allPnjs[pnjConcerne].infos.numDialogue = newNum;
    }

    private int comparePnjById(Pnj x, Pnj y)
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

    private void chargerPnj()
    {
        string path = Application.persistentDataPath + "/savePnj" + GameManager.instance.infos.numSauvegarde + ".json";
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            PnjBox box = JsonUtility.FromJson<PnjBox>(fileContent);
            PnjBox.PnjInfos[] allPnjInfos = box.allPnjs;
            int i = 0;
            foreach ( Pnj p in allPnjs)
            {
                p.infos.numDialogue = allPnjInfos[i].numDialogue;
                i++;
            }
        }
    }

    public void savePnjs()
    {
        string path = Application.persistentDataPath + "/savePnj" + GameManager.instance.infos.numSauvegarde + ".json";
        
        PnjBox.PnjInfos[] allPnjInfos = new PnjBox.PnjInfos[allPnjs.Count];
        int i = 0;
        foreach (Pnj p in allPnjs)
        {
            allPnjInfos[i] = p.infos;
            i++;
        }
        PnjBox box = new PnjBox();
        box.allPnjs = allPnjInfos;
        string allPnjJson = JsonUtility.ToJson(box);
        File.WriteAllText(path, allPnjJson, Encoding.UTF8);
    }

    [Serializable]
    public struct PnjBox
    {
        [Serializable]
        public struct PnjInfos
        {
            public int id;
            public int numDialogue;
        }
        public PnjInfos[] allPnjs;
    }

}
