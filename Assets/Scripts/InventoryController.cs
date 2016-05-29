using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{

	public enum InventoryItemType
	{
		Empty,
		Oil,
		Detector,
		Drill
	}

	struct InventoryItem
	{
		public InventoryItemType type;
		public int quantity;

		public InventoryItem (InventoryItemType newType, int newQuantity)
		{
			type = newType;
			quantity = newQuantity;
		}
	}

	private static Dictionary<InventoryItemType, int> MaxStackSizes = new Dictionary<InventoryItemType, int> {
		{InventoryItemType.Empty, 0},
		{InventoryItemType.Oil, 1},
		{InventoryItemType.Detector, 3},
		{InventoryItemType.Drill, 1}
	};

	public int Size;
	public GameObject[] SlotIcons;
	public Texture2D[] IconTextures;

	private InventoryItem[] contents;

	void Start ()
	{
		//Initialise inventory
		contents = new InventoryItem[Size];
		contents [0] = new InventoryItem (InventoryItemType.Detector, MaxStackSizes [InventoryItemType.Detector]); //First slot is full of detectors.
		contents [1] = new InventoryItem (InventoryItemType.Drill, 1); //Second slot has a drill.
		for (int i = 2; i < Size; ++i) { //Remaining slots are empty.
			contents [i] = new InventoryItem (InventoryItemType.Empty, 0);
		}

		if (SlotIcons.Length != Size) {
			throw new UnityException ("Inventory must have as many Inventory box children as it does inventory slots.");
		}

		UpdateUI ();
	}

	//Returns the number of the specified item type that the player has in their inventory.
	public int GetQuantityOf (InventoryItemType itemType)
	{
		int sum = 0;
		foreach (InventoryItem item in contents) {
			if (item.type == itemType) {
				sum += item.quantity;
			}
		}

		return sum;
	}

	//Attempt to add items to the inventory; returns the overflow.
	public int AddItem (InventoryItemType type, int quantity)
	{
		//First pass; Attempt to add to existing stack
		for (int i = 0; i < Size; ++i) {
			if (contents [i].type == type) {
				int storable = Mathf.Min (quantity, MaxStackSizes [type] - contents [i].quantity);
				contents [i].quantity += storable;
				quantity -= storable;
				UpdateUI ();
				if (quantity == 0) {
					return 0;
				}
			}
		}

		//Second pass; Attempt to drop into empty slot
		for (int i = 0; i < Size; ++i) {
			if (contents [i].type == InventoryItemType.Empty) {
				int storable = Mathf.Min (quantity, MaxStackSizes [type]);
				contents [i].type = type;
				contents [i].quantity = storable;
				quantity -= storable;

				UpdateUI ();
				if (quantity == 0) {
					return 0;
				}
			}
		}

		//Return the number of items we failed to add.
		return quantity;
	}


	//Attempt to deduct items from inventory; returns the number actually deducted.
	public int DeductItem (InventoryItemType type, int quantity)
	{
		int total_removed = 0;
		for (int i = 0; i < Size; ++i) {
			if (contents [i].type == type) {
				int removed = Mathf.Min (contents [i].quantity, quantity);
				contents [i].quantity -= removed;
				quantity -= removed;
				total_removed += removed;

				if (contents [i].quantity == 0) {
					contents [i].type = InventoryItemType.Empty;
				}
			}
		}
		UpdateUI ();
		return total_removed;
	}

	private void UpdateUI ()
	{
		for (int i = 0; i < Size; ++i) {
			SlotIcons [i].GetComponent<GUITexture> ().texture = IconTextures [(int)contents [i].type];
			SlotIcons [i].GetComponent<GUIText> ().text = (contents [i].quantity > 1) ? contents [i].quantity.ToString () : "";
		}
	}
}
