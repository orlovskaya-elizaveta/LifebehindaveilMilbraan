using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//скрипт будет делать хз что вообще, я запуталась, нафиг он нужен, в нем нет нихрена и одновременно есть всё и все его дергают
//пусть живет, в общем

public class UserData : MonoBehaviour
{
    //ГГДата наследуется от CreatureData (поэтому имеет инвентарь и статы) и добавляет от себя квесты (в скриптах квестов поменяла обращение, чтобы работало)
    //поэтому к инвентарю, квестам и статам ГГ обращаемся через userData.ggData.нужныйАттрибут()
    public GGData ggData;

    private void Awake()
    {

        ggData = new GGData();



        //тестовое
        ggData.stats.Set("Attack", 20);

    }




    




}
