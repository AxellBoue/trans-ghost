using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitIntro : MonoBehaviour
{

    private Vector3 vitesse;

    // Start is called before the first frame update
    void Start()
    {
        vitesse = new Vector3(0.0f, 100.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += vitesse * Time.deltaTime;
        if (transform.position.y >= 100)
        {
            Destroy(this.gameObject);
        }
    }
}
