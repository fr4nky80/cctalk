using System.Threading.Tasks;

namespace CashSystem.Protocols.CCTalk
{
    internal interface ICCTalkCommunicationManager
    {
        Task SendMessageAsync(IMessage message);
    }
}