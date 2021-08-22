using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExtrasScript : MonoBehaviour
{
    public GameObject extrasButton;

    // Update is called once per frame
    void Update()
    {
        if (BattleRating.GotSRank)
        {
            extrasButton.gameObject.SetActive(true);
        }
        else
        {
            extrasButton.gameObject.SetActive(false);
        }
    }
}
