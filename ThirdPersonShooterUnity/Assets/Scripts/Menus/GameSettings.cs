using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [HideInInspector]
    public int enemies = 0;
    public GameObject zombieEnemy;

    public static GameSettings instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void ApplySettings()
    {
        for (int i = 0; i <= enemies; i++)
        {
            Instantiate<GameObject>(zombieEnemy, Vector3.zero, Quaternion.identity);
        }
    }

    public void Settings(int enemiesQuantity)
    {
        enemies = enemiesQuantity;
    }
}
