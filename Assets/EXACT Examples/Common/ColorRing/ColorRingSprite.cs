using UnityEngine;

using System.Collections.Generic;

namespace Exact.Example
{
    public class ColorRingSprite : ColorRingBase
    {
        [SerializeField]
        GameObject segmentPrefab;

        [SerializeField]
        float ledSize = 0.4f;

        float intensity = 1.0f;
        List<SpriteRenderer> segments = new List<SpriteRenderer>();
        List<Color> segmentColors = new List<Color>();

        public override void SetNumberOfSegments(int num)
        {
            if (num == segments.Count) { return; }
            else if (num > segments.Count)
            {
                for (int i = segments.Count; i < num; i++)
                {
                    segments.Add(Instantiate(segmentPrefab, transform).GetComponent<SpriteRenderer>());
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

            float r = 0.5f - ledSize / 2;
            float segmentSize = Mathf.PI * 2 / num;

            for (int i = 0; i < num; i++)
            {
                float t = segmentSize * -i - Mathf.PI / 2;
                segments[i].transform.localPosition = new Vector3(Mathf.Cos(t) * r, Mathf.Sin(t) * r, -0.1f);
                segments[i].transform.localScale = Vector3.one * ledSize;
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
