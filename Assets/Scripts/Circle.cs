using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour 
{
    public GameController gameController;

    public Transform Thwimp1;
    public Transform Thwimp2;

    public Transform Eerie1;
    public Transform Eerie2;

    Vector3 velocity = Vector3.zero;
    float jumpSpeed = 4.3F;
    float localTime = 0;
    bool collideWithLines = false;
    Vector2 collisionSphere;

    void Start ()
    {
        PauseCircle();
        Thwimp1.position = new Vector3(Mathf.Cos(localTime) + collisionSphere.x, Mathf.Sin(localTime) + collisionSphere.y, 0);
        Thwimp2.position = new Vector3(Mathf.Cos(localTime + (Mathf.PI)) + collisionSphere.x, Mathf.Sin(localTime + (Mathf.PI)) + collisionSphere.y, 0);
        Eerie1.position = new Vector3(Mathf.Cos(localTime + (Mathf.PI * 0.5F)) + collisionSphere.x, Mathf.Sin(localTime + (Mathf.PI * 0.5F)) + collisionSphere.y, 0);
        Eerie2.position = new Vector3(Mathf.Cos(localTime + (Mathf.PI * 1.5F)) + collisionSphere.x, Mathf.Sin(localTime + (Mathf.PI * 1.5F)) + collisionSphere.y, 0);
        Eerie1.GetComponent<Animator>().enabled = false;
        Eerie2.GetComponent<Animator>().enabled = false;
        if (Thwimp1.position.x > 0) Thwimp1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Thwimp1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
        if (Thwimp2.position.x > 0) Thwimp2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Thwimp2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
        if (Eerie1.position.x > 0) Eerie1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Eerie1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
        if (Eerie2.position.x > 0) Eerie2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Eerie2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20; 
	}

    public void PauseCircle() 
    {
        velocity = GetComponent<Rigidbody2D>().velocity;
        GetComponent<Rigidbody2D>().Sleep();
    }

    public void ResumeCircle() 
    {
        GetComponent<Rigidbody2D>().WakeUp();
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

	void Update () 
    {
        if (gameController.gameMode == GameController.GameMode.GamePlay) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!Eerie1.GetComponent<Animator>().enabled) Eerie1.GetComponent<Animator>().enabled = true;
                if (!Eerie2.GetComponent<Animator>().enabled) Eerie2.GetComponent<Animator>().enabled = true;

                Jump();
            }

            localTime += Time.deltaTime;

            collisionSphere = transform.position;
            collisionSphere.x -= 0.55F;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(collisionSphere, 1.05F);
            collideWithLines = false;
            foreach (Collider2D c in colliders) if (c.tag == "Line") collideWithLines = true;
            if (!collideWithLines) EndGame();

            Thwimp1.position = new Vector3(Mathf.Cos(localTime) + collisionSphere.x, Mathf.Sin(localTime) + collisionSphere.y, 0);
            Thwimp2.position = new Vector3(Mathf.Cos(localTime + (Mathf.PI)) + collisionSphere.x, Mathf.Sin(localTime + (Mathf.PI)) + collisionSphere.y, 0);
            Eerie1.position = new Vector3(Mathf.Cos(localTime + (Mathf.PI * 0.5F)) + collisionSphere.x, Mathf.Sin(localTime + (Mathf.PI * 0.5F)) + collisionSphere.y, 0);
            Eerie2.position = new Vector3(Mathf.Cos(localTime + (Mathf.PI * 1.5F)) + collisionSphere.x, Mathf.Sin(localTime + (Mathf.PI * 1.5F)) + collisionSphere.y, 0);

            if (Thwimp1.position.x > 0) Thwimp1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Thwimp1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
            if (Thwimp2.position.x > 0) Thwimp2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Thwimp2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
            if (Eerie1.position.x > 0) Eerie1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Eerie1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
            if (Eerie2.position.x > 0) Eerie2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6; else Eerie2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20; 
        }

	}

    public void Jump() 
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;
    }

    public void EndGame()
    {
        Eerie1.GetComponent<Animator>().enabled = false;
        Eerie2.GetComponent<Animator>().enabled = false;
        gameController.EndGame();
        PauseCircle();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Step")
        {
            if (gameController.gameMode == GameController.GameMode.GameMenu)
                gameController.ContinueForwardLine();
            else
            {
                gameController.ContinueLine();
                gameController.IncrementScore();
            }
        }
    }

}
