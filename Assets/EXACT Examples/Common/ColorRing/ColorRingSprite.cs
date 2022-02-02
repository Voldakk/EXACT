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

        List<SpriteRenderer> segments = new List<SpriteRenderer>();

        public override void SetNumberOfSegments(int num)
        {
            if (num == segments.Count) { return; }
            else if (num > segments.Count)
            {
                for (int i = segments.Count; i < num; i++)
                {
                    segments.Add(Instantiate(segmentPrefab, transform).GetComponent<SpriteRenderer>());
                }
            }
            else
            {
                for (int i = segments.Count - 1; i >= num; i--)
                {
                    segments.RemoveAt(i);
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
