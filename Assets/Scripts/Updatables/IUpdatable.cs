using System;

public interface IUpdatable
{
    public event Action<IUpdatable, bool> SetUpdateActive;
    public void Update(float deltaTime);
}
