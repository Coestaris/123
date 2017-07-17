﻿using CWA.DTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace TestsForLib
{
    public class Program
    {
        //public static unsafe ushort crc(byte* data, ulong dataLen)
        //{
        //    byte x;
        //    ushort crc = 0xFFFF;
        //    ulong length = dataLen;
        //    for (ulong i = 0; i <= length; ++i)
        //    {
        //        byte b = data[i];
        //        //if (b == -1)
        //        //{
        //        ////status = DTP_ANSWER_STATUS::Error;
        //        //dataBytesLen = 5;
        //        //dataBytes = new byte[5] { 1, i & 0xFF, (i >> 8) & 0xFF, (i >> 16) & 0xFF, (i >> 24) & 0xFF };
        //        //goto Exit;
        //        //}
        //        x = (byte)(crc >> 8 ^ b);
        //        x ^= (byte)(x >> 4);
        //        crc = (ushort)((crc << 8) ^ ((x << 12)) ^ ((x << 5)) ^ (x));
        //    }
        //    return crc;
        //}

        static void Main(string[] args)
        {
            SerialPort port = new SerialPort("COM6", 115200);
            DTPMaster master = new DTPMaster(
                new SerialPacketReader(port, 5000),
                new SerialPacketWriter(port));

            if (!master.Device.SyncTyme())
                Console.WriteLine("Cant sync time");

            if (!master.Device.Test())
                Console.WriteLine("Cant test device");


            var a = master.CreateDirectoryHandler("/Tools/Scripts/");

            Console.WriteLine(a.IsExists);


            Console.WriteLine("DIR INFO");
            Console.WriteLine(a.DirectoryInfo);
            Console.WriteLine("SUB DIRS");
            Console.WriteLine(string.Join("\n", "    --   " + a.SubDirectroies.Select(p => p.DirectoryPath).ToArray()));
            Console.WriteLine("SUB FILES");
            Console.WriteLine(string.Join("\n", "    --   " + a.SubFiles.Select(p => p.FilePath).ToArray()));


            Console.ReadKey();

            /*

            var a = master.CreateFileHandler("/File.txt").Open(false);
            var bf = a.GetBinnaryFile();

            const int maxLength = 500;

            byte[] buffer = new byte[a.Length];

            var len = a.Length;

            var currIndex = 0;
            var delta = 0;
            var index = 0;
            while(currIndex < len)
            {

                if (currIndex + maxLength > len)
                {
                    delta = len - currIndex;
                    currIndex = len;
                }
                else
                {
                    currIndex += maxLength;
                    delta = maxLength;
                }
                Console.WriteLine("{0} {1}", currIndex, delta);
                var res = bf.ReadByteArray(delta);
                if (!res.Succeed)
                    Console.WriteLine("Cant read Packet!");
                else
                {
                    Buffer.BlockCopy(res.Result, 0, buffer, index, delta);
                    index += delta;
                }
            }

            File.Create("test.txt").Close();
            File.WriteAllBytes("test.txt", buffer);

            a.Close();


            */
            //a.ClearAllBytes();
            //Dictionary<string, object> dict = new Dictionary<string, object>()
            //{
            //    { "IntValue1", 2 },
            //    { "IntValue2", 123123 },
            //    { "IntValue3", -6898 },
            //    { "ByteValue1", (byte)93 },
            //    { "ByteValue2", (byte)6 },
            //    { "ByteValue3", (byte)222 },
            //    { "ShortValue1", (short)3123 },
            //    { "ShortValue2", (short)32 },
            //    { "ShortValue3", (short)8999 },
            //    { "LongValue1", 333333333333L },
            //    { "LongValue2", 2222222222222222222L },
            //    { "LongValue3", 11111111111111111111L },
            //    { "FloatValue1", 23f },
            //    { "FloatValue2", 2323.12312312f },
            //    { "FloatValue3", 111111.22222233f },
            //    { "DoubleValue1", 123123.123123123 },
            //    { "DoubleValue2", 3333333333.333333333 },
            //    { "DoubleValue3", 228228/22+(double)655/23231 },
            //    { "BoolValue1", true },
            //    { "BoolValue2", true }
            //};
            //foreach (var item in dict)
            //{
            //    var type = item.Value.GetType();
            //    if (type == typeof(byte))
            //    {
            //        if (!bf.Write((byte)item.Value))
            //            Console.WriteLine("Cant write {0}", item.Key);
            //        else Console.WriteLine("Succs: {0}", item.Key);
            //    }
            //    else if (type == typeof(short))
            //    {
            //        if (!bf.Write((short)item.Value))
            //            Console.WriteLine("Cant write {0}", item.Key);
            //        else Console.WriteLine("Succs: {0}", item.Key);
            //    }
            //    else if (type == typeof(int))
            //    {
            //        if (!bf.Write((int)item.Value))
            //            Console.WriteLine("Cant write {0}", item.Key);
            //        else Console.WriteLine("Succs: {0}", item.Key);
            //    }
            //    else if (type == typeof(long))
            //    {
            //        if (!bf.Write((long)item.Value))
            //            Console.WriteLine("Cant write {0}", item.Key);
            //        else Console.WriteLine("Succs: {0}", item.Key);
            //    }
            //    else if (type == typeof(float))
            //    {
            //        if (!bf.Write((float)item.Value))
            //            Console.WriteLine("Cant write {0}", item.Key);
            //        else Console.WriteLine("Succs: {0}", item.Key);
            //    }
            //    else if (type == typeof(double))
            //    {
            //        if (!bf.Write((double)item.Value))
            //            Console.WriteLine("Cant write {0}", item.Key);
            //        else Console.WriteLine("Succs: {0}", item.Key);
            //    }
            //    else if (type == typeof(bool))
            //    {
            //        if (!bf.Write((bool)item.Value))
            //            Console.WriteLine("Cant write {0}", item.Key);
            //        else Console.WriteLine("Succs: {0}", item.Key);
            //    }
            //    else Console.WriteLine("Unkown type {0}", type.FullName);
            //}
            //bf.CursorPos = 0;
            //var res1 = bf.ReadByteArray(a.Length);
            //if (res1.Succeed)
            //    Console.WriteLine('{' + string.Join(", ", res1.Result) + '}');
            //bf.CursorPos = 0;
            //foreach (var item in dict)
            //{
            //    var type = item.Value.GetType();
            //    if (type == typeof(byte))
            //    {
            //        var res = bf.Read<byte>();
            //        if (!res.Succeed) Console.WriteLine("Cant read {0}", item.Key);
            //        else Console.WriteLine("{0}. Readed: {1}. Ori: {2}", item.Key, res.Result, item.Value);
            //    }
            //    else if (type == typeof(short))
            //    {
            //        var res = bf.Read<short>();
            //        if (!res.Succeed) Console.WriteLine("Cant read {0}", item.Key);
            //        else Console.WriteLine("{0}. Readed: {1}. Ori: {2}", item.Key, res.Result, item.Value);
            //    }
            //    else if (type == typeof(int))
            //    {
            //        var res = bf.Read<int>();
            //        if (!res.Succeed) Console.WriteLine("Cant read {0}", item.Key);
            //        else Console.WriteLine("{0}. Readed: {1}. Ori: {2}", item.Key, res.Result, item.Value);
            //    }
            //    else if (type == typeof(long))
            //    {
            //        var res = bf.Read<long>();
            //        if (!res.Succeed) Console.WriteLine("Cant read {0}", item.Key);
            //        else Console.WriteLine("{0}. Readed: {1}. Ori: {2}", item.Key, res.Result, item.Value);
            //    }
            //    else if (type == typeof(float))
            //    {
            //        var res = bf.Read<float>();
            //        if (!res.Succeed) Console.WriteLine("Cant read {0}", item.Key);
            //        else Console.WriteLine("{0}. Readed: {1}. Ori: {2}", item.Key, res.Result, item.Value);
            //    }
            //    else if (type == typeof(double))
            //    {
            //        var res = bf.Read<double>();
            //        if (!res.Succeed) Console.WriteLine("Cant read {0}", item.Key);
            //        else Console.WriteLine("{0}. Readed: {1}. Ori: {2}", item.Key, res.Result, item.Value);
            //    }
            //    else if (type == typeof(bool))
            //    {
            //        var res = bf.Read<bool>();
            //        if (!res.Succeed) Console.WriteLine("Cant read {0}", item.Key);
            //        else Console.WriteLine("{0}. Readed: {1}. Ori: {2}", item.Key, res.Result, item.Value);
            //    }
            //    else Console.WriteLine("Unkown type {0}", type.FullName);
            //}
        }
    }

    //public abstract class _Sample_DTP_TEST
    //{
    //    protected const string ComName = "COM5";

    //    protected const int ComSpeed = 115200;

    //    protected Sender sender = new Sender(SenderType.SevenByteName, "Coestar");

    //    protected static SerialPort port = new SerialPort(ComName, ComSpeed);

    //    protected PacketListener listener = new PacketListener(new SerialPacketReader(port, 3000), new SerialPacketWriter(port));

    //    protected PacketHandler a;

    //    public abstract void Start();
    //}

    //public class _Sample : _Sample_DTP_TEST
    //{
    //    public override void Start()
    //    {
    //        a = new PacketHandler(sender, listener);

    //        if(!a.DeviceTest()) Console.WriteLine("Cant test device");

    //        if (a.Device_SyncTime() == 0) Console.WriteLine("Cant sync time");

    //        var f = new SdCardFile("/help.txt", a);

    //        if (!f.IsExists)
    //            f.Create();

    //        f.Open();

    //        var bf = f.GetBinnaryFile();
    //        f.ClearAllBytes();

    //        if (!bf.Write(228)) Console.WriteLine("ERR1");
    //        if (!bf.Write(666666)) Console.WriteLine("ERR2");
    //        if (!bf.Write(999999)) Console.WriteLine("ERR3");
    //        if (!bf.Write(1111111)) Console.WriteLine("ERR4");


    //        bf.CursorPos = 0;
    //        var res1 = bf.Read<int>();
    //        if (res1.Succeed) Console.WriteLine(res1.Result);
    //        else Console.WriteLine("_ERR1");

    //        res1 = bf.Read<int>();
    //        if (res1.Succeed) Console.WriteLine(res1.Result);
    //        else Console.WriteLine("_ERR2");

    //        res1 = bf.Read<int>();
    //        if (res1.Succeed) Console.WriteLine(res1.Result);
    //        else Console.WriteLine("_ERR3");

    //        res1 = bf.Read<int>();
    //        if (res1.Succeed) Console.WriteLine(res1.Result);
    //        else Console.WriteLine("_ERR4");

    //        f.Close();
    //    }
    //}

    //    public class _Sample_DTP_DataReader : _Sample_DTP_TEST
    //    {
    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);

    //            var openFile = a.File_Open("/asd.md5", false);

    //            if (openFile != PacketHandler.WriteReadFileHandleResult.OK)
    //            {
    //                Console.WriteLine(openFile.ToString());
    //                return;
    //            }

    //            int len = 0;

    //            if(!a.File_GetLength(out len)) Console.WriteLine("cant get length");

    //            Console.WriteLine(len);

    //            int packetLen = 2000;

    //            if (len <= packetLen)
    //            {
    //                var readData = a.File_Read(0, len);
    //                if (readData.Status != PacketHandler.WriteReadFileHandleResult.OK)
    //                {
    //                    Console.WriteLine(readData.ToString());
    //                    return;
    //                }
    //                Console.Write(string.Join("", readData.Result.Select(p => (char)p)));
    //            }
    //            else
    //            {
    //                int i = 0;
    //                byte[] totalBuff = new byte[len];
    //                int readedBytes = 0;
    //                while (true)
    //                {
    //                    i++;
    //                    byte[] buffer = new byte[0];
    //                    int maxCount = 10;

    //                    bool succsess = false;

    //                    while (!succsess)
    //                    {
    //                        try
    //                        {
    //                            Console.WriteLine(readedBytes);
    //                           // var readData = a.DTP_GetBytesOfFile(out buffer, readedBytes, packetLen);
    //                          //  if (readData != PacketHandler.WriteReadFileHandleResult.OK)
    //                         //   {
    //                         //       Console.WriteLine(readData.ToString());
    //                        //        throw new Exception();
    //                        //    }
    //                            succsess = true;
    //                        }
    //                        catch (WrongPacketInputException e)
    //                        {
    //                            Console.Write("Error: ");
    //                            Console.WriteLine(e.Type);
    //                            a.Listener.PacketReader.Reset();
    //                            Console.WriteLine("Tryes: " + maxCount--.ToString());

    //                        }
    //                        catch(Exception e)
    //                        {
    //                            Console.Write("Unknown error: ");
    //                            Console.WriteLine(e.Message);
    //                            a.Listener.PacketReader.Reset();
    //                            Console.WriteLine("Tryes: " + maxCount--.ToString());

    //                        }

    //                        if(maxCount == 0)
    //                        {
    //                            Console.WriteLine("ERROR. Out of tries");
    //                            return;
    //                        }
    //                    }
    //                    //Console.WriteLine(i);
    //                    Console.WriteLine($"[{(double)readedBytes / len * 100 :0.#}]Packet#{i}/{len / packetLen}. Packet Len: {buffer.Length}.");
    //                    Buffer.BlockCopy(buffer, 0, totalBuff, readedBytes, buffer.Length);
    //                    readedBytes += buffer.Length;
    //                    //buffer = null;
    //                }
    //                Console.WriteLine(readedBytes);
    //                File.WriteAllBytes("dp.zip", totalBuff);


    //            }

    //        }
    ///*
    //        public bool tryRead()
    //        {
    //            try
    //            {
    //                var readData = a.DTP_GetBytesOfFile(out buffer, readedBytes, packetLen);
    //                if (readData != PacketHandler.WriteReadFileHandleResult.OK)
    //                {
    //                    Console.WriteLine(readData.ToString());
    //                    throw new Exception();
    //                }

    //            }
    //            catch
    //            {

    //            }
    //        }*/
    //    }

    //    public class _Sample_DTP_DateTime : _Sample_DTP_TEST
    //    {
    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);
    //            //GetDate();

    //            var openFileRes = a.File_Open("Tools.md5", false);

    //            if(openFileRes != PacketHandler.WriteReadFileHandleResult.OK)
    //            {
    //                Console.WriteLine(openFileRes.ToString());
    //                return;
    //            }

    //            var res = a.File_GetCrC16(PacketHandler.HashAlgorithm.CRC32);

    //            if(res.Status != PacketHandler.WriteReadFileHandleResult.OK)
    //            {
    //                Console.WriteLine(res.Status.ToString());
    //                if (res.Status == PacketHandler.WriteReadFileHandleResult.CantReadData) Console.WriteLine(res.ErrorByteIndex);
    //                return;
    //            } else
    //            {
    //                Console.WriteLine("Ok");
    //                Console.WriteLine(string.Join(",",res.Result));
    //            }
    //            if (!a.File_Close())
    //            {
    //                Console.WriteLine("Cant close file");
    //                return;
    //            }
    //        }

    //        private void GetDate()
    //        {
    //            for (int i = 0; i <= 5; i++) 
    //            {
    //                DateTime dt;
    //                if (!a.Device_GetTime(out dt)) Console.WriteLine("Cant get datetime");
    //                Console.WriteLine(dt.ToString());
    //                Thread.Sleep(1000);
    //            }

    //        }
    //    }

    //    public class _Sample_DTP_CardInfo : _Sample_DTP_TEST
    //    {
    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);
    //            Console.WriteLine(a.Device_GetCardInfo().ToString());
    //        }
    //    }

    //    public class _Sample_DTP_FileInfo : _Sample_DTP_TEST
    //    {
    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);
    //            Console.WriteLine(a.File_GetInfo("Tools.md5").ToString());
    //        }
    //    }

    //    //public class _Sample_DTP_WriteToFile : _Sample_DTP_TEST
    //    //{

    //    //    private DateTime startTime;

    //    //    private long totalBytes;

    //    //    private string DirName = "c:\\Tools";

    //    //    private FileSender fileSender;

    //    //    private int PacketSize = 3200;

    //    //    private int _prcounter, _countOfPrData, _lProgress, _oneSProgress;

    //    //    private long total;

    //    //    private bool _end = false;

    //    //    private double Speed, lSpeed;

    //    //    private double _left, _lastpr;

    //    //    public override void Start()
    //    //    {
    //    //        startTime = DateTime.Now;

    //    //        new Thread(timer).Start();

    //    //        Console.WriteLine("Starting: " + startTime);
    //    //        a = new PacketHandler(sender, listener);
    //    //        fileSender = new FileSender(a);
    //    //        fileSender.CheckSum = false;
    //    //        fileSender.SendingProcessChanged += FileSender_SendingProcessChanged;
    //    //        fileSender.SendingError += FileSender_SendingError;
    //    //        fileSender.SendingEnd += FileSender_SendingEnd;
    //    //        totalBytes = new FileInfo("diablopatch_20140908.zip").Length;
    //    //        fileSender.SendFile("diablopatch_20140908.zip", "dp.zip");

    //    //        //SendDir(DirName, "");
    //    //        //Console.WriteLine("END!");
    //    //        //Console.WriteLine("Bytes sent: " + totalBytes);
    //    //        //Console.WriteLine((startTime - DateTime.Now).TotalSeconds);

    //    //    }

    //    //    private void timer()
    //    //    {
    //    //        while (!_end)
    //    //        {
    //    //            _oneSProgress = _prcounter - _lProgress;
    //    //            _lProgress = _prcounter;
    //    //            _countOfPrData += _oneSProgress;
    //    //            if(lSpeed == 0) Speed = (double)_oneSProgress * PacketSize / 1024 * 2;
    //    //            else Speed = (lSpeed + (double)_oneSProgress * PacketSize / 1024 * 2) / 2;
    //    //            lSpeed = Speed;
    //    //            if (_lastpr == 0 || _lastpr == float.PositiveInfinity)
    //    //            {
    //    //                if (_oneSProgress == 0) _left = float.PositiveInfinity;
    //    //                else _left = (total - _countOfPrData) / _oneSProgress / 2;
    //    //            }
    //    //            else
    //    //            {
    //    //                if (_oneSProgress == 0) _left = float.PositiveInfinity;
    //    //                else _left = (_lastpr + (total - _countOfPrData) / _oneSProgress / 2) / 2;
    //    //            }
    //    //            _lastpr = _left;
    //    //            Thread.Sleep(500);
    //    //        }
    //    //    }

    //    //    public void SendDir(string dirname, string DirPrefix)
    //    //    {
    //    //        var di = new DirectoryInfo(DirPrefix  + (DirPrefix == ""? "": "\\")  + dirname);
    //    //        if (a.DTP_CreateDirectory((DirPrefix + (DirPrefix == "" ? "" : "\\") + di.Name).Replace('\\', '/'), true) != PacketHandler.FileDirHandleResult.OK) ;
    //    //        foreach (var c in di.GetFiles())
    //    //        {
    //    //            Console.Write("[" +DateTime.Now + "]: " +  DirPrefix + (DirPrefix == "" ? "" : "\\") + di.Name + "\\" + c.Name + "||| ");
    //    //            SendFile(DirPrefix + (DirPrefix == "" ? "" : "\\") + di.Name + "\\" + c.Name);
    //    //        }
    //    //        foreach(var c in di.GetDirectories())
    //    //        {
    //    //            SendDir(c.Name, DirPrefix + "\\" + di.Name);
    //    //        }
    //    //    }

    //    //    public void SendFile(string filename)
    //    //    {
    //    //        var result = fileSender.SendFile("C:\\" + filename, filename.Replace('\\','/'));
    //    //        totalBytes += new FileInfo("C:\\" + filename).Length;
    //    //    }

    //    //    private void FileSender_SendingEnd(FileSender.EndArgs arg)
    //    //    {
    //    //        _end = true;
    //    //        Console.WriteLine("Done in {0}! Speed {1:0.##} KBytes/s", arg.TimeSpend, totalBytes / arg.TimeSpend / 1024);
    //    //    }

    //    //    private void FileSender_SendingError(FileSender.ErrorArgs arg)
    //    //    {
    //    //        Console.Write("ERROR! Code {0}, IsCritical {1}", arg.Error.ToString(), arg.IsCritical);
    //    //    }

    //    //    private void FileSender_SendingProcessChanged(FileSender.ProcessArgs arg)
    //    //    {
    //    //        _prcounter = (int)arg.PacketSended;
    //    //        total = arg.PacketSended + arg.PacketsLeft;
    //    //        Console.WriteLine("[{2:0}%]. Packet#{0}/{1}. Time Left: {3:0.####} sec. Speed: {4:0.####}KBytes", arg.PacketSended, total, (double)arg.PacketSended / total * 100, _left, Speed);
    //    //        //Console.Write("+");
    //    //    }
    //    //}

    //    public class _Sample_DTP_DeleteCteateFiles : _Sample_DTP_TEST
    //    {
    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);
    //            //DeleteFiles();
    //            CreateFiles();
    //        }

    //        private string[] FileNames = { "LongName123as.txt", "ds.longExtention", "vc.lon", "sx.das", "Fotos/LongName123as323.d" };

    //        private void DeleteFiles()
    //        {
    //            foreach (var b in FileNames)
    //            {
    //                var res = a.File_Delete(b);
    //                if (res == PacketHandler.FileDirHandleResult.OK) Console.WriteLine($"Succsessfully deleted \"{b}\"");
    //                else if (res == PacketHandler.FileDirHandleResult.Fail) Console.WriteLine($"Сan`t delete \"{b}\"");
    //                else Console.WriteLine($"File \"{b}\" not exists");
    //            }
    //        }

    //        private void CreateFiles()
    //        {
    //            foreach(var b in FileNames)
    //            {
    //                var res = a.File_Create(b);
    //                if(res == PacketHandler.FileDirHandleResult.OK) Console.WriteLine($"Succsessfully created \"{b}\"");
    //                else if(res == PacketHandler.FileDirHandleResult.Fail) Console.WriteLine($"Cant create \"{b}\"");
    //                else Console.WriteLine($"Cant create \"{b}\", File just exists");
    //            }
    //        }
    //    }

    //    public class _Sample_DTP_Directories : _Sample_DTP_TEST
    //    {
    //        public string[] names = { "dir41", "dir221", "lolname", "wtasd", "dir1/dir2", "dir1/dir23", "dir2/dir22"};

    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);
    //            CreateDirs();
    //        }

    //        private void CreateDirs()
    //        {
    //            foreach(var b in names)
    //            {
    //                var status = a.Dir_Delete(b, false);

    //                if(status == 0) Console.WriteLine("\"{0}\" was successfully created", b);
    //                else if(status == PacketHandler.FileDirHandleResult.Fail) Console.WriteLine("Can`t create directory \"{0}\"", b);
    //                else if(status == PacketHandler.FileDirHandleResult.FileOrDirJustExist) Console.WriteLine("\"{0}\" already exists", b);

    //            }
    //        }
    //    }

    //    /*
    //    public  class _Sample_DTP_FileTree : _Sample_DTP_TEST
    //    {
    //        Directory root = new Directory() { Name = "root" };

    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);
    //            RecRealTime("/", "", true);

    //            // Get tree nodes 
    //            //root = RecNode("/");
    //            //root.PrintPretty("", true);

    //        }

    //        private void RecRealTime(string dirName, string indent, bool last)
    //        {
    //            List<string> result_files = new List<string>();
    //            List<string> result_dirs = new List<string>();
    //            if (!a.DTP_GetDirectoriesAndFiles(dirName, out result_dirs, out result_files)) Console.WriteLine("Cant get files");
    //            Console.Write(indent);
    //            if (last)
    //            {
    //                Console.Write("└─");
    //                indent += "  ";
    //            }
    //            else
    //            {
    //                Console.Write("├─");
    //                indent += "│  ";
    //            }
    //            var jj = dirName.Split('/');
    //            Console.WriteLine('[' + jj.Last() + ']');
    //            for (int i = 0; i < result_files.Count; i++)
    //            {
    //                if (result_dirs.Count == 0 && i == result_files.Count - 1) Console.WriteLine(indent + "└─" + result_files[i]);
    //                else Console.WriteLine(indent + "├─" + result_files[i]);
    //            }
    //            for (int i = 0; i < result_dirs.Count; i++) RecRealTime(dirName + '/' + result_dirs[i], indent, i == result_dirs.Count - 1);
    //        }

    //        public class Directory
    //        {
    //            public string Name;
    //            public List<Directory> Dirs;
    //            public List<string> Files;

    //            public void PrintPretty(string indent, bool last)
    //            {
    //                Console.Write(indent);
    //                if (last)
    //                {
    //                    Console.Write("└─");
    //                    indent += "  ";
    //                }
    //                else
    //                {
    //                    Console.Write("├─");
    //                    indent += "│  ";
    //                }
    //                var jj = Name.Split('/');
    //                Console.WriteLine('[' + jj.Last() + ']');
    //                for (int i = 0; i < Files.Count; i++)
    //                {
    //                    if (Dirs.Count == 0 && i == Files.Count - 1) Console.WriteLine(indent + "└─" + Files[i]);
    //                    else Console.WriteLine(indent + "├─" + Files[i]);
    //                }
    //                for (int i = 0; i < Dirs.Count; i++) Dirs[i].PrintPretty(indent, i == Dirs.Count - 1);
    //            }
    //        }

    //        private Directory RecNode(string dirName)
    //        {
    //            Directory result = new Directory();
    //            List<string> result_files = new List<string>();
    //            List<string> result_dirs = new List<string>();
    //            if (!a.DTP_GetDirectoriesAndFiles(dirName, out result_files, out result_dirs)) Console.WriteLine("Cant get files");
    //            result.Name = dirName == "/" ? "root" : dirName;
    //            result.Files = new List<string>();
    //            result.Dirs = new List<Directory>();
    //            foreach (var a in result_dirs)
    //            {
    //                result.Dirs.Add(new Directory());
    //                result.Dirs[result.Dirs.Count - 1] = RecNode(dirName + '/' + a);
    //            }
    //            result.Files.AddRange(result_files);
    //            return result;
    //        }
    //    }
    //    */

    //    public class _Sample_DTP_Info : _Sample_DTP_TEST
    //    {
    //        public override void Start()
    //        {
    //            a = new PacketHandler(sender, listener);

    //            //Console.WriteLine(a.DTP_GetInfo().ToString());
    //        }
    //    }
}
