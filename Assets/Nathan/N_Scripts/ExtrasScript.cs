using UnityEngine;

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
