using System.Collections;
using UnityEngine;

public static class Lerper
{
    public static void LerpFloat(this MonoBehaviour mb,float start, float target, float lerpDuration, System.Action<float> action, bool fixedUpdate, AnimationCurve curve = null){
        mb.StartCoroutine(LerpFloatRoutine(start,target,lerpDuration,action,fixedUpdate,curve));
    }

    public static void LerpVector(this MonoBehaviour mb,Vector3 start, Vector3 target, float lerpDuration, System.Action<Vector3> action, AnimationCurve curve = null){
        mb.StartCoroutine(LerpVectorRoutine(start,target,lerpDuration,action,curve));
    }

    static IEnumerator LerpFloatRoutine(float start, float target, float lerpDuration, System.Action<float> action, bool fixedUpdate, AnimationCurve curve = null){
        float timeElapsed = 0f;
        float currentValue;
        float t = 0f;

        while(timeElapsed < lerpDuration){
            t = curve != null ? curve.Evaluate(timeElapsed / lerpDuration) : (timeElapsed / lerpDuration);
            currentValue = Mathf.Lerp(start,target, t);
            action(currentValue);
            timeElapsed += Time.deltaTime;
            if(fixedUpdate) yield return new WaitForFixedUpdate();
            else yield return null;  //stop the execution of the coroutine until the next frame
        }
        currentValue = target;  //because timeElapsed/lerpDuration will never be equal to 1
        action(currentValue);
    }

    static IEnumerator LerpVectorRoutine(Vector3 start, Vector3 target, float lerpDuration, System.Action<Vector3> action, AnimationCurve curve = null){
        float timeElapsed = 0f;
        Vector3 currentValue;
        float t = 0f;

        while(timeElapsed < lerpDuration){
            t = curve != null ? curve.Evaluate(timeElapsed / lerpDuration) : (timeElapsed / lerpDuration);
            currentValue = Vector3.Lerp(start,target, curve.Evaluate(timeElapsed / lerpDuration));
            action(currentValue);
            timeElapsed += Time.deltaTime;
            yield return null;  //stop the execution of the coroutine until the next frame
        }
        currentValue = target;  //because timeElapsed/lerpDuration will never be equal to 1
    }
}
