using UnityEngine;
using System.Collections;

namespace Utility
{
    public static class VectorConverter
    {

        public static Vector2 ToV2(this Vector3 v) => new(v.x, v.y);
        public static Vector2 ToV2(this Vector2Int v) => new((float)v.x, (float)v.y);
        public static Vector2 ToV2(this Vector3Int v) => new((float)v.x, (float)v.y);

        public static Vector3 ToV3(this Vector2 v) => new(v.x, v.y, 0);
        public static Vector3 ToV3(this Vector2Int v) => new((float)v.x, (float)v.y, 0.0f);
        public static Vector3 ToV3(this Vector3Int v) => new((float)v.x, (float)v.y, (float)v.z);

        public static Vector3Int ToV3Int(this Vector3 v) => new((int)v.x, (int)v.y, (int)v.z);
        public static Vector3Int ToV3Int(this Vector2 v) => new((int)v.x, (int)v.y, 0);
        public static Vector3Int ToV3Int(this Vector2Int v) => new((int)v.x, (int)v.y, 0);

        public static Vector2Int ToV2Int(this Vector2 v) => new((int)v.x, (int)v.y);
        public static Vector2Int ToV2Int(this Vector3 v) => new((int)v.x, (int)v.y);
        public static Vector2Int ToV2Int(this Vector3Int v) => new((int)v.x, (int)v.y);

    }
}