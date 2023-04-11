using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tile : MonoBehaviour {

    public GameObject tileIDText;
    public GameObject tileBG;
    public GameObject circle;
    public GameObject diamond;
    public GameObject hexagon;
    public GameObject rectangle;
    public GameObject square;
    public GameObject triangle;
    public GameObject text1;
    public GameObject text2;
    public GameObject tileNumText;        

    public int numAttributes = 0;

    public string currID = string.Empty;
    public string currBGColor = string.Empty;
    public string currShape = string.Empty;
    public string currShapeColor = string.Empty;
    public string currNum = string.Empty;
    public string currNumColor = string.Empty;
    public string currText1 = string.Empty;
    public string currText2 = string.Empty;
    public string currTextColor = string.Empty;

    private Color   currColor;
    private Color32 currColor32;
    private GameObject shape;
    private float currLayer = -1.0f;
    private float textScaler = 1.5f;
    private string numAsDigit;
    
    private const float NUM_TEXT_SPACE = 0.75f;
    private const float TILE_OUTLINE   = 0.95f;
    private const float SHAPE_INSET    = 0.70f;
    private const float RECT_Y_SCALE   = 0.35f;
    private const float TRI_Y_OFFSET   = 0.15f;
    private const float DIA_Y_SCALE    = 0.45f;
    private const float TEXT_V_SPACE   = 0.22f;

//----------------------------------------------------------------------------------------------------------------------

    private void SetCurrID(List<string> availIDs) {
        currID = availIDs[0];
        availIDs.RemoveAt(0);
        //Debug.Log("currID rand = " + currID);
    }

    public string GetCurrID() {
        return currID;
    }
    
    public int GetNumAttributes() {
        return numAttributes;
    }

    private float GetCurrLayer() {
        //Debug.Log("currLayer = " + currLayer);
        return currLayer -= 0.1f;
    }

    private void SetCurrBGColor(List<string> availColors) {
        currBGColor = availColors[0];
        availColors.RemoveAt(0);
        numAttributes++;
        // Debug.Log("currBGColor rand = " + currBGColor);
    }

    public string GetCurrBGColor() {
        return currBGColor;
    }

    private void SetCurrShape(List<string> availShapes) {
        currShape = availShapes[0];
        availShapes.RemoveAt(0);
        numAttributes++;
        //Debug.Log("currShape rand = " + currShape);
    }

    public string GetCurrShape() {
        return currShape;
    }

    private void SetCurrShapeColor(List<string> availColors) {
        currShapeColor = availColors[0];

        while (currShapeColor == currBGColor) {
            availColors.RemoveAt(0);
            availColors.Add(currShapeColor);    
            currShapeColor = availColors[0];
        }

        availColors.RemoveAt(0);
        numAttributes++;
        //Debug.Log("currShapeColor rand = " + currShapeColor);
    }

    public string GetCurrShapeColor() {
        return currShapeColor;
    }

    private void SetCurrNum(List<string> availNums) {
        currNum = availNums[0];        
        availNums.RemoveAt(0);
        numAttributes++;
        //Debug.Log("currNum rand = " + currNum);
    }

    public string GetCurrNum() {
        return currNum;
    }

    private void SetCurrNumColor(List<string> availColors) {
        currNumColor = availColors[0];

        while (currNumColor == currShapeColor || currNumColor == currBGColor || currNumColor == currTextColor) {
            availColors.RemoveAt(0);
            availColors.Add(currNumColor);    
            currNumColor = availColors[0];
        }

        availColors.RemoveAt(0);
        numAttributes++;
        //Debug.Log("currNumColor rand = " + currNumColor);
    }

    public string GetCurrNumColor() {
        return currNumColor;
    }

    private void SetCurrText1(List<string> availColors) {
        currText1 = availColors[0];        
        availColors.RemoveAt(0);
        numAttributes++;
        //Debug.Log("currText1 rand = " + currText1);
    }

    public string GetCurrText1() {
        return currText1;
    }

    private void SetCurrText2(List<string> availShapes) {
        currText2 = availShapes[0];        
        availShapes.RemoveAt(0);
        numAttributes++;
        //Debug.Log("currText2 rand = " + currText2);
    }

    public string GetCurrText2() {
        return currText2;
    }

    private void SetCurrTextColor(List<string> availColors) {
        currTextColor = availColors[0];

        while (currTextColor == currShapeColor || currTextColor == currBGColor) {
            availColors.RemoveAt(0);
            availColors.Add(currTextColor);    
            currTextColor = availColors[0];
        }

        availColors.RemoveAt(0);
        numAttributes++;
        //Debug.Log("currTextColor rand = " + currTextColor);
    }

    public string GetCurrTextColor() {
        return currTextColor;
    }

    public string GetNumAsDigit() {
        return numAsDigit;
    }

//----------------------------------------------------------------------------------------------------------------------

    public void Populate(float tileScale, int tileID, int numTiles, List<string> availIDs,
        List<string> availBGColors, List<string> availShapes, List<string> availShapeColors, 
        List<string> availText1, List<string> availText2, List<string> availTextColors,
        List<string> availNums, List<string> availNumColors, int complexity) {

        //Debug.Log("populating... " + name);
        
        GameObject thisTile = GameObject.Find("tile" + tileID);
        GameObject thisCanvas = GameObject.Find("canvas" + tileID);
        Vector3 tilePosition = thisTile.transform.position;
        Vector3 localScale = thisTile.transform.localScale;

        SetCurrID(availIDs);
        tileIDText.GetComponent<TMP_Text>().text = GetCurrID();
        tileIDText.GetComponent<TMP_Text>().fontSize = (localScale.x * (textScaler * 2));
        GameObject currID = Instantiate(tileIDText, tilePosition + new Vector3 
            (0, ((tileScale / -2) - NUM_TEXT_SPACE), 0), Quaternion.identity);
        currID.name = "currID";
        currID.transform.SetParent(thisTile.transform);
        currID.transform.SetParent(thisCanvas.transform);

        SetCurrBGColor(availBGColors);
        //REF: https://docs.unity3d.com/ScriptReference/ColorUtility.TryParseHtmlString.html
        currColor = Color.clear; ColorUtility.TryParseHtmlString(GetCurrBGColor(), out currColor);

        GameObject currBGColor = Instantiate(tileBG, tilePosition + new Vector3 (0, 0, GetCurrLayer()), 
            Quaternion.identity);
        currBGColor.transform.localScale = localScale * TILE_OUTLINE;
        currBGColor.name = "currBGColor";
        currBGColor.GetComponent<Renderer>().material.color = currColor;
        currBGColor.transform.SetParent(thisTile.transform);
        
        if (complexity >= 2) {
            SetCurrShape(availShapes);
            switch (GetCurrShape()) {
                case "circle":
                    shape = circle;
                    break;
                case "diamond":
                    shape = diamond;
                    break;
                case "hexagon":
                    shape = hexagon;
                    break;
                case "rectangle":
                    shape = rectangle;
                    break;
                case "square":
                    shape = square;
                    break;
                case "triangle":
                    shape = triangle;
                    break;
                default:
                    Debug.Log("no currShape set during Populate()");
                    Debug.Break();
                    break;
            }

            SetCurrShapeColor(availShapeColors);
            currColor = Color.clear; ColorUtility.TryParseHtmlString(GetCurrShapeColor(), out currColor);                
            GameObject currShape = Instantiate(shape, tilePosition + 
                new Vector3 (0, 0, GetCurrLayer()), Quaternion.identity);
            currShape.transform.localScale = localScale * (SHAPE_INSET);
            
            //shape specific adjustments for conformity
            if (shape == rectangle) { 
                currShape.transform.localScale -= new Vector3(0, (currShape.transform.localScale.y * RECT_Y_SCALE), 0); 
            }
            if (shape == triangle) { 
                currShape.transform.position -= new Vector3(0, (currShape.transform.localScale.y * TRI_Y_OFFSET), 0);
            }
            if (shape == diamond) { 
                currShape.transform.localScale += new Vector3(0, (currShape.transform.localScale.y * DIA_Y_SCALE), 0);
                currShape.transform.Rotate(0, 0, 90);
            }

            currShape.name = "currShape";
            currShape.GetComponent<Renderer>().material.color = currColor;
            currShape.transform.SetParent(thisTile.transform);
        }

        if (complexity >= 3) {
            SetCurrNum(availNums);
            switch (GetCurrNum()) {
                case "zero":
                    numAsDigit = "0";
                    break;
                case "one":
                    numAsDigit = "1";
                    break;    
                case "two":
                    numAsDigit = "2";
                    break;
                case "three":
                    numAsDigit = "3";
                    break;
                case "four":
                    numAsDigit = "4";
                    break;
                case "five":
                    numAsDigit = "5";
                    break;
                case "six":
                    numAsDigit = "6";
                    break;
                case "seven":
                    numAsDigit = "7";
                    break;
                case "eight":
                    numAsDigit = "8";
                    break;
                case "nine":
                    numAsDigit = "9";
                    break;
                default:
                    Debug.Log("no currNum set during Populate()");
                    Debug.Break();
                    break;
            }

            SetCurrNumColor(availNumColors);
            currColor = Color.clear; ColorUtility.TryParseHtmlString(GetCurrNumColor(), out currColor);
            currColor32 = currColor;
            tileNumText.GetComponent<TMP_Text>().text = numAsDigit;
            tileNumText.GetComponent<TMP_Text>().fontSize = (localScale.x * (textScaler * 2.5f));
            tileNumText.GetComponent<TMP_Text>().color = currColor32;
            GameObject currNum = Instantiate(tileNumText, tilePosition + new Vector3 
                (0, 0, GetCurrLayer()), Quaternion.identity);
            currNum.name = "currNum";
            currNum.transform.SetParent(thisTile.transform);
            currNum.transform.SetParent(thisCanvas.transform);
        }

        if (complexity == 4) {
            SetCurrText1(availText1);
            SetCurrTextColor(availTextColors);
            currColor = Color.clear; ColorUtility.TryParseHtmlString(GetCurrTextColor(), out currColor);
            currColor32 = currColor;
            text1.GetComponent<TMP_Text>().text = GetCurrText1();
            text1.GetComponent<TMP_Text>().fontSize = (localScale.x * textScaler);
            text1.GetComponent<TMP_Text>().color = currColor32;
            GameObject currText1 = Instantiate(text1, tilePosition + new Vector3 
                (0, (localScale.y * TEXT_V_SPACE), GetCurrLayer()), Quaternion.identity);
            currText1.name = "currText1";
            currText1.transform.SetParent(thisTile.transform);
            currText1.transform.SetParent(thisCanvas.transform);

            SetCurrText2(availText2);
            text2.GetComponent<TMP_Text>().text = GetCurrText2();
            text2.GetComponent<TMP_Text>().fontSize = (localScale.x * textScaler);
            text2.GetComponent<TMP_Text>().color = currColor32;
            GameObject currText2 = Instantiate(text2, tilePosition + new Vector3 
                (0, -(localScale.y * TEXT_V_SPACE), GetCurrLayer()), Quaternion.identity);
            currText2.name = "currText2";
            currText2.transform.SetParent(thisTile.transform);
            currText2.transform.SetParent(thisCanvas.transform);
        }
    }
}
