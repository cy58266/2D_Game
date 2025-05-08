using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public PlayerControll player;

    public EnemySpanwer spanwer;

    private Door doorExit;

    public bool gameOver;

    public List<Enemy> enemies = new List<Enemy>();

    public float NowHealth;
    public int SenceNumber = 0;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //player = FindObjectOfType<PlayerControll>();
        //doorExit = FindObjectOfType<Door>();
    }

    public void Update()
    {
        //Debug.Log("!@#@$!$@!#!@#@#"+gameStart);
        if (player != null)
            gameOver = player.isDead;
        //Debug.Log("!@#@#@$!$" + SceneManager.GetActiveScene().name);
        //if(SceneManager.GetActiveScene().buildIndex != 0)
        if (UIManger.instance == null) return;
        UIManger.instance.GameOverUI(gameOver);
    }

    public void IsEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void EnemyDead(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            doorExit.OpenDoor();
            SaveData();
        }
    }

    public void IsPlayer(PlayerControll controll)
    {
        
        if(controll.isLocalPlayer)
            player = controll;
    }

    public void IsExitDoor(Door door)
    {
        doorExit = door;
    }

    public void RestartScene()
    {
        SenceNumber = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.DeleteKey("playerHealth");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("sceneIndex"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));
        else
            NewGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main");
        PlayerPrefs.DeleteKey("playerHealth");
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public float LoadHealth()
    {
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", 3f);
            Debug.Log("111");
        }
        
        PlayerPrefs.SetFloat("playerHealth", 3f);

        float currentHealth = PlayerPrefs.GetFloat("playerHealth");
        //Debug.Log("222");
        
        return currentHealth;
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("playerHealth", player.health);
        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.Save();
    }
    public void NextLevel()
    {
        SenceNumber += 1;
        //NowHealth = player.health;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + SenceNumber, LoadSceneMode.Additive);
        if (SenceNumber == 1)
        {
            
            player.transform.position = new Vector3(-65, 38, 0);
            if (isServer)
            {
            }
            spanwer.OnStartServer();
            //UIManger.instance.UpdateHealth(NowHealth);
        }
        if (SenceNumber == 2)
        {
            player.transform.position = new Vector3(-47, 132, 0);
        }
        if (SenceNumber == 3)
        {
            player.transform.position = new Vector3(-65, 38, 0);
            gameOver = true;

        }
        //NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
