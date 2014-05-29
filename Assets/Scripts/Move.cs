using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{

    private NavMeshAgent nav;
   // private OTSprite _sprite;
    private Cursor _cursor;

    // Use this for initialization
    void Start()
    {
        DoorInfosDatas di = GameController.Instance.doorInfosDatas;
        if (di != null)
        {
            GameObject obj = GameObject.Find(di.NameDoorOfNextScene);
            if (obj != null)
            {
                transform.position = new Vector3(obj.transform.position.x + di.DecalHorizontal, 3, obj.transform.position.z + di.DecalVertical);
            }
        }

        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;

        //_sprite = GetComponent<OTSprite>();
        _cursor = GameController.Instance.cursor;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.Finished == true)
            return;
        if (_cursor.LastElemTouched != null)
        {
            nav.destination = _cursor.LastContactPoint;
        }
        //_sprite.RotateTowards(new Vector2(nav.steeringTarget.x, nav.steeringTarget.z));
    }
}
