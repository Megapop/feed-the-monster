using UnityEngine;
using System.Collections;
using ArabicSupport;

public class Fix3dTextCS : MonoBehaviour
{
    public string text;
    public bool tashkeel = true;
    public bool hinduNumbers = true;

    void Start()
    {
        gameObject.GetComponent<TextMesh>().text = ArabicFixer.Fix(text, tashkeel, hinduNumbers);
    }
}
