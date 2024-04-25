using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllGuestsShower : MonoBehaviour
{
    [SerializeField] private List<GuestItem> _items;
    [SerializeField] private GuestsShower _guestsShower;
    [SerializeField] private TMP_InputField _searchInput;
    [SerializeField] private GameObject _filterPanel;
    private List<GuestItem> _activeElements = new List<GuestItem>();
    private int _currentFilter = 0;

    private void Awake()
    {
        foreach (var item in _items)
        {
            item.Show += ShowInfo;
        }
        SetFilter(0);
    }

    public void SetFilter(int index)
    {
        _currentFilter = index;
        _searchInput.text = "";
        _filterPanel.SetActive(false);
        UpdateItems();
    }

    public void HideFilter()
    {
        _filterPanel.SetActive(false);
    }

    public void ShowFilter()
    {
        _filterPanel.SetActive(true);
    }

    public void SearchByFilter(string text)
    {
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        for (int i = 0; i < _activeElements.Count; i++)
        {
            switch (_currentFilter)
            {
                case 0:
                    {
                        string name = guests.Guests[_activeElements[i].GetIndex()].Name.ToLower();
                        if (name.Contains(text.ToLower()))
                        {
                            _activeElements[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            _activeElements[i].gameObject.SetActive(false);
                        }
                        break;
                    }
                case 1:
                    {
                        string name = guests.Guests[_activeElements[i].GetIndex()].RoomNumber.ToString();
                        if (name.Contains(text.ToLower()))
                        {
                            _activeElements[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            _activeElements[i].gameObject.SetActive(false);
                        }
                        break;
                    }
            }

        }
    }

    private void OnEnable()
    {
        UpdateItems();
        _searchInput.onValueChanged.AddListener(SearchByFilter);
    }

    private void OnDisable()
    {
        _searchInput.onValueChanged.RemoveListener(SearchByFilter);
    }

    private void UpdateItems()
    {
        var guests = SaveSystem.LoadData<GuestsSaveData>();
        foreach (var item in _items)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < guests.Guests.Count; i++)
        {
            _items[i].SetInfo(i);
            _items[i].gameObject.SetActive(true);
            _activeElements.Add(_items[i]);
        }
    }

    public void ShowInfo(int index)
    {
        _guestsShower.ShowInfo(index);
    }
}
