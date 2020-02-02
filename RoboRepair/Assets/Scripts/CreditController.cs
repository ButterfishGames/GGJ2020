using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour
{
    public float initWaitTime;
    public float endWaitTime;
    public float scrollSpeed;
    public float endY;

    public RectTransform creditContent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RollCredits());
    }

    private IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(initWaitTime);
        while (creditContent.anchoredPosition.y < endY)
        {
            Vector3 newPos = creditContent.anchoredPosition;
            newPos += scrollSpeed * Vector3.up;
            newPos.y = Mathf.Clamp(newPos.y, -Mathf.Infinity, endY);

            creditContent.anchoredPosition = newPos;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(endWaitTime);
        LevelLoader.singleton.LoadScene(0);
    }
}
