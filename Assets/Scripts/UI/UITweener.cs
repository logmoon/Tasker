using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class UITweener
{
    public event Action UITweeningComplete;
    private Action _onComplete;

    public void Scale(GameObject obj, Vector2 from, Vector2 to, float time, AnimationCurve ease, Action onComplete = null)
    {
        _onComplete = onComplete;
        
        LeanTween.cancel(obj);
        obj.transform.localScale = from;
        LeanTween.scale(obj, to, time).setIgnoreTimeScale(true).setEase(ease).setOnComplete(OnComplete);
    }

    public void Fade(GameObject obj, float from, float to, float time, AnimationCurve ease, Action onComplete = null)
    {
        _onComplete = onComplete;
        
        CanvasGroup cg = null;

        if (!obj.GetComponent<CanvasGroup>())
            cg = obj.AddComponent<CanvasGroup>();
        else
            cg = obj.GetComponent<CanvasGroup>();

        LeanTween.cancel(obj);
        cg.alpha = from;
        LeanTween.alphaCanvas(cg, to, time).setIgnoreTimeScale(true).setEase(ease).setOnComplete(OnComplete);
    }

    
    public IEnumerator ButtonClick(GameObject obj, Vector2 from, Vector2 to, Vector2 reset, float timeBtwScaling, Vector2 scaleDuration, AnimationCurve ease, Action onComplete = null)
    {
        Scale(obj, from, to, scaleDuration.x, ease);
        yield return new WaitForSecondsRealtime(timeBtwScaling);
        Scale(obj, to, reset, scaleDuration.y, ease, onComplete);
    }

    public void TintColor(GameObject obj, Color from, Color to, float time, AnimationCurve ease, Action onComplete = null)
    {
        _onComplete = onComplete;
        
        Image image = obj.GetComponent<Image>();
        if (image == null)
            return;
            
        LeanTween.cancel(obj);
        image.color = from;
        LeanTween.color(obj, to, time).setIgnoreTimeScale(true).setEase(ease).setOnComplete(OnComplete);
    }
    
    private void OnComplete()
    {
        _onComplete?.Invoke();
        UITweeningComplete?.Invoke();
    }
}

public enum UIAnimationType
{
    Fade,
    Scale
}
