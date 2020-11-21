using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Swipe : MonoBehaviour
{
    private float maxFlickTime = 1f;
    private float maxAcceleration = 20f;
    private float dragTime = 0f;

    private bool isDraging;
    private float acceleration;
    private Vector2 startPosition;
    private Side swipeDirection;
    private Vector2 swipeDelta;

    public UnityAction<Side, Vector2> SwipeAction { get; set; }
    public UnityAction<Side> FlickAction { get; set; }

    private void Update()
    {
        //mouse
        if (Input.GetMouseButtonDown(0) && Input.touchCount == 0)
        {
            isDraging = true;
            startPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Reset();
        }

        //touch
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                isDraging = true;
                startPosition = touch.position;
            }
            else if(touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            {
                Reset();
            }
        }

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if(Input.touchCount > 0)
            {
                swipeDelta = Input.GetTouch(0).deltaPosition;
            }
            else if(Input.GetMouseButton(0))
            {
                swipeDelta = new Vector2(Input.GetAxis(TextStorage.MouseX) / Time.deltaTime, Input.GetAxis(TextStorage.MouseY) / Time.deltaTime);
            }

            dragTime += Time.deltaTime;
            CalculateDirection();
            //drag action
            SwipeAction?.Invoke(swipeDirection, swipeDelta);
        }
    }

    private void Reset()
    {
        if(dragTime < maxFlickTime)
        {
            acceleration = swipeDelta.magnitude / dragTime;

            if(acceleration > maxAcceleration)
            {
                CalculateDirection();

                //flick
                FlickAction?.Invoke(swipeDirection);
            }
        }

        startPosition = swipeDelta = Vector2.zero;
        isDraging = false;
        dragTime = 0f;
        acceleration = 0f;
    }

    private void CalculateDirection()
    {
        //horizontal swipe
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)
            {
                swipeDirection = Side.Right;
            }
            else
            {
                swipeDirection = Side.Left;
            }
        }
        else
        {
            if (swipeDelta.y > 0)
            {
                swipeDirection = Side.Up;
            }
            else
            {
                swipeDirection = Side.Down;
            }
        }
    }
}
