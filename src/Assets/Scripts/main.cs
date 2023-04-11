using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class main : MonoBehaviour {

    public TMP_Text score;
    public TMP_Text hiScore;
    public TMP_Text diffMode;
    public TMP_Text tip;
    public GameObject endWipe;
    public GameObject readyWipe;
    public GameObject breakMinder;
    
    public AudioSource sfxSource;
    public AudioClip correct;
    public AudioClip incorrect;
    public AudioClip levelUp;
    public AudioClip gameover;
    public AudioClip highscore;
    public AudioClip retry;
    public AudioSource bgmSource;
    public AudioClip normal;
    public AudioClip fast;
    public AudioClip waiting;
    
    public int numTiles     = 4; 
    public float tileScale  = 3;
    public int complexity   = 4;  
    public int numQueries   = 1; 

    public bool enableTimer      = true;
    public bool progressiveTimer = false;
    public float initialTime     = 10.0f;
    public float progressiveTime = 10.0f;
    public float timeWitherRate  = 0.10f;

    public bool shuffleIDs = true;
    public bool moreColors = true;
    public bool moreShapes = true;

    public bool vanishIDs       = false;
    public float vanishDelay    = 3.0f;
    public float vanishDuration = 3.0f;

    private timer timerScript;
    private field thisField;
    private TMP_InputField userInput;
    private flash flashScript;
    private bool gameOver = false;
    private bool endGame = false;
    private float accRate;
    private int ansAttempt = 0; //can declare as 1 if fail state non-answer is to be included in calc
    private int ansCorrect;
    private int currLevel;
    private int numClearsForLevelUp = 5;
    private float gameTimeStart;
    private float gameTimeEnd;
    private float gameDuration = 0.0f;
    private int gameMode;
    private string gameModeStr;
    private string difficultyStr;

    private const float WAIT_HOLD_SEC = 2.0f; //amount of time to disable input in end game
    private const float FAST_BMG_TIME = 5.0f;
    private const float NORM_BMG_TIME = 10.0f;
    private const float BREAK_MINDER_LIMIT = 1800; //30 minutes

//----------------------------------------------------------------------------------------------------------------------

    void Awake () {
        gameTimeStart = Time.time;
        thisField = GameObject.Find("field").GetComponent<field>();

        currLevel = PlayerPrefs.GetInt("levelSelect", 1);
        SetDifficulty(currLevel);
        gameMode = PlayerPrefs.GetInt("gameMode", 0);
        gameModeStr = SetMode(gameMode);
        diffMode.text = difficultyStr + " • " + gameModeStr;
    }
    
    void Start() {
        //QUIRK: functions may generate a NRE upon entering a debug break state if subsequent routine(s) 
        //       happen to execute on the same frame. cannot occur during normal gameplay
        thisField.EnableOverlay();
        thisField.BuildField(numTiles, tileScale, shuffleIDs, complexity, moreColors, moreShapes, 
            vanishIDs, vanishDelay, vanishDuration);
        thisField.GenerateQA(numTiles, numQueries);
        thisField.SetTimer(enableTimer, initialTime);
        progressiveTime = initialTime;
        timerScript = GameObject.Find("timer").GetComponent<timer>();

        userInput = GameObject.Find("input").GetComponent<TMP_InputField>();
        hiScore.text = PlayerPrefs.GetInt("hiscore", 1000).ToString("d6");
        flashScript = userInput.GetComponent<flash>();
    }

    void Update() {
        if ((timerScript.GetTimeLeft() < FAST_BMG_TIME) && (bgmSource.clip == normal)) {
            PlayBGM(fast);
        } else if ((timerScript.GetTimeLeft() > NORM_BMG_TIME) && (bgmSource.clip == fast)) {
            PlayBGM(normal);
        }

        if (!gameOver) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { 

                userInput.text = userInput.text.ToLower();

                if (thisField.GetAnswer() == userInput.text) {
                    //Debug.Log("correct");
                    PlaySFX(correct);
                    flashScript.FlashInput(Color.white); //to interupt 'incorrect' red, if necessary
                    tip.text = string.Empty;
                    
                    ansAttempt++;
                    ansCorrect++;

                    if (gameModeStr != "freeplay") {
                        float bonus = 0.0f;
                        bonus = currLevel;
                        if (timerScript.enabled) bonus *= (1 + (timerScript.GetTimeLeft() / progressiveTime));
                        //Debug.Log("bonus multiplier: " + bonus);

                        int crit = Random.Range(0, 6); //for a small luck element
                        
                        float pointsEarned = (((numTiles * complexity * numQueries) * bonus) * 10) + crit;
                        int currScore; int.TryParse(score.text, out currScore);
                        score.text = (currScore + (int)pointsEarned).ToString("d6");
                    }
            
                    if (gameModeStr == "progressive") {
                        if ((ansCorrect % numClearsForLevelUp == 0) && (currLevel < 5)) {
                            PlaySFX(levelUp); //overwrites 'correct' sfx
                            currLevel++;
                            //Debug.Log("Level up, currLevel: " + currLevel);
                            SetDifficulty(currLevel);
                            diffMode.text = difficultyStr + " • " + gameModeStr;

                        } else if ((ansCorrect % numClearsForLevelUp == 0) && (currLevel == 5)) {
                            PlaySFX(levelUp); //overwrites 'correct' sfx
                            //Debug.Log("progressive plus mode");
                            numQueries++;
                            if (numQueries > 10) {
                                numQueries = 10;
                            } else {
                                diffMode.text += "+";
                            }
                        }
                    }

                    thisField.DestroyField();
                    thisField.BuildField(numTiles, tileScale, shuffleIDs, complexity, moreColors, moreShapes, 
                        vanishIDs, vanishDelay, vanishDuration);
                    thisField.GenerateQA(numTiles, numQueries);
                    
                    if (progressiveTimer && enableTimer) {
                        progressiveTime *= (1 - timeWitherRate);
                        timerScript.ResetTimeLeft(progressiveTime);    
                    } else {
                        timerScript.ResetTimeLeft(initialTime);
                        progressiveTime = initialTime;
                    }
                    enableTimer = (enableTimer == true) ? timerScript.enabled = true : timerScript.enabled = false;

                } else {
                    //Debug.Log("incorrect");
                    PlaySFX(incorrect);
                    flashScript.FlashInput(Color.red);
                    tip.text = string.Empty;

                    //REF: https://stackoverflow.com/questions/1046740/
                    Regex r = new Regex("^[0-9]*$");
                    if (r.IsMatch(userInput.text) && userInput.text != string.Empty) {
                        tip.text = "numerical responses must be in word format";
                    }

                    ansAttempt++;
                    
                    userInput.Select();
                    userInput.ActivateInputField();
                }
            }

            //DEV: get answer
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab)) {
                //Debug.Log("answer provided");
                userInput.text = thisField.GetAnswer();
            }

            //DEV: refresh field, does not reset timer
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.BackQuote)) {
                //Debug.Log("field refreshed");
                thisField.DestroyField();
                thisField.BuildField(numTiles, tileScale, shuffleIDs, complexity, moreColors, moreShapes, 
                    vanishIDs, vanishDelay, vanishDuration);
                thisField.GenerateQA(numTiles, numQueries);
            }

        } else if (!endGame){
            StopBGM();
            PlaySFX(gameover);
            StartCoroutine(WaitForBGM(waiting));
            StartCoroutine(WaitForInput(WAIT_HOLD_SEC));
            tip.text = string.Empty;

            if (vanishIDs) {
                foreach(TMP_Text currTileID in GameObject.FindObjectsOfType<TMP_Text>()) {
                    if (currTileID.name == "currID") {
                        currTileID.color = Color.white; // to restore alpha
                    }
                }
            }

            if (ansAttempt == 0) {
                accRate = 0;
            } else {
                accRate = ((float)ansCorrect / (float)ansAttempt) * 100;
            }

            TMP_Text endGameText = GameObject.Find("query").GetComponent<TMP_Text>();

            endGameText.text = "<b><cspace=-0.05em>GAME OVER";
            int currScore; int.TryParse(score.text, out currScore);

            if (currScore > PlayerPrefs.GetInt("hiscore")) {
                PlayerPrefs.SetInt("hiscore", currScore);
                PlayerPrefs.Save();
                hiScore.text = PlayerPrefs.GetInt("hiscore").ToString("d6");
                endGameText.text = "<b><cspace=-0.05em>NEW HIGH SCORE!";
                StartCoroutine(WaitForSFX(gameover.length - 0.25f, highscore));
            }
            
            endGameText.text += "\n</cspace></b><size=75%>The correct answer was: \"" + thisField.GetAnswer() + 
                "\"</size>" + "<size=8%>\n\n<b><size=80%><cspace=-0.1em>ACCURACY RATE: " + accRate.ToString("f2") + 
                "</cspace>%";
            endGame = true;

            GameObject.Find("Placeholder").GetComponent<TMP_Text>().text = "PLEASE HOLD...";

            gameTimeEnd = Time.time;
            gameDuration = gameTimeEnd - gameTimeStart;
            float gameSessionTotal = PlayerPrefs.GetFloat("breaktime", 0);
            gameSessionTotal += gameDuration;
            PlayerPrefs.SetFloat("breaktime", gameSessionTotal);

            if (gameSessionTotal >= BREAK_MINDER_LIMIT && PlayerPrefs.GetInt("breaknag") == 1) {
                //Debug.Log("its's time for a break");
                breakMinder.transform.SetSiblingIndex(2); //promote to top render level
                breakMinder.SetActive(true);
                PlayerPrefs.SetFloat("breaktime", 0);
            }

            gameDuration *= currLevel; //mastery multiplier for difficulty level
            gameDuration += PlayerPrefs.GetFloat("masteryTime", 0);
            PlayerPrefs.SetFloat("masteryTime", gameDuration);
            PlayerPrefs.Save();

        } else {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                 
                userInput.text = userInput.text.ToLower();

                if (userInput.text == "yes" || userInput.text == "y") {
                    PlaySFX(retry);
                    userInput.text = string.Empty;
                    tip.text = string.Empty;
                    GameObject.Find("Placeholder").GetComponent<TMP_Text>().text = "OK.";
                    GameObject newWipe = Instantiate(readyWipe, new Vector3(0, 0, 0), Quaternion.identity);
                    newWipe.name = "wipe";

                } else if (userInput.text == "no" || userInput.text == "n") {
                    //Debug.Log("quitting...");
                    PlaySFX(retry);
                    userInput.text = string.Empty;
                    tip.text = string.Empty;
                    GameObject.Find("Placeholder").GetComponent<TMP_Text>().text = "BYE.";
                    GameObject newWipe = Instantiate(endWipe, new Vector3(0, 0, 0), Quaternion.identity);
                    newWipe.name = "wipe";

                } else {
                    userInput.text = string.Empty;
                    tip.text = "enter \"yes\" to try again or \"no\" to exit";
                    userInput.Select();
                    userInput.ActivateInputField();
                }
            }
        }
    }

//----------------------------------------------------------------------------------------------------------------------

    IEnumerator WaitForInput(float duration) {
        userInput.text = string.Empty;
        userInput.enabled = false;
        yield return new WaitForSeconds(duration);
        userInput.enabled = true;
        GameObject.Find("Placeholder").GetComponent<TMP_Text>().text = "TRY AGAIN?";
        userInput.Select();
        userInput.ActivateInputField();
        yield break;
    }

    IEnumerator WaitForSFX(float duration, AudioClip sfx) {
        yield return new WaitForSeconds(duration);
        PlaySFX(sfx);
        yield break;
    }

    //overload for waiting until currently playing sfx has finisheh
    IEnumerator WaitForSFX(AudioClip sfx) {
        yield return new WaitWhile (()=> sfxSource.isPlaying);
        PlaySFX(sfx);
        yield break;
    }

    IEnumerator WaitForBGM(AudioClip bgm) {
        yield return new WaitWhile (()=> sfxSource.isPlaying);
        
        bgmSource.volume = 0.1f;
        PlayBGM(bgm);

        float currentVol  = bgmSource.volume;
        float desiredVol  = 0.5f; 
        float currentTime = 0.0f;
        float fadeInDur   = 5.0f;

        while (currentTime < fadeInDur) {
            float vol = Mathf.Lerp(currentVol, desiredVol, currentTime / fadeInDur);
            bgmSource.volume = vol;

            currentTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }

//----------------------------------------------------------------------------------------------------------------------

    void PlaySFX(AudioClip sfx) {
        sfxSource.clip = sfx;
        sfxSource.Play();
    }

    void PlayBGM(AudioClip bgm) {
        bgmSource.clip = bgm;
        bgmSource.Play();
    }

    void StopBGM() {
        bgmSource.Stop();
    }

//----------------------------------------------------------------------------------------------------------------------

    public void SetGameOverState (bool gameOver) {
        this.gameOver = gameOver;
    }

    void SetDifficulty(int level) {
        switch (level) {
            case 1:
                difficultyStr = "novice";

                numTiles   = 2; 
                complexity = 2;
                numQueries = 1;

                enableTimer      = true;
                progressiveTimer = false;
                initialTime      = 30.0f;
                timeWitherRate   = 0.125f;
                progressiveTime  = initialTime;

                moreColors = false;
                moreShapes = false;
                shuffleIDs = false;
                vanishIDs  = false;
                break;
            case 2:
                difficultyStr = "intermediate";

                numTiles   = 3; 
                complexity = 3;
                numQueries = 1;

                enableTimer      = true;
                progressiveTimer = false;
                initialTime      = 20.0f;
                timeWitherRate   = 0.100f;
                progressiveTime  = initialTime;

                moreColors = false;
                moreShapes = true;
                shuffleIDs = false;
                vanishIDs  = false;
                break;
            case 3:
                difficultyStr = "experienced";

                numTiles   = 4; 
                complexity = 4;
                numQueries = 1;

                enableTimer      = true;
                progressiveTimer = false;
                initialTime      = 15.0f;
                timeWitherRate   = 0.075f;
                progressiveTime  = initialTime;

                moreColors = false;
                moreShapes = true;
                shuffleIDs = false;
                vanishIDs  = false;
                break;
            case 4:
                difficultyStr = "advanced";

                numTiles   = 4; 
                complexity = 4;
                numQueries = 2;

                enableTimer      = true;
                progressiveTimer = false;
                initialTime      = 12.5f;
                timeWitherRate   = 0.050f;
                progressiveTime  = initialTime;

                moreColors = true;
                moreShapes = true;
                shuffleIDs = true;
                vanishIDs  = false;
                break;
            case 5:
                difficultyStr = "expert";

                numTiles   = 4; 
                complexity = 4;
                numQueries = 2;

                enableTimer      = true;
                progressiveTimer = true;
                initialTime      = 10.0f;
                timeWitherRate   = 0.025f;
                progressiveTime  = initialTime;

                moreColors = true;
                moreShapes = true;
                shuffleIDs = true;
                vanishIDs  = true;
                vanishDelay    = 2.0f;
                vanishDuration = 3.0f;
                break;
            default:
                Debug.Log("SetDifficulty() out of intended range, defaulting to editor vals");
                break;
        }
    }

    //NOTE: mode-specific modifiers overwrite difficulty variable assignments
    string SetMode(int mode) {
        string modeStr = string.Empty;
        
        switch (mode) {
            case 0:
                modeStr = "progressive";
                break;
            case 1:
                modeStr = "static";
                progressiveTimer = true;
                break;
            case 2:
                modeStr = "freeplay";
                enableTimer = false;
                break;
            case 3:
                modeStr = "custom";
                difficultyStr = "user defined"; //EXT: hash of user config as string
                break;
            default:
                Debug.Log("SetMode()) out of intended range, defaulting to progressive");
                break;
        }
        return modeStr;
    }
}

