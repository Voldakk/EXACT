using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace Exact.Example
{
    public class ColorRingUI : ColorRingBase
    {
        [SerializeField]
        GameObject segmentPrefab;

        List<Image> segments = new List<Image>();

        public override void SetNumberOfSegments(int num)
        {
            if (num == segments.Count) { return; }
            else if (num > segments.Count)
            {
                for (int i = segments.Count; i < num; i++)
                {
                    segments.Add(Instantiate(segmentPrefab, transform, false).GetComponent<Image>());
                }
            }
            else
            {
                for (int i = segments.Count - 1; i >= num; i--)
                {
                    segments.RemoveAt(i);
                }
            }

            float fillAmount = 1.0f / num;
            float segmentSize = 360.0f / num;

            for (int i = 0; i < num; i++)
            {
                segments[i].fillAmount = fillAmount;
                segments[i].rectTransform.localRotation = Quaternion.AngleAxis(segmentSize * i, -Vector3.forward);
            }
        }

        public override void SetSegmentColor(int segment, Color color)
        {
            if (segment >= segments.Count)
            {
                Debug.LogError("Index out of range");
                return;
            }
            segments[segment].color = color;
        }

        public override void SetUniformColor(Color color)
        {
            foreach (var segment in segments)
            {
                segment.color = color;
            }
        }

        public override Color GetColor(int segment)
        {
            if (segment >= segments.Count)
            {
                Debug.LogError("Index out of range");
                return Color.black;
            }
            return segments[segment].color;
        }
    }
}
