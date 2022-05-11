using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    Camera mainCamera;
    float minX, maxX, minY, maxY;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f,0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f,1f));
        
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    //玩家移動限制(畫面內)
    public Vector3 PlayerMoveblePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }

    //箭矢輪迴
    public Vector3 ArrowSpawnPosition(Vector3 arrowPos)
    {        
        Vector3 position = Vector3.zero;
        if(arrowPos.x > maxX)
        {
            position.x = minX;
            position.y = -arrowPos.y;
        }else if(arrowPos.x < minX)
        {
            position.x = maxX;
            position.y = -arrowPos.y;
        }
        else if(arrowPos.y > maxY)
        {
            position.y = minY;
            position.x = -arrowPos.x;
        }
        else if(arrowPos.y < minY)
        {
            position.y = maxY;
            position.x = -arrowPos.x;
        }       
        return position;
    }

    //敵人生成(畫面內+避開玩家當前位置)
    public Vector3 RandomEnemySpawnPosition(Volume enVolume, Vector3 playerPos)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + enVolume.paddingX , maxX - enVolume.paddingX);
        position.y = Random.Range(minY + enVolume.paddingY , maxY - enVolume.paddingY);        
        if(
            (position.x - enVolume.paddingX)  < (playerPos.x + 1)&&
            (position.x + enVolume.paddingX) > (playerPos.x - 1)||
            (position.y - enVolume.paddingY) > (playerPos.y + 1)&&
            (position.y + enVolume.paddingY) < (playerPos.y - 1)
            ) 
        {
            position = RandomEnemySpawnPosition(enVolume, playerPos);
        }
        return position;
    }

    //敵人移動限制(右半畫面)
    public Vector3 RandomRightHalfPosition()
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(0 , maxX);
        position.y = Random.Range(minY , maxY);

        return position;
    }

    /// <summary>
    /// 判斷傳入pos之xy是否界於視口內,返回bool值
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool DetcetIfInViewport(Vector2 pos)
    {
        if(pos.x >= minX && pos.x <= maxX && pos.y >= minY && pos.y <= maxY)
        {
            return true;
        }
        return false;
    }
}
