using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    
    [Header("Movement Stats")]
    public int speed;
    public float jumpHeight;
    
    
    [Header("Utility Stats")]
    [SerializeField] private int maxHealth;
    public int health;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
