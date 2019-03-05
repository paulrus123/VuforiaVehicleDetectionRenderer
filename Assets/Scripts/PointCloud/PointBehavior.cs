using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBehavior : MonoBehaviour
{
    float fadeTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Color _startColor = Random.ColorHSV();
        Color _endColor = new Color(_startColor.r, _startColor.g, _startColor.b, 0.0f);

        StartCoroutine(FadeOut(fadeTime, _startColor, _endColor));
        Destroy(gameObject, fadeTime);
    }

    //https://answers.unity.com/questions/1230671/how-to-fade-out-a-game-object-using-c.html
    private IEnumerator FadeOut(float duration, Color startCol, Color endCol)
    {
        float lerpStart_Time = Time.time;
        float lerpProgress;
        bool lerping = true;
        while (lerping)
        {
            yield return new WaitForEndOfFrame();
            lerpProgress = Time.time - lerpStart_Time;

            GetComponent<MeshRenderer>().material.color = Color.Lerp(startCol, endCol, lerpProgress / duration);

            if (lerpProgress >= duration)
            {
                lerping = false;
            }
        }
        yield break;
    }
}
