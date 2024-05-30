using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantitiyText;
    private Outline outline;
    // Start is called before the first frame update

    public int index;
    public bool equipped;
    public int quantitiy;
    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        outline.enabled = equipped;
    }
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantitiyText.text = quantitiy > 1 ? quantitiy.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantitiyText.text = string.Empty;
    }
    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }

}
