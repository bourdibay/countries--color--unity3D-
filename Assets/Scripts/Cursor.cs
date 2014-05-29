using UnityEngine;
using System.Collections;
using System;

public class Cursor : MonoBehaviour
{
    public Texture2D crosshair = null;

    private Vector3 _posCursor;
    public Vector3 PosCursor
    {
        get { return _posCursor; }
    }

    private GameObject _lastElemTouched = null;
    public GameObject LastElemTouched
    {
        get { return _lastElemTouched; }
    }
    private Vector3 _lastContactPoint = Vector3.zero;
    public Vector3 LastContactPoint
    {
        get { return _lastContactPoint; }
    }

    private Rect pos;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Screen.showCursor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.Finished == true)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = getObjectDetected();
            if (obj != null)
            {
                _lastElemTouched = obj;
            }
        }
    }

    private GameObject getObjectDetected()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 2000, Color.green);
        if (Physics.Raycast(ray, out hit, 2000.0f))
        {
            _lastContactPoint = hit.point;
            return hit.transform.gameObject;
        }
        return null;
    }

    void OnGUI()
    {
        GUI.depth = -10; // priority max pour etre devant les boutons

        if (crosshair != null)
        {
            _posCursor = new Vector3(Input.mousePosition.x - (crosshair.width / 2), 0.0f,
                            (Screen.height - Input.mousePosition.y) - (crosshair.height / 2));
            pos = new Rect(Input.mousePosition.x - (crosshair.width / 2),
                            (Screen.height - Input.mousePosition.y) - (crosshair.height / 2),
                            crosshair.width,
                            crosshair.height);
            GUI.DrawTexture(pos, crosshair);
        }
    }
}
