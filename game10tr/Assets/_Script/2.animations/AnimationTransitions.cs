using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class transitions : MonoBehaviour
{
    // phải lấy input vertical và input horizontal trong hàm tổng còn x,y ở trong này là hướng.
    // phải thiết lập animator cho từng vũ khí khi đứng yên, di chuyển, không nên dùng chung
    public void Transition(GameObject up,GameObject down,GameObject side, float x, float y)
    {
        
        
        if ((x == 1 || x == -1) && y == 0)
        {
            side.SetActive(true);
            up.SetActive(false);
            down.SetActive(false);
        }
        //directionX == 0 ||directionX==1||directionX==-1 && 
        //hien bom dang di len
        else if (y == 1)
        {
            side.SetActive(false);
            up.SetActive(true);
            down.SetActive(false);
        }
        //directionX == 0 && 
        //hien bom dang di xuong
        else if (y == -1)
        {
            up.SetActive(false);
            side.SetActive(false);
            down.SetActive(true);
        }
        
    }
}

