using UnityEngine;

public class FinalCombatState : MonoBehaviour
{
    public static FinalCombatState Instance;

    public int currentPhase = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool CanActivate(int phase)
    {
        return phase == currentPhase + 1;
    }

    public void AdvancePhase()
    {
        currentPhase++;
    }
}