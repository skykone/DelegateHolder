using UnityEngine;
namespace MaxyGames.uNode.Nodes {
    
    [NodeMenu("DelegateHolder", "Subscribe DelegateHolder")] //Add the node to the menu
    public class SubscribeDelegateHolder : IFlowNode {
        [Tooltip("myDelegateHolder")]
        public DelegateHolder myDelegateHolder;
        [Tooltip("myStringMethodName")]
        public string myStringMethodName;
        [Tooltip("myMethodAction")]
        public System.Action<string> myMethodAction;
        public void Execute(object graph)
        {
            var myStrHandlers = myDelegateHolder.strHandlers;
            DelegateHolder.MyEventHandler myEvtHandlr = myDelegateHolder.CreateMEH(myMethodAction);
            myStrHandlers[myStringMethodName] = myEvtHandlr;
            
            myDelegateHolder.Subscribe(myEvtHandlr);
            

        }
    }    
    
    [NodeMenu("DelegateHolder", "Unsubscribe DelegateHolder")] //Add the node to the menu
    public class UnsubscribeDelegateHolder : IFlowNode {
        [Tooltip("myDelegateHolder")]
        public DelegateHolder myDelegateHolder;
        [Tooltip("myStringMethodName")]
        public string myStringMethodName;
        public void Execute(object graph)
        {
            var myStrHandlers = myDelegateHolder.strHandlers;
            DelegateHolder.MyEventHandler myEvtHandlr;
            var myEvtHdlrExists = myStrHandlers.TryGetValue(myStringMethodName, out myEvtHandlr);
            if (myEvtHdlrExists)
            {
                myDelegateHolder.Unsubscribe(myEvtHandlr);
            }
            else
            {
                Debug.LogError("Method named: \"" + myStringMethodName + "\" was not defined prior to use of Unsubscribe so nothing happened here. You should Subscribe to it first once.");
            }

        }
    }
    
    [NodeMenu("DelegateHolder", "Invoke DelegateHolder")] //Add the node to the menu
    public class InvokeDelegateHolder : IFlowNode {
        [Tooltip("myDelegateHolder")]
        public DelegateHolder myDelegateHolder;
        [Tooltip("myString")]
        public string myString;
        public void Execute(object graph)
        {
            myDelegateHolder.Invoke(myString);
        }
    } 
    
    [NodeMenu("DelegateHolder", "Reset DelegateHolder")] //Add the node to the menu
    public class ResetDelegateHolder : IFlowNode {
        [Tooltip("myDelegateHolder")]
        public DelegateHolder myDelegateHolder;
        public void Execute(object graph)
        {
            myDelegateHolder.ResetSubscriptions();
        }
    } 
}
