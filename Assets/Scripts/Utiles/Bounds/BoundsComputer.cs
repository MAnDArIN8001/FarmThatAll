using UnityEngine;
using UnityEngine.AI;

namespace Utiles.Bounds
{
    public class BoundsComputer
    {
        public void GetClothestPositionForPoint(Vector3 destination, Vector3 point, NavMeshObstacle obstacle)
        {
            NavMeshHit navMeshHit;

            float radius = obstacle.radius;

            Vector3 direction = (point - destination).normalized;
            Vector3 nearTarget = point - direction * radius;
        }
    }
}