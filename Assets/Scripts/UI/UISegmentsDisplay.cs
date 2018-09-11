using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class SegmentDisplay
{
    public Sprite SegmentEmpty;
    public Sprite SegmentFull;
}

public class UISegmentsDisplay : MonoBehaviour
{
    //public SegmentDisplay[] segmentsDisplays;

    public Sprite SegmentEmpty;
    public Sprite SegmentFull;


    public void Init(int numSegments)
    {

    }

    public void Fill(int index)
    {
        transform.GetChild(index).gameObject.GetComponent<Image>().sprite = SegmentFull;
    }
}
