using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Orbitate (this Vector2 orbit)
    {
        return new Vector3 (orbit.x, 0, orbit.y);
    }
}

public static class UniverseHelper
{
    public static Vector3 Circular ()
    {
        return new Vector3 (1, 0, 1);
    }

}