using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
    public enum GameMode {GameMenu, GamePlay, GamePause, GameEnd};

    public GameMode gameMode = GameMode.GameMenu;

    public Transform lines;
    public Circle circle;

    public GameObject[] linesPrefab;

    public Transform leftSide;
    public Transform rightSide;
    public Transform startMessage;

    public GUIText finalMessage;
    public GUIText scoreText;
    public GUIText highScoreText;
    public GUIText newHighScore;

    public Material background; 
    
    List<GameObject> gameLines;

    public float[] layers;

    int currentLayer = 0;
    
    float speed = 2.5F;
    float score = 0;
    float maxScore = 0;
    float startMaxScore = 0;
    float nextSpawnPosition = -5;
    //float newHighScoreFontSize = 30;
    float newHighScoreAnimationTimeCounter = 0;
    float bgSlideSpeed = 0.01F;

    bool newScoreReached = false;
    bool playNewScoreAnimation = false;
    bool firstExitClick = false;

    void Start () 
    {
        leftSide.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x , leftSide.position.y, leftSide.position.z);
        rightSide.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x, leftSide.position.y, leftSide.position.z);
        gameLines = new List<GameObject>();
        finalMessage.gameObject.active = false;
        for (int i = 0; i < 15; i++) ContinueForwardLine();
        maxScore = PlayerPrefs.GetFloat("HighScore", 0);
        startMaxScore = maxScore;
	}

    public void StartGame() 
    {
        gameMode = GameMode.GamePlay;
        circle.GetComponent<Rigidbody2D>().gravityScale = 1;
        startMessage.gameObject.active = false;
    }

    public void EndGame() 
    {
        gameMode = GameMode.GameEnd;
        finalMessage.gameObject.SetActive(true);
        if (playNewScoreAnimation) newHighScore.color = new Color(newHighScore.color.r, newHighScore.color.g, newHighScore.color.b, 0);
        if (newScoreReached) 
        {
            finalMessage.color = Color.red;
            PlayerPrefs.SetFloat("HighScore", maxScore);
            PlayerPrefs.Save();
        }
        finalMessage.text = "Your score is "+score;
    }

    public void PauseGame() 
    {
        gameMode = GameMode.GamePause;
    }

    public void ResumeGame() 
    {
        gameMode = GameMode.GamePlay;
    }

    public void ResetGame()
    {
        gameMode = GameMode.GameMenu;
    }

    public void ContinueLine()
    {
        int nextStep = -1 ;

        nextSpawnPosition += (0.545F * 2);

        if(currentLayer == -4) nextStep = Random.Range(1, 3); else if (currentLayer == 4) nextStep = Random.Range(0, 2); else nextStep = Random.Range(0, 3);
        
        Vector3 spawPosition = new Vector3(nextSpawnPosition, 0, 0);

        if(nextStep == 0) 
        {
            spawPosition.y = (layers[currentLayer + 4] + layers[currentLayer + 3]) / 2;  
            currentLayer--;
        }
        else if(nextStep == 1) 
        {
            spawPosition.y = layers[currentLayer + 4];
        }
        else if (nextStep == 2)
        {
            spawPosition.y = (layers[currentLayer + 4] + layers[currentLayer + 5]) / 2;  
            currentLayer++;
        }

        GameObject direction = (GameObject)Instantiate(linesPrefab[nextStep], spawPosition, Quaternion.identity);

        direction.transform.parent = lines;

        gameLines.Add(direction);

        clearPreviousLines();
    }

    public void ContinueForwardLine()
    {
        int nextStep = 1;
        nextSpawnPosition += (0.545F * 2);
        GameObject direction = (GameObject)Instantiate(linesPrefab[nextStep], new Vector3(nextSpawnPosition, layers[currentLayer + 4], 0), Quaternion.identity);
        direction.transform.parent = lines;
        gameLines.Add(direction);
        clearPreviousLines();
    }

    public void clearPreviousLines()
    {
        if (gameLines[0].transform.position.x < leftSide.position.x)
        {
            Destroy(gameLines[0]);
            gameLines.RemoveAt(0);
        }
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = "Score:"+score;
        if (score > maxScore) 
        {
            if (startMaxScore != 0 && !newScoreReached) 
            {
                playNewScoreAnimation = true;
                newScoreReached = true;
                newHighScore.gameObject.SetActive(true);
            }
            
            maxScore = score;
            highScoreText.text = "High score:" + maxScore;
        }
    }

	void Update () 
    {
        if (gameMode == GameMode.GameMenu)
        {
            if(Input.GetMouseButtonDown(0)) StartGame();
        }
        else if (gameMode == GameMode.GamePlay)
        {
            lines.Translate(Vector3.left * speed * Time.deltaTime);
            nextSpawnPosition -= speed * Time.deltaTime;

            background.mainTextureOffset = new Vector2(background.mainTextureOffset.x + (bgSlideSpeed * Time.deltaTime), 0);

            speed *= 1.0005F;

            if (playNewScoreAnimation)
            {
                newHighScoreAnimationTimeCounter += Time.deltaTime;

                if (newHighScoreAnimationTimeCounter < 0.1F)
                {
                    newHighScore.color = Color.red;
                }
                else if(newHighScoreAnimationTimeCounter < 0.2F)
                {
                    newHighScore.color = Color.yellow;
                }
                else if (newHighScoreAnimationTimeCounter < 0.3F)
                {
                    newHighScore.color = Color.red;
                }
                else if (newHighScoreAnimationTimeCounter < 0.4F)
                {
                    newHighScore.color = Color.yellow;
                }
                else if (newHighScoreAnimationTimeCounter < 0.5F)
                {
                    newHighScore.color = Color.red;
                }
                else if (newHighScoreAnimationTimeCounter < 0.6F)
                {
                    newHighScore.color = Color.yellow;
                }
                else if(newHighScoreAnimationTimeCounter < 0.7F)
                {
                    newHighScore.color = Color.red;
                }
                else if (newHighScoreAnimationTimeCounter < 0.8F)
                {
                    newHighScore.color = Color.yellow;
                }
                else if (newHighScoreAnimationTimeCounter < 0.9F)
                {
                    newHighScore.color = Color.red;
                }
                else if (newHighScoreAnimationTimeCounter > 1.9F)
                {
                    newHighScore.enabled = false;
                    newHighScoreAnimationTimeCounter = 0;
                    playNewScoreAnimation = false;
                }

            }
        }
        else if (gameMode == GameMode.GameEnd)
        {
            if (Input.GetMouseButtonDown(0) && firstExitClick) Application.LoadLevel("main"); else firstExitClick = true;
        }

	}

}
