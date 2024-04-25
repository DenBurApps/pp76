using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyShower : MonoBehaviour
{
	[SerializeField] private UniWebView _uni;
	private void Start()
	{
		OpenPrivacy();
	}

	public void OpenPrivacy()
	{
		var reg = SaveSystem.LoadData<RegistrationSaveData>();
		_uni.Load(reg.Link);
		_uni.Show();
	}
}
