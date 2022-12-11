using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenLogic : MonoBehaviour
{
    private int _finalHeartsAmount;
    // Start is called before the first frame update
    void Start()
    {
        _finalHeartsAmount = PlayerPrefs.HasKey("Hearts") ? PlayerPrefs.GetInt("Hearts") : 0;

        if (_finalHeartsAmount >= 50)
        {
            GameObject.Find("Canvas/Backgrounds/Dragon").SetActive(true);
        } else if (_finalHeartsAmount >= 25)
        {
            GameObject.Find("Canvas/Backgrounds/Cat").SetActive(true);
        }
        else
        {
            GameObject.Find("Canvas/Backgrounds/Rat").SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
