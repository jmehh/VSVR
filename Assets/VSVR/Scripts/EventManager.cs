using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class EventManager : MonoBehaviour
{

    [Serializable]
    public struct Event
    {
        public string name;
        public string inputBinding;
        public string[] args;
    }

    public Event[] events;

    public Dictionary<string, Action<string[]>> actions = new Dictionary<string, Action<string[]>>();

    void Start()
    {


        foreach (Event ev in events)
        {
            if(!actions.ContainsKey(ev.name))
                actions.Add(ev.name,null);

            if (ev.inputBinding != String.Empty) // event bound to user input
            {
                var action = new InputAction(ev.name);
                action.AddBinding(ev.inputBinding);

                action.started += context =>
                {
                    actions[ev.name].Invoke(ev.args);
                };

                action.Enable();
            } 

        }


    }

}
