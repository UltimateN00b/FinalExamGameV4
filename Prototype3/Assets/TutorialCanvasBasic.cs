using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvasBasic : MonoBehaviour
{
    private bool _hiding;
    private bool _showing;

    // Start is called before the first frame update
    void Start()
    {
        ResetObjects();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject popupHeading = Utilities.SearchChild("PopupHeading", this.gameObject);

        if (Input.GetKeyDown(KeyCode.E))
        {
            MakeTutorialPopup("Poison Dice", "Your enemy has poison dice. These dice will deal damage at the start of every turn.");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            HideTutorialPopup();
        }

        if (_showing)
        {
            if (popupHeading.GetComponent<Text>().color.a >= 1)
            {
                Time.timeScale = 0;
                _showing = false;
            }
        }

        if (_hiding)
        {
            if (popupHeading.GetComponent<Text>().color.a <= 0)
            {
                Time.timeScale = 1;
                _hiding = false;
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }
                ResetObjects();
            }
        }
    }

    public void MakeTutorialPopup(string heading, string body)
    {
        ResetObjects();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }

        GameObject faderBG = Utilities.SearchChild("FaderBG", this.gameObject);
        GameObject popupBG = Utilities.SearchChild("PopupBG", this.gameObject);
        GameObject popupHeading = Utilities.SearchChild("PopupHeading", this.gameObject);
        GameObject popupBody = Utilities.SearchChild("PopupBody", this.gameObject);

        faderBG.GetComponent<MyUIFade>().FadeIn();
        popupBG.GetComponent<EntryTransitions>().GrowFromCenter();

        popupHeading.GetComponent<Text>().text = heading;
        popupBody.GetComponent<Text>().text = body;

        popupHeading.GetComponent<MyUIFade>().FadeIn();
        popupBody.GetComponent<MyUIFade>().FadeIn();

        _showing = true;
    }

    public void HideTutorialPopup()
    {
        Time.timeScale = 1;

        GameObject faderBG = Utilities.SearchChild("FaderBG", this.gameObject);
        GameObject popupBG = Utilities.SearchChild("PopupBG", this.gameObject);
        GameObject popupHeading = Utilities.SearchChild("PopupHeading", this.gameObject);
        GameObject popupBody = Utilities.SearchChild("PopupBody", this.gameObject);

        faderBG.GetComponent<MyUIFade>().FadeOut();
        popupBG.GetComponent<EntryTransitions>().ShrinkFromCenter();

        popupHeading.GetComponent<MyUIFade>().FadeOut();
        popupBody.GetComponent<MyUIFade>().FadeOut();

        _hiding = true;
    }

    private void ResetObjects()
    {
        GameObject faderBG = Utilities.SearchChild("FaderBG", this.gameObject);
        GameObject popupBG = Utilities.SearchChild("PopupBG", this.gameObject);
        GameObject popupHeading = Utilities.SearchChild("PopupHeading", this.gameObject);
        GameObject popupBody = Utilities.SearchChild("PopupBody", this.gameObject);

        Color faderBGColor = faderBG.GetComponent<Image>().color;
        faderBGColor.a = 0;
        faderBG.GetComponent<Image>().color = faderBGColor;

        popupBG.GetComponent<RectTransform>().localScale = Vector3.zero;

        Color popupHeadingColor = popupHeading.GetComponent<Text>().color;
        popupHeadingColor.a = 0;
        popupHeading.GetComponent<Text>().color = popupHeadingColor;

        Color popupBodyColor = popupBody.GetComponent<Text>().color;
        popupBodyColor.a = 0;
        popupBody.GetComponent<Text>().color = popupBodyColor;

        _hiding = false;
        _showing = false;
    }
}
