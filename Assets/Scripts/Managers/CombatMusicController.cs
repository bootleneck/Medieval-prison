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
        if (enemy == null)
            return;

        Debug.Log($"[CombatMusic] Register: {enemy.name}");

        enemiesInCombat.Add(enemy);

        if (!isCombatMusicPlaying)
        {
            isCombatMusicPlaying = true;
            Debug.Log("[CombatMusic] PLAY COMBAT MUSIC");
            GameMusicController.Instance?.PlayCombatMusic();
        }
    }

    public void UnregisterEnemyCombat(EnemyBrain enemy)
    {
        if (enemy == null)
            return;

        Debug.Log($"[CombatMusic] Unregister: {enemy.name}");

        enemiesInCombat.Remove(enemy);

        if (enemiesInCombat.Count == 0)
        {
            isCombatMusicPlaying = false;
            Debug.Log("[CombatMusic] RESTORE SCENE MUSIC");
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