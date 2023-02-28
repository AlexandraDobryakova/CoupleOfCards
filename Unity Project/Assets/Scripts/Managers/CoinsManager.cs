using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public int CurrentCoins
    {
        get =>  PlayerPrefs.GetInt("Coins", startCoins);
        private set => PlayerPrefs.SetInt("Coins", value);
    }
    
    [SerializeField] private int startCoins;
    [SerializeField] private int costOfPassLevel;
    [SerializeField] private int hintCost;
    
    
    public void AddCoinsForPassLevel()
    {
        CurrentCoins += costOfPassLevel;
    }

    public void TryToSubtractCoinsForHint()
    {
        if (CurrentCoins >= hintCost) SubtractCoinsForHint();
    }

    private void SubtractCoinsForHint()
    {
        CurrentCoins -= hintCost;
    }

    public int GetHintCost()
    {
        return hintCost;
    }

    public int GetCostOfPssLevel()
    {
        return costOfPassLevel;
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public bool HasCoinsForHint()
    {
        return CurrentCoins >= hintCost;
    }
}
