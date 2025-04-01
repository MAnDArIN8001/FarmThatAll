namespace Building.BuildingSystemStates
{
    public abstract class BuildingSystemBaseState
    {
        public abstract void EnterState(BuildingSystem buildingSystem);
        public abstract void UpdateState();
        public abstract void ExitState(BuildingSystem buildingSystem);
    }
}