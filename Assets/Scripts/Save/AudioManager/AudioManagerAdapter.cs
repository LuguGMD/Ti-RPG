using RPG.Audio;
using RPG.Save;
using UnityEngine;

namespace RPG.Save
{
    public class AudioManagerAdapter : SaveAdapter<AudioManager>
    {
        public override void ClassToData(AudioManager classSave)
        {
            AudioManagerData dataSave = SaveManager.SaveData.AudioManagerData;
            dataSave.MasterVolume = classSave.GetVolume(FMODBusEnum.Master);
            dataSave.MusicVolume = classSave.GetVolume(FMODBusEnum.Music);
            dataSave.SoundEffectVolume = classSave.GetVolume(FMODBusEnum.SFX);
        }

        public override void DataToClass(AudioManager classSave)
        {
            AudioManagerData dataSave = SaveManager.SaveData.AudioManagerData;
            float masterVolume = dataSave.MasterVolume;
            float musicVolume = dataSave.MusicVolume;
            float soundEffectVolume = dataSave.SoundEffectVolume;
            classSave.SetVolume(FMODBusEnum.Master, masterVolume);
            classSave.SetVolume(FMODBusEnum.Music, musicVolume);
            classSave.SetVolume(FMODBusEnum.SFX, soundEffectVolume);
        }
    }
}
