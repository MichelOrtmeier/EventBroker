using EventBroker.EventAttributes;
using EventBroker.MethodInstances;
using System.Reflection;

public static class ObjectExtensionsForReflection
{
    private const BindingFlags AllMembers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    public static MethodInstance[] GetMethodInstancesWithAttribute<T>(this object instance) where T : Attribute
    {
        MethodInfo[] methods = instance.GetType().GetMethods(AllMembers);
        IEnumerable<MethodInstance> methodInstances = from method in methods
                                                      where method.GetCustomAttributes<T>(true).Any()
                                                      select MethodInstanceFactory
                                                        .CreateMethodInstance(instance, method);
        return methodInstances.ToArray();
    }

    public static EventInfo[] GetEventsWithAttribute<T>(this object instance) where T : Attribute
    {
        EventInfo[] events = instance.GetType().GetEvents(AllMembers)
                      .Where((myEvent) => myEvent.GetCustomAttributes<EventPublicationAttribute>().Any())
                      .ToArray();
        return events;
    }
}