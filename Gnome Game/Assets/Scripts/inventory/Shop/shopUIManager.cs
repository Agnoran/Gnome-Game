using UnityEngine;

public class shopUIManager : MonoBehaviour
{
    [Header("References")]
    public shopManager shop;
    public GameObject slotPrefab;
    public Transform slotParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildShopUI();
    }

    public void BuildShopUI()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < shop.shopInventory.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotParent);

            shopSlotUI slotUI = slotObject.GetComponent<shopSlotUI>();

            slotUI.Setup(shop, i);
        }
    }

    public void RefreshShopUI()
    {
        BuildShopUI();
    }
}
