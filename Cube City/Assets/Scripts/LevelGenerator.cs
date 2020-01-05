using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject W_Cube, B_Cube;
    public int Width = 10;
    public int Depth = 10;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Width; i += 1){
            for (int j = 0; j < Depth; j += 1 ){
                if ((i % 2 == 0 && j % 2 == 1) || (i%2 == 1 && j %2 == 0)) Instantiate(W_Cube, new Vector3(i, 0, j), Quaternion.identity);
                else Instantiate(B_Cube, new Vector3(i, 0, j), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
