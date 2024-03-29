# DelegateHolder

A set of custom nodes and functionality to be used with uNode Visual Scripting, Unity asset.

------

December 2022 Update 1 - single argument is object now rather than string, for more flexibility on what you can pass in as the argument when invoking. 
Tested with uNode 2.1.3 (uNode version 2.1.3 released 11/30/2022)

![](https://cdn.discordapp.com/attachments/994083232712773645/1048901026419322880/1_.png)
![](https://cdn.discordapp.com/attachments/994083232712773645/1048901026788409395/2_.png)

------

In case one wonders if it is possible to use the Unity Asset uNode Visual Scripting version 2.1.1 to register event, invoke event, unregister event, and clear event, including any arbitrary method whether it is on the same uNode graph or even just any arbitrary method, and whether it is a Unity Event or a custom event, and whether it is called from Update function, FixedUpdate function, from a custom coroutine, or even from outside of a coroutine from elsewhere, it should be possible to do all of this with uNode.


If you really want to be able to do this with a set of custom nodes in particular,
well it should be possible to create custom class and also custom nodes, specifically custom implementation of IFlowNode as described in [uNode documentation about Custom Node - docs.maxygames.com](http://docs.maxygames.com/unode/manual/guide/creating-custom-node.html) to decide one or more arbitrary events to be registered, invoked, unregistered, and cleared. Then the custom uNode nodes themselves can be used in any uNode Class Component Graph to achieve the desired functionality.

This is my implementation DelegateHolder.cs a possible initial implementation of this.

I do not really like this way of doing it too much, and it needs some improvement, for example right now it support one string as parameter for each event method signature so maybe need to reimplement it more generically to be more useful for wider variety of method signatures and parameter types,
and there are some more improvements which should be made, I made the example very quickly just to demonstrate it is possible to do it in uNode, but some improvements should be made to this and it is not really recommended to be used in production exactly like this.

Just in case that anyone was really interested to try to do it like this specifically, and insisting on the use of custom uNode nodes as the way to do it, here is one possible way of doing it using uNode custom nodes:

1. Place DelegateHolder.cs and uNodeCustomNodes.cs in your Unity project's Assets folder.
2. In uNode create a new Component Class and name it something such as test_component_del_h
3. Define and implement the Component Class as described in [uNode Component Class Definition and Implementation](#unode-component-class-definition-and-implementation) for an example of how these custom nodes can be used.
4. Save and Compile the uNode Class Component Graph.
5. Create an empty GameObject and add component uNodeSpawner, and set the graph to be the test_component_del_h Class Component graph you just made
6. Now press play and notice it should work.


#### uNode Component Class Definition and Implementation

Because it could cause the Unity editor to hang or even crash (or have to be crashed) if not careful, such as by setting any of the local booleans to false which may create a kind of infinite loop that should never be done if the regions highlighted in red below are ever entered, I will not provide the class component prefab for now, instead only some screenshots and you can follow along and build the component class yourself manually and carefully. 


First  make a DelegateHolder named uMyDelegateHolder, which should just have the default values

![](https://cdn.discordapp.com/attachments/994083232712773645/994102558920540230/delh_var1_.png)

and a string named function1_name with value of Function1

![](https://cdn.discordapp.com/attachments/994083232712773645/994102559113490502/delh_var2_.png)

For the following Functions in the graph, define them like this:

- For Start build it like this:

![](https://cdn.discordapp.com/attachments/994083232712773645/994102559352553532/delh_Start_.png)

What happens here is very first thing, a Method Action Function1 is defined and fed into the "My Method Action" input of custom node "Subscribe DelegateHolder". For purposes of simplicity, the Method body in this example is actually going to be one of the very methods on the same uNode graph itself! In this case the body is just the Function1 we have in the same graph.

For My Delegate Holder input using the variable uMyDelegateHolder which will be always the same throughout the graph, since in this example we will want to just use the same DelegateHolder container for this particular graph.

For the "My String Method Name" using the variable "function1_name". Later from even inside the very event itself, we will unsubscribe from this specific event to demonstrate functionality of unsubscribing from a specific event that was previously subscribed to, and keeping the name in variable will make this easier to do.

Finally, using custom node Invoke DelegateHolder and pass in the same uMyDelegateHolder and also using the string value "myStep_1" which will be passed into the node as well (later and also in the console output it will be demonstrated that Function1 will definitely receive this "myStep_1" and be able to output it as well).

Effectively this "Invoke" will go ahead and execute any of the one or more method action subscribed to in DelegateHolder. In this case it will end up executing the one Method Action, which is Function1, and that is what will be executed.


- For Function1    build it like this:

![](https://cdn.discordapp.com/attachments/994083232712773645/994102559616815134/delh_Function1_.png)


By the way, for Function1 and for all the other functions like this from now on, make sure to use the uNode Inspector to define one string parameter named str for the function, like this:

![](https://cdn.discordapp.com/attachments/994083232712773645/994111879549034587/delh_sig_.png)



In this one the specific funtion1_name is passed in to a custom unsubscribe node. 
In this specific case, it happen to be actually unsubscribing the very same method which is running right now, and it is the same one which wa subscribed to in the Start function above.

After this unsubscription, the custom nodes defined after that will end up subscribing two methods, 
Function11 and Function111 
and then finally Invoke, with the string passed of "myStep_2".

This will end up causing BOTH of the functions Function11 and Function 111 to execute at the same time in parallel, since there are actually two methods subscribed just prior to the Invoke. Both of them will execute at the same time.


- For Function11   build it like this:

![](https://cdn.discordapp.com/attachments/994083232712773645/994102559822319636/delh_Functiuon_11_.png)

This is just to simply illustrate the method being run successfully. It will log 11 to console to show it is from Function11, 
and log the string passed in from Invoke to demonstrate the working functionality of the argument that is passed in from Invoke.
In this case it should log the string of "myStep_2"

- For Function111  build it like this:

![](https://cdn.discordapp.com/attachments/994083232712773645/994102560086573066/delh_Function111_.png)

This one works kind of like Function 1 except this time, a custom node Reset DelegateHolder is used which will clear all subscriptions without needing to unsubscribe from each one individually - so this will in effect remove both subscriptions even without them being individually removed .
Then, an Function1111 is registered, intended to be the final method for this example, and invoked.

- For Function1111 build it like this:

![](https://cdn.discordapp.com/attachments/994083232712773645/994102560321458298/delh_Function1111_.png)

This one is similar to Function 11 just to illustrate it is working - it logs a couple of things to show it is working, and at the end resets the DelegateHandler to remove any subscriptions - and then the method is done.

- That is it

Now save and compile the Class Component graph.

Create an empty GameObject put it anywhere in your active scene.
Add a uNodeSpawner Component to the GameObject and set the graph to be the test_component_del_h Class Component graph you just made. 

![](https://cdn.discordapp.com/attachments/994083232712773645/994102558719221872/delh_uNodeSpawner_.png)

Now press play in Unity Editor. 
It should give the following expected output when run:

![](https://cdn.discordapp.com/attachments/994083232712773645/994102558501122118/delh_console_output_.png)


That is one way you can do something like this in uNode.

If you really like it this kind of way, you can use it if you want, but you should be careful of some things. For example if you want the same invocation to repeat again as shown in the red warning above, you may want to first either at least wrap this in a Coroutine and using yield, or otherwise making sure you have custom way the event is fired that is intermittent or otherwise not just the exact same continuous call over and over again, so that the Unity Editor does not crash due to infinite calling of the same method again and again, for one. 

Also note I made the above example very quickly and so in general this example should be used just to study or test on some approaches in uNode, since this example implementation may benefit from some improvements before used in production.


THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
