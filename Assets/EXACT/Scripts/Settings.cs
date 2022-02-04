using UnityEngine;

namespace Exact
{
    [CreateAssetMenu(menuName = "EXACT/Settings", fileName = "EXACTSettings")]
    public class Settings : ScriptableObject
    {
        public string host = "127.0.0.1";
        public int port;
    }
}
