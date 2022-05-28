using System;


public interface ICollisionEventSender
{
    public event Action OnCollisionEnter;
}
