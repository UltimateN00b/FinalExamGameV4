using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryTransitions : MonoBehaviour
{
    public float growthRate = 5f;

    private bool _growFromCenter;
    private bool _shrinkFromCenter;

    private Vector3 _originalScale;

    // Start is called before the first frame update
    void Start()
    {
        _growFromCenter = false;
        _shrinkFromCenter = false;

        _originalScale = this.GetComponent<RectTransform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_growFromCenter)
        {
            if (this.GetComponent<RectTransform>().localScale.x < _originalScale.x)
            {
                this.GetComponent<RectTransform>().localScale += new Vector3(growthRate, growthRate, growthRate) * Time.deltaTime;
            }
            else if (this.GetComponent<RectTransform>().localScale.x > _originalScale.x)
            {
                this.GetComponent<RectTransform>().localScale = _originalScale;
                _growFromCenter = false;
            }
            else
            {
                _growFromCenter = false;
            }
        }

        if (_shrinkFromCenter)
        {
            if (this.GetComponent<RectTransform>().localScale.x > 0)
            {
                this.GetComponent<RectTransform>().localScale -= new Vector3(growthRate, growthRate, growthRate) * Time.deltaTime;
            }
            else if (this.GetComponent<RectTransform>().localScale.x <= 0)
            {
                this.GetComponent<RectTransform>().localScale = Vector3.zero;
                _shrinkFromCenter = false;
            }
            else
            {
                _shrinkFromCenter = false;
            }
        }
    }

    public void GrowFromCenter()
    {
        _growFromCenter = true;
        this.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void ShrinkFromCenter()
    {
        _shrinkFromCenter = true;
        this.GetComponent<RectTransform>().localScale = _originalScale;
    }
}
