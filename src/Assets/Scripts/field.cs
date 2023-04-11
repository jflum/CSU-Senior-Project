using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class field : MonoBehaviour {
    
    public GameObject tile;
    public GameObject queryText;
    public GameObject timer;
    public GameObject inputField;
    public GameObject scoreboard;
    public GameObject timerBG;
    public GameObject menus;
    private vanish vanishScript;
    private TMP_InputField inputState;

    public string query  = string.Empty;
    public string answer = string.Empty;

    private List<string> availColorsList = new List<string> { "black", 
                                                              "blue",
                                                              "green",
                                                              "orange",
                                                              "purple",
                                                              "red",
                                                              "yellow",
                                                              "white" };

    private List<string> moreColorsList  = new List<string> { "cyan",
                                                              "magenta" };                                                              

    private List<string> availShapesList = new List<string> { "circle",
                                                              "rectangle",
                                                              "square",
                                                              "triangle" };

    private List<string> moreShapesList  = new List<string> { "diamond",
                                                              "hexagon" };
    
    private List<string> availNumsList   = new List<string> { "zero",
                                                              "one",
                                                              "two",
                                                              "three",
                                                              "four",
                                                              "five",
                                                              "six",
                                                              "seven",
                                                              "eight",
                                                              "nine" };

    private List<string> availIDNums;
    private List<string> availBGColors;
    private List<string> availShapes;
    private List<string> availShapeColors;
    private List<string> availNums;
    private List<string> availNumColors;
    private List<string> availText1;
    private List<string> availText2;
    private List<string> availTextColors;

    private const int TILE_X_SPACE = 1;
    private const int FIELD_SCALE  = 18;
    private const float TILE_Y_POS   = 1.75f;
    private const float TIMER_Y_POS  = 3.7f;
    private const float QUERY_Y_POS  = -2.15f;
    private const float INPUT_Y_POS  = -3.9f;

//----------------------------------------------------------------------------------------------------------------------

    private void SetAnswer(string answerPart) {
        if (string.IsNullOrEmpty(answer)) {
            answer += answerPart;
        } else {
            answer += (" " + answerPart);
        }
    }

    public string GetAnswer() {
        return answer;
    }

    private void SetQuery(string attribute, string tileID) {
        string questionFormat = "What " + attribute + " tile #" + tileID + "?\n";
        
        if (string.IsNullOrEmpty(query)) {
            query += questionFormat;
        } else {
            query += ("" + questionFormat);
        }
    }

    //overload for questions that omit tileID information
    private void SetQuery(string attribute) {
        string questionFormat = "What " + attribute + "?\n";
        
        if (string.IsNullOrEmpty(query)) {
            query += questionFormat;
        } else {
            query += ("" + questionFormat);
        }
    }
    
    public string GetQuery() {
        return query;
    }

//----------------------------------------------------------------------------------------------------------------------

    public void BuildField(int numTiles, float tileScale, bool shuffleIDs, int complexity, bool moreColors, 
            bool moreShapes, bool vanishIDs, float vanishDelay, float vanishDuration) {
        
        float fieldReq = (tileScale * numTiles) + (TILE_X_SPACE * (numTiles - 1));

        //prevents a field of 0 tiles
        if (numTiles < 1) {
            Debug.Log("a field with (" + numTiles + ") tiles is invalid");
            Debug.Break();
            return;
        }

        //prevents drawing tiles out of current boundaries
        if (fieldReq > FIELD_SCALE) {
            Debug.Log("qty (" + numTiles + ") tiles of scale (" + tileScale + 
                ") with spacing (" + TILE_X_SPACE + ") will not fit on current field size (" 
                + FIELD_SCALE + ")");
            Debug.Break();
            return;
        }

        //prevents a field larger than the number of colors available
        if (numTiles > availColorsList.Count) {
            Debug.Log("a field with (" + numTiles + ") tiles cannot be drawn with only (" + 
                availColorsList.Count + ") colors");
            Debug.Break();
            return;
        }

        //prevents a field larger than the number of shapes available
        if (numTiles > availShapesList.Count) {
            Debug.Log("a field with (" + numTiles + ") tiles cannot be drawn with only (" + 
                availShapesList.Count + ") shapes");
            Debug.Break();
            return;
        }

        //prevents an invalid complexity value
        if (complexity < 1 || complexity > 4) {
            Debug.Log("a field with tile complexity of (" + complexity + ") is invalid, " +
                "value must be between (1) and (4) with current implementation");
            Debug.Break();
            return;
        }

        availIDNums = new List<string>();
        for (int i = 1; i <= numTiles; i++) {
            availIDNums.Add(i.ToString());
        }

        if (shuffleIDs) {
            availIDNums = ShuffleList(availIDNums);
        }

        if (moreColors && !availColorsList.Contains(moreColorsList[0])) {
            for (int i = 0; i < moreColorsList.Count; i++) {
                availColorsList.Add(moreColorsList[i]);
            }
        } else if (!moreColors) {
            for (int i = 0; i < moreColorsList.Count; i++) {
                availColorsList.Remove(moreColorsList[i]);
            }
        }

        if (moreShapes && !availShapesList.Contains(moreShapesList[0])) {
            for (int i = 0; i < moreShapesList.Count; i++) {
                availShapesList.Add(moreShapesList[i]);
            }
        } else if (!moreShapes) {
            for (int i = 0; i < moreShapesList.Count; i++) {
                availShapesList.Remove(moreShapesList[i]);
            }
        }

        //NOTE: minor memory optimization could be achieved via unallocated lists that will not be used within
        //      Populate() when complexity is less than 4. would req. function overload/extension for fewer attributes    

        availBGColors = new List<string>(availColorsList);
        if (availBGColors.Contains("white")) availBGColors.Remove("white"); //personal pref
        availBGColors = ShuffleList(availBGColors);
        //Debug.Log("availBGColors: " + PrintList(availBGColors));

        if (complexity >= 2) {
            availShapes = new List<string>(availShapesList);
            availShapes = ShuffleList(availShapes);
            availShapeColors = new List<string>(availColorsList);
            availShapeColors = ShuffleList(availShapeColors);
            //Debug.Log("availShapes: " + PrintList(availShapes));
        }

        if (complexity >= 3) {
            availNums = new List<string>(availNumsList);
            availNums = ShuffleList(availNums);
            availNumColors = new List<string>(availColorsList);
            availNumColors = ShuffleList(availNumColors);
        }

        if (complexity == 4) {
            availText1 = new List<string>(availColorsList);
            availText1 = ShuffleList(availText1);
            availText2 = new List<string>(availShapesList);
            availText2 = ShuffleList(availText2);
            availTextColors = new List<string>(availColorsList);
            if (availTextColors.Contains("black")) availTextColors.Remove("black"); //personal pref
            availTextColors = ShuffleList(availTextColors);
        }

        float leftBound  = (FIELD_SCALE / -2);
        float fieldSpace = FIELD_SCALE - (tileScale * numTiles);
        float tileOffset = (fieldSpace - (TILE_X_SPACE * (numTiles - 1))) / 2; 
        float tileXPos   = leftBound + tileOffset + (tileScale / 2);

        for (int i = 1; i <= numTiles; i++) {
            float x = tileXPos + ((i - 1) * (tileScale + TILE_X_SPACE));
            float y = TILE_Y_POS;

            Vector3 position = new Vector3(x, y, 0);

            GameObject newTile = (GameObject)Instantiate(tile, position, Quaternion.identity);
            newTile.name = "tile" + i.ToString();
            newTile.transform.localScale = new Vector3(tileScale, tileScale, tileScale);
            newTile.transform.Find("canvas").name = "canvas" + i.ToString();
            
            int tileID = i; //for clarity in Populate()
            newTile.GetComponent<tile>().Populate(tileScale, tileID, numTiles, availIDNums, availBGColors, availShapes, 
                availShapeColors, availText1, availText2, availTextColors, availNums, availNumColors, complexity);
            newTile.transform.SetParent(this.transform);
            
            if (vanishIDs) {
                vanishScript = newTile.transform.GetChild(0).GetChild(0).GetComponent<vanish>();
                vanishScript.SetDelay(vanishDelay);
                vanishScript.SetDuration(vanishDuration);
                vanishScript.enabled = true;
            }
        }        
    }

//----------------------------------------------------------------------------------------------------------------------

    public void GenerateQA(int numTiles, int numQueries) {

        tile selectedTile;
        int randID;
        int randAttribute;
        string tileID;
        string currCombo;
        List<string> selectionCombos = new List<string>();

        query  = string.Empty;
        answer = string.Empty;

        //prevents an unlikely infinite loop in the followiung do/while block
        if (numQueries > (numTiles * GameObject.Find("tile1").GetComponent<tile>().GetNumAttributes())) {
            Debug.Log("(" + numQueries + ") unique queries is not possible with (" + numTiles + 
                ") tiles of (" + GameObject.Find("tile1").GetComponent<tile>().GetNumAttributes() + ") attributes");
            Debug.Break();
            return;
        }

        for (int i = 1; i <= numQueries; i++) {            
            do {
                randID = Random.Range(1, numTiles + 1);
                selectedTile = GameObject.Find("tile" + randID).GetComponent<tile>();
                tileID = selectedTile.GetComponent<tile>().GetCurrID();
                randAttribute = Random.Range(1, selectedTile.GetNumAttributes() + 1);
                currCombo = tileID + ", " + randAttribute;
            } while (selectionCombos.Contains(currCombo));

            // Debug.Log("selected tileID for query: " + tileID);
            // Debug.Log("selected attribute for query: " + randAttribute);
            // Debug.Log("selected currCombo for query: " + currCombo);

            selectionCombos.Add(currCombo);

            switch (randAttribute) {
                case 1:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrBGColor());
                    SetQuery("is the background color of", tileID);
                    break;
                case 2:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrShape());
                    SetQuery("shape appears in", tileID);
                    break;
                case 3:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrShapeColor());
                    SetQuery("is the color of the " + selectedTile.GetComponent<tile>().GetCurrShape());
                    break;
                case 4:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrNum());
                    SetQuery("number is displayed in", tileID);
                    break;
                case 5:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrNumColor());
                    //SetQuery("is the number " + selectedTile.GetComponent<tile>().GetCurrNum() + "'s color");
                    SetQuery("is the color of the number \"" + selectedTile.GetComponent<tile>().GetNumAsDigit() + 
                        "\"");
                    break;
                case 6:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrText1());
                    SetQuery("color is written in", tileID);
                    break;
                case 7:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrText2());
                    SetQuery("shape is written in", tileID);
                    break;
                case 8:
                    SetAnswer(selectedTile.GetComponent<tile>().GetCurrTextColor());
                    SetQuery("color is the text of \"" + selectedTile.GetComponent<tile>().GetCurrText1() + 
                        "\" and \"" + selectedTile.GetComponent<tile>().GetCurrText2() + "\"");
                    break;
                default:
                    Debug.Log("no randAttribute assigned during GenerateQuery() or randAttribute of (" +
                        randAttribute + ") is not definted in switch case");
                    Debug.Break();
                    break;
            }
        }

        // Debug.Log("query for selected attribute(s): \"" + GetQuery() + "\"");
        // Debug.Log("answer for selected attribute(s): \"" + GetAnswer() + "\"");

        GameObject newQueryText = Instantiate(queryText, new Vector3(0, QUERY_Y_POS, 0), Quaternion.identity);
        GameObject queryCanvas = GameObject.Find("canvas");
        newQueryText.name = "query";
        newQueryText.transform.SetParent(queryCanvas.transform);
        newQueryText.GetComponent<TMP_Text>().text = GetQuery();

        if (GameObject.Find("input") == null) {
            GameObject newInputField = Instantiate(inputField, new Vector3(0, INPUT_Y_POS, 0), Quaternion.identity);
            newInputField.transform.SetParent(queryCanvas.transform);
            newInputField.name = "input";
            inputState = newInputField.GetComponent<TMP_InputField>();
            inputState.Select();
            inputState.ActivateInputField();

        } else {
            inputState = GameObject.Find("input").GetComponent<TMP_InputField>();
            inputState.text = string.Empty;
            inputState.Select();
            inputState.ActivateInputField();
        }
    }

//----------------------------------------------------------------------------------------------------------------------

    public void SetTimer(bool enableTimer, float initialTime) {
        timerBG.SetActive(true);
        GameObject newTimer = Instantiate (timer, new Vector3 (0, (TIMER_Y_POS), 0), Quaternion.identity);
        GameObject headerCanvas = GameObject.Find("header");
        newTimer.name = "timer";
        newTimer.transform.SetParent(headerCanvas.transform);
        newTimer.GetComponent<timer>().SetTime(initialTime);
        if (enableTimer) newTimer.GetComponent<timer>().enabled = true;
    }

    public void EnableOverlay() {
        scoreboard.SetActive(true);
        menus.SetActive(true);
    }

    public void DestroyField() {
        foreach(Transform child in this.transform) {
            child.gameObject.SetActive(false);
        }
        GameObject.Find("query").SetActive(false);
    }

    public List<string> ShuffleList(List<string> list) {
        for (int i = 0; i < list.Count; i++) {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    public string PrintList(List<string> list) {
        string temp = string.Empty;
        
        for (int i = 0; i < list.Count; i++) {
            temp += (list[i] + ", ");
        }
        return temp;
    }
}
