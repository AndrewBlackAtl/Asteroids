using UnityEngine;

public class SpriteGraphics : IGraphics
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly Sprite active;
    private readonly Sprite inactive;

    public SpriteGraphics(SpriteRenderer spriteRenderer, Sprite active, Sprite inactive)
    {
        this.spriteRenderer = spriteRenderer;
        this.active = active;
        this.inactive = inactive;
    }

    public void SetActive(bool value)
    {
        spriteRenderer.sprite = value ? active : inactive;
    }
}
