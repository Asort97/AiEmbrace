using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class MemoriesManager
{
    // база воспоминаний 

    // модификаторы важности при выборе воспоминания
    public double recencyK = 1;
    public double importanceK = 1;
    public double contextK = 1;


    // все воспоминания
    public List<Memory> memories;


    // самые актуальные воспоминания на основе контекста,
    // нет лимита на количество воспоминаний, но есть лимит на их общую длину
    // возвращает мультистроку с набором всех воспоминаний
    public async Task<string> GetActualMemories(ChatHisoty context, int currentDate, int tokenLimit)
    {
        // для каждого воспоминания посчитать важность + актуальность + соответствие контексту

        // текстовые внедрения чата
        List<TextEmbeddingsVectorResponse> contextEmbeddings = new List<TextEmbeddingsVectorResponse>();
        for (int k = 0; k < 5 && context.Size() - k - 1 >= 0; k++)
        {
            contextEmbeddings.Add(await ClientAPI.instance.TextEmbeddings(context.GetReply(context.Size() - k - 1).message));
        }

        double[] values = new double[memories.Count];
        int i = 0;
        foreach (var memory in memories)
        {
            // считаем ембединг воспоминания
            TextEmbeddingsVectorResponse memoryEmbeddings = await ClientAPI.instance.TextEmbeddings(memory.description);
            // сравниваем каждый ембединг сообщений и воспоминания и выбираем максимальный
            var maxSimilarity = 0d;
            for (int k = 0; k < contextEmbeddings.Count; k++)
            {
                TextSimilarityResponse similarity = await ClientAPI.instance.TextSimilarity(contextEmbeddings[k].vector, memoryEmbeddings.vector);
                if (similarity.value > maxSimilarity)
                {
                    maxSimilarity = similarity.value;
                }
            }
            
            // высчитыавем полную актуальность для воспоминания
            var imp = (memory.importance / 10d) * importanceK;
            var rec = Math.Pow(0.995d, currentDate - memory.gameDate) * recencyK;
            var con = maxSimilarity * contextK;
            values[i] = imp + rec + con;
            i++;
        }
        // отсортировать по значению и соединять, пока не достигнут лимит
        string result = "";
        int resultSize = 0;
        bool[] flags = new bool[memories.Count];
        while (true)
        {
            // находим самое подходящее воспоминание среди тех что еще не добавили
            int top_i = -1;
            double top_v = -1;
            for (int j = 0; j < memories.Count; j++)
            {
                if (!flags[j] && values[j] > top_v)
                {
                    top_v = values[j];
                    top_i = j;
                }
            }

            // проверки на остановку
            if (top_i == -1)
            {
                break;
            }
            int memoryTokenSize = (await ClientAPI.instance.CountTokens(memories[top_i].Remember(currentDate))).tokens;
            if (resultSize + memoryTokenSize + 2 > tokenLimit)
            {
                break;
            }
            // вставляем самое актуальное воспоминание в результат
            result += memories[top_i].Remember(currentDate);
            // увеличиваем лимит
            resultSize += memoryTokenSize;
            flags[top_i] = true;
        }

        return result;
    }


    public void AddMemory(string _description, int _date, int _importance)
    {
        Memory memory = new Memory { description = _description, gameDate = _date, importance = _importance };
        // todo: calculate token lenght

        // todo: calculate embeddings

        memories.Add(memory);
    }

}
