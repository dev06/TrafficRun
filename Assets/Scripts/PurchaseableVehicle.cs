using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseableVehicle : MonoBehaviour
{
	public static PurchaseableVehicle active;
	public bool isPurchasedByDefault, isUnlockedByDefault;
	public Purchaseable purchaseableObject;
	private bool isPurchased, isUnlocked;


	public void Init()
	{
		IsUnlocked = LevelController.Instance.Level >= purchaseableObject.level;
		if (!IsUnlocked)
		{
			IsPurchased = false;
		}
		else
		{
			IsPurchased = PlayerPrefs.HasKey(purchaseableObject.id + "_IS_PURCHASED") ? bool.Parse(PlayerPrefs.GetString(purchaseableObject.id + "_IS_PURCHASED")) : false;
		}
	}

	public void Purchase()
	{
		IsPurchased = true;
		IsUnlocked = true;
		GameController.Instance.ModifyCoins(-purchaseableObject.cost);
		PlayerPrefs.SetString(purchaseableObject.id + "_IS_PURCHASED", IsPurchased.ToString());
		Haptic.Instance.VibrateTwice(.1f, HapticIntensity.Medium);
	}

	public bool IsPurchased
	{
		get
		{
			if (isPurchasedByDefault)
			{
				return true;
			}
			else
			{
				return isPurchased;
			}
		}

		set
		{
			this.isPurchased = value;
		}
	}

	public bool IsUnlocked
	{
		get
		{
			if (isUnlockedByDefault)
			{
				return true;
			} else
			{
				return isUnlocked;
			}
		}

		set
		{
			this.isUnlocked = value;
		}
	}

	public bool CanPurchase
	{
		get {
			return GameController.Instance.Gold >= Cost;
		}
	}

	public ObjectID ID
	{
		get {
			return purchaseableObject.id;
		}
	}

	public float Cost
	{
		get { return purchaseableObject.cost; }
	}

	public float Level
	{
		get { return purchaseableObject.level; }
	}

	public int GetIndex()
	{
		return transform.GetSiblingIndex();
	}

	public float SpeedMult
	{
		get {
			return purchaseableObject.speedMult;
		}
	}


	public Trail FX_Trail
	{
		get {return transform.GetChild(0).GetComponent<PlayerModel>().fx_trail; }
	}

	public static void SetActiveVehicle(PurchaseableVehicle v)
	{
		active = v;
		if (EventManager.OnVehicleActive != null)
		{
			EventManager.OnVehicleActive(v);
		}
		PlayerPrefs.SetInt("ACTIVE_VEHICLE_ID", (int)active.ID);
	}
}
