using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public static WinCondition instance { get; private set; }

    public int requiredKills = 5;
    public int currentKills = 0;

    private void Awake() => instance = this;
    
    public void IncrementKills()
    {
        this.currentKills++;

        if (this.currentKills >= this.requiredKills)
            GameUI.instance.Win();
    }
}
