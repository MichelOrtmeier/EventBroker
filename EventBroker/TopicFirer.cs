using EventBroker.MethodInstances;
using System.Diagnostics;

public class TopicFirer
{
    List<MethodInstance> subscribers;
    object sender;
    object[] parameters;
    EventArgs args;

    public TopicFirer(List<MethodInstance> subscribers, object sender, EventArgs args)
    {
        this.subscribers = subscribers;
        this.sender = sender;
        this.args = args;
        dynamic dynamicallyCastedArgs = args;
        parameters = new object[] { sender, dynamicallyCastedArgs };
    }

    public void Fire()
    {
        for (int i = subscribers.Count - 1; i >= 0; i--)
        {
            InvokeMethod(subscribers[i]);
        }
    }

    private void InvokeMethod(MethodInstance method)
    {
        try
        {
            method.Invoke(parameters);
        }
        catch (Exception ex)
        {
            OnMethodInvocationFailed(method, ex);
        }
    }

    private void OnMethodInvocationFailed(MethodInstance method, Exception ex)
    {
        Debug.Write("While calling a subscribing method, something went wrong: ");
        Debug.WriteLine(ex.ToString());
    }
}