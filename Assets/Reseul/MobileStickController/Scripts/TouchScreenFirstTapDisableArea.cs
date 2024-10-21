using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TouchScreenFirstTapDisableArea : MonoBehaviour
{
    private RectTransform _rectTransform;

    public Rect Area => _rectTransform.rect;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
}
