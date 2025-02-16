using UnityEngine;

/// <summary>
/// Scriptable object to describe a purchasable shop item.
/// </summary>
[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/ShopItem")]
public class ShopItem : ScriptableObject
{
    //// TODO: Add a turret field or additional info for what you are getting after purchasing this item.
    
    [SerializeField]
    private uint cost;

    public uint Cost => cost;
}
