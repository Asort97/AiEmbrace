using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AICharactersPersonalData
{
    public List<AICharacterPersonalDataPair> aiCharacterDataList = new List<AICharacterPersonalDataPair>();

    public AICharacterPersonalData GetAICharacterData(string name)
    {
        // пытаемся достать персонажа из списка, иначе возвращаем null
        if (aiCharacterDataList.Where(x => x.name == name).FirstOrDefault() == null)
        {
            return null;
        }
        return aiCharacterDataList.Where(x => x.name == name).FirstOrDefault().aICharacterData;
    }

    public void SetAICharacterData(string name, AICharacterPersonalData aICharacterData)
    {
        AICharacterPersonalDataPair pair = aiCharacterDataList.Where(x => x.name == name).FirstOrDefault();
        if (pair != null)
        {
            pair.aICharacterData = aICharacterData;
            Debug.Log("pair.aICharacterData = " + pair.aICharacterData);

        }
        else
        {
            Debug.Log("pair == null");
            aiCharacterDataList.Add(new AICharacterPersonalDataPair() { name = name, aICharacterData = aICharacterData });
        }
    }
}

[Serializable]
public class AICharacterPersonalDataPair
{
    public string name;
    public AICharacterPersonalData aICharacterData;
}
