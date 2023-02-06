namespace Questao5.Application;

public abstract class Message
{
    public string MessageType { get; protected set; }
    public Guid EntityId { get; protected set; }

    protected Message()
    {
        MessageType = GetType().Name;
    }
}
