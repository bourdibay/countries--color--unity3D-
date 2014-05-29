using UnityEngine;
using System.Collections;

public class MainCharacter : MonoBehaviour
{

    public string tagCollider = "entry";

    void Start()
    {
        GameController.Instance.displayMessage();
    }

    void OnCollisionEnter(Collision hit)
    {
        string tag = hit.contacts[0].otherCollider.tag;
        if (tag == tagCollider)
        {

            DoorInfos di = hit.gameObject.GetComponent<DoorInfos>();
            Entry en = hit.gameObject.GetComponent<Entry>();
            if (di != null)
            {
                GameController.Instance.doorInfosDatas = new DoorInfosDatas(di);
                if (en != null)
                {
                    if (GameController.Instance.isValidColor(en.color) == true)
                    {
                        GameController.Instance.setColor(en.color);
                        if (GameController.Instance.completeColor() == true)
                        {
                            GameController.Instance.Score += 10;
                            GameController.Instance.initColor();
                            GameController.Instance.regenerateIndexCountries();
                        }
                    }
                    else
                    {
                        GameController.Instance.initColor();
                    }
                }
                else
                    Debug.LogError("Error: no entry set");
                Application.LoadLevel("game");
            }
            else
                Debug.LogError("Error: exit does not hav a DoorInfos script attached.");
        }
        else if (tag == "exit")
        {
            Debug.Log("QUIT");
            Application.Quit();
        }

    }
}
