using UnityEngine;

namespace Building.BuildingSystemStates
{
    public class BuildingSystemIdleState : BuildingSystemBaseState
    {
        public override void EnterState(BuildingSystem buildingSystem)
        {
            Debug.Log("Entering Idle State");
        }

        public override void UpdateState()
        {
            
        }

        public override void ExitState(BuildingSystem buildingSystem)
        {
            
        }
    }
}