﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifestealDice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStopped()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int diceRoll = Random.Range(1, 5);

        GameObject.Find("ClickTheDice").GetComponent<Text>().text = "DICE VALUES: ";

        FreezeOnRoll(diceRoll);

        if (DiceManager.APFound() == false)
        {

            GameObject ap = DiceManager.FindEmptyTypeTotal();
            ap.GetComponent<Text>().text = "AP: ";
            ap.transform.GetChild(0).GetComponent<Text>().enabled = true;

            if (!TurnManager.GetCurrTurnCharacter().tag.Contains("Enemy"))
            {
                GameObject.Find("ConfirmAttackButton").GetComponent<CustomButton>().Enable();
            }

            DiceManager.SetAPFound(true);

        }

        string rollValueName = "RollValue" + Dice.LastDiceClicked().name.ToCharArray()[Dice.LastDiceClicked().name.Length - 1];
        GameObject.Find(rollValueName).GetComponent<Text>().text = diceRoll.ToString();

        int rollTotal = int.Parse(GameObject.Find("RollTotal").GetComponent<Text>().text);
        rollTotal += diceRoll;
        GameObject.Find("RollTotal").GetComponent<Text>().text = rollTotal.ToString();
        GameObject.Find("RollTotal").GetComponent<Text>().enabled = true;

        int attackTotal = int.Parse(DiceManager.FindTypeTotalGameObject("AP").transform.GetChild(0).GetComponent<Text>().text);
        attackTotal += diceRoll;
        DiceManager.FindTypeTotalGameObject("AP").transform.GetChild(0).GetComponent<Text>().text = attackTotal.ToString();

        if (diceRoll == 1)
        {
            DiceManager.DisableAllButtons();
            GameObject.Find("ConfirmAttackButton").GetComponent<CustomButton>().Disable();
            GameObject.Find("AttackMissed").GetComponent<BillboardMessage>().ShowMessage();
        }
    }

    private void FreezeOnRoll(int num)
    {
        Dice.LastDiceClicked().GetComponent<Animator>().enabled = false;

        DiceType myDiceType = DiceManager.SearchDiceType("LifeSteal");

        List<Sprite> freezeSprites = myDiceType.GetFreezeSprites();

        Sprite freezeSprite = null;

        foreach (Sprite s in freezeSprites)
        {
            if (s.name.Contains(num.ToString()))
            {
                freezeSprite = s;
            }
        }

        Dice.LastDiceClicked().GetComponent<Image>().sprite = freezeSprite;
    }

    public void OnAttack()
    {
        if (DiceManager.FindTypeTotalGameObject("AP") != null)
        {
            //Calculate health returned from lifesteal (half of damage dealt)
            int attackTotal = int.Parse(DiceManager.FindTypeTotalGameObject("AP").transform.GetChild(0).GetComponent<Text>().text);
            int lifeSteal = (int)(Mathf.Round(attackTotal * 0.5f));

            //Raise health back up
            GameObject healthCanvas = Utilities.SearchChild("HealthCanvas", TurnManager.GetCurrTurnCharacter());
            GameObject healthBar = Utilities.SearchChild("HealthBar", healthCanvas);

            healthBar.GetComponent<HealthBar>().ChangeHealth(lifeSteal);

            //Indicate health change with UI lettering
            GameObject lifeStealCanvas = Utilities.SearchChild("LifeStealCanvas", TurnManager.GetCurrTurnCharacter());
            GameObject lifeStealIndicator = Utilities.SearchChild("LifestealIndicator", lifeStealCanvas);
            lifeStealIndicator.GetComponent<LifeStealIndicator>().ShowLifestealChange(lifeSteal);
        }
    }
}