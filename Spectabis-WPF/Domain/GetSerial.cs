using SevenZip;
using Spectabis_WPF.Domain;
using System;
using System.Linq;

namespace Spectabis_WPF
{
    class GetSerial
    {
        private static string BaseDirectory = App.BaseDirectory;

        public static string GetSerialNumber(string _isoDir)
        {
            string _filename;
            string gameserial = "NULL";

            //Checks, if process is 32-bit or 64
            if (IntPtr.Size == 4)
            {
                //32-bit
                SevenZipBase.SetLibraryPath(BaseDirectory + @"lib\7z-x86.dll");
            }
            else if (IntPtr.Size == 8)
            {
                //64-bit
                SevenZipBase.SetLibraryPath(BaseDirectory + @"lib\7z-x64.dll");
            }



            //Opens the archive
            using (SevenZipExtractor archivedFile = new SevenZipExtractor(_isoDir))
            {
                try
                {
                    //loops throught each file name
                    foreach (var file in archivedFile.ArchiveFileData)
                    {
                        _filename = new string(file.FileName.Take(4).ToArray());
                        //If filename contains region code...
                        if (GetRegions.regionList.Contains(_filename))
                        {
                            //Return forged serial number
                            gameserial = file.FileName.Replace(".", String.Empty);
                            gameserial = gameserial.Replace("_", "-");
                            return gameserial;
                        }
                    }
                }
                catch
                {
                    return gameserial;
                }
                return gameserial;
            }
        }
    }
}
