using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field : MonoBehaviour
{
    public GameObject tile;
    private const int TILE_X_SPACE = 1;
    private const int TILE_Y_POS = 2;
    private const int FIELD_SCALE = 18;

    public void BuildField(int numTiles, bool populate) {
        
        float tileScale = tile.transform.localScale.x;
        float fieldReq = (tileScale * numTiles) + (TILE_X_SPACE * (numTiles - 1));

        //prevents a field of 0 tiles
        if (numTiles < 1) {
            Debug.Log("a field with (" + numTiles + ") cards is invalid");
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

        //TODO: check for existing instances of cards. objects should be modded, not destoryed and recreated for normal gameplay

        float leftBound = (FIELD_SCALE / -2);
        float fieldSpace = FIELD_SCALE - (tileScale * numTiles);
        float tileOffset = (fieldSpace - (TILE_X_SPACE * (numTiles - 1))) / 2; 
        float tileXPos = leftBound + tileOffset + (tileScale / 2);

        Debug.Log("leftBound = " + leftBound);
        Debug.Log("tileScale = " + tileScale);
        Debug.Log("fieldSpace = " + fieldSpace);
        Debug.Log("tileOffset = " + tileOffset);
        Debug.Log("tileXPos = " + tileXPos);

        for (int i = 0; i <= numTiles - 1; i++) {
            float x = tileXPos + (i * (tileScale + TILE_X_SPACE));
            float y = TILE_Y_POS;

            Vector3 position = new Vector3(x, y, 0);

            GameObject newTile = (GameObject)Instantiate(tile, position, Quaternion.identity);
            newTile.name = "tile" + i.ToString();

            if (populate) {
                newTile.GetComponent<tile>().Populate();
            }
        }        
    }
}
