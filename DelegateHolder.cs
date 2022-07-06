
using System;
using System.Collections.Generic;

public class DelegateHolder
{
    public delegate void MyEventHandler(string foo);
        
    public event MyEventHandler SomethingHappened;

    public Dictionary<string, MyEventHandler> strHandlers;

    public DelegateHolder()
    {
        
    }

    public MyEventHandler CreateMEH(System.Action<string> f)
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

    public void Invoke( string s)
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
