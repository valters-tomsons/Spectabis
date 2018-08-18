using System;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Windows.Forms;
using TheGamesDBAPI;
using System.Net;
using System.IO;
using System.Diagnostics;
using SevenZipExtractor;
using System.Linq;
using System.Collections.Generic;

namespace PCSX2_Spectabis
{
    public partial class addGameForm : MaterialForm
    {

        public Delegate ControlCreator;
        public string ImgPath;
        public string realTitle;
        private readonly MaterialSkinManager materialSkinManager;
        public static string gameloc;
        public static string gameserial;
        public static string filename;
        public List<string> regionList = new List<string>();

        public addGameForm()
        {
            InitializeComponent();

            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            
            //Adds items to region list
            regionList.Add("SLUS");
            regionList.Add("SCUS");
            regionList.Add("SCES");
            regionList.Add("SLES");
            regionList.Add("SCPS");
            regionList.Add("SLPS");
            regionList.Add("SLPM");
            regionList.Add("PSRM");
            regionList.Add("SCED");
            regionList.Add("SLPM");
            regionList.Add("SIPS");
        }

        //Add Game Button
        private void addGameButton_Click(object sender, EventArgs e)
        {
            //Invokes addGame method in mainForm
            string isoPath = gamePath.Text;
            //string gameTitle = gameName.Text;
            //string ImgPath = ImagePath.Text;

            if(autoArt.Checked == true)
            {
                string _databaseurl = "http://thegamesdb.net/";

                //pings thegamesdb, if not reachable - stop
                try
                {
                    WebRequest.Create(_databaseurl).GetResponse();

                    foreach (GameSearchResult game in GamesDB.GetGames(gameName.Text, "Sony Playstation 2"))
                    {
                        //Gets game's database ID
                        Game newGame = GamesDB.GetGame(game.ID);
                        //Trim title
                        realTitle = game.Title.Replace(":", "");
                        realTitle = game.Title.Replace(@"/", "");
                        realTitle = game.Title.Replace(@".", "");
                        //Sets image
                        ImgPath = "http://thegamesdb.net/banners/" + newGame.Images.BoxartFront.Path;
                        Debug.WriteLine(realTitle + " found!");
                        //Stops at the first game
                        break;
                    }
                }
                catch
                {
                    MessageBox.Show("Cannot connect to game database. Please try later!");
                    return;
                }

            }
            else
            {
                //Sets image to set path
                ImgPath = gameName.Text;
                realTitle = titleName.Text;
            }

            //Adds game to mainform list
            if(realTitle == null)
            {
                MessageBox.Show("Game not found, try different name.");
                return;
            }

            Debug.WriteLine(ImgPath);
            Debug.WriteLine(isoPath);
            Debug.WriteLine(realTitle);

            ControlCreator.DynamicInvoke(ImgPath, isoPath, realTitle);

            this.Close();
        }

        //Creates file dialog instance
        OpenFileDialog browseIso = new OpenFileDialog();

        //Browse ISO Button
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {

            //File Filter
            browseIso.Filter = "ISO image (.iso)|*.iso|Media Descriptor File (.mdf)|*.mdf|Image File (.img)|*.img|Compressed ISO (.cso)|*.cso|gzip archive (.gz)|*.gz|BIN file (.bin)|*.bin";

            if (browseIso.ShowDialog() == DialogResult.OK)
            {
                //Sets path into textbox
                gamePath.Text = browseIso.FileName;

                //skips, if unsupported file type file

                if((browseIso.FileName.EndsWith(".cso") == false) && (browseIso.FileName.EndsWith(".bin") == false) && (browseIso.FileName.EndsWith(".gz") == false))
                {

                    //Gets the game serial number from file
                    using (ArchiveFile archiveFile = new ArchiveFile(browseIso.FileName))
                    {
                        foreach (Entry entry in archiveFile.Entries)
                        {
                            //If any file in archive starts with a regional number, save it in variable
                            filename = new string(entry.FileName.Take(4).ToArray());
                            if (regionList.Contains(filename))
                            {
                                gameserial = entry.FileName.Replace(".", String.Empty);
                                gameserial = gameserial.Replace("_", "-");

                                Console.WriteLine("Serial = " + gameserial);
                            }
                        }
                    }

                    //Gets game name from pcsx2 game db
                    using (var reader = new StreamReader(Properties.Settings.Default.EmuDir + @"\GameIndex.dbf"))
                    {
                        bool serialFound = false;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (line.Contains("Serial = " + gameserial))
                            {
                                serialFound = true;
                            }
                            else if (serialFound == true)
                            {
                                Debug.WriteLine(line);
                                gameName.Text = line.Replace("Name   = ",String.Empty);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void customArt_CheckedChanged(object sender, EventArgs e)
        {
            if (autoArt.Checked == true)
            {
                artLabel.Text = "Game Name";
                titleLabel.Visible = false;
                titleName.Visible = false;
            }
            else
            {
                artLabel.Text = "Path to image";
                titleLabel.Visible = true;
                titleName.Visible = true;

            }
        }

    }
}
