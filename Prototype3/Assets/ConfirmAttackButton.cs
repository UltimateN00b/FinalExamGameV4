using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmAttackButton : MonoBehaviour
{
    public float speed;
    public float animationWaitTime;

    private bool _startAttack;

    private bool _hasMovedToAttackPos;
    private bool _hasPlayedAttackAnim;

    private float _animationTimer;

    private Vector3 _originalPos;

    private GameObject _uiPlayer;
    private GameObject _uiSkinPlayer;
    private GameObject _uiEnemy;
    private GameObject _uiSkinEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        _uiPlayer = GameObject.Find("UIPlayer");
        _uiSkinPlayer = GameObject.Find("UISkinPlayer");
        _uiEnemy = GameObject.Find("UIEnemy");
        _uiSkinEnemy = GameObject.Find("UISkinEnemy");

        _uiEnemy.SetActive(false);
        _uiSkinEnemy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_startAttack)
        {
            List<Character> targets = DiceManager.GetCurrTargets();
            float step = speed * Time.deltaTime; // calculate distance to move

            if (!_hasMovedToAttackPos)
            {
                Vector3 targetPos = GameObject.Find("AttackTarget").transform.position;
                targetPos.y = DiceManager.GetCurrCharacter().gameObject.transform.position.y;
                targetPos.z = DiceManager.GetCurrCharacter().gameObject.transform.position.z;

                if (Vector3.Distance(DiceManager.GetCurrCharacter().gameObject.transform.position, targetPos) > 0.1f)
                {
                    DiceManager.GetCurrCharacter().gameObject.transform.position = Vector3.MoveTowards(DiceManager.GetCurrCharacter().gameObject.transform.position, targetPos, step);
                }
                else
                {
                    foreach (Character c in targets)
                    {
                        //Manage damage animations
                        if (c.tag.Contains("Player"))
                        {
                            GameObject.Find("Ayanda").GetComponent<Animator>().SetBool("takingDamage", true);
                        } else
                        {
                            GameObject.Find("AyandaMonster").GetComponent<Animator>().SetBool("takingDamage", true);
                        }

                        //Manage particle effects
                        AudioManager.PlaySound(Resources.Load("Explosions") as AudioClip);

                        GameObject effectsAnimator = Utilities.SearchChild("EffectsAnimator", c.gameObject);
                        effectsAnimator.GetComponent<SpriteRenderer>().enabled = true;
                        effectsAnimator.GetComponent<Animator>().enabled = true;
                    }
                    _hasMovedToAttackPos = true;

                }
            }
            else if (!_hasPlayedAttackAnim)
            {
                _animationTimer += Time.deltaTime;

                if (_animationTimer >= animationWaitTime)
                {
                    _animationTimer = 0.0f;


                    Character currChar = DiceManager.GetCurrCharacter();
                    DiceManager.ExecuteAttack();

                    for (int i = 0; i < GameObject.Find("DiceCanvas").transform.childCount; i++)
                    {
                        GameObject currChild = GameObject.Find("DiceCanvas").transform.GetChild(i).gameObject;

                        if (currChild.activeInHierarchy)
                        {
                            currChild.GetComponent<Dice>().InvokeOnAttackEvent();
                        }
                    }

                    _hasPlayedAttackAnim = true;
                }
            }
            else
            {
                DiceManager.GetCurrCharacter().gameObject.transform.position = Vector3.MoveTowards(DiceManager.GetCurrCharacter().gameObject.transform.position, _originalPos, step);

                if (Vector3.Distance(DiceManager.GetCurrCharacter().gameObject.transform.position, _originalPos) > 0.1f)
                {
                    DiceManager.GetCurrCharacter().gameObject.transform.position = Vector3.MoveTowards(DiceManager.GetCurrCharacter().gameObject.transform.position, _originalPos, step);
                }
                else
                {

                    _startAttack = false;
                    _hasMovedToAttackPos = false;
                    _hasPlayedAttackAnim = false;

                    //VERY IMPORTANT MAKE SURE THIS LINE IS IN EVERY ABILITY!!!
                    TurnManager.FinishAttack();
                }
            }
        }
    }

    public void ConfirmAttack()
    {
        this.GetComponent<CustomButton>().Disable();

        if (DiceManager.GetCurrTargets().Count <= 0)
        {
            DiceManager.AutoSetTargets();
        }

        if (DiceManager.GetCurrTargets().Count > 0)
        {
            _originalPos = TurnManager.GetCurrTurnCharacter().gameObject.transform.position;

            _startAttack = true;

            DiceManager.CurrCombatStage = DiceManager.CombatStage.ExecutingAttack;

            GameObject diceCanvas = GameObject.Find("DiceCanvas");

            //Manage attack animations
            if (TurnManager.GetCurrTurnCharacter().tag.Contains("Player"))
            {
                GameObject.Find("Ayanda").GetComponent<Animator>().SetBool("attack", true);
            }
            else
            {
                GameObject.Find("AyandaMonster").GetComponent<Animator>().SetBool("attack", true);
            }
        }
    }

    public void HideUI()
    {
        DiceManager.ClearAllDiceTotals();

        if (TurnManager.GetCurrTurnCharacter().tag.Contains("Player"))
        {
            _uiSkinPlayer.SetActive(false);
            _uiPlayer.SetActive(false);

            this.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            _uiSkinEnemy.SetActive(false);
            _uiEnemy.SetActive(false);
        }
    }

    public void ShowUI()
    {
        if (TurnManager.GetCurrTurnCharacter().tag.Contains("Player"))
        {
            _uiSkinPlayer.SetActive(true);
            _uiPlayer.SetActive(true);

            this.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            _uiSkinEnemy.SetActive(true);
            _uiEnemy.SetActive(true);
        }

        for (int i = 0; i < GameObject.Find("DiceCanvas").transform.childCount; i++)
        {
            if (GameObject.Find("DiceCanvas").transform.GetChild(i).gameObject.activeInHierarchy)
            {
                GameObject.Find("DiceCanvas").transform.GetChild(i).GetComponent<ShakeObject>().Shake();
                GameObject.Find("DiceCanvas").transform.GetChild(i).GetComponent<Animator>().enabled = true;
                string questionMarkName = "QuestionMark" + GameObject.Find("DiceCanvas").transform.GetChild(i).name.ToCharArray()[GameObject.Find("DiceCanvas").transform.GetChild(i).name.Length - 1];
                GameObject.Find(questionMarkName).GetComponent<Image>().enabled = true;
            }
        }
    }
}
