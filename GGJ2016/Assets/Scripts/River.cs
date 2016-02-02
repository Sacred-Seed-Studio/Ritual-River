using UnityEngine;
using System.Collections;

public class River : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player" && other.tag != "PlayerStunned") return;
        MusicController.controller.PlaySong(MusicType.WayUp);
        GameController.controller.torchesVisible = false;
        Debug.Log("Touching river ");
        //GameController.controller.CurrentWaterLevel = GameController.controller.BucketSize;
        StartCoroutine(FillWater());
    }

    IEnumerator FillWater()
    {
        GameController.controller.player.allowedToMove = false;
        int count = 0;
        GameController.controller.player.movement.ChangeSpeed(true);
        MusicController.controller.PlaySound(MusicType.GetWater);
        while (count < GameController.controller.BucketSize)
        {
            GameController.controller.CurrentWaterLevel = 1;
            count++;
            yield return new WaitForSeconds(0.05f);
        }
        GameController.controller.SetMonkeysToMode(EnemyState.Killin);
        GameController.controller.player.allowedToMove = true;
        yield return null;
    }
}
