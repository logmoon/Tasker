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
    private Image ButtonImage => GetComponent<Image>();

    [SerializeField] public bool CanPress = true;

    [Header("Animation")]
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Vector2 from = new Vector2(.8f, .8f);
    [SerializeField] private Vector2 to = new Vector2(1.15f, 1.15f);
    [SerializeField] private Vector2 scaleDurationFromTo = new Vector2(.07f, .06f);
    [SerializeField] private float timeBtwScale = .1f;
    
    [Header("Hover Animation")]
    [SerializeField] private Vector2 hoverScale = new Vector2(1.05f, 1.05f);
    [SerializeField] private float hoverScaleDuration = 0.1f;
    [SerializeField] private float colorTintFactor = 0.1f;


    [Header("Button Sprites")]
    [SerializeField] private bool useImageSpriteAsIdle = true;
    [SerializeField] private Sprite buttonIdle;
    [SerializeField] private Sprite buttonHover;
    [SerializeField] private Sprite buttonPressed;

    [Header("Button Callbacks")]
    [SerializeField] private UnityEvent onButtonHover;
    [SerializeField] private UnityEvent onButtonPressed;
    [SerializeField] private UnityEvent onButtonExit;


    private bool isHovered = false;
    [HideInInspector]
    public bool IsClicking = false;
    
    private Color defaultButtonImageColor;
    private UITweener uiTweener;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (Application.isPlaying)
            return;

        transition = Selectable.Transition.None;
    }
#endif

    protected override void Awake()
    {
        base.Awake();
        defaultButtonImageColor = ButtonImage.color;
        ResetButton();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        uiTweener ??= new UITweener();
        isHovered = false;
        IsClicking = false;
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

    private Color ApplyFactor(Color c, float factor)
    {
        return new Color(
            c.r - factor,
            c.g - factor,
            c.b - factor,
            c.a + factor);
    }

    public void ResetButton()
    {
        if (IsClicking)
        {
            onButtonPressed?.Invoke();
            IsClicking = false;
        }

        if (useImageSpriteAsIdle && ButtonImage.sprite != null)
        {
            buttonIdle = ButtonImage.sprite;
            buttonHover = ButtonImage.sprite;
            buttonPressed = ButtonImage.sprite;
        }
        else if (buttonIdle != null)
        {
            ButtonImage.sprite = buttonIdle;
        }

        StopAllCoroutines();
        LeanTween.cancel(this.gameObject);
        this.gameObject.transform.localScale = isHovered ? hoverScale : Vector3.one;
        ButtonImage.color = isHovered ? ApplyFactor(defaultButtonImageColor, colorTintFactor) : defaultButtonImageColor;
    }

    #region Hover
    private void Hover()
    {
        if (IsClicking || isHovered == true) return;

        ResetButton();

        if (!interactable)
            return;

        onButtonHover?.Invoke();

        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }

        if (!useImageSpriteAsIdle && buttonHover != null)
        {
            ButtonImage.sprite = buttonHover;
        }

        ButtonImage.color = ApplyFactor(defaultButtonImageColor, colorTintFactor);
        uiTweener.Scale(gameObject, Vector2.one, hoverScale, hoverScaleDuration, easeCurve);

        AudioManager.Instance.PlayAudio(AudioType.BTN_HOVER);

        isHovered = true;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Hover();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        Hover();
    }
    #endregion


    #region Lose Hover
    private void LoseHover()
    {
        if (IsClicking || isHovered == false) return;

        ResetButton();

        if (!interactable)
            return;

        onButtonExit?.Invoke();

        if (EventSystem.current.currentSelectedGameObject == this.gameObject
            && !EventSystem.current.alreadySelecting)
            EventSystem.current.SetSelectedGameObject(null);
        
        ButtonImage.color = defaultButtonImageColor;
        uiTweener.Scale(gameObject, hoverScale, Vector2.one, hoverScaleDuration, easeCurve);

        isHovered = false;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        LoseHover();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        LoseHover();
    }
    #endregion


    #region Press
    private void Press()
    {
        if (!CanPress) return;

        ResetButton();

        if (!interactable)
            return;

        if (!useImageSpriteAsIdle && buttonPressed != null)
            ButtonImage.sprite = buttonPressed;

        IsClicking = true;
        StartCoroutine(uiTweener.ButtonClick(gameObject, from, to, hoverScale, timeBtwScale, scaleDurationFromTo, easeCurve, () => 
        {
            onButtonPressed?.Invoke();
            IsClicking = false;

            if (!useImageSpriteAsIdle && buttonHover != null)
                ButtonImage.sprite = buttonHover;

            ResetButton();

        }));
        
        AudioManager.Instance.PlayAudio(AudioType.BTN_CLICK);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Press();
    }
    public void OnSubmit(BaseEventData eventData)
    {
        Press();
    }
    #endregion

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
