using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        // Event Subscriber
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;

        // When OnStateChanged.Invoke is called, all event subscribers (like StoveCounter.OnStateChanged)
        // that have registered to the OnStateChanged event using += will immediately receive the event and be executed.
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);

        // or do the statement below. 
        //bool showVisual = false;
        //if (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        //{
        //    showVisual = true;
        //    stoveOnGameObject.SetActive(showVisual);
        //    particlesGameObject.SetActive(showVisual);
        //}
        //else
        //{
        //    showVisual = false;
        //    stoveOnGameObject.SetActive(showVisual);
        //    particlesGameObject.SetActive(showVisual);
        //}
    }
}
