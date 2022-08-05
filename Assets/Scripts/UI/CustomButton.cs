using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class CustomButton : Selectable, IPointerClickHandler, ISubmitHandler
{
    private Image buttonImage => GetComponent<Image>();

    [Header("Animation")]
    [SerializeField] private AnimationCurve easeCurve;
    [SerializeField] private Vector2 from = new Vector2(.8f, .8f);
    [SerializeField] private Vector2 to = new Vector2(1.2f, 1.2f);
    [SerializeField] private Vector2 scaleDurationFromTo;
    [SerializeField] private float timeBtwScale = .1f;


    [Header("Button Sprites")]
    [SerializeField] private bool useImageSpriteAsIdle = false;
    [SerializeField] private Sprite buttonIdle;
    [SerializeField] private Sprite buttonHover;
    [SerializeField] private Sprite buttonPressed;

    [Header("Button Callbacks")]
    [SerializeField] private UnityEvent onButtonHover;
    [SerializeField] private UnityEvent onButtonPressed;
    [SerializeField] private UnityEvent onButtonExit;


    [HideInInspector]
    public bool isSelected = false;
    
    private UITweener uiTweener;


    protected override void Awake()
    {
        base.Awake();

        ResetButton();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        uiTweener ??= new UITweener();
        ResetButton();
    }

    public void AddListener(Action action)
    {
        UnityAction unityAction = new UnityAction(action);
        onButtonPressed.AddListener(unityAction);
    }

    public void RemoveAllListeners()
    {
        onButtonPressed = null;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        ResetButton();

        isSelected = true;

        if (!interactable)
            return;

        if (onButtonHover != null)
        {
            onButtonHover.Invoke();
        }

        if (!useImageSpriteAsIdle && buttonHover != null)
        {
            buttonImage.sprite = buttonHover;
        }

        // TODO: Play sound effect
    }

    public override void OnSelect(BaseEventData eventData)
    {
        ResetButton();

        if (!interactable)
            return;

        if (onButtonHover != null)
        {
            onButtonHover.Invoke();
        }

        if (!useImageSpriteAsIdle && buttonHover != null)
        {
            buttonImage.sprite = buttonHover;
        }
        
        //if (!isSelected)
        // TODO: Play sound effect
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        ResetButton();

        if (!interactable)
            return;

        if (onButtonExit != null)
        {
            onButtonExit.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetButton();

        if (!interactable)
            return;

        if (!useImageSpriteAsIdle && buttonPressed != null)
            buttonImage.sprite = buttonPressed;

        StartCoroutine(uiTweener.ButtonClick(gameObject, from, to, timeBtwScale, scaleDurationFromTo, easeCurve, () => 
        {
            onButtonPressed?.Invoke();

            if (!useImageSpriteAsIdle && buttonHover != null)
                buttonImage.sprite = buttonHover;

            ResetButton();

        }));
        
        // TODO: Play sound effect
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        ResetButton();
        isSelected = false;

        if (!interactable)
            return;

        if (onButtonExit != null)
        {
            onButtonExit.Invoke();
        }
    }



    public void ResetButton()
    {
        if (useImageSpriteAsIdle && buttonImage.sprite != null)
        {
            buttonIdle = buttonImage.sprite;
            buttonHover = buttonImage.sprite;
            buttonPressed = buttonImage.sprite;
        }
        else if (buttonIdle != null)
        {
            buttonImage.sprite = buttonIdle;
        }

        LeanTween.scale(this.gameObject, new Vector3(1, 1, 1), 0);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        ResetButton();

        if (!interactable)
            return;

        if (!useImageSpriteAsIdle && buttonPressed != null)
            buttonImage.sprite = buttonPressed;

        StartCoroutine(uiTweener.ButtonClick(gameObject, from, to, timeBtwScale, scaleDurationFromTo, easeCurve, () => 
        {
            onButtonPressed?.Invoke();

            if (!useImageSpriteAsIdle && buttonHover != null)
                buttonImage.sprite = buttonHover;

            ResetButton();

        }));
        
        // TODO: Play sound effect
    }

    public void SetInteractable(bool state)
    {
        interactable = state;

        ResetButton();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
