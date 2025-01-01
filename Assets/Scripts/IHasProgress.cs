using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.UIElements.Experimental;

public interface IHasProgress
{
    // A publisher is a component in the observer pattern(used in event-driven programming) responsible for:
    // Defining Events: Declaring events that other components (subscribers) can listen to.
    // Raising Events: Triggering (raising) the events to notify subscribers about changes or specific actions.
    // This is Publisher
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;    

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }


}
