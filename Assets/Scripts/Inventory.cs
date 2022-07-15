using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Inventory and UI
public class Inventory : MonoBehaviour
{
    //Values
    [SerializeField] private int coinCount;
    [SerializeField] private int loopCount;
    [SerializeField] private int loopsToWin = 5;
    //Add Coin
    public void AddCoin(int amount)
    {
        //Add the amount to the current coin count
        coinCount += amount;
    }

    //Remove Coin
    public void RemoveCoin(int amount)
    {
        //Remove the amount from the current coin count
        coinCount -= amount;
    }

    //add Loop
    public void AddLoop(int amount)
    {
        //Add the amount to the current loop count
        loopCount += amount;
        loopsToWin = 5; //debug - hardcore override = not sure where my code remember it as "3" :<
        if (loopCount >= loopsToWin)
        {
            GameManager.instance.GameOverSequence();
        }
        GameManager.instance.UpdateLoops(loopCount);
    }
    

    //Remove Loop
    public void RemoveLoop(int amount)
    {
        //Remove the amount from the current loop count
        loopCount -= amount;
        GameManager.instance.UpdateLoops(loopCount);
    }
   
}