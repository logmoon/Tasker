using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public static readonly string FLAG_ON = "On";
    public static readonly string FLAG_OFF = "Off";
    public static readonly string FLAG_NONE = "None";

    [Header("Menu Settings")]
    public MenuIndexes index;

    public GameObject objectToSelectOnEnable;

    [Header("Menu Animation")]
    [SerializeField] private bool useAnimation;
    [SerializeField] private UIAnimationType animationType;

    [SerializeField] private Vector2 from;
    [SerializeField] private Vector2 to = new Vector2(1, 0);
    [SerializeField] private float animationTime;
    [SerializeField] private AnimationCurve easeCurve;
    private UITweener uiTweener;
    
    private bool _isOn;

    public bool isOn
    {
        get
        {
            return _isOn;
        }
        private set
        {
            _isOn = value;
        }
    }
    
    public string targetState { get; private set; }

    private void OnEnable()
    {
        uiTweener ??= new UITweener();
        
        uiTweener.UITweeningComplete += AnimationComplete;
    }

    private void OnDisable()
    {
        uiTweener.UITweeningComplete -= AnimationComplete;
    }

    public void Animate(bool on)
    {
        if (useAnimation)
        {
            Vector2 _from = on ? from : to;
            Vector2 _to = on ? to : from;
            
            switch (animationType)
            {
                case UIAnimationType.Fade:
                    uiTweener.Fade(gameObject, _from.x, _to.x, animationTime, easeCurve);
                    break;
                case UIAnimationType.Scale:
                    uiTweener.Scale(gameObject, _from, _to, animationTime, easeCurve);
                    break;
            }
            
            targetState = on ? FLAG_ON : FLAG_OFF;
        }
        else
        {
            if (!on)
            {
                gameObject.SetActive(false);
                isOn = false;
            }
            else
            {
                isOn = true;
            }
        }
    }

    private void AnimationComplete()
    {
        bool on = (targetState == FLAG_ON) ? true : false;
        
        targetState = FLAG_NONE;

        Log("Menu ["+index+"] finished transitioning to " + (on ? "on" : "off"));

        if (!on)
        {
            isOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            isOn = true;
            if (EventSystem.current != null && !EventSystem.current.alreadySelecting)
            {
                EventSystem.current.SetSelectedGameObject(objectToSelectOnEnable);
            }
        }
    }


    private void Log(string msg)
    {
        if (!MenuSettings.Get().debugSystem) return;
        Debug.Log("[Menu Data]: " + msg);
    }
    private void LogWarning(string msg)
    {
        if (!MenuSettings.Get().debugSystem) return;
        Debug.LogWarning("[Menu Data]: " + msg);
    }
}
