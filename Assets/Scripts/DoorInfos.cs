using UnityEngine;
using System.Collections;

public class DoorInfos : MonoBehaviour {

    public string nameDoorOfNextScene;
    public float decalHorizontal;
    public float decalVertical;
}

public class DoorInfosDatas
{
    private string _nameDoor;
    private float _decaH;
    private float _decaV;

    public string NameDoorOfNextScene { get { return _nameDoor; } }
    public float DecalHorizontal { get { return _decaH; } }
    public float DecalVertical { get { return _decaV; } }

    public DoorInfosDatas(DoorInfosDatas cpy)
    {
        _nameDoor = cpy.NameDoorOfNextScene;
        _decaH = cpy.DecalHorizontal;
        _decaV = cpy.DecalVertical;
    }
    public DoorInfosDatas(DoorInfos cpy)
    {
        _nameDoor = cpy.nameDoorOfNextScene;
        _decaH = cpy.decalHorizontal;
        _decaV = cpy.decalVertical;
    }
}
