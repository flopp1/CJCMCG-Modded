using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.GZip;
using SharpCompress.Compressors.Xz;
using SkiaSharp;
using CJCMCG;

Console.Title = ("MIDI Counter Generator Modded Version 0.2 by Conjac Jelly Charlieyan and flopp");
MainPart mp = new();
mp.Read();
mp.Render();

namespace CJCMCG
{
    struct Pairli
    {
        public long x;
        public double y;
        public int trk, cnt;
        public Pairli(long a, double b, int c, int d)
        {
            x = a;
            y = b;
            trk = c;
            cnt = d;
        }
        public static bool operator <(Pairli fx, Pairli fy)
        {
            if (fx.x != fy.x)
            {
                return fx.x < fy.x;
            }
            else if (fx.trk != fy.trk)
            {
                return fx.trk > fy.trk;
            }
            else if (fx.cnt != fy.cnt)
            {
                return fx.cnt < fy.cnt;
            }
            else
            {
                return false;
            }
        }
        public static bool operator >(Pairli fx, Pairli fy)
        {
            if (fx.x != fy.x)
            {
                return fx.x > fy.x;
            }
            else if (fx.trk != fy.trk)
            {
                return fx.trk < fy.trk;
            }
            else if (fx.cnt != fy.cnt)
            {
                return fx.cnt > fy.cnt;
            }
            else
            {
                return false;
            }
        }
    }
    struct Pairls
    {
        public long x;
        public String y;
        public int trk, cnt;
        public Pairls(long a, String b, int c, int d)
        {
            x = a;
            y = b;
            trk = c;
            cnt = d;
        }
        public static bool operator <(Pairls fx, Pairls fy)
        {
            if (fx.x != fy.x)
            {
                return fx.x < fy.x;
            }
            else if (fx.trk != fy.trk)
            {
                return fx.trk < fy.trk;
            }
            else if (fx.cnt != fy.cnt)
            {
                return fx.cnt < fy.cnt;
            }
            else
            {
                return false;
            }
        }
        public static bool operator >(Pairls fx, Pairls fy)
        {
            if (fx.x != fy.x)
            {
                return fx.x > fy.x;
            }
            else if (fx.trk != fy.trk)
            {
                return fx.trk > fy.trk;
            }
            else if (fx.cnt != fy.cnt)
            {
                return fx.cnt > fy.cnt;
            }
            else
            {
                return false;
            }
        }
    }
    class MainPart
    {
        public static string filein, fileout;
        int fps;
        string pat;
        long alltic = 0;
        int delay;
        static int resol;
        int mul;
        static int Toint(int x)
        {
            return x < 0 ? x + 256 : x;
        }
        public void Read()
        {
            Console.WriteLine("Note: There should be an ffmpeg.exe in Counter's folder.");
            Console.WriteLine("Note: .zip .xz .gz .7z .tar .rar (or combinations of '.xz's like .xz.xz) files are allowed, but please use Fast Render on them.");
            string sss;
            Console.Write("Input MIDI filename: ");
            filein = Console.ReadLine();
            if (filein[0] == '\"')
            {
                string file1n = "";
                for (int i = 1; i < filein.Length - 1; i++)
                {
                    file1n += filein[i];
                }
                filein = file1n;
            }
            Console.Write("Input video filename (Default: MIDIname+.counter.mov): ");
            fileout = (sss = Console.ReadLine()) == "" ? filein + ".counter.mov" : sss;
            if (fileout[0] == '\"')
            {
                string file1n = "";
                for (int i = 1; i < fileout.Length - 1; i++)
                {
                    file1n += fileout[i];
                }
                fileout = file1n;
            }
            Console.Write("Input video fps (Default: 60): ");
            fps = Convert.ToInt32((sss = Console.ReadLine()) == "" ? "60" : sss);
            Console.Write("Input delay start seconds (Default: 3): ");
            delay = Convert.ToInt32((sss = Console.ReadLine()) == "" ? "3" : sss);
            delay *= fps;
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("If you want to know how to edit patterns, please read README.txt in the Patterns folder.");
            Console.Write("Input pattern ID (Default: 0): ");
            pat = (sss = Console.ReadLine()) == "" ? "0" : sss;
            Console.Write("Input multiplier (Default: 1): ");
            mul = Convert.ToInt32((sss = Console.ReadLine()) == "" ? "1" : sss);
        }

        struct Patterns
        {
            public int W, H;
            public string fon;
            public int fontsz;
            public string patterns;

            public void Readpattern(string id)
            {
                string fol = "Patterns/" + id + ".txt";
                StreamReader inpp = new StreamReader(fol);
                W = Convert.ToInt32(inpp.ReadLine());
                H = Convert.ToInt32(inpp.ReadLine());
                fon = inpp.ReadLine();
                fontsz = Convert.ToInt32(inpp.ReadLine());
                patterns = "";
                while (true)
                {
                    string s = inpp.ReadLine();
                    if (s == null)
                    {
                        break;
                    }
                    patterns += s;
                    patterns += "\n";
                }
            }
            public static string Tocom(long s)
            {
                string S = Convert.ToString(s);
                string SS = "";
                for (int i = S.Length - 1; i >= 0; i--)
                {
                    if ((S.Length - i) % 3 == 0 && i != 0 && S[i - 1] != '-')
                    {
                        SS = "," + S[i] + SS;
                    }
                    else
                    {
                        SS = S[i] + SS;
                    }
                }
                return SS;
            }
            public string Getstring(long nc, long an, double bp, double tm, long np, long po, long ti, long at)
            {
                string ss = patterns;
                ss = ss.Replace("{0}", Convert.ToString(nc));
                ss = ss.Replace("{0,}", Tocom(nc));
                ss = ss.Replace("{1}", Convert.ToString(an));
                ss = ss.Replace("{1,}", Tocom(an));
                ss = ss.Replace("{1-0}", Convert.ToString(an - nc));
                ss = ss.Replace("{1-0,}", Tocom(an - nc));
                ss = ss.Replace("{2}", Convert.ToString(Math.Round(bp * 10) / 10));
                ss = ss.Replace("{3}", Convert.ToInt32(tm * 100) / 100 + "." + (Convert.ToInt32(tm * 100) % 100 / 10) + (Convert.ToInt32(tm * 100) % 10));
                ss = ss.Replace("{4}", Convert.ToString(np));
                ss = ss.Replace("{4,}", Tocom(np));
                ss = ss.Replace("{5}", Convert.ToString(po));
                ss = ss.Replace("{5,}", Tocom(po));
                ss = ss.Replace("{6}", Convert.ToString(ti));
                ss = ss.Replace("{6,}", Tocom(ti));
                ss = ss.Replace("{7}", Convert.ToString(at));
                ss = ss.Replace("{7,}", Tocom(at));
                ss = ss.Replace("{7-6}", Convert.ToString(at - ti));
                ss = ss.Replace("{7-6,}", Tocom(at - ti));
                ss = ss.Replace("{8}", Convert.ToString(ti / resol + 1));
                ss = ss.Replace("{9}", Convert.ToString(at / resol + 1));
                ss = ss.Replace("{9-8}", Convert.ToString(at / resol - ti / resol));
                ss = ss.Replace("{A}", Convert.ToString(resol));
                ss = ss.Replace("{A,}", Tocom(resol));
                return ss;
            }

            public SKImage Draw(long nc, long an, double bp, double tm, long np, long po, long ti, long at, int times)
            {
                nc *= times;
                an *= times;
                np *= times;
                po *= times;
                var font = new SKFont(SKTypeface.FromFamilyName(fon), fontsz, 1, 0);
                var surface = SKSurface.Create(new SKImageInfo(W, H));
                SKCanvas canvas = surface.Canvas;
                canvas.Clear(SKColors.Black);
                var paint = new SKPaint
                {
                    TextSize = 64.0f,
                    IsAntialias = false,
                    Color = new SKColor(255, 255, 255),
                    Style = SKPaintStyle.Fill
                };
                canvas.DrawText(Getstring(nc, an, bp, tm, np, po, ti, at), 0, 0, font, paint);
                return surface.Snapshot();
            }
        }
        Patterns pts;
        
        Process StartNewSSFF(string path)
        {
            Process ffmpeg = new();
            string args = "" +
                    " -f rawvideo -s " + pts.W + "x" + pts.H + " -strict -2 " + " -pix_fmt argb"
                    + " -r " + Convert.ToString(fps) + " -i - " + "-vf vflip -vcodec libx264 -crf 0 ";
            args += " -y \"" + path + "\"";
            Console.WriteLine(args);
            ffmpeg.StartInfo = new ProcessStartInfo("ffmpeg", args)
            {
                RedirectStandardInput = true,
                UseShellExecute = false,
                RedirectStandardError = false
            };
            try
            {
                ffmpeg.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}\nThere was an error starting the ffmpeg process\nIs ffmpeg.exe in the same folder as this program?", ex.Message);
            }
            return ffmpeg;
        }
        static bool CanDec(string s)
        {
            return s.EndsWith(".mid") || s.EndsWith(".xz") || s.EndsWith(".zip") || s.EndsWith(".7z") || s.EndsWith(".rar") || s.EndsWith(".tar") || s.EndsWith(".gz");
        }
        static Stream AddXZLayer(Stream input)
        {
            try
            {
                Process xz = new Process();
                xz.StartInfo = new ProcessStartInfo("xz", "-dc --threads=0")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                };
                xz.Start();
                Task.Run(() =>
                {
                    input.CopyTo(xz.StandardInput.BaseStream);
                    xz.StandardInput.Close();
                });
                return xz.StandardOutput.BaseStream;
            }
            catch (Exception)
            {
                Console.WriteLine("xz.exe not found, trying internal decompress with lower speed and lower compatibility...");
                return new XZStream(input);
            }
        }
        static Stream AddZipLayer(Stream input)
        {
            var zip = new ZipArchive(input, ZipArchiveMode.Read);
            foreach (var entry in zip.Entries)
            {
                if (CanDec(entry.Name))
                {
                    filein = entry.Name;
                    return entry.Open();
                }
            }
            throw new Exception("No compatible file found in the .zip");
        }
        static Stream AddRarLayer(Stream input)
        {
            var zip = RarArchive.Open(input);
            foreach (var entry in zip.Entries)
            {
                if (CanDec(entry.Key))
                {
                    filein = entry.Key;
                    return entry.OpenEntryStream();
                }
            }
            throw new Exception("No compatible file found in the .rar");
        }
        static Stream Add7zLayer(Stream input)
        {
            var zip = SevenZipArchive.Open(input);
            foreach (var entry in zip.Entries)
            {
                if (CanDec(entry.Key))
                {
                    filein = entry.Key;
                    return entry.OpenEntryStream();
                }
            }
            throw new Exception("No compatible file found in the .7z");
        }
        static Stream AddTarLayer(Stream input)
        {
            var zip = TarArchive.Open(input);
            foreach (var entry in zip.Entries)
            {
                if (CanDec(entry.Key))
                {
                    filein = entry.Key;
                    return entry.OpenEntryStream();
                }
            }
            throw new Exception("No compatible file found in the .tar");
        }
        static Stream AddGZLayer(Stream input)
        {
            var zip = GZipArchive.Open(input);
            foreach (var entry in zip.Entries)
            {
                if (CanDec(entry.Key))
                {
                    filein = entry.Key;
                    return entry.OpenEntryStream();
                }
            }
            throw new Exception("No compatible file found in the .gz");
        }

        public void Render()
        {
            Stream inpp = File.Open(filein, FileMode.Open, FileAccess.Read, FileShare.Read);
            while (!filein.EndsWith(".mid"))
            {
                if (filein.EndsWith(".xz"))
                {
                    inpp = AddXZLayer(inpp);
                    filein = filein.Substring(0, filein.Length - 3);
                }
                else if (filein.EndsWith(".zip"))
                {
                    inpp = AddZipLayer(inpp);
                }
                else if (filein.EndsWith(".rar"))
                {
                    inpp = AddRarLayer(inpp);
                }
                else if (filein.EndsWith(".7z"))
                {
                    inpp = Add7zLayer(inpp);
                }
                else if (filein.EndsWith(".tar"))
                {
                    inpp = AddTarLayer(inpp);
                }
                else if (filein.EndsWith(".gz"))
                {
                    inpp = AddGZLayer(inpp);
                }
            }
            int ReadByte()
            {
                int b = inpp.ReadByte();
                if (b == -1) throw new Exception("Unexpected file end");
                return b;
            }
            for (int i = 0; i < 4; ++i)
            {
                ReadByte();
            }
            for (int i = 0; i < 4; ++i)
            {
                ReadByte();
            }
            ReadByte();
            ReadByte();
            int trkcnt;
            trkcnt = (Toint(ReadByte()) * 256) + Toint(ReadByte());
            Console.WriteLine("Track Count: {0}", trkcnt);
            resol = (Toint(ReadByte()) * 256) + Toint(ReadByte());
            ArrayList bpm = new()
            {
                new Pairli(0, 500000 / resol, 99999, 0)
            };
            long noteall = 0;
            int nowtrk = 1;
            ArrayList nts = new(), nto = new();
            for (int trk = 0; trk < trkcnt; trk++)
            {
                int bpmcnt = 0;
                //int lrccnt = 0;
                long notes = 0;
                long leng = 0;
                ReadByte();
                ReadByte();
                ReadByte();
                ReadByte();
                for (int i = 0; i < 4; i++)
                {
                    leng = leng * 256 + Toint(ReadByte());
                }
                int lstcmd = 256;
                Console.WriteLine("Reading track {1}/{2}, Size {3}", nowtrk, trk + 1, trkcnt, leng);
                int getnum()
                {
                    int ans = 0;
                    int ch = 256;
                    while (ch >= 128)
                    {
                        ch = Toint(ReadByte());
                        leng--;
                        ans = ans * 128 + (ch & 0b01111111);
                    }
                    return ans;
                }
                int get()
                {
                    if (lstcmd != 256)
                    {
                        int lstcmd2 = lstcmd;
                        lstcmd = 256;
                        return lstcmd2;
                    }
                    leng--;
                    return Toint(ReadByte());
                }
                int TM = 0;
                int prvcmd = 256;
                while (true)
                {
                    TM += getnum();
                    int cmd = ReadByte();
                    leng--;
                    if (cmd < 128)
                    {
                        lstcmd = cmd;
                        cmd = prvcmd;
                    }
                    prvcmd = cmd;
                    int cm = cmd & 0b11110000;
                    if (cm == 0b10010000)
                    {
                        get();
                        ReadByte();
                        leng--;
                        while (nts.Count <= TM)
                        {
                            nts.Add(0L);
                        }
                        nts[TM] = (Convert.ToInt64(nts[TM]) + 1L);
                        notes++;
                    }
                    else if (cm == 0b10000000)
                    {
                        get();
                        ReadByte();
                        leng--;
                        while (nto.Count <= TM)
                        {
                            nto.Add(0L);
                        }
                        nto[TM] = (Convert.ToInt64(nto[TM]) + 1L);
                    }
                    else if (cm == 0b11000000 || cm == 0b11010000 || cmd == 0b11110011)
                    {
                        get();
                    }
                    else if (cm == 0b11100000 || cm == 0b10110000 || cmd == 0b11110010 || cm == 0b10100000)
                    {
                        get();
                        ReadByte();
                        leng--;
                    }
                    else if (cmd == 0b11110000)
                    {
                        if (get() == 0b11110111)
                        {
                            continue;
                        }
                        do
                        {
                            leng--;
                        } while (ReadByte() != 0b11110111);
                    }
                    else if (cmd == 0b11110100 || cmd == 0b11110001 || cmd == 0b11110101 || cmd == 0b11111001 || cmd == 0b11111101 || cmd == 0b11110110 || cmd == 0b11110111 || cmd == 0b11111000 || cmd == 0b11111010 || cmd == 0b11111100 || cmd == 0b11111110)
                    {
                    }
                    else if (cmd == 0b11111111)
                    {
                        cmd = get();
                        if (cmd == 0)
                        {
                            ReadByte(); ReadByte(); ReadByte();
                            leng -= 3;
                        }
                        else if (cmd >= 1 && cmd <= 10 && cmd != 5 || cmd == 0x7f)
                        {
                            long ff = getnum();
                            while (ff-- > 0)
                            {
                                ReadByte();
                                leng--;
                            }
                        }
                        else if (cmd == 0x20 || cmd == 0x21)
                        {
                            ReadByte(); ReadByte(); leng -= 2;
                        }
                        else if (cmd == 0x2f)
                        {
                            ReadByte();
                            leng--;
                            break;
                        }
                        else if (cmd == 0x51)
                        {
                            bpmcnt++;
                            ReadByte();
                            leng--;
                            int BPM = get();
                            BPM = BPM * 256 + get();
                            BPM = BPM * 256 + get();
                            bpm.Add(new Pairli(TM, 1.0 * BPM / resol, trk, bpmcnt));
                        }
                        else if (cmd == 0x54 || cmd == 0x58)
                        {
                            ReadByte(); ReadByte(); ReadByte(); ReadByte(); ReadByte();
                            leng -= 5;
                        }
                        else if (cmd == 0x59)
                        {
                            ReadByte(); ReadByte(); ReadByte();
                            leng -= 3;
                        }
                        else if (cmd == 0x0a)
                        {
                            int ss = get();
                            while (ss-- > 0)
                            {
                                ReadByte();
                                leng--;
                            }
                        }
                    }
                }
                while (leng > 0)
                {
                    ReadByte();
                    leng--;
                }
                if (TM > alltic)
                {
                    alltic = TM;
                }
                while (nts.Count <= TM)
                {
                    nts.Add(0);
                }
                while (nto.Count <= TM)
                {
                    nto.Add(0);
                }
                noteall += notes;
                nowtrk++;
            }
            Console.WriteLine("Reading finished. Note count: {0}", noteall);
            Console.WriteLine("Sorting tempo events");
            for (int i = 0; i < bpm.Count; i++)
            {
                for (int j = i + 1; j < bpm.Count; j++)
                {
                    if (((Pairli)bpm[i]) > ((Pairli)bpm[j]))
                    {
                        Pairli xx = (Pairli)bpm[i];
                        bpm[j] = bpm[i];
                        bpm[i] = xx;
                    }
                }
            }
            Console.WriteLine("Generating time for bpm events...");
            double[] tmc = new double[bpm.Count];
            tmc[0] = 0;
            for (int i = 1; i < bpm.Count; i++)
            {
                tmc[i] = tmc[i - 1] + (((Pairli)bpm[i]).x - ((Pairli)bpm[i - 1]).x) * ((Pairli)bpm[i - 1]).y;
            }
            Console.WriteLine("Reading pattern...");
            pts.Readpattern(pat);
            Process ffmp = StartNewSSFF(fileout);
            int bpmptr = 0;
            double tmm = 0;
            long notecnt = 0;
            int tmdf = 0;
            byte[] imagedata;
            for (int i = 0; i < delay; i++)
            {
                SKImage img = pts.Draw(0, noteall, 120, 0, 0, 0, 0, alltic, mul);
                MemoryStream ms = new();
                img.Encode(SKEncodedImageFormat.Bmp, 100).SaveTo(ms);
                imagedata = ms.GetBuffer();
                ffmp.StandardInput.BaseStream.Write(imagedata, 54, imagedata.Length - 54);
                tmdf++;
            }
            long[] history = new long[fps];
            long poly = 0;
            int now = 0;
            while (now < nts.Count)
            {
                for (; now <= tmm; now++)
                {
                    if (now >= nts.Count)
                    {
                        break;
                    }
                    notecnt += Convert.ToInt64(nts[now]);
                    poly += Convert.ToInt64(nts[now]);
                    poly -= Convert.ToInt64(nto[now]);
                }
                SKImage img = pts.Draw(notecnt, noteall, 60000000.0 / resol / ((Pairli)bpm[bpmptr]).y, 1.0 * (tmdf - delay) / fps, notecnt - history[tmdf % fps], poly, tmm > alltic ? alltic : Convert.ToInt64(tmm), alltic, mul);
                MemoryStream ms = new();
                img.Encode(SKEncodedImageFormat.Bmp, 100).SaveTo(ms);
                imagedata = ms.GetBuffer();
                ffmp.StandardInput.BaseStream.Write(imagedata, 54, imagedata.Length - 54);
                history[tmdf % fps] = notecnt;
                tmdf++;
                long tmnow = Convert.ToInt64((tmdf - delay) * 1000000.0 / fps);
                while (bpmptr < bpm.Count - 1 && tmc[bpmptr + 1] < Convert.ToDouble(tmnow))
                {
                    bpmptr++;
                }
                tmm = Convert.ToInt32(((Pairli)bpm[bpmptr]).x + (tmnow - tmc[bpmptr]) / ((Pairli)bpm[bpmptr]).y);
            }
            for (int i = 0; i < 5 * fps; i++)
            {
                SKImage img = pts.Draw(noteall, noteall, 60000000.0 / resol / ((Pairli)bpm[bpmptr]).y, 1.0 * (tmdf - delay) / fps, notecnt - history[tmdf % fps], 0, alltic, alltic, mul);
                MemoryStream ms = new();
                img.Encode(SKEncodedImageFormat.Bmp, 100).SaveTo(ms);
                imagedata = ms.GetBuffer();
                ffmp.StandardInput.BaseStream.Write(imagedata, 54, imagedata.Length - 54);
                history[tmdf % fps] = noteall;
                tmdf++;
            }
            ffmp.Close();
            Console.WriteLine("Converting finished. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
