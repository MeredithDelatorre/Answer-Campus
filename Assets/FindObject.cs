using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObject : MonoBehaviour
{
    public GameObject[] foundObjects;
    // Start is called before the first frame update

    public void Found(ItemOutcome outcome)
    {
        Debug.Log("Clicked on " + outcome.title);
        gameObject.SetActive(false);

    }

}
