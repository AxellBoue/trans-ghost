using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    private Text textBox;
    private Text nameBox;
    private Image imageBox;
    private GameObject questionsBox;
    private RectTransform flecheQuestions;

    private Player perso;
    private SouvenirBox boxSouvenirs;

    private GameObject interagir;

    private TextAsset file;
    private DialoguesFile tousLesDialogues;

    private GestionPnjs gestionPnj;

    private string langue = "fr";
    private DialoguesFile.dialogueDiscussion.dialoguePhrase[] dialogueEnCours;
    private int i = 0;
    private bool[] questionsDejaPosees = new bool[] { false, false, false, false };
    private int numQuestions = 0;  //numéto des questions dans le fichier dialogue json
    bool inQuestion = false;

    //navigation entre les questions
    int currentQuestionNum = 0;
    List<GameObject> ListeQuestions = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // lien vers tous les trucs à modifier
        textBox = transform.GetChild(1).GetComponent<Text>();
        nameBox = transform.GetChild(2).GetComponent<Text>();
        imageBox = transform.GetChild(0).GetComponent<Image>();
        questionsBox = transform.GetChild(4).gameObject;
        flecheQuestions = questionsBox.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
        gestionPnj = GameObject.FindObjectOfType<GestionPnjs>();

        perso = GameObject.FindObjectOfType<Player>();
        boxSouvenirs = GameObject.FindObjectOfType<SouvenirBox>();

        interagir = transform.parent.GetChild(1).gameObject;

        // charge le fichier texte
        file = Resources.Load<TextAsset>("dialogues");
        tousLesDialogues = JsonUtility.FromJson<DialoguesFile>(file.text);


    }

    // Update is called once per frame
    void Update()
    {
        if (inQuestion && boxSouvenirs.hidden)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetButtonDown("bas"))
            {
                currentQuestionNum += 1;
                if (currentQuestionNum >= ListeQuestions.Count)
                {
                    currentQuestionNum = 0;
                }
                selectQuestion(currentQuestionNum);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("haut"))
                {
                currentQuestionNum -= 1;
                if (currentQuestionNum < 0)
                {
                    currentQuestionNum = ListeQuestions.Count -1;
                }
                selectQuestion(currentQuestionNum);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                int num = (int)Char.GetNumericValue(ListeQuestions[currentQuestionNum].gameObject.name.ToCharArray()[5]);
                choisiQuestion(num);
            }
        }
    }

    public void parler(int idDialogue)
    {
        if (boxSouvenirs.hidden)
        {
            if (i == 0)
            {
                startDialogue(idDialogue);
                if (!perso.haveHadTuto)
                {
                    perso.haveHadTuto = true;
                    perso.hideTuto();
                }
            }
            else
            {
                if (!inQuestion)
                {
                    nextPhrase();
                }
                else
                {
                    int num = (int)Char.GetNumericValue(ListeQuestions[currentQuestionNum].gameObject.name.ToCharArray()[5]);
                    choisiQuestion(num);
                }
            }
        }
    }

    private void startDialogue(int idDialogue)
    {
        perso.setBloque(true);
        dialogueEnCours = tousLesDialogues.allDialogues[idDialogue].textes;
        // a factoriser / renvoyer a la fonction nextPhrase parce que la on peut pas commencer avec une question
        if (langue == "fr")
        {
            textBox.text = dialogueEnCours[i].texte_fr;
        }
        else
        {
            textBox.text = dialogueEnCours[i].texte_en;
        }
        nameBox.text = dialogueEnCours[i].name;
        imageBox.sprite = Resources.Load<Sprite>("portraits/" + dialogueEnCours[i].name);
        gameObject.SetActive(true);
        interagir.SetActive(false);
        i++;
    }

    private void nextPhrase(bool fromQuestion = false)
    {
        if (i >= dialogueEnCours.Length)
        {
            closeBox();
            return;
        }
        else // si on n'est pas à la fin du dialogue
        {
            if (dialogueEnCours[i].name == "gagne souvenir")
            {
                boxSouvenirs.StartCoroutine("gagneSouvenirLater", int.Parse(dialogueEnCours[i].texte_fr));
                i++;
                nextPhrase();
                return;
            }
            else if (dialogueEnCours[i].name == "change num dialogue")
            {
                gestionPnj.changeDialoguePnj(int.Parse(dialogueEnCours[i].texte_fr), int.Parse(dialogueEnCours[i].texte_en));
                i++;
                nextPhrase();
                return;
            }
            else if (dialogueEnCours[i].texte_fr == "back to question")
            {
                i = numQuestions;
                if (questionsDejaPosees == new bool[] { true, true, true, true })
                {
                    i = dialogueEnCours[numQuestions].questions_fr[0].finishSendTo;
                }
                nextPhrase(true);
                return;
            }
            else if (dialogueEnCours[i].name == "change retour questions")
            {
                numQuestions = int.Parse(dialogueEnCours[i].texte_fr); // texte_fr est le nouveau numéro
                questionsDejaPosees[int.Parse(dialogueEnCours[i].texte_en)] = false; // texte_en est le numéro de la question rajoutée qu'il faut repasser en true
                i++;
                nextPhrase();
                return;

            }
            else if (dialogueEnCours[i].questions_fr.Length == 0) // si c'est un dialogue normal sans questions
            {
                if (langue == "fr")
                {
                    textBox.text = dialogueEnCours[i].texte_fr;
                }
                else
                {
                    textBox.text = dialogueEnCours[i].texte_en;
                }
                nameBox.text = dialogueEnCours[i].name;
                imageBox.sprite = Resources.Load<Sprite>("portraits/" + dialogueEnCours[i].name);
                i++;
            }
            else // si c'est des questions
            {
                inQuestion = true;
                ListeQuestions = new List<GameObject>();
                if (!fromQuestion)
                {
                    questionsDejaPosees = new bool[] { false, false, false, false };
                    numQuestions = i;
                }
                textBox.text = " ";
                nameBox.text = dialogueEnCours[i].name;
                questionsBox.SetActive(true);
                string[] questionsTextes;
                if (langue == "fr")
                {
                    questionsTextes = new string[] { dialogueEnCours[i].questions_fr[0].question1, dialogueEnCours[i].questions_fr[0].question2, dialogueEnCours[i].questions_fr[0].question3, dialogueEnCours[i].questions_fr[0].question4 };
                }
                else
                {
                    questionsTextes = new string[] { dialogueEnCours[i].questions_en[0].question1, dialogueEnCours[i].questions_en[0].question2, dialogueEnCours[i].questions_en[0].question3, dialogueEnCours[i].questions_en[0].question4 };
                }
                for (int j = 0; j < 4; j++)
                {
                    if (questionsTextes[j] == "")
                    {
                        questionsDejaPosees[j] = true;
                    }
                    if (questionsDejaPosees[j] == true) // si c'était true de base ou si la question n'a pas de texte, n'affiche pas le boutton
                    {
                        questionsBox.transform.GetChild(j).gameObject.SetActive(false);
                    }
                    else
                    {
                        questionsBox.transform.GetChild(j).gameObject.SetActive(true);
                        questionsBox.transform.GetChild(j).GetComponentInChildren<Text>().text = questionsTextes[j];
                        ListeQuestions.Add(questionsBox.transform.GetChild(j).gameObject);
                        questionsBox.transform.GetChild(j).GetComponent<BoutonQuestion>().num = ListeQuestions.Count-1;
                    }
                }
                if (ListeQuestions.Count != 0)
                {
                   selectQuestion(0);
                }
            }

        }
    }


    public void choisiQuestion(int numQuestion)
    {
        int[] goTos = new int[] { dialogueEnCours[i].questions_fr[0].goTo1, dialogueEnCours[i].questions_fr[0].goTo2, dialogueEnCours[i].questions_fr[0].goTo3, dialogueEnCours[i].questions_fr[0].goTo4 };
        i = goTos[numQuestion];
        questionsDejaPosees[numQuestion] = true;
        questionsBox.SetActive(false);
        inQuestion = false;
        nextPhrase();
    }

    public void selectQuestion(int numQuestion)  // en passant la souris dessus ou en naviguant avec clavier
    {
        currentQuestionNum = numQuestion;
        flecheQuestions.anchoredPosition = new Vector2(flecheQuestions.anchoredPosition.x, ListeQuestions[numQuestion].GetComponent<RectTransform>().anchoredPosition.y);
    }


    public void closeBox()
    {
        i = 0;
        gameObject.SetActive(false);
        perso.setBloque(false);
        interagir.SetActive(true);
    }

    [Serializable]
    public struct DialoguesFile
    {
        [Serializable]
        public struct dialogueDiscussion
        {
            public int id;
            public dialoguePhrase[] textes;

            [Serializable]
            public struct dialoguePhrase
            {
                public string name;
                public string texte_fr;
                public string texte_en;
                public questions[] questions_fr;
                public questions[] questions_en;

                [Serializable] 
                public struct questions
                {
                    public string question1;
                    public int goTo1;
                    public string question2;
                    public int goTo2;
                    public string question3;
                    public int goTo3;
                    public string question4;
                    public int goTo4;
                    public int finishSendTo;
                }

            }
        }
        public dialogueDiscussion[] allDialogues;
    }

    
}
