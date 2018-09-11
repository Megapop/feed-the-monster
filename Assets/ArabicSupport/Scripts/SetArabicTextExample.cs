using UnityEngine;
using System.Collections;
using ArabicSupport;

public class SetArabicTextExample : MonoBehaviour
{
    public string text;

    void Start()
    {
        gameObject.GetComponent<GUIText>().text = "This sentence (wrong display):\n" + text +
            "\n\nWill appear correctly as:\n" + ArabicFixer.Fix(text, false, false);
    }
}
