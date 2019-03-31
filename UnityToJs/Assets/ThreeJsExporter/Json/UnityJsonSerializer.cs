using UnityEngine;

namespace Json
{
    public class UnityJsonSerializer: IJsonSerializer
    {
        public string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj, true);
        }
    }
}