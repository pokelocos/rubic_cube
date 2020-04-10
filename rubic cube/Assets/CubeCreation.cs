using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreation : MonoBehaviour
{

    #region CANCER, ELIMINAR, HAAAAAAA!!!
    public Part part_Pref;
    private void CreateParts(int x, int y, int z)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < z; k++)
                {
                    //if (i == 1 && j == 1 && k == 1) { Debug.Log(i + "," + j + "," + k); continue; }  // centro

                    var part = Instantiate(part_Pref, this.transform);
                    part.transform.position = new Vector3(i - (x - 1 / 2f), j - (y - 1 / 2f), k - (z - 1 / 2f));
                   
                }
            }
        }
    }
    #endregion
}
