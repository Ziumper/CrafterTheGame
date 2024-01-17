using UnityEngine;

namespace Crafter.Game
{
    public static class GameObjectExtensions
    {
        public static void RotateRandomAround(this GameObject target, GameObject center, float radius)
        {
            float angles = Random.value * 360;
            Vector3 position;
            position.x = center.transform.position.x + radius * Mathf.Sin(angles * Mathf.Deg2Rad);
            position.z = center.transform.position.z + radius * Mathf.Cos(angles * Mathf.Deg2Rad);
            position.y = center.transform.position.y + radius;

            Quaternion rottation = Quaternion.FromToRotation(Vector3.forward, center.transform.position - position);
            target.transform.position = position;
            target.transform.rotation = rottation;
        }
    }

}