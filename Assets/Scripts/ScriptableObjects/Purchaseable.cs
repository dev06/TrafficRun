using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectID
{
	Cop,
	GarbageTruck,
	Ambo,
	FireTruck,
}

[CreateAssetMenu (fileName = "purchaseable", menuName = "Scriptable Objects/Purchaseable")]
public class Purchaseable : ScriptableObject
{
	public ObjectID id;
	public int level;
	public int cost;
	public float speedMult;
}

