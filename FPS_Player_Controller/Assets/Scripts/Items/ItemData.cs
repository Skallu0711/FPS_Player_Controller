using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Data parameters")]
    
    [SerializeField] private int id;
    public int Id => id;
    
    [SerializeField] private string itemName;
    public string ItemName => itemName;
    
}