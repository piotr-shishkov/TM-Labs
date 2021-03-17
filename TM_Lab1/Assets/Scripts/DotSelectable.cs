using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotSelectable : MonoBehaviour
{
	public Image DotImage;
	public Sprite SelectedSprite;
	public Sprite UnselectedSprite;

	public void OnPointerEnter()
	{
		DotImage.sprite = SelectedSprite;
	}

	public void OnPointerExit()
	{
		DotImage.sprite = UnselectedSprite;
	}
}
