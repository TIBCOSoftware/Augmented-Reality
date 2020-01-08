
using TIBCO.EFTL;

public interface IEFTLReceiver
{
     void receiveEFTL(IMessage[] messages);
     void OnConnect();
}
