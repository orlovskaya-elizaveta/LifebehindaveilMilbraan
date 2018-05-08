using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnScene : MonoBehaviour {

    //всплывающая панель описания
    private GameObject DescriptionPanel;

    //ссылка на инвентарь
    private Inventory inv;

    //характеристики объекта
    ItemData itemData = new ItemData
    {
        id = 1
    };

    private void Start()
    {
        //находим дочернюю панель и делаем ее неактивной
        DescriptionPanel = this.gameObject.transform.Find("DescriptionPanel").gameObject;
        DescriptionPanel.SetActive(false);
    }

    //наводим - появляется описание
    void OnMouseOver()
    {
        DescriptionPanel.SetActive(true);
    }

    //убираем курсор - описание пропадает
    void OnMouseExit()
    {
        DescriptionPanel.SetActive(false);
    }

    //по клику подбираем предмет в инвентарь и удаляем его со сцены
    void OnMouseUp()
    {

        //находим наш инвентарь
        inv = GameObject.Find("InventoryManager").GetComponent<Inventory>();

        //добавляем в него данные об объекте
        inv.items.Add(itemData);

        //удаляем объект со сцены
        Destroy(gameObject);
    }


    private void Update()
    {
        OnAlt();
    }

    //подсвечивание всех предметов на земле при нажатии alt
    void OnAlt()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            DescriptionPanel.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            DescriptionPanel.SetActive(false);
        }
    }
}
