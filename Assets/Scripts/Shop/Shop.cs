using System;
using UnityEngine;

/// <summary>
/// Shop class responsible for handling shop's buy logic.
/// Should exist only one instance per save.
/// </summary>
public class Shop : MonoBehaviour
{
    [SerializeField]
    private ShopItem[] shopItems;

    [SerializeField]
    private uint amountOfCoins;

    /// <summary>
    /// Purchase an item from the shop.
    /// </summary>
    /// <returns>Tuple of the bought shop item and transaction cancellation.</returns>
    public (ShopItem, Action) Purchase(int index)
    {
        ShopItem shopItem = shopItems[index];
        uint costOfItem = shopItem.Cost;

        if (amountOfCoins < costOfItem)
        {
            throw new InsufficientFundsException(costOfItem, amountOfCoins);
        }

        amountOfCoins -= costOfItem;

        return (
            shopItem,
            delegate
            {
                amountOfCoins += costOfItem;
            }
        );
    }

    public void DepositCoins(uint amount)
    {
        amountOfCoins += amount;
    }

    public ShopItem[] ShopItems => shopItems;
    public uint AmountOfCoins => amountOfCoins;
}
