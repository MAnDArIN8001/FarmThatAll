using System;

namespace Utiles
{
    public class LevelManager
    {
        public event Action<int> OnLevelChanged;
        
        public int  CurrentLevel { get; private set; }

        public static LevelManager Instance { get; private set; }

        public LevelManager()
        {
            Instance = this;
        }

        public void IncreaseLevel(int increaseValue = 1)
        {
            CurrentLevel += increaseValue;
            
            OnLevelChanged?.Invoke(CurrentLevel);
        }

        public void DecreaseLevel(int decreaseValue = 1)
        {
            CurrentLevel -= decreaseValue;
            CurrentLevel = CurrentLevel < 0 ? 0 : CurrentLevel;
            
            OnLevelChanged?.Invoke(CurrentLevel);
        }
    }
}