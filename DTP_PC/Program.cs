﻿using CWA.DTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace TestsForLib
{
    public class Program
    {
        public static unsafe ushort crc(byte* data, ulong dataLen)
        {
            byte x;
            ushort crc = 0xFFFF;
            ulong length = dataLen;
            for (ulong i = 0; i <= length; ++i)
            {
                byte b = data[i];
                //if (b == -1)
                //{
                ////status = DTP_ANSWER_STATUS::Error;
                //dataBytesLen = 5;
                //dataBytes = new byte[5] { 1, i & 0xFF, (i >> 8) & 0xFF, (i >> 16) & 0xFF, (i >> 24) & 0xFF };
                //goto Exit;
                //}
                x = (byte)(crc >> 8 ^ b);
                x ^= (byte)(x >> 4);
                crc = (ushort)((crc << 8) ^ ((x << 12)) ^ ((x << 5)) ^ (x));
            }
            return crc;
        }


        static unsafe void Main(string[] args)
        {

            new _Sample_DTP_Info().Start();
            //ushort result;
            //byte[] dataRaw = File.ReadAllBytes("Tools.md5");
            //fixed (byte* data = dataRaw)
            //{
             //   result = crc(data, (ulong)dataRaw.Length);
           // }
            
           // Console.WriteLine(string.Join(",", BitConverter.GetBytes(result)));
            //44 120
            //new _Sample_DTP_WriteToFile().Start();
            //new _Sample_DTP_DateTime().Start();
            Console.ReadKey();
        }
    }

    public abstract class _Sample_DTP_TEST
    {
        protected const string ComName = "COM4";

        protected const int ComSpeed = 115200;

        protected Sender sender = new Sender(SenderType.SevenByteName, "Coestar");

        protected static SerialPort port = new SerialPort(ComName, ComSpeed);

        protected PacketListener listener = new PacketListener(new SerialPacketReader(port, 3000), new SerialPacketWriter(port));

        protected PacketHandler a;

        public abstract void Start();
    }

    public class _Sample_DTP_DataReader : _Sample_DTP_TEST
    {
        public override void Start()
        {
            a = new PacketHandler(sender, listener);

            var openFile = a.DTP_OpenFile("/asd.md5", false);

            if (openFile != PacketHandler.WriteReadFileHandleResult.OK)
            {
                Console.WriteLine(openFile.ToString());
                return;
            }

            int len = 0;

            if(!a.DTP_GetLenOfFile(out len)) Console.WriteLine("cant get length");

            Console.WriteLine(len);

            int packetLen = 2000;

            if (len <= packetLen)
            {
                var readData = a.DTP_GetBytesOfFile(0, len);
                if (readData.Status != PacketHandler.WriteReadFileHandleResult.OK)
                {
                    Console.WriteLine(readData.ToString());
                    return;
                }
                Console.Write(string.Join("", readData.Result.Select(p => (char)p)));
            }
            else
            {
                int i = 0;
                byte[] totalBuff = new byte[len];
                int readedBytes = 0;
                while (true)
                {
                    i++;
                    byte[] buffer = new byte[0];
                    int maxCount = 10;

                    bool succsess = false;

                    while (!succsess)
                    {
                        try
                        {
                            Console.WriteLine(readedBytes);
                           // var readData = a.DTP_GetBytesOfFile(out buffer, readedBytes, packetLen);
                          //  if (readData != PacketHandler.WriteReadFileHandleResult.OK)
                         //   {
                         //       Console.WriteLine(readData.ToString());
                        //        throw new Exception();
                        //    }
                            succsess = true;
                        }
                        catch (WrongPacketInputException e)
                        {
                            Console.Write("Error: ");
                            Console.WriteLine(e.Type);
                            a.Listener.PacketReader.Reset();
                            Console.WriteLine("Tryes: " + maxCount--.ToString());
                            
                        }
                        catch(Exception e)
                        {
                            Console.Write("Unknown error: ");
                            Console.WriteLine(e.Message);
                            a.Listener.PacketReader.Reset();
                            Console.WriteLine("Tryes: " + maxCount--.ToString());

                        }

                        if(maxCount == 0)
                        {
                            Console.WriteLine("ERROR. Out of tries");
                            return;
                        }
                    }
                    //Console.WriteLine(i);
                    Console.WriteLine($"[{(double)readedBytes / len * 100 :0.#}]Packet#{i}/{len / packetLen}. Packet Len: {buffer.Length}.");
                    Buffer.BlockCopy(buffer, 0, totalBuff, readedBytes, buffer.Length);
                    readedBytes += buffer.Length;
                    //buffer = null;
                }
                Console.WriteLine(readedBytes);
                File.WriteAllBytes("dp.zip", totalBuff);


            }

        }
/*
        public bool tryRead()
        {
            try
            {
                var readData = a.DTP_GetBytesOfFile(out buffer, readedBytes, packetLen);
                if (readData != PacketHandler.WriteReadFileHandleResult.OK)
                {
                    Console.WriteLine(readData.ToString());
                    throw new Exception();
                }

            }
            catch
            {

            }
        }*/
    }

    public class _Sample_DTP_DateTime : _Sample_DTP_TEST
    {
        public override void Start()
        {
            a = new PacketHandler(sender, listener);
            //GetDate();

            var openFileRes = a.DTP_OpenFile("Tools.md5", false);

            if(openFileRes != PacketHandler.WriteReadFileHandleResult.OK)
            {
                Console.WriteLine(openFileRes.ToString());
                return;
            }

            var res = a.DTP_GetCRC16OfFile(PacketHandler.HashAlgorithm.CRC32);

            if(res.Status != PacketHandler.WriteReadFileHandleResult.OK)
            {
                Console.WriteLine(res.Status.ToString());
                if (res.Status == PacketHandler.WriteReadFileHandleResult.CantReadData) Console.WriteLine(res.ErrorByteIndex);
                return;
            } else
            {
                Console.WriteLine("Ok");
                Console.WriteLine(string.Join(",",res.Result));
            }
            if (!a.DTP_CloseFile())
            {
                Console.WriteLine("Cant close file");
                return;
            }
        }

        private void GetDate()
        {
            for (int i = 0; i <= 5; i++) 
            {
                DateTime dt;
                if (!a.DTP_GetDateTime(out dt)) Console.WriteLine("Cant get datetime");
                Console.WriteLine(dt.ToString());
                Thread.Sleep(1000);
            }
        
        }
    }

    public class _Sample_DTP_CardInfo : _Sample_DTP_TEST
    {
        public override void Start()
        {
            a = new PacketHandler(sender, listener);
            Console.WriteLine(a.DTP_GetCardInfo().ToString());
        }
    }

    public class _Sample_DTP_FileInfo : _Sample_DTP_TEST
    {
        public override void Start()
        {
            a = new PacketHandler(sender, listener);
            Console.WriteLine(a.DTP_GetFileInfo("Tools.md5").ToString());
        }
    }

    public class _Sample_DTP_WriteToFile : _Sample_DTP_TEST
    {

        private DateTime startTime;

        private long totalBytes;

        private string DirName = "c:\\Tools";

        private FileSender fileSender;

        private int PacketSize = 3200;

        private int _prcounter, _countOfPrData, _lProgress, _oneSProgress;

        private long total;

        private bool _end = false;

        private double Speed, lSpeed;

        private double _left, _lastpr;

        public override void Start()
        {
            startTime = DateTime.Now;

            new Thread(timer).Start();

            Console.WriteLine("Starting: " + startTime);
            a = new PacketHandler(sender, listener);
            fileSender = new FileSender(a);
            fileSender.CheckSum = false;
            fileSender.SendingProcessChanged += FileSender_SendingProcessChanged;
            fileSender.SendingError += FileSender_SendingError;
            fileSender.SendingEnd += FileSender_SendingEnd;
            totalBytes = new FileInfo("diablopatch_20140908.zip").Length;
            fileSender.SendFile("diablopatch_20140908.zip", "dp.zip");

            //SendDir(DirName, "");
            //Console.WriteLine("END!");
            //Console.WriteLine("Bytes sent: " + totalBytes);
            //Console.WriteLine((startTime - DateTime.Now).TotalSeconds);
            
        }

        private void timer()
        {
            while (!_end)
            {
                _oneSProgress = _prcounter - _lProgress;
                _lProgress = _prcounter;
                _countOfPrData += _oneSProgress;
                if(lSpeed == 0) Speed = (double)_oneSProgress * PacketSize / 1024 * 2;
                else Speed = (lSpeed + (double)_oneSProgress * PacketSize / 1024 * 2) / 2;
                lSpeed = Speed;
                if (_lastpr == 0 || _lastpr == float.PositiveInfinity)
                {
                    if (_oneSProgress == 0) _left = float.PositiveInfinity;
                    else _left = (total - _countOfPrData) / _oneSProgress / 2;
                }
                else
                {
                    if (_oneSProgress == 0) _left = float.PositiveInfinity;
                    else _left = (_lastpr + (total - _countOfPrData) / _oneSProgress / 2) / 2;
                }
                _lastpr = _left;
                Thread.Sleep(500);
            }
        }

        public void SendDir(string dirname, string DirPrefix)
        {
            var di = new DirectoryInfo(DirPrefix  + (DirPrefix == ""? "": "\\")  + dirname);
            if (a.DTP_CreateDirectory((DirPrefix + (DirPrefix == "" ? "" : "\\") + di.Name).Replace('\\', '/'), true) != PacketHandler.FileDirHandleResult.OK) ;
            foreach (var c in di.GetFiles())
            {
                Console.Write("[" +DateTime.Now + "]: " +  DirPrefix + (DirPrefix == "" ? "" : "\\") + di.Name + "\\" + c.Name + "||| ");
                SendFile(DirPrefix + (DirPrefix == "" ? "" : "\\") + di.Name + "\\" + c.Name);
            }
            foreach(var c in di.GetDirectories())
            {
                SendDir(c.Name, DirPrefix + "\\" + di.Name);
            }
        }

        public void SendFile(string filename)
        {
            var result = fileSender.SendFile("C:\\" + filename, filename.Replace('\\','/'));
            totalBytes += new FileInfo("C:\\" + filename).Length;
        }

        private void FileSender_SendingEnd(FileSender.EndArgs arg)
        {
            _end = true;
            Console.WriteLine("Done in {0}! Speed {1:0.##} KBytes/s", arg.TimeSpend, totalBytes / arg.TimeSpend / 1024);
        }

        private void FileSender_SendingError(FileSender.ErrorArgs arg)
        {
            Console.Write("ERROR! Code {0}, IsCritical {1}", arg.Error.ToString(), arg.IsCritical);
        }

        private void FileSender_SendingProcessChanged(FileSender.ProcessArgs arg)
        {
            _prcounter = (int)arg.PacketSended;
            total = arg.PacketSended + arg.PacketsLeft;
            Console.WriteLine("[{2:0}%]. Packet#{0}/{1}. Time Left: {3:0.####} sec. Speed: {4:0.####}KBytes", arg.PacketSended, total, (double)arg.PacketSended / total * 100, _left, Speed);
            //Console.Write("+");
        }
    }

    public class _Sample_DTP_DeleteCteateFiles : _Sample_DTP_TEST
    {
        public override void Start()
        {
            a = new PacketHandler(sender, listener);
            //DeleteFiles();
            CreateFiles();
        }

        private string[] FileNames = { "LongName123as.txt", "ds.longExtention", "vc.lon", "sx.das", "Fotos/LongName123as323.d" };

        private void DeleteFiles()
        {
            foreach (var b in FileNames)
            {
                var res = a.DTP_DeleteFile(b);
                if (res == PacketHandler.FileDirHandleResult.OK) Console.WriteLine($"Succsessfully deleted \"{b}\"");
                else if (res == PacketHandler.FileDirHandleResult.Fail) Console.WriteLine($"Сan`t delete \"{b}\"");
                else Console.WriteLine($"File \"{b}\" not exists");
            }
        }

        private void CreateFiles()
        {
            foreach(var b in FileNames)
            {
                var res = a.DTP_CreateFile(b);
                if(res == PacketHandler.FileDirHandleResult.OK) Console.WriteLine($"Succsessfully created \"{b}\"");
                else if(res == PacketHandler.FileDirHandleResult.Fail) Console.WriteLine($"Cant create \"{b}\"");
                else Console.WriteLine($"Cant create \"{b}\", File just exists");
            }
        }
    }

    public class _Sample_DTP_Directories : _Sample_DTP_TEST
    {
        public string[] names = { "dir41", "dir221", "lolname", "wtasd", "dir1/dir2", "dir1/dir23", "dir2/dir22"};

        public override void Start()
        {
            a = new PacketHandler(sender, listener);
            CreateDirs();
        }

        private void CreateDirs()
        {
            foreach(var b in names)
            {
                var status = a.DTP_DeleteDirectory(b, false);

                if(status == 0) Console.WriteLine("\"{0}\" was successfully created", b);
                else if(status == PacketHandler.FileDirHandleResult.Fail) Console.WriteLine("Can`t create directory \"{0}\"", b);
                else if(status == PacketHandler.FileDirHandleResult.FileOrDirJustExist) Console.WriteLine("\"{0}\" already exists", b);

            }
        }
    }

    /*
    public  class _Sample_DTP_FileTree : _Sample_DTP_TEST
    {
        Directory root = new Directory() { Name = "root" };

        public override void Start()
        {
            a = new PacketHandler(sender, listener);
            RecRealTime("/", "", true);

            // Get tree nodes 
            //root = RecNode("/");
            //root.PrintPretty("", true);

        }

        private void RecRealTime(string dirName, string indent, bool last)
        {
            List<string> result_files = new List<string>();
            List<string> result_dirs = new List<string>();
            if (!a.DTP_GetDirectoriesAndFiles(dirName, out result_dirs, out result_files)) Console.WriteLine("Cant get files");
            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "│  ";
            }
            var jj = dirName.Split('/');
            Console.WriteLine('[' + jj.Last() + ']');
            for (int i = 0; i < result_files.Count; i++)
            {
                if (result_dirs.Count == 0 && i == result_files.Count - 1) Console.WriteLine(indent + "└─" + result_files[i]);
                else Console.WriteLine(indent + "├─" + result_files[i]);
            }
            for (int i = 0; i < result_dirs.Count; i++) RecRealTime(dirName + '/' + result_dirs[i], indent, i == result_dirs.Count - 1);
        }

        public class Directory
        {
            public string Name;
            public List<Directory> Dirs;
            public List<string> Files;

            public void PrintPretty(string indent, bool last)
            {
                Console.Write(indent);
                if (last)
                {
                    Console.Write("└─");
                    indent += "  ";
                }
                else
                {
                    Console.Write("├─");
                    indent += "│  ";
                }
                var jj = Name.Split('/');
                Console.WriteLine('[' + jj.Last() + ']');
                for (int i = 0; i < Files.Count; i++)
                {
                    if (Dirs.Count == 0 && i == Files.Count - 1) Console.WriteLine(indent + "└─" + Files[i]);
                    else Console.WriteLine(indent + "├─" + Files[i]);
                }
                for (int i = 0; i < Dirs.Count; i++) Dirs[i].PrintPretty(indent, i == Dirs.Count - 1);
            }
        }

        private Directory RecNode(string dirName)
        {
            Directory result = new Directory();
            List<string> result_files = new List<string>();
            List<string> result_dirs = new List<string>();
            if (!a.DTP_GetDirectoriesAndFiles(dirName, out result_files, out result_dirs)) Console.WriteLine("Cant get files");
            result.Name = dirName == "/" ? "root" : dirName;
            result.Files = new List<string>();
            result.Dirs = new List<Directory>();
            foreach (var a in result_dirs)
            {
                result.Dirs.Add(new Directory());
                result.Dirs[result.Dirs.Count - 1] = RecNode(dirName + '/' + a);
            }
            result.Files.AddRange(result_files);
            return result;
        }
    }
    */
    public class _Sample_DTP_Info : _Sample_DTP_TEST
    {
        public override void Start()
        {
            a = new PacketHandler(sender, listener);

            //Console.WriteLine(a.DTP_GetInfo().ToString());
        }
    }
}
