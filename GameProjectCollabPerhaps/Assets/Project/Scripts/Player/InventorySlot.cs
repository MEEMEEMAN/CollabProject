using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public Image image;
    public TextMeshProUGUI itemText;
    Sprite slotImage = null;
    [SerializeField]protected ItemBase m_ContainedItem;
    Vector3 imageOriginalPos;
    [SerializeField] Image parentImage;
    [SerializeField] Transform originalParent;
    public event Action<InventorySlot> DoubleClickInteraction;
    public event Action<InventorySlot, InventorySlot> ItemDropInteraction;

    public ItemBase Item
    {
        get{ return m_ContainedItem; }
        set
        {
            m_ContainedItem = value;
            if(slotImage == null)
            slotImage = image.sprite;

            if(m_ContainedItem == null)
            {
                image.sprite = slotImage;
                itemText.text = "";
            }
            else
            {
                itemText.text = Item.GetIdentifier();

                if (Item.InventoryImage != null)
                    image.sprite = m_ContainedItem.InventoryImage;
                else
                    image.sprite = GameManager.instance.GetUIManager().getDefaultSprite();
            }

        }
    }

    void Start()
    {
        imageOriginalPos = image.rectTransform.localPosition;
    }

    void Update()
    {
        if (m_ContainedItem == null)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }

    }

    public virtual void OnValidate()
    {
        if (parentImage == null)
            parentImage = GetComponent<Image>();

        parentImage.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
        originalParent = transform;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        

        if (image == null)
        {
            image = GetComponentInChildren<Image>();
        }

        if(itemText == null)
        {
            itemText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Left)
        {
            if(eventData.clickCount >= 2)
            {   
                DoubleClickInteraction(this);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(draggedSlot != null)
        {
            draggedSlot.image.rectTransform.position = Input.mousePosition + new Vector3(0f, 0f, 10f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedSlot != null)
        {
            draggedSlot.image.transform.SetParent(draggedSlot.originalParent);
            draggedSlot.image.rectTransform.localPosition = draggedSlot.imageOriginalPos;

            if (currentlyHovering != null && currentlyHovering.Item == null)
            {
                ItemDropInteraction(draggedSlot, currentlyHovering);
            }
        }

        draggedSlot = null;
    }

    static InventorySlot currentlyHovering;
    public void OnPointerEnter(PointerEventData eventData)
    {
        currentlyHovering = this;
        parentImage.color = new Color(1f, 1f, 1f, 0.8f);

        if(draggedSlot != null)
        {
            if(InventoryUI.CanDrop(draggedSlot, currentlyHovering))
            {
                parentImage.color = new Color(0f, 1f, 0f, parentImage.color.a);
            }
            else
            {
                parentImage.color = new Color(1f, 0f, 0f, parentImage.color.a);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentlyHovering = null;
        parentImage.color = new Color(1f, 1f, 1f, 0.4f);
    }

    static InventorySlot draggedSlot;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (draggedSlot == null && Item != null)
            {
                draggedSlot = this;
                draggedSlot.image.transform.SetParent(GameManager.instance.GetInventoryUI().onDragTransformParent);
            }
        }
    }
}
