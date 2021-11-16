using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamMateSettings : MonoBehaviour
{

    //Settings
    public float timeToReload;
    public float damage;
    public float healthPoints;
    public float speed;
    public float visionRadius;
    public float attackRadius;

    //Weapon
    public GameObject[] typeOfWeapon;
    public Material[] teamColor;
    int typeWeapon;

    //UI
    public TextMeshProUGUI teamName;
    public TextMeshProUGUI health;
    public TextMeshProUGUI status;
    public Canvas teamMateCanvas;

    //Other References
    public GameObject mainCam;
    GameManager gameManager;
    
    


    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        TeamMateSetUp();
        mainCam = GameObject.Find("Main Camera");
        teamMateCanvas.transform.localScale = new Vector3(-1f,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        health.text = healthPoints.ToString();
       if(healthPoints <= 0)
        {
            Death();
        }
        teamMateCanvas.transform.LookAt(mainCam.transform.position);

    }
    void TeamMateSetUp()
    {
        if (transform.position.z < 0)
        {
            gameObject.GetComponent<MeshRenderer>().material = teamColor[0];
            teamName.text = "RED";
            teamName.color = Color.red;
        }
        else if (transform.position.z > 0)
        {
            gameObject.GetComponent<MeshRenderer>().material = teamColor[1];
            teamName.text = "BLUE";
            teamName.color = Color.blue;
        }
        health.text = healthPoints.ToString();
        status.text = "Поиск";

        typeWeapon = Random.Range(0, typeOfWeapon.Length);
        
        switch (typeWeapon)
        {
            case 0:
                typeOfWeapon[0].SetActive(true);
                visionRadius = 25f;
                attackRadius = 10f;
                healthPoints = 120f;
                damage = 10f;
                timeToReload = 0.5f;
                break;
            case 1:
                typeOfWeapon[1].SetActive(true);
                visionRadius = 25f;
                attackRadius = 8f;
                healthPoints = 100f;
                damage = 20f;
                timeToReload = 1f;
                break;
            case 2:
                typeOfWeapon[2].SetActive(true);
                visionRadius = 25f;
                attackRadius = 25f;
                healthPoints = 100f;
                damage = 30f;
                timeToReload = 3f;
                break;
            default:
                print("Error");
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        healthPoints -= damage;
        health.text = healthPoints.ToString();
    }

    public void Death()
    {
        if(teamName.text == "BLUE")
        {
            gameManager.blueTeamCounter--;
        }
        if (teamName.text == "RED")
        {
            gameManager.redTeamCounter--;
        }
        Destroy(gameObject);
        print("gameover");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    
}
