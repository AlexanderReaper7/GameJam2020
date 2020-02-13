using System;
using System.Collections.Generic;
using System.Linq;
// System.IO and Xml serialization needed for saving highscores //Dennis 20-02-12
using System.Xml.Serialization;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    // Alexander 200212
    public class Highscore
    {
        public SaveData currentSaveData;
        public const int MaxScores = 6;
        readonly string FilePath = "Save.dat";

        [Serializable]
        public struct SaveData
        {
            public DataEntry[] entries;

            public SaveData(DataEntry[] entries)
            {
                this.entries = entries;
            }

            public static SaveData GenerateEmpty()
            {
                DataEntry[] arr = new DataEntry[MaxScores];
                for (int i = 0; i < MaxScores; i++)
                {
                    arr[i] = new DataEntry("000", 0);
                }
                return new SaveData(arr);
            }
            
        }

        [Serializable]
        public struct DataEntry
        {
            public readonly string name;
            public readonly int score;

            public DataEntry(string name, int score)
            {
                this.name = name;
                this.score = score;
            }
        }

        public Highscore()
        {
            // if file not exist, new
            if (!File.Exists(FilePath))
            {
                SaveData data = SaveData.GenerateEmpty();

                DoSave(data, FilePath);
                currentSaveData = data;
            }
            // if file exist, read
            else
            {
                currentSaveData = LoadData(FilePath);
            }
        }

        public static SaveData LoadData(string Filename)
        {
            SaveData data;
            string fullpath = Filename;
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                data = (SaveData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
            return data;
        }

        public static void DoSave(SaveData data, string Filename)
        {
            // Sort scores
            IOrderedEnumerable<DataEntry> scores = from entry in data.entries orderby entry.score descending select entry;
            data = new SaveData(scores.ToArray());
            // Save file
            FileStream stream = File.Open(Filename, FileMode.Create);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                stream.Close();
            }
        }
        

        public void Draw(SpriteBatch spriteBatch, SpriteFont scoreFont, Vector2 scoreBoardPosition, Vector2 scoreFontSpacing, Color fontColor)
        {
            spriteBatch.DrawString(scoreFont, "HIGHSCORES:", scoreBoardPosition, fontColor);

            //if (currentSaveData.entries[0].score == 0)
            //{
            //    spriteBatch.DrawString(scoreFont, "No score entries", scoreBoardPosition + scoreFontSpacing, fontColor);
            //}
            //else
            {
                for (int i = 0; i < MaxScores; i++)
                    //if (currentSaveData.entries[0].score > 0)
                    {
                        spriteBatch.DrawString(scoreFont, 
                            i + 1 + ": " + currentSaveData.entries[i].name + " timed at " + currentSaveData.entries[i].score + " seconds", 
                            scoreBoardPosition + (i+2) * scoreFontSpacing, fontColor);
                    }
            }
        }


    }

}
