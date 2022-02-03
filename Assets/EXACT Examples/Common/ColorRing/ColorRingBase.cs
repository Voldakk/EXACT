using UnityEngine;

namespace Exact.Example
{
    public abstract class ColorRingBase : MonoBehaviour
    {
        public abstract void SetNumberOfSegments(int num);
        public abstract void SetUniformColor(Color color);
        public abstract void SetSegmentColor(int segment, Color color);
        public abstract Color GetColor(int segment);
        public abstract void SetIntensity(float intensity);
    }
}
