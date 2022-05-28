using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Spawner 
{
    private readonly Vector2 sceneDimension;
    private readonly float horizontalOffset;
    private readonly float verticalOffset;


    public Spawner(Vector2 sceneDimension, float horizontalOffset, float verticalOffset) 
    {
        this.sceneDimension = sceneDimension;
        this.horizontalOffset = horizontalOffset;
        this.verticalOffset = verticalOffset;
    }

    private protected Vector2 GetStartPos() 
    {
        float firstX;
        float secondX;
        float firstY;
        float secondY;

        //horizontal or vertical axis
        if (Random.value > 0.5f)
        {
            firstX = -sceneDimension.x + sceneDimension.x * horizontalOffset;
            secondX = sceneDimension.x - sceneDimension.x * horizontalOffset;
            //top or bottom
            if (Random.value > 0.5f)
            {
                firstY = sceneDimension.y - sceneDimension.y * verticalOffset;
                secondY = sceneDimension.y;
            }
            else
            {
                firstY = -sceneDimension.y;
                secondY = -sceneDimension.y + sceneDimension.y * verticalOffset;
            }
        }
        else
        {
            firstY = -sceneDimension.y + sceneDimension.y * verticalOffset;
            secondY = sceneDimension.y - sceneDimension.y * verticalOffset;
            //right or left
            if (Random.value > 0.5f)
            {
                firstX = sceneDimension.x - sceneDimension.x * horizontalOffset;
                secondX = sceneDimension.x;
            }
            else
            {
                firstX = -sceneDimension.x;
                secondX = -sceneDimension.x + sceneDimension.x * horizontalOffset;
            }
        }

        return new Vector2(Random.Range(firstX, secondX), Random.Range(firstY, secondY));
    }
} 
