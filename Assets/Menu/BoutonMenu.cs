using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BoutonMenu : MonoBehaviour, IPointerEnterHandler
{
    private Menu menu;
    private MenuInGame menuInGame;
    public Vector2 decalage = new Vector2(-170.0f, 0.0f);
    public int id = 0;
    private string scene;
    public bool reverseFleche = false;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene().name;
        if (scene == "menuDepart")
        {
            menu = GameObject.FindObjectOfType<Menu>();
        }
        else
        {
            menuInGame = GameObject.FindObjectOfType<MenuInGame>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (scene == "menuDepart")
        {
            menu.selectBouton(this);
        }
        else
        {
            menuInGame.selectBouton(this);
        }
    }
}
