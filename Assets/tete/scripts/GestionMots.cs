using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestionMots : MonoBehaviour
{

    // tableau des mots souvenirs du départ, savoir s'ils sont actifs ou a supprimer
    // Liste des mots souvenirs devenus mots slots ou pas

    public MotSouvenir motDragged = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
    public struct AllMots
    {
        public MotsInfos[] allMots;
        [Serializable]
        public struct MotsInfos
        {
            public int Id;
            public string texteFr;
            public string texteEn;
            public string texteTurnIntoFr;
            public string texteTurnIntoEn;
            public string motsAttendus;
            public int nombreDUtilisations;
            public float positionX;
            public float positionY;
            public float width;
            public bool isMotSlot;
            public bool isActive;
        }
    }
}
