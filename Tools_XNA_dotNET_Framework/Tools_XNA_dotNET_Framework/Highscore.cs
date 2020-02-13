using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools_XNA
{
    public class Highscore
    {
        // Variables for the score text | Julius 18-12-09
        private Vector2 scorePosition = new Vector2(3, 60);
        private Vector2 scoreFontSpace = new Vector2(0, 40);


        // Variables for Score, Name and ArrayIdentity | Julius 18-12-09
        private float playerScore;
        private int nameArray = 0;
        private int arrayLeangth = 5;

        // The name of the file | Julius 18-12-09
        public readonly string Filename = "saveFile.dat";

        // Get code | Julius 19-02-12
        public float PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }

        // SaveFile Content | Julius 18-12-09
        [Serializable]
        public struct SaveData
        {
            public int[] Level;
            public string[] Name;
            public float[] Time;
            // Count = Total amount of Arrays | Julius 18-12-09
            public int Count;

            public SaveData(int count)
            {
                Level = new int[count];
                Name = new string[count];
                Time = new float[count];

                Count = count;

            }
        }

        // Function for opening the file and write | Julius 18-12-09
        public void DoSave(SaveData data, string filename)
        {
            // Open the file, if not exist then create a new one | Julius 18-12-09
            FileStream stream = File.Open(filename, FileMode.Create);
            try
            {
                // Convert to XML and try to open the stream | Julius 18-12-09
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close File | Julius 18-12-09
                stream.Close();
            }

        }

        // Function for opening the file and read | Julius 18-12-09
        public SaveData LoadData(string Filename)
        {
            SaveData data;

            // Get the path of the save game | Julius 18-12-09
            string fullpath = Filename;

            // Open the file | Julius 18-12-09
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            try
            {
                // Read the data from the file | Julius 18-12-09
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                data = (SaveData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file | Julius 18-12-09
                stream.Close();
            }

            return (data);

        }

        // Function for easily saving score and sorting it | Julius 18-12-09
        public void SaveHighScore(int level, string name, float time)
        {
            // Create the data to save | Julius 18-12-09
            SaveData data = LoadData(Filename);


            // Look if current score is higher than those on the list | Julius 18-12-09
            int scoreIndex = -1;
            // ForLoop to check every array's value | Julius 18-12-09
            for (int i = 0; i < data.Count; i++)
            {
                if (time < data.Time[i])
                {
                    // Remember which Array | Julius 18-12-09
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                // New highscore found ... put all the other arrays one step lower (making space for the new score) | Julius 18-12-09
                for (int i = data.Count - 1; i > scoreIndex; i--)
                {
                    data.Level[i] = data.Level[i - 1];
                    data.Name[i] = data.Name[i - 1];
                    data.Time[i] = data.Time[i - 1];
                }
                // Save the new score | Julius 18-12-09
                data.Level[scoreIndex] = level;
                data.Name[scoreIndex] = name;
                data.Time[scoreIndex] = time;

                DoSave(data, Filename);
            }

            MenuManager.score = data.Time[0];

        }

        //// Function for resetting the scoreboard | Julius 18-12-09
        //public void ResetHighScore()
        //{
        //    SaveData data = LoadData(Filename);
        //    // With a for-loop | Julius 18-12-09
        //    for (int i = 0; i < arrayLeangth; i++)
        //    {
        //        data.Level[i] = 0;
        //        data.Name[i] = "   ";
        //        data.Time[i] = 10000;
        //    }

        //    // Save the data into the file | Julius 18-12-09
        //    DoSave(data, Filename);

        //}


        public void Initialize()
        {
            // Create a new savefile if one does not exist | Julius 19-02-12
            if (!File.Exists(Filename))
            {
                // Insert data into the file | Julius 19-02-12
                SaveData data = new SaveData(5);
                for (int i = 0; i < arrayLeangth; i++)
                {
                    data.Level[i] = 0;
                    data.Name[i] = "   ";
                    data.Time[i] = 10000;
                }

                // Save the data into the file | Julius 18-12-09
                DoSave(data, Filename);
            }
        }


        public void Draw(SpriteBatch spriteBatch, SpriteFont scoreFont)
        {
            // Draw date and score on highscore list | Julius 18-12-09


            if (LoadData(Filename).Time[0] == 100000)
            {
                spriteBatch.DrawString(scoreFont, "Play to get a score", scorePosition + scoreFontSpace, Color.White);
            }
            else
            {
                // Creates a list for the total amount of arrays | Julius 18-12-09
                for (int i = 0; i < arrayLeangth; i++)
                {
                    // if score is more than 0 blobs, then draw the score on screen | Julius 18-12-09
                    spriteBatch.DrawString(scoreFont, "Level: " + (LoadData(Filename).Level[i]+1), scorePosition + (3 * i + 1) * scoreFontSpace, Color.White);
                    spriteBatch.DrawString(scoreFont, "Time: " + LoadData(Filename).Time[i] + " Seconds", scorePosition + (3 * i) * scoreFontSpace, Color.White);
                }
            }
        }



    }
}
