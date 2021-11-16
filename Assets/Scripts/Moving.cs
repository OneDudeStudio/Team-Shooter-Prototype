using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    //Move references
    Vector3 startPosition;
    Vector3 targetPosition;
    [SerializeField] float rangeX;
    [SerializeField] float rangeZ;
    [SerializeField] bool isMoving = true;
    [SerializeField] bool isFinished = false;

    //Shoot references
    public bool nobodySpot ;
    public bool isShooting = false;
    public bool alreadyInShooting = false;
    public CapsuleCollider shoottrigger;
    TeamMateSettings teamMateController;
    TeamMateSettings targetController;
    GameObject target;

    private void Awake()
    {
        teamMateController = GetComponent<TeamMateSettings>();
    }
    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        nobodySpot = true;
        targetPosition = new Vector3(Random.Range(-rangeX, rangeX), transform.position.y, Random.Range(-rangeZ, rangeZ));
        shoottrigger.radius = teamMateController.visionRadius; 
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MoveTeamMate();

        if(target == null)
        {
            nobodySpot = true;
        }
        else if(target != null)
        {
            nobodySpot = false;
        }
    }
    //Move Settings//////////////////////////////////////////////////////
    void MoveTeamMate()
    {
        if (isFinished == false && isMoving == true && isShooting == false)
        {
            teamMateController.status.text = "Ищу соперника";
            startPosition = transform.position;
            transform.LookAt(targetPosition, Vector3.up);
            float step = teamMateController.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(startPosition, targetPosition, step);
        }
        else if (isShooting == true)
        {
            teamMateController.status.text = "В перестрелке";
            isMoving = false;
            isFinished = true;
        }
        if (Vector3.Distance(transform.position, targetPosition) < 0.02f && isMoving)
        {
            teamMateController.status.text = "Ожидаю";
            isFinished = true;
            isMoving = false;
            Invoke("NextSpot", 2f);
        }
    }
    void NextSpot()
    {
        targetPosition = new Vector3(Random.Range(-rangeX, rangeX), transform.position.y, Random.Range(-rangeZ, rangeZ));
        isFinished = false;
        isMoving = true;
    }
    // Shoot settings//////////////////////////////////////////////////// 3
    private void OnTriggerEnter(Collider other)// определяет первый target
    {
        if (other.gameObject.GetComponent<TeamMateSettings>().teamName.text
            != gameObject.GetComponent<TeamMateSettings>().teamName.text && nobodySpot == true)
        {
            //задается цель 
            target = other.gameObject;
            targetController = other.gameObject.GetComponent<TeamMateSettings>();
            // идет к точке в которой был враг во время первого контакта
            targetPosition = other.gameObject.transform.position;
            nobodySpot = false;
        }
    }

    private void OnTriggerStay(Collider other)// проверки на расстояния до target, стрельба
    {
        if(target != null && nobodySpot == false)// если есть заранее выбранный target
        {
            teamMateController.status.text = "Нашел врага";
            //смотреть на  target
            transform.LookAt(target.transform.position, Vector3.up);
            //проверка на дистанцию до target если оно больше чем радиус атаки, то продолжать сближаться
            if (Vector3.Distance(transform.position, targetPosition) > teamMateController.attackRadius)
            {
                //сближение с целью
                teamMateController.status.text = "Сближаюсь";
                targetPosition = target.gameObject.transform.position;
            }
            //проверка на дистанцию до target если оно равно или меньше радиуса атаки, то атаковать
            else if (Vector3.Distance(transform.position, targetPosition) <= teamMateController.attackRadius)
            {
                transform.LookAt(target.transform.position, Vector3.up);
                teamMateController.status.text = "Веду огонь";
                //выстрелы
                if (alreadyInShooting == false)
                {
                    isShooting = true;
                    StartCoroutine("ShotingController");
                }
            }
        }

        else if(target == null)// если предыдущая цель потеряна
        {
            nobodySpot = true;
            isShooting = false;
            alreadyInShooting = false;
            if (other.gameObject.GetComponent<TeamMateSettings>().teamName.text
            != gameObject.GetComponent<TeamMateSettings>().teamName.text && nobodySpot == true)
            {
                //задается цель 
                target = other.gameObject;
                targetController = other.gameObject.GetComponent<TeamMateSettings>();
                // идет к точке в которой был враг во время первого контакта
                targetPosition = other.gameObject.transform.position;
                nobodySpot = false;
                return;
            }
            
        }
    }

    IEnumerator ShotingController()
    {
        alreadyInShooting = true;
        // условия при которых может вестись стрельба
        while(targetController.healthPoints >0 
            && target != null 
            && Vector3.Distance(transform.position, targetPosition) <= teamMateController.attackRadius)
        {
            yield return new WaitForSeconds(teamMateController.timeToReload / 2);
            Fire();
            yield return new WaitForSeconds(teamMateController.timeToReload / 2);
        }
        // выход из состояния перестрелки
        isShooting = false;
        nobodySpot = true;
        alreadyInShooting = false;
        Invoke("NextSpot", 1f);
        yield break;
    }
    void Fire()
    {
        targetController.healthPoints -= teamMateController.damage;
        return;
    }

}
