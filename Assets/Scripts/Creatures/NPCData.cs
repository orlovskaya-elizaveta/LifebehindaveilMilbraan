using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : CreatureData
{
    public string dialog;
    public int questID;
    public string name;

    public NPCData(string NPCname)
    {
        questID = -1;//нет квеста
        //TODO вынести чтение файла в NPCController
        name = NPCname;
        System.IO.StreamReader file = new System.IO.StreamReader("NPC_QuestID_Dialog.txt", System.Text.Encoding.GetEncoding(1251));
        string line;
        while ((line = file.ReadLine()) != null)
        {
            if (line == name)
            {
                line = file.ReadLine();
                int.TryParse(line, out questID);
                dialog = file.ReadLine();
            }
        }
        file.Close();
    }
}
