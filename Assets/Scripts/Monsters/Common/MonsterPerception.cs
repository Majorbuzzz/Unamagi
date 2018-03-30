using UnityEngine;

public class MonsterPerception : MonoBehaviour
{
    public MonsterBody _monsterBody;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        var playerIsInRange = other.gameObject.CompareTag("Player");

        if (playerIsInRange)
            _monsterBody.PlayerIsInRange(other.gameObject);
    }
}
