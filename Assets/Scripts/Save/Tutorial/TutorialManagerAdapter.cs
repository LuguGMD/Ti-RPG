using RPG.Save;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Tutorial
{
    public class TutorialManagerAdapter : SaveAdapter<TutorialManager>
    {
        public override void ClassToData(TutorialManager classSave)
        {
            TutorialManagerData dataSave = SaveManager.SaveData.TutorialManagerData;
            dataSave.TutorialDatas = new TutorialData[classSave.TutorialData.Count];
            int tutorialCount = 0;

            foreach(KeyValuePair<string, bool> tutorialData in classSave.TutorialData)
            {
                TutorialData data = new TutorialData();
                data.TutorialKey = tutorialData.Key;
                data.IsCompleted = tutorialData.Value;
                dataSave.TutorialDatas[tutorialCount] = data;

                tutorialCount++;
            }

        }

        public override void DataToClass(TutorialManager classSave)
        {
            TutorialManagerData dataSave = SaveManager.SaveData.TutorialManagerData;
            TutorialData[] tutorialDatas = dataSave.TutorialDatas;
            foreach (TutorialData tutorialData in tutorialDatas)
            {
                classSave.RegisterTutorial(tutorialData);
            }
        }
    }
}
