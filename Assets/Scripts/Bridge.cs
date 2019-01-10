using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
	[SerializeField] private BreakablePart[] breakableParts;

	[System.Serializable] private struct BreakablePart
	{
		public Collider2D collider;
		public GameObject sprite;
	}
}
