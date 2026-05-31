using System.Collections.Generic;
using UnityEngine;

public class CombatMusicController : MonoBehaviour
{
    public static CombatMusicController Instance;

    private HashSet<EnemyBrain> enemiesInCombat = new HashSet<EnemyBrain>();

    private bool isCombatMusicPlaying;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemyCombat(EnemyBrain enemy)
    {
        if (enemy == null) return;

        enemiesInCombat.Add(enemy);

        if (!isCombatMusicPlaying)
        {
            isCombatMusicPlaying = true;
            GameMusicController.Instance?.PlayCombatMusic();
        }
    }

    public void UnregisterEnemyCombat(EnemyBrain enemy)
    {
        if (enemy == null) return;

        enemiesInCombat.Remove(enemy);

        if (enemiesInCombat.Count == 0)
        {
            isCombatMusicPlaying = false;
            GameMusicController.Instance?.RestoreSceneMusic();
        }
    }

    public void ForceClear()
    {
        enemiesInCombat.Clear();
        isCombatMusicPlaying = false;
        GameMusicController.Instance?.RestoreSceneMusic();
    }
}