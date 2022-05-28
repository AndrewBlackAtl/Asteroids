public abstract class UpdatableBase : IUpdatable
{
    private bool isActive;


    public UpdatableBase()
    {
        isActive = true;
        UpdatableController.Add(this);
    }

    private protected void SetUpdateActive(bool value)
    {
        if (!isActive && value)
        {
            isActive = true;
            UpdatableController.Add(this);
        }
        else if (isActive && !value)
        {
            isActive = false;
            UpdatableController.Remove(this);
        }
    }

    public abstract void Update(float deltaTime);
}
