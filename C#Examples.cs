void Start()
{
    sword = GameObject.FindGameObjectWithTag("Sword");  //sets the object sword to the item in game with tag 'Sword'
    swordAni = sword.GetComponent<Animator>();          //Gets animation associated with sword object

    GameObject[] enemyTargets = GameObject.FindGameObjectsWithTag("Enemy"); //Stores in the array all objects with tag 'Enemy'

    if (enemyTargets != null)
    {
        foreach (GameObject go in enemyTargets) //for each obj in enemyTargets, add to array targets which is of type GameObject
        {
            targets.Add(go);
            print(targets.Count);
        }
    }
}


void Update()
{

    RaycastHit hit;
    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
    {
        if (hit.transform.gameObject.tag == "Enemy")    //If mouse is over object with Enemy Tag
        {
            target = hit.collider.gameObject;   //Whatever the raycast hit detects, is stored as target

            if (Input.GetMouseButtonDown(0)) //If Left mouse click 
            {
                swordAni.SetTrigger("Swing");
                EnemyHit();
                print("Enemy hit"); //debug statement
            }
        }
    }
}


public void EnemyHit()
//Purpose: This function will store the enemy that the player is attacking and reduce it's health.
//Pre: Player must of attacked.
//Post: Enemy health is lowered.
{

    Enemy eh = (Enemy)target.GetComponent("Enemy"); //eh - enemy health

    if (canTakeDamage)
    {

        if (GreatSkeletonFight)
        {
            GreatSkeletonOfAhhh GreatSkel = (GreatSkeletonOfAhhh)target.GetComponent("GreatSkeletonOfAhhh");
            GreatSkel.EnemyDecreaseHP();
        }

        if (!GreatSkeletonFight)
        {
            eh.EnemyDecreaseHP();           //Decreases enemy HP
        }


        canTakeDamage = false;

        if (!GreatSkeletonFight)            //Any regular skeleton fight
            StartCoroutine(TimeBuffer());   //Halts game by 1 seconds after player hits skelly

        if (GreatSkeletonFight)                 //The final boss fight
            StartCoroutine(FinalTimeBuffer());  //Halts game by 3 seconds after player hits AHHH
    }

}

public void EnemyDecreaseHP()
    //Purpose: Helper function for an enemey class to lower their respected HP. If their HP drops below lowHP, die
    //Pre: An enemy object
    //Post: Enemy object HP is subtracted by 5. An animation plays. Enemy dies if HP less than LowHP
{

    if (enemyTakeDamage)
    {
        enemyHP -= 5;
        animation.Play("gethit");       //Plays hit animation
        print(enemyHP);

        if (enemyHP <= lowHP)
        {

            animation.Play("die");
            this.collider.enabled = false;
            GameObject.Find("SkeletonCamera").camera.enabled = false;
        }

    }
}

void OnGUI()
//Purpose: Provides various GUI elements
{
    if (isPause) //Opens the pause menu
    {

        if (GUI.Button(new Rect(90, 100, 80, 20), "Exit?")) //Exit button returning you to titlescreen
        {
            Application.LoadLevel("TitleScreen");
        }
        if (GUI.Button(new Rect(90, 70, 120, 20), "Restart Level?"))    //Restart player to beginning of current level
        {
            Application.LoadLevel(level);   //restarts current levelS
        }
    }
    else //Provides constant HP indicator when isPause isn't true. Will display during gameplay
    {
        GUI.Label(new Rect(10, 10, 100, 60), "Health: ");   //This is the health bar
        GUI.Label(new Rect(20, 20, 100, 60), health);   //This is the health amount
    }

    if (isDead) //If player has died. Display "YOU HAVE DIED"
    {
        GUI.TextArea(new Rect(50, 50, 100, 50), "YOU HAVE DIED");

    }

}