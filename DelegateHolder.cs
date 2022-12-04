
using System;
using System.Collections.Generic;

public class DelegateHolder
{
    public delegate void MyEventHandler(object foo);
        
    public event MyEventHandler SomethingHappened;

    public Dictionary<string, MyEventHandler> strHandlers;

    public DelegateHolder()
    {
        
    }

    public MyEventHandler CreateMEH(System.Action<object> f)
    {
        return new MyEventHandler(f);
    }
    
    public void Subscribe(MyEventHandler handler)
    {

        SomethingHappened += handler;
    }
 
    public void Unsubscribe(MyEventHandler handler)
    {

        SomethingHappened -= handler;
    }

    public void ResetSubscriptions()
    {
        SomethingHappened = delegate { };
    }

    public void Invoke( object s)
    {
        try
        {
            SomethingHappened.Invoke(s);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }


}
