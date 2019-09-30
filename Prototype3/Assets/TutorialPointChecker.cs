using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPointChecker : MonoBehaviour
{

    public string poisonPopupHeading;
    public string poisonPopupMessage;

    private static bool _hasShownPoisonDicePopup;

    // Start is called before the first frame update
    void Start()
    {
        _hasShownPoisonDicePopup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasShownPoisonDicePopup)
        {
            if (SceneManager.GetActiveScene().name.Contains("TinyDiceDungeonCombat"))
            {
                if (TurnManager.GetCurrTurnCharacter().tag.Contains("Enemy"))
                {
                    GameObject.Find("TutorialCanvas_Basic").GetComponent<TutorialCanvasBasic>().MakeTutorialPopup(poisonPopupHeading, poisonPopupMessage);
                    _hasShownPoisonDicePopup = true;
                }
            }
        }
    }
}
