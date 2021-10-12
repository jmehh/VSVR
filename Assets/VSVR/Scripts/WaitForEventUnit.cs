using System;
using Ludiq;
using Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaitForEventUnit : Unit
{
    [DoNotSerialize]
    public ControlInput start;

    [DoNotSerialize]
    public ControlOutput end;

    [DoNotSerialize]
    public ValueInput eventName;

    [DoNotSerialize]
    public ValueOutput response;

    private string[] eventArgs;

    protected override void Definition()
    {
        start = ControlInputCoroutine("Start", WaitForEvent);

        eventName = ValueInput<string>("EventName", string.Empty);

        response = ValueOutput<string>("Response", (flow) => { return eventArgs[0]; });

        end = ControlOutput("End");
    }

    private IEnumerator WaitForEvent(Flow flow)
    {
        EventManager eventManager = GameObject.FindGameObjectWithTag("Experiment").GetComponent<EventManager>();
        bool eventTriggered = false;

        string name = flow.GetValue<String>(eventName);

        eventManager.actions[name] += (string[] args) =>
        {
            eventTriggered = true;
            eventArgs = args;
        };

        while (!eventTriggered)
        {
            yield return new WaitForSeconds(0f);
        }

        DataManager.NewData("ResponseRecorded", new List<string>(eventArgs));

        yield return end;
    }

}