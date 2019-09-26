using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader : MonoBehaviour
{

    public float fadeAmount;

    private bool _fadeIn;
    private bool _fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadeIn)
        {
            if (this.GetComponent<AudioSource>().volume < 1)
            {
                this.GetComponent<AudioSource>().volume += fadeAmount;
            } else
            {
                _fadeIn = false;
            }
        } else if (_fadeOut)
        {
            if (this.GetComponent<AudioSource>().volume > 0)
            {
                this.GetComponent<AudioSource>().volume -= fadeAmount;
            } else
            {
                _fadeOut = false;
            }
        }
    }

    public void FadeIn()
    {
        _fadeIn = true;
    }

    public void FadeOut()
    {
        _fadeOut = true;
    }
}
