// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// Specifies a field to use as label for an item in the inspector. This is especially useful for arrays of compound types.
	/// </summary>
	/// <example>In this example, the "type" field of ArrayItem will be used as the item
	/// label for the array in the inspector.
	/// <code>
	/// 
	///[Serializable]
	///public enum EnumNames
	///{
	///	  Label1,
	///	  Label2
	///}
	///
	///[Serializable]
	///public class ArrayItem
	///{
	///	  public EnumNames type;
	///   public int value; 
	///}
	///
	///public class LabelFieldTest : MonoBehaviour
	///{
	///   [LabelField("type")]
	///   public ArrayItem[] items;
	///}
	/// </code>
	/// </example>
	[Version(2, 5)]
	[AttributeUsage(AttributeTargets.Field)]
	public class LabelFieldAttribute : PropertyAttribute
	{
		public LabelFieldAttribute(string labelField)
		{
			LabelField = labelField;
		}

		public string LabelField { get; }
	}
}
