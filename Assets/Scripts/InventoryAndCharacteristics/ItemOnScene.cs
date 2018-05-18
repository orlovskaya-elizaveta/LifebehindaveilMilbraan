using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnScene : MonoBehaviour {

    //всплывающая панель описания
    private GameObject DescriptionPanel;
    private Transform TextPanel;

    //ссылка на инвентарь
    private Inventory inv;

    public UserData userData;

    //характеристики объекта
    ItemData itemData;

    private void Start()
    {
        //находим дочернюю панель и делаем ее неактивной
        DescriptionPanel = this.gameObject.transform.Find("DescriptionPanel").gameObject;
        DescriptionPanel.SetActive(false);

        itemData = new ItemData();

        //криворукое рандомное заполнение данных
        int randNumb = Random.Range(0, 4); //это временное
        System.IO.StreamReader file = new System.IO.StreamReader("ItemsData.txt", System.Text.Encoding.GetEncoding(1251));
        string line;
        int i = 0;
        while (((line = file.ReadLine()) != null) && (i != randNumb * 4))
        {
            i++;      
        }
        bool res = int.TryParse(line, out itemData.id);//записываем id
        line = file.ReadLine();
        itemData.name = line;//записываем имя
        line = file.ReadLine();
        itemData.descriptionItem = line;//описание
        line = file.ReadLine();
        itemData.pathIcon = line;//и путь до иконки

        file.Close();

        //найдем всплывающую панель как дочерний объект и поместим на нее текст описания
        TextPanel = DescriptionPanel.transform.GetChild(1);
        TextPanel.GetComponent<UnityEngine.UI.Text>().text = itemData.name + "\n\n" + itemData.descriptionItem;
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
        userData = GameObject.Find("UserData").GetComponent<UserData>();

        userData.ggData.inventory.PutItem(itemData);
        /*
        //находим наш инвентарь
        inv = userData.inventory;

        //добавляем в него данные об объекте TODO обращаться не напрямую к списку
        inv.items.Add(itemData);*/

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
