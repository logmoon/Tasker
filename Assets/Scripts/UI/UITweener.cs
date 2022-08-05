using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UITweener
{
    public event Action UITweeningComplete;
    private Action _onComplete;

    public void Scale(GameObject obj, Vector2 from, Vector2 to, float time, AnimationCurve ease, Action onComplete = null)
    {
        if (onComplete != null)
            _onComplete = onComplete;
        
        LeanTween.cancel(obj);
        obj.transform.localScale = from;
        LeanTween.scale(obj, to, time).setEase(ease).setOnComplete(OnComplete);
    }

    public void Fade(GameObject obj, float from, float to, float time, AnimationCurve ease, Action onComplete = null)
    {
        if (onComplete != null)
            _onComplete = onComplete;
        
        CanvasGroup cg = null;

        if (!obj.GetComponent<CanvasGroup>())
            cg = obj.AddComponent<CanvasGroup>();
        else
            cg = obj.GetComponent<CanvasGroup>();

        LeanTween.cancel(obj);
        cg.alpha = from;
        LeanTween.alphaCanvas(cg, to, time).setEase(ease).setOnComplete(OnComplete);
    }

    
    public IEnumerator ButtonClick(GameObject obj, Vector2 from, Vector2 to, float timeBtwScaling, Vector2 scaleDuration, AnimationCurve ease, Action onComplete = null)
    {
        Scale(obj, from, to, scaleDuration.x, ease);
        yield return new WaitForSeconds(timeBtwScaling);
        Scale(obj, to, new Vector2(1f, 1f), scaleDuration.y, ease, onComplete);
    }

    private void OnComplete()
    {
        if (_onComplete != null)
            _onComplete?.Invoke();
        
        UITweeningComplete?.Invoke();
    }
}

public enum UIAnimationType
{
    Fade,
    Scale
}
