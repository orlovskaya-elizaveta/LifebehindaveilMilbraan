using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnInv : MonoBehaviour {

    public ItemData itemData;
    public int countItem; //TODO

    //ссылка на панель для вывода данных о предмете
    private Transform DescriptionField;

    void Start () {

        //находим панель как дочерний объект
        DescriptionField = transform.GetChild(0);
        //и выводим в нее текст
        DescriptionField.GetComponent<UnityEngine.UI.Text>().text = itemData.name + "\n" + itemData.descriptionItem;

        //Артем1101: начало дополнения
        //Смена картинки у предмета в инвентаре
        transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(itemData.pathIcon);
        //Артем1101: конец дополнения
    }

}
