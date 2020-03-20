
namespace Framework
{
    public interface IExecuteHandler
    {

        void Execute(int eventCode, params object[] message);

    }
}