using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Shop))]
public class ShopDebug : Editor
{
    private int shopItemIndex = 0;
    private int coinAmount = 0;
    private Action cancelPurchase = null;
    private ShopItem shopItem = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Shop shop = (Shop)target;

        GUILayout.Space(10);
        GUILayout.Label("Debug Section", EditorStyles.boldLabel);

        coinAmount = EditorGUILayout.IntField("Amount of coins to deposit", coinAmount);

        if (GUILayout.Button("Deposit coins"))
        {
            shop.DepositCoins((uint)coinAmount);
        }

        GUILayout.Space(10);

        shopItemIndex = EditorGUILayout.IntField("Index of an element to buy", shopItemIndex);

        if (GUILayout.Button("Purchase item"))
        {
            (shopItem, cancelPurchase) = shop.Purchase(shopItemIndex);
        }

        if (GUILayout.Button("Cancel purchase"))
        {
            cancelPurchase();
            shopItem = null;
        }

        EditorGUILayout.ObjectField("Purchased item", shopItem, typeof(ShopItem), false);
    }
}
