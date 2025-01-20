namespace CashSystem.Protocols.CCTalk
{
    public interface IHandler
    {
        byte[] Handle(IMessage message);
    }
}