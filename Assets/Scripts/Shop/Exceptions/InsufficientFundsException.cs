using System;

public class InsufficientFundsException : Exception
{
    public uint CostOfItem { get; private set; }
    public uint CurrentAmountOfCoins { get;  private set; }

    public InsufficientFundsException(uint costOfItem, uint currentAmountOfCoins) : base("Insufficient funds to complete the purchase.")
    {
        this.CostOfItem = costOfItem;
        this.CurrentAmountOfCoins = currentAmountOfCoins;
    }
}