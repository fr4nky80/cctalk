namespace CashSystem.Protocols.CCTalk
{
    public interface IHandlerFactory
    {
        IHandler CreateHandle(byte command);
    }
}