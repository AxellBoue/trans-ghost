using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    private BoxCollider2D bounds;
    private Camera cam;
    Vector2 cameraSize;

    // Start is called before the first frame update
    void Start()
    {
        bounds = GameObject.Find("CameraBounds").GetComponent<BoxCollider2D>();
        cam = GetComponent<Camera>();
        cameraSize = new Vector2(cam.orthographicSize * Screen.width / Screen.height, cam.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x,bounds.bounds.min.x + cameraSize.x, bounds.bounds.max.x - cameraSize.x), Mathf.Clamp(target.position.y, bounds.bounds.min.y + cameraSize.y, bounds.bounds.max.y- cameraSize.y),transform.position.z);
    }
}
