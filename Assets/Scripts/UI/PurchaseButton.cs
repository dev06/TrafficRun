using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class PurchaseButton : ButtonEventHandler
{

	public PurchaseableVehicle vehicle;

	[Header("UI Fields")]
	public Image icon;

	[Header("Sprites")]
	public Sprite unlockedSprite;
	public Sprite lockedSprite;

	[Header("Text Fields")]
	public TextMeshProUGUI lockedLevelText;
	public TextMeshProUGUI costText;
	public TextMeshProUGUI notEnoughCoinCostText;

	[Header("Groups")]
	public CanvasGroup lockedGroup;
	public CanvasGroup unlockedGroup;
	public CanvasGroup notEnoughCoinsGroup;

	private Toggle _toggle;

	void Start()
	{
		_toggle = GetComponent<Toggle>();
	}

	public override void OnPointerClick(PointerEventData data)
	{
		if (vehicle.IsPurchased)
		{
			base.OnPointerClick(data);
			ButtonClick();
		}
	}

	public void ButtonClick() {
		if (vehicle == null) { return; }
		if (!vehicle.IsUnlocked) { return; }
		if (vehicle.IsUnlocked && !vehicle.IsPurchased && vehicle.CanPurchase)
		{
			purchase();
		}
		if (vehicle.IsPurchased)
		{
			if (PurchaseableVehicle.active != vehicle)
			{
				PurchaseableVehicle.SetActiveVehicle(vehicle);
			}
			Haptic.Vibrate(HapticIntensity.Light);
			_toggle.isOn = true;
		}

	}

	public void purchase()
	{
		lockedGroup.alpha = unlockedGroup.alpha = 0f;
		vehicle.Purchase();
		if (EventManager.OnVehiclePurchase != null)
		{
			EventManager.OnVehiclePurchase(vehicle);
		}

		var parameters = new Dictionary<string, object> ();
		//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "DISP_LEVEL_COMPLETE", LevelToDisplay);
		//GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "ABS_LEVEL_COMPLETE", Level);

		parameters["Vehicle"] = vehicle.ID;
		parameters["Level"] = LevelController.Instance.Level;
		parameters["Money"] = GameController.Instance.Gold;
		FacebookManager.Instance.EventSent ("Vehicle Purchase", 1, parameters);
		_toggle.isOn = true;
	}

	public void updateData()
	{
		if (vehicle == null) { return; }
		_toggle.interactable = vehicle.IsUnlocked && vehicle.IsPurchased;
		icon.sprite = unlockedSprite;

		_toggle.isOn = PurchaseableVehicle.active == vehicle;

		//vehicle is not unlocked
		if (!vehicle.IsUnlocked)
		{
			lockedGroup.alpha = 1f;
			unlockedGroup.alpha = 0f;
			notEnoughCoinsGroup.alpha = 0f;
			icon.sprite = lockedSprite;
		}

		//vehicle is unlocked, can purchase but is not purchased
		if (vehicle.IsUnlocked && !vehicle.IsPurchased && vehicle.CanPurchase)
		{
			lockedGroup.alpha = 0f;
			notEnoughCoinsGroup.alpha = 0f;
			unlockedGroup.alpha = 1f;
		}

		//vehicle is unlocked, can purchase but is not purchased
		if (vehicle.IsUnlocked && !vehicle.IsPurchased && !vehicle.CanPurchase)
		{
			lockedGroup.alpha = 0f;
			unlockedGroup.alpha = 0f;
			notEnoughCoinsGroup.alpha = 1f;
		}





		//vehicle is unlocked and purchased
		if (vehicle.IsUnlocked && vehicle.IsPurchased)
		{
			lockedGroup.alpha = unlockedGroup.alpha = notEnoughCoinsGroup.alpha =  0f;
		}


		costText.text = vehicle.Cost.ToString();
		lockedLevelText.text = "Level " + vehicle.Level.ToString();
		notEnoughCoinCostText.text = vehicle.Cost.ToString();
	}

	public void OnButtonClick()
	{


	}
}
