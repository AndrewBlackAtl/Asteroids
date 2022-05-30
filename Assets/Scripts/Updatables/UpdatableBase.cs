public abstract class UpdatableBase : IUpdatable
{
    private bool isActive;


    public UpdatableBase()
    {
        isActive = true;
        UpdatableController.I.Add(this);
    }

    protected void SetUpdateActive(bool value)
    {
        if (!isActive && value)
        {
            isActive = true;
            UpdatableController.I.Add(this);
        }
        else if (isActive && !value)
        {
            isActive = false;
            UpdatableController.I.Remove(this);
        }
    }

    public abstract void Update(float deltaTime);
}
