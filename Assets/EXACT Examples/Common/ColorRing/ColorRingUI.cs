using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace Exact.Example
{
    public class ColorRingUI : ColorRingBase
    {
        [SerializeField]
        GameObject segmentPrefab;

        float intensity = 1.0f;
        List<Image> segments = new List<Image>();
        List<Color> segmentColors = new List<Color>();

        public override void SetNumberOfSegments(int num)
        {
            if (num == segments.Count) { return; }
            else if (num > segments.Count)
            {
                for (int i = segments.Count; i < num; i++)
                {
                    segments.Add(Instantiate(segmentPrefab, transform, false).GetComponent<Image>());
                    segmentColors.Add(Color.black);
                }
            }
            else
            {
                for (int i = segments.Count - 1; i >= num; i--)
                {
                    segments.RemoveAt(i);
                    segmentColors.RemoveAt(i);
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
            segmentColors[segment] = color;
            color *= intensity;
            color.a = 1;
            segments[segment].color = color;
        }

        public override void SetUniformColor(Color color)
        {
            Color displayColor = color * intensity;
            displayColor.a = 1;

            for (int i = 0; i < segments.Count; i++)
            {
                segmentColors[i] = color;
                segments[i].color = displayColor;
            }
        }

        public override Color GetColor(int segment)
        {
            if (segment >= segments.Count)
            {
                Debug.LogError("Index out of range");
                return Color.black;
            }
            return segmentColors[segment];
        }

        public override void SetIntensity(float intensity)
        {
            this.intensity = intensity;

            for (int i = 0; i < segments.Count; i++)
            {
                Color displayColor = segmentColors[i] * intensity;
                displayColor.a = 1;
                segments[i].color = displayColor;
            }
        }
    }
}
