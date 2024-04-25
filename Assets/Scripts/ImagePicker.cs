using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class ImagePicker
{
	private static string _path = "";
	public static Action<string> ImagePicked;

	public static bool SetImage(string path, Image image)
    {
		Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
		if (texture != null)
		{
			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
			image.sprite = sprite;
			return true;
		}
        else
        {
			return false;
        }
	}

    public static void PickImage(Image image)
    {
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					_path = "";
					return;
				}
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
				image.sprite = sprite;
				_path = path;
			}
            else
            {
				_path = "";
            }
			ImagePicked?.Invoke(_path);
		});
	}

}
