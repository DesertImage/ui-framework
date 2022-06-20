using UnityEngine;

namespace DesertImage.Extensions
{
    public static class VectorExtensions
    {
        public static float AngleBetweenToDots(this object sender, Vector2 dot1, Vector2 dot2)
        {
            return Mathf.Atan2(dot2.y - dot1.y, dot2.x - dot1.x) * 180 / Mathf.PI;
        }

        public static Vector3 GetRandomOffset(this Vector3 vector, float minOffset = -.5f, float maxOffset = .5f)
        {
            return new Vector3(vector.x + Random.Range(minOffset, maxOffset),
                vector.y + Random.Range(minOffset, maxOffset), vector.z + Random.Range(minOffset, maxOffset));
        }

        public static Vector2 GetRandomOffset(this Vector2 vector, float minOffset = -1f, float maxOffset = 1f)
        {
            return new Vector2(vector.x + Random.Range(minOffset, maxOffset),
                vector.y + Random.Range(minOffset, maxOffset));
        }

        public static float GetDistanceTo(this Transform transform, Transform targetTransform)
        {
            return (transform.position - targetTransform.position).sqrMagnitude;
        }

        public static float GetDistanceTo(this Vector3 vector, Vector3 targetPositiion)
        {
            // return (vector - targetPositiion).sqrMagnitude;
            return Vector3.Distance(vector, targetPositiion);
        }

        public static float GetXDistance(this Vector3 vector, Vector3 targetVector)
        {
            // return Mathf.Abs(vector.x - targetVector.x);
            return Vector3.Distance(vector, targetVector);
        }

        public static void SetYPosition(this Transform transform, float value)
        {
            var position = transform.position;

            transform.position = new Vector3(position.x, value, position.z);
        }

        public static Vector3 SetX(ref this Vector3 vector, float value)
        {
            return new Vector3(value, vector.y, vector.z);
        }

        public static Vector3 SetY(ref this Vector3 vector, float value)
        {
            return new Vector3(vector.x, value, vector.z);
        }

        public static Vector3 SetZ(ref this Vector3 vector, float value)
        {
            return new Vector3(vector.x, vector.y, value);
        }

        public static Vector3 SetX(ref this Vector2 vector, float value)
        {
            return new Vector3(value, vector.y);
        }

        public static Vector3 SetY(ref this Vector2 vector, float value)
        {
            return new Vector3(vector.x, value);
        }

        public static Vector3 WithZeroY(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0f, vector3.z);
        }

        public static Vector3 WithY(this Vector3 vector3, float value)
        {
            return new Vector3(vector3.x, value, vector3.z);
        }

        public static Vector3 WithX(this Vector3 vector3, float value)
        {
            return new Vector3(value, vector3.y, vector3.z);
        }

        public static Vector3 WithX(this Vector2 vector2, float value)
        {
            return new Vector3(value, vector2.y);
        }

        public static Vector3 WithY(this Vector2 vector2, float value)
        {
            return new Vector3(vector2.x, value);
        }

        public static Vector3 WithZ(this Vector3 vector3, float value)
        {
            return new Vector3(vector3.x, vector3.y, value);
        }

        public static Vector3 WithZ(this Vector2 vector2, float value)
        {
            return new Vector3(vector2.x, vector2.y, value);
        }

        public static Vector3[] GetPointsAroundTargetPoint(this Vector3 targetPoint, int pointsCount, float radius)
        {
            var points = new Vector3[pointsCount];

            for (var i = 0; i < pointsCount; i++)
            {
                var radians = 2 * Mathf.PI / pointsCount * i;

                var vertical = Mathf.Sin(radians);
                var horizontal = Mathf.Cos(radians);

                var spawnDir = new Vector3(horizontal, 0, vertical);

                points[i] = targetPoint + spawnDir * radius;
            }

            return points;
        }

        public static Vector2[] GetPointsAround2DTargetPoint(this Vector3 targetPoint, int pointsCount, float radius)
        {
            var points = new Vector2[pointsCount];

            for (var i = 0; i < pointsCount; i++)
            {
                var radians = 2 * Mathf.PI / pointsCount * i;

                var vertical = Mathf.Sin(radians);
                var horizontal = Mathf.Cos(radians);

                var spawnDir = new Vector3(horizontal, vertical, 0);

                points[i] = targetPoint + spawnDir * radius;
            }

            return points;
        }

        public static Vector2[] GetPointsSemiAround2DTargetPoint(this Vector3 targetPoint, int pointsCount,
            float radius)
        {
            var points = new Vector2[pointsCount];

            for (var i = 0; i < pointsCount; i++)
            {
                var angle = Mathf.Lerp(0f, 3f, i / (float)(pointsCount - 1));

                var x = Mathf.Sin(angle);
                var y = Mathf.Cos(angle);

                var spawnDir = new Vector3(y, x, 0);

                points[i] = targetPoint + spawnDir * radius;
            }

            return points;
        }

        public static Vector3 GetRandom2DPosInRadius(this Vector3 point, float radius = 1f)
        {
            var offset = Random.insideUnitCircle;

            return point + new Vector3(offset.x, offset.y, 0f) * radius;
        }

        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
        {
            return new Vector3
            (
                Mathf.Clamp(vector.x, min.x, max.x),
                Mathf.Clamp(vector.y, min.y, max.y),
                Mathf.Clamp(vector.z, min.z, max.z)
            );
        }

        public static Vector3 Clamp(this Vector3 vector, Vector2 min, Vector2 max)
        {
            return new Vector3
            (
                Mathf.Clamp(vector.x, min.x, max.x),
                Mathf.Clamp(vector.y, min.y, max.y),
                vector.z
            );
        }

        public static Vector3 WorldToScreenPos(this Vector3 position, Camera camera)
        {
            return camera.WorldToScreenPoint(position);
        }

        public static Vector3 ScreenToWorldPos(this Vector3 position, Camera camera)
        {
            return camera.ScreenToWorldPoint(position);
        }
    }
}