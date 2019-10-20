using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDay()
    {
        Utilities.SearchChild("DayNum", this.gameObject).GetComponent<Text>().text = NextDaySceneStarter.GetDayNum().ToString();
    }

    public void UpdateAyandaSleep()
    {
        UpdateDay();

        AyandaFeels ayandaFeels = Utilities.SearchChild("AyandaFeels", this.gameObject).GetComponent<AyandaFeels>();
        AyandaPortrait ayandaPortrait = Utilities.SearchChild("AyandaPortrait", this.gameObject).GetComponent<AyandaPortrait>();

        float sleepMeterValue = GameObject.Find("SleepMeter").GetComponent<Slider>().value;

        string sleepString = "";

        if (sleepMeterValue <= 1 && sleepMeterValue >= 0.8f)
        {
            sleepString = "Energised";
        } else if (sleepMeterValue <= 0.79f && sleepMeterValue >= 0.6f)
        {
            sleepString = "Rested";
        }
        else if (sleepMeterValue <= 0.59f && sleepMeterValue >= 0.4f)
        {
            sleepString = "Awake...ish";
        }
        else if (sleepMeterValue <= 0.39f && sleepMeterValue >= 0.2f)
        {
            sleepString = "Tired";
        }
        else
        {
            sleepString = "Exhausted";
        }

        ayandaFeels.ChangeFeeling(sleepString);

        if (!sleepString.Equals("Awake...ish")) //The sprite name probably shouldn't include an ellipses - so this check is just for safety purposes 
        {
            ayandaPortrait.ChangeSprite(sleepString);
        } else
        {
            ayandaPortrait.ChangeSprite("Awakeish");
        }
    }
}
