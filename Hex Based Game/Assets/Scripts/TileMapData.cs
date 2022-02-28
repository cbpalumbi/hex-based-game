using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapData
{
    private char[,] mapData;
    private int width;
    private int height;

    public TileMapData() {
        width = 5;
        height = 5;
        mapData = new char[,] { //just some default data 
            {'A', 'A', 'A', 'A', 'A'},
            {'A', 'A', 'B', 'B', 'A'},
            {'A', 'B', 'A', 'A', 'A'},
            {'A', 'A', 'A', 'B', 'A'},
            {'A', 'A', 'A', 'A', 'A'}
        };
    }
    
    public TileMapData(int width_in, int height_in, char[,] data_in){
        width = width_in;
        height = height_in;
        mapData = data_in;
    }

    public char[,] GetMapData() {
        return mapData;
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }
    
}
