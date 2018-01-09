using UnityEngine;
using System.Collections;

public class Boo : MonoBehaviour 
{
    public Transform leftSide;
    public Transform rightSide;

    Vector3 nextPosition;
    float speed = 0.005F;
    float translateX;
    float translateY;

    float minY = -4F;
    float maxY = 4F;
    float minX = -5;
    float maxX = 5;

    bool direction;
    bool stayHear = false;

	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        nextPosition = getRandomPosition();
        maxX = rightSide.position.x + 0.5F;
        minX = leftSide.position.x - 0.5F;
	}
	
	void Update ()
    {
        

        //if (Vector2.Distance(transform.position, nextPosition) > speed) 




       /* if (Vector2.Distance(transform.position, nextPosition) > speed)
        {

            //if (!isNear(transform.position.x, nextPosition.x, speed)) 
            //{
            //    if (nextPosition.x > transform.position.x) { translateX = speed; } else { translateX = -speed; }
            //} 
            //else translateX = 0;

            //if(!isNear(transform.position.y, nextPosition.y, speed)) 
            //{
            //    if(nextPosition.y > transform.position.y) translateY = speed; else translateY = -speed;
            //}
            //else translateY = 0;

            //transform.Translate(new Vector2(translateX, translateY));

        }
        else
        {
            //if (stayHear) { }
            //else
            //{
                nextPosition = getRandomPosition();
            //    stayHear = true;
            //}
        }*/

    }

    public Vector3 getRandomPosition()
    {
        Debug.Log("randCall");
        Vector3 NewP;
        do
        {
            NewP = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        }
        while (Vector3.Distance(NewP, transform.position) < (speed * 5));
        Debug.Log(NewP);
        return NewP;
    }

    public float Distance(float dimension, float newDimension)
    {
        return Vector2.Distance(new Vector2(dimension, 0), new Vector2(newDimension, 0));
    }

    public bool isNear(float dimension, float newDimension,  float epsilon)
    {
        return Distance(dimension, newDimension) < epsilon;
    }
}
