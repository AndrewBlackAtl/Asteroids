using UnityEngine;


public class GameObjectGraphics : IGraphics
{
    private readonly GameObject gameObject;


    public GameObjectGraphics(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SetPosition(Vector2 pos)
    {
        gameObject.transform.position = pos;
    }

    public void SetRotation(Quaternion rot)
    {
        gameObject.transform.rotation = rot;
    }

    public void SetScale(float value)
    {
        gameObject.transform.localScale = new Vector3(value, value, value);
    }
}
