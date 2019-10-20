using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextDaySceneStarter : MonoBehaviour
{
    private static int _numDays;

    // Start is called before the first frame update
    void Start()
    {
        _numDays = 1;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name.Contains("TheNextDay"))
        {
            _numDays++;
        }
    }

    public static void NextDay()
    {
        GameObject.Find("FadeCanvas 1").GetComponent<FadeCanvasLegacy>().ChangeScene("TheNextDay" + _numDays);
    }

    public static int GetDayNum()
    {
        return _numDays;
    }
}
