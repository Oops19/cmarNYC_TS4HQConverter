using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime;
using s4pi.Package;
using s4pi.ImageResource;
using s4pi.Interfaces;
using Ookii.Dialogs;
using Xmods.DataLib;

namespace TS4HQConverter
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        static string PackageFilter = "Package files (*.package)|*.package|All files (*.*)|*.*";
        //  static string PNGFilter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
        Size[] outputSizes;
        Size[] outputSizePets;
        int[] outputMipMaps;
        double sharpenStrength = 50;
        byte[] argbValues;
        byte[] alphaValues;

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Ready";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            outputSizes = new Size[] { new Size(2048, 4096), new Size(1024, 2048), new Size(512, 1024) };
            outputSizePets = new Size[] { new Size(4096, 2048), new Size(2048, 1024), new Size(1024, 512) };
            outputMipMaps = new int[] { 13, 12, 11 };
            argbValues = new byte[2048 * 4096 * 4];      //size for max dimensions needed
            alphaValues = new byte[2048 * 4096 * 4];

            for (int i = 0; i < 3; i++)
            {
                string sizeOption = outputSizes[i].Width.ToString() + "x" + outputSizes[i].Height.ToString() +
                    " humanoid / " + outputSizePets[i].Width.ToString() + "x" + outputSizePets[i].Height.ToString() + " pets";
                RLE2dimensions_comboBox.Items.Add(sizeOption);
                RLESdimensions_comboBox.Items.Add(sizeOption);
            }
            HQ_radioButton.Checked = true;
            Sharpen_maskedTextBox.Text = ((int)sharpenStrength).ToString();
        }

        internal string GetFilename(string title, string filter)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = filter;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Title = title;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal Package OpenPackage(string packagePath, bool readwrite)
        {
            try
            {
                Package package = (Package)Package.OpenPackage(0, packagePath, readwrite);
                return package;
            }
            catch
            {
                MessageBox.Show("Unable to read valid package data from " + packagePath);
                return null;
            }
        }

        internal string GetSaveFilename(string title, string filter, string defaultFilename)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = PackageFilter;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Title = title;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "package";
            saveFileDialog1.OverwritePrompt = true;
            if (String.CompareOrdinal(defaultFilename, " ") > 0) saveFileDialog1.FileName = defaultFilename;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        internal bool WritePackage(string filename, Package pack)
        {
            try
            {
                pack.SaveAs(filename);
                pack.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file " + filename + ". Original error: " + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return false;
            }
        }

        //internal bool WriteDDS(DdsFile dds)
        //{
        //    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        //    saveFileDialog1.Filter = "DDS files (*.dds)|*.dds|All files (*.*)|*.*";
        //    saveFileDialog1.FilterIndex = 1;
        //    saveFileDialog1.Title = "Save dds texture";
        //    saveFileDialog1.AddExtension = true;
        //    saveFileDialog1.CheckPathExists = true;
        //    saveFileDialog1.DefaultExt = "dds";
        //    saveFileDialog1.OverwritePrompt = true;
        //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        dds.Save(ms);
        //        using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, System.IO.FileAccess.Write))
        //        {
        //            ms.Position = 0;
        //            ms.CopyTo(fs);
        //            fs.Close();
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        private void PackageSelect_button_Click(object sender, EventArgs e)
        {
            bool sizeHQ = HQ_radioButton.Checked;
            string packpath = GetFilename("Select package to convert", PackageFilter);
            Package pack = null;
            pack = OpenPackage(packpath, false);
            if (pack == null) return;

            toolStripStatusLabel1.Text = "";
            ProcessPackage(pack, Path.GetFileName(packpath), sizeHQ);

            string newpackname = Path.GetFileNameWithoutExtension(packpath) + (sizeHQ ? "HQ" : "NonHQ");
            string filename = GetSaveFilename("Save converted package", PackageFilter, newpackname);
            if (string.Compare(filename, " ") <= 0)
            {
                pack.Dispose();
                toolStripStatusLabel1.Text = "Ready";
                toolStripStatusLabel2.Text = "";
                toolStripStatusLabel3.Text = "";
                return;
            }
            //try
            //{
            //    WritePackage(filename, pack);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: Could not create package " + filename + ". Original error: " + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
            //    return;
            //}

            toolStripStatusLabel1.Text = "Saving";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            statusStrip1.Refresh();
            while (!WritePackage(filename, pack))
            {
                DialogResult res = MessageBox.Show("Try again?", "Could not save package", MessageBoxButtons.RetryCancel);
                if (res == DialogResult.Cancel)
                {
                    break;
                }
            }
            toolStripStatusLabel1.Text = "Ready";
        }

        private void ProcessPackage(Package pack, string packname, bool sizeHQ)
        {
            Predicate<IResourceIndexEntry> predLRLE = r => r.ResourceType == (uint)ResourceTypes.LRLE;
            List<IResourceIndexEntry> iriesl = pack.FindAll(predLRLE);
            Predicate<IResourceIndexEntry> predRLE2 = r => r.ResourceType == (uint)ResourceTypes.RLE2;
            List<IResourceIndexEntry> iries2 = pack.FindAll(predRLE2);
            Predicate<IResourceIndexEntry> predRLES = r => r.ResourceType == (uint)ResourceTypes.RLES;
            List<IResourceIndexEntry> iriess = pack.FindAll(predRLES);
            Predicate<IResourceIndexEntry> predSKIN = r => r.ResourceType == (uint)ResourceTypes.SKIN;
            List<IResourceIndexEntry> iriest = pack.FindAll(predSKIN);
            Predicate<IResourceIndexEntry> predCASP = r => r.ResourceType == (uint)ResourceTypes.CASP;
            List<IResourceIndexEntry> iriesc = pack.FindAll(predCASP);
            int tot = iriesl.Count + iries2.Count + iriess.Count + iriest.Count;

            List<TGI> shadows = new List<TGI>();
            foreach (IResourceIndexEntry irie in iriesc)
            {
                using (Stream s = pack.GetResource(irie))
                {
                    try
                    {
                        CASP casp = new CASP(new BinaryReader(s));
                        if (!shadows.Contains(casp.LinkList[casp.ShadowIndex])) shadows.Add(casp.LinkList[casp.ShadowIndex]);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            int current = 1;
           // int savecount = 0;
            toolStripStatusLabel2.Text = packname + ": ";

            foreach (IResourceIndexEntry irie in iriesl)
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                //   GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                //   GC.Collect(2, GCCollectionMode.Forced, true, true);

                toolStripStatusLabel3.Text = "LRLE Texture " + current.ToString() + " of " + tot.ToString();
                statusStrip1.Refresh();
                current++;
                //savecount++;
                //if (savecount >= 50)
                //{
                //    toolStripStatusLabel3.Text = "Saving...";
                //    statusStrip1.Refresh();
                //    pack.SavePackage();
                //    pack.Dispose();
                //    pack = OpenPackage(newpackname, true);
                //    savecount = 0;
                //    toolStripStatusLabel3.Text = "LRLE Texture " + current.ToString() + " of " + tot.ToString();
                //}
                //else
                //{
                //    toolStripStatusLabel3.Text = "LRLE Texture " + current.ToString() + " of " + tot.ToString();
                //}
                //statusStrip1.Refresh();

                Predicate<IResourceIndexEntry> pred = r => r.ResourceType == irie.ResourceType & r.ResourceGroup == irie.ResourceGroup & r.Instance == irie.Instance;
                IResourceIndexEntry ie = pack.Find(pred);
                using (Stream s = pack.GetResource(ie))
                {
                    if (s.Length < 16) continue;
                    Bitmap image;
                    try
                    {
                        LRLE lrle = new LRLE(new BinaryReader(s));
                        image = lrle.image;
                        lrle = null;
                    }
                    catch
                    {
                        try
                        {
                            RLEResource rle = new RLEResource(1, s);
                            DdsFile dds = new DdsFile();
                            dds.Load(rle.ToDDS(), false);
                            image = new Bitmap(dds.Image);
                            dds.Dispose();
                            rle.Dispose();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (image == null) continue;

                    Size targetRle2Size;
                    if (image.Width < image.Height)
                    {
                        targetRle2Size = outputSizes[RLE2dimensions_comboBox.SelectedIndex];
                    }
                    else if (image.Width > image.Height)
                    {
                        targetRle2Size = outputSizePets[RLE2dimensions_comboBox.SelectedIndex];
                    }
                    else
                    {
                        continue;
                    }
                    if (image.Width == targetRle2Size.Width && image.Height == targetRle2Size.Height) continue;
                    if (sizeHQ && KeepSize_checkBox.Checked && image.Width > targetRle2Size.Width && image.Height > targetRle2Size.Height) continue;

                    Bitmap imageHQ = new Bitmap(targetRle2Size.Width, targetRle2Size.Height);
                    using (var g = Graphics.FromImage(imageHQ))
                    {
                        using (ImageAttributes wrapMode = new ImageAttributes())
                        {
                            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                            g.CompositingMode = CompositingMode.SourceCopy;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.DrawImage(image, new Rectangle(0, 0, targetRle2Size.Width, targetRle2Size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                            g.Save();
                        }
                    }
                    image.Dispose();

                    if (Sharpen_checkBox.Checked && HQ_radioButton.Checked)
                    {
                        Rectangle rect = new Rectangle(0, 0, imageHQ.Width, imageHQ.Height);
                        BitmapData bmpData = imageHQ.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        int rgbWidth = Math.Abs(bmpData.Stride);

                        // Copy values into arrays
                        for (int i = 0; i < bmpData.Height; i++)
                        {
                            System.Runtime.InteropServices.Marshal.Copy(IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), argbValues, i * rgbWidth, rgbWidth);
                        }

                        Sharpen(argbValues, bmpData.Width, bmpData.Height, 4, WhichMatrix.Gaussian3x3, sharpenStrength);      //optional sharpening

                        // Copy back to final bitmap
                        for (int i = 0; i < bmpData.Height; i++)
                        {
                            System.Runtime.InteropServices.Marshal.Copy(argbValues, i * rgbWidth, IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), rgbWidth);
                        }

                        imageHQ.UnlockBits(bmpData);
                    }

                    LRLE lrleHQ = new LRLE(imageHQ);
                    imageHQ.Dispose();
                    pack.ReplaceResource(irie, new Resource(1, lrleHQ.Stream));
                }
            }

            foreach (IResourceIndexEntry irie in iries2)
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                //   GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                //   GC.Collect(2, GCCollectionMode.Forced, true, true);

                toolStripStatusLabel3.Text = "RLE2 Texture " + current.ToString() + " of " + tot.ToString();
                statusStrip1.Refresh();
                current++;
                //savecount++;
                //if (savecount >= 50)
                //{
                //    toolStripStatusLabel3.Text = "Saving...";
                //    statusStrip1.Refresh();
                //    pack.SavePackage();
                //    pack.Dispose();
                //    pack = OpenPackage(newpackname, true);
                //    savecount = 0;
                //    toolStripStatusLabel3.Text = "RLE2 Texture " + current.ToString() + " of " + tot.ToString();
                //}
                //else
                //{
                //    toolStripStatusLabel3.Text = "RLE2 Texture " + current.ToString() + " of " + tot.ToString();
                //}
                //statusStrip1.Refresh();

                Predicate<IResourceIndexEntry> pred = r => r.ResourceType == irie.ResourceType & r.ResourceGroup == irie.ResourceGroup & r.Instance == irie.Instance;
                IResourceIndexEntry ie = pack.Find(pred);
                using (Stream s = pack.GetResource(ie))
                {
                    if (s.Length < 124) continue;
                    RLEResource rle = new RLEResource(1, s);

                    Size targetRle2Size;
                    if (rle.Width < rle.Height)
                    {
                        targetRle2Size = outputSizes[RLE2dimensions_comboBox.SelectedIndex];
                    }
                    else if (rle.Width > rle.Height)
                    {
                        targetRle2Size = outputSizePets[RLE2dimensions_comboBox.SelectedIndex];
                    }
                    else
                    {
                        continue;
                    }
                    if (rle.Width == targetRle2Size.Width && rle.Height == targetRle2Size.Height) continue;
                    if (sizeHQ && KeepSize_checkBox.Checked && rle.Width > targetRle2Size.Width && rle.Height > targetRle2Size.Height) continue;

                    DdsFile dds = new DdsFile();
                    dds.Load(rle.ToDDS(), false);

                    rle.Dispose();
                    DdsFile ddsHQ = new DdsFile();
                    ddsHQ = ResizeDDS(dds, targetRle2Size.Width, targetRle2Size.Height, sizeHQ, argbValues, alphaValues, Sharpen_checkBox.Checked, shadows.Contains(new TGI(irie)), SkipGrayscale_checkBox.Checked, KeepMipMaps_checkBox.Checked);
                    RLEResource resize = new RLEResource(1, null);
                    MemoryStream ms = new MemoryStream();
                    ddsHQ.Save(ms);
                    resize.ImportToRLE(ms, RLEResource.RLEVersion.RLE2);
                    ms.Dispose();
                    dds.Dispose();
                    ddsHQ.Dispose();
                    pack.ReplaceResource(irie, new Resource(1, resize.Stream));
                    resize.Dispose();
                }
            }

            foreach (IResourceIndexEntry irie in iriess)
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                //   GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                //   GC.Collect(2, GCCollectionMode.Forced, true, true);

                toolStripStatusLabel3.Text = "Specular " + current.ToString() + " of " + tot.ToString();
                statusStrip1.Refresh();
                current++;
                //savecount++;
                //if (savecount >= 50)
                //{
                //    toolStripStatusLabel3.Text = "Saving...";
                //    statusStrip1.Refresh();
                //    pack.SavePackage();
                //    pack.Dispose();
                //    pack = OpenPackage(newpackname, true);
                //    savecount = 0;
                //    toolStripStatusLabel3.Text = "Specular " + current.ToString() + " of " + tot.ToString();
                //}
                //else
                //{
                //    toolStripStatusLabel3.Text = "Specular " + current.ToString() + " of " + tot.ToString();
                //}
                //statusStrip1.Refresh();

                Predicate<IResourceIndexEntry> pred = r => r.ResourceType == irie.ResourceType & r.ResourceGroup == irie.ResourceGroup & r.Instance == irie.Instance;
                IResourceIndexEntry ie = pack.Find(pred);
                using (Stream s = pack.GetResource(ie))
                {
                    if (s.Length < 124) continue;
                    RLEResource rles = new RLEResource(1, s);

                    Size targetRlesSize;
                    if (rles.Width < rles.Height)
                    {
                        targetRlesSize = outputSizes[RLESdimensions_comboBox.SelectedIndex];
                    }
                    else if (rles.Width > rles.Height)
                    {
                        targetRlesSize = outputSizePets[RLESdimensions_comboBox.SelectedIndex];
                    }
                    else
                    {
                        continue;
                    }
                    if (rles.Width == targetRlesSize.Width && rles.Height == targetRlesSize.Height) continue;
                    if (sizeHQ && KeepSize_checkBox.Checked && rles.Width > targetRlesSize.Width && rles.Height > targetRlesSize.Height) continue;

                    DdsFile dds = new DdsFile();
                    dds.Load(rles.ToDDS(), false);

                    Bitmap image = dds.GetOpaqueImage();
                    Bitmap imageHQ = new Bitmap(targetRlesSize.Width, targetRlesSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (var g = Graphics.FromImage(imageHQ))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(image, new Rectangle(0, 0, targetRlesSize.Width, targetRlesSize.Height));
                        g.Save();
                    }
                    image.Dispose();
                    Bitmap alpha = dds.GetGreyscaleFromAlpha();
                    Bitmap alphaHQ = new Bitmap(targetRlesSize.Width, targetRlesSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (var g = Graphics.FromImage(alphaHQ))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(alpha, new Rectangle(0, 0, targetRlesSize.Width, targetRlesSize.Height));
                        g.Save();
                    }
                    alpha.Dispose();
                    Bitmap mask = rles.ToSpecularMaskImage();
                    Bitmap imageMaskHQ = new Bitmap(targetRlesSize.Width, targetRlesSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (var g = Graphics.FromImage(imageMaskHQ))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(mask, new Rectangle(0, 0, targetRlesSize.Width, targetRlesSize.Height));
                        g.Save();
                    }
                    mask.Dispose();
                    imageHQ = imageHQ.SetAlphaFromImage(alphaHQ);

                    dds.CreateImage(imageHQ, false);
                    dds.GenerateMipMaps();
                    RLEResource resize = new RLEResource(1, null);
                    MemoryStream ms = new MemoryStream();
                    dds.Save(ms);
                    resize.ImportToRLESwithMask(ms, imageMaskHQ);
                    ms.Dispose();
                    pack.ReplaceResource(irie, new Resource(1, resize.Stream));
                    rles.Dispose();
                    dds.Dispose();
                    imageHQ.Dispose();
                    alphaHQ.Dispose();
                    imageMaskHQ.Dispose();
                    resize.Dispose();
                }
            }

            foreach (IResourceIndexEntry irie in iriest)
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                //  GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                //  GC.Collect(2, GCCollectionMode.Forced, true, true);

                toolStripStatusLabel3.Text = "DDS Texture " + current.ToString() + " of " + tot.ToString();
                statusStrip1.Refresh();
                current++;
                //savecount++;
                //if (savecount >= 50)
                //{
                //    toolStripStatusLabel3.Text = "Saving...";
                //    statusStrip1.Refresh();
                //    pack.SavePackage();
                //    pack.Dispose();
                //    pack = OpenPackage(newpackname, true);
                //    savecount = 0;
                //    toolStripStatusLabel3.Text = "DDS Texture " + current.ToString() + " of " + tot.ToString();
                //}
                //else
                //{
                //    toolStripStatusLabel3.Text = "DDS Texture " + current.ToString() + " of " + tot.ToString();
                //}
                //statusStrip1.Refresh();

                Predicate<IResourceIndexEntry> pred = r => r.ResourceType == irie.ResourceType & r.ResourceGroup == irie.ResourceGroup & r.Instance == irie.Instance;
                IResourceIndexEntry ie = pack.Find(pred);
                using (Stream s = pack.GetResource(ie))
                {
                    if (s.Length < 124) continue;
                    DdsFile dds = new DdsFile();
                    dds.Load(s, false);
                    //   Bitmap image = dds.Image;

                    Size targetRle2Size;
                    if (dds.Width < dds.Height)
                    {
                        targetRle2Size = outputSizes[RLE2dimensions_comboBox.SelectedIndex];
                    }
                    else if (dds.Width > dds.Height)
                    {
                        targetRle2Size = outputSizePets[RLE2dimensions_comboBox.SelectedIndex];
                    }
                    else
                    {
                        continue;
                    }
                    if (dds.Width == targetRle2Size.Width && dds.Height == targetRle2Size.Height) continue;
                    if (sizeHQ && KeepSize_checkBox.Checked && dds.Width > targetRle2Size.Width && dds.Height > targetRle2Size.Height) continue;
                    if (!sizeHQ && dds.Width == 2 * targetRle2Size.Width && dds.Height == 2 * targetRle2Size.Height && dds.MipMaps == outputMipMaps[RLE2dimensions_comboBox.SelectedIndex] + 1)
                    {
                        dds.RemoveMipMap();
                        MemoryStream mst = new MemoryStream();
                        dds.Save(mst);
                        pack.ReplaceResource(irie, new Resource(1, mst));
                        continue;
                    }

                    Bitmap image = dds.GetOpaqueImage();
                    Bitmap imageHQ = new Bitmap(targetRle2Size.Width, targetRle2Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (var g = Graphics.FromImage(imageHQ))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(image, new Rectangle(0, 0, targetRle2Size.Width, targetRle2Size.Height));
                        g.Save();
                    }
                    image.Dispose();
                    Bitmap alpha = dds.GetGreyscaleFromAlpha();
                    Bitmap alphaHQ = new Bitmap(targetRle2Size.Width, targetRle2Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (var g = Graphics.FromImage(alphaHQ))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(alpha, new Rectangle(0, 0, targetRle2Size.Width, targetRle2Size.Height));
                        g.Save();
                    }
                    alpha.Dispose();
                    imageHQ = imageHQ.SetAlphaFromImage(alphaHQ);

                    if (KeepMipMaps_checkBox.Checked && sizeHQ && dds.Width * 2 == targetRle2Size.Width && dds.Height * 2 == targetRle2Size.Height &&
                        dds.MipMaps == outputMipMaps[RLE2dimensions_comboBox.SelectedIndex] - 1)
                    {
                        dds.AddMipMap(imageHQ);
                    }
                    else
                    {
                        dds.CreateImage(imageHQ, false);
                        dds.GenerateMipMaps();
                    }

                    MemoryStream ms = new MemoryStream();
                    dds.Save(ms);
                    pack.ReplaceResource(irie, new Resource(1, ms));
                    dds.Dispose();
                    imageHQ.Dispose();
                }
            }
        }

        private DdsFile ResizeDDS(DdsFile dds, int width, int height, bool sizeHQ, byte[] argbValues, byte[] alphaValues, bool sharpen, bool isShadow, bool skipGrayscale, bool keepMipMaps)
        {
            if (keepMipMaps && !sizeHQ && dds.Width == 2 * width && dds.Height == 2 * height && dds.MipMaps == outputMipMaps[RLE2dimensions_comboBox.SelectedIndex] + 1)
            {
                dds.RemoveMipMap();
                return dds;
            }

            Bitmap image = dds.Image;
            Bitmap alpha = new Bitmap(image);

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Alpha image
            Rectangle rectA = new Rectangle(0, 0, alpha.Width, alpha.Height);
            BitmapData bmpDataA = alpha.LockBits(rectA, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int rgbWidth = Math.Abs(bmpData.Stride);
            int alphaWidth = Math.Abs(bmpDataA.Stride);
            int numBytes = rgbWidth * bmpData.Height;
            int numBytesA = alphaWidth * bmpDataA.Height;
            if (numBytes > argbValues.Length) argbValues = new byte[numBytes];
            if (numBytesA > alphaValues.Length) alphaValues = new byte[numBytesA];

            if (bmpData.Height != bmpDataA.Height)
            {
                MessageBox.Show("RGB and alpha heights don't match!");
                return null;
            }

            // Copy values into arrays
            for (int i = 0; i < bmpData.Height; i++)
            {
                System.Runtime.InteropServices.Marshal.Copy(IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), argbValues, i * rgbWidth, rgbWidth);
                System.Runtime.InteropServices.Marshal.Copy(IntPtr.Add(bmpDataA.Scan0, i * bmpDataA.Stride), alphaValues, i * alphaWidth, alphaWidth);
            }

            bool grayscale = true;

            for (int i = 0; i < numBytes; i += 4)
            {
                // byte arrays are in format BGRA (Blue, Green, Red, Alpha), blank alpha channel of argb and rgb of alpha, getting separate images for rgb and alpha
                if (argbValues[i + 3] == 0)             //set rgb at edges of transparency to prevent lines around alpha areas
                {
                    int[] offsets = new int[] { i - bmpData.Stride >= 0 ? i - bmpData.Stride : -1,
                                                i + bmpData.Stride <= numBytes - 3 ? i + bmpData.Stride : -1,
                                                i - 4 >= 0 ? i - 4 : -1,
                                                i + 4 <= numBytes - 3 ? i + 4 : -1 };
                    foreach (int off in offsets)
                    {
                        if (off >= 0 && argbValues[off + 3] != 0)
                        {
                            argbValues[i] = argbValues[off];
                            argbValues[i + 1] = argbValues[off + 1];
                            argbValues[i + 2] = argbValues[off + 2];
                        }
                    }
                }
                else        //test for grayscale, which are probably shadow textures
                {
                    if (argbValues[i] != argbValues[i + 2] || (argbValues[i + 1] > argbValues[i] + 6 || argbValues[i + 1] < argbValues[i] - 6))
                    {
                        grayscale = false;
                    }
                }
                argbValues[i + 3] = 255;                //resize alpha separately from rgb
                alphaValues[i] = 0;
                alphaValues[i + 1] = 0;
                alphaValues[i + 2] = 0;
            }

            // Copy back to bitmaps
            for (int i = 0; i < bmpData.Height; i++)
            {
                System.Runtime.InteropServices.Marshal.Copy(argbValues, i * rgbWidth, IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), rgbWidth);
                System.Runtime.InteropServices.Marshal.Copy(alphaValues, i * alphaWidth, IntPtr.Add(bmpDataA.Scan0, i * bmpDataA.Stride), alphaWidth);
            }

            image.UnlockBits(bmpData);
            alpha.UnlockBits(bmpDataA);

            Bitmap imageHQ;
            imageHQ = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);     //scale rgb
            using (var g = Graphics.FromImage(imageHQ))
            {
                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(image, new Rectangle(0, 0, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    g.Save();
                }
            }
            image.Dispose();

            Bitmap alphaHQ = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);     //scale alpha
            using (var g = Graphics.FromImage(alphaHQ))
            {
                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    //  g.CompositingMode = CompositingMode.SourceCopy;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //  g.SmoothingMode = SmoothingMode.HighQuality;
                    //  g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(alpha, new Rectangle(0, 0, width, height), 0, 0, alpha.Width, alpha.Height, GraphicsUnit.Pixel, wrapMode);
                    g.Save();
                }
            }
            alpha.Dispose();

            rect = new Rectangle(0, 0, imageHQ.Width, imageHQ.Height);
            bmpData = imageHQ.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Alpha HQ image
            rectA = new Rectangle(0, 0, alphaHQ.Width, alphaHQ.Height);
            bmpDataA = alphaHQ.LockBits(rectA, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            rgbWidth = Math.Abs(bmpData.Stride);
            alphaWidth = Math.Abs(bmpDataA.Stride);
            numBytes = rgbWidth * bmpData.Height;
            numBytesA = alphaWidth * bmpDataA.Height;

            if (numBytes > argbValues.Length) argbValues = new byte[numBytes];
            if (numBytesA > alphaValues.Length) alphaValues = new byte[numBytesA];

            if (bmpData.Height != bmpDataA.Height)
            {
                MessageBox.Show("HQ RGB and alpha heights don't match!");
                return null;
            }

            // Copy values into arrays
            for (int i = 0; i < bmpData.Height; i++)
            {
                System.Runtime.InteropServices.Marshal.Copy(IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), argbValues, i * rgbWidth, rgbWidth);
                System.Runtime.InteropServices.Marshal.Copy(IntPtr.Add(bmpDataA.Scan0, i * bmpDataA.Stride), alphaValues, i * alphaWidth, alphaWidth);
            }

            if (sharpen && !isShadow && (grayscale ? (skipGrayscale ? false : true) : true)) Sharpen(argbValues, bmpData.Width, bmpData.Height, 4, WhichMatrix.Gaussian3x3, sharpenStrength);      //optional sharpening

            for (int i = 0; i < numBytes; i += 4)
            {
                argbValues[i + 3] = alphaValues[i + 3];    //copy resized alpha to rgb
            }

            // Copy back to final bitmap
            for (int i = 0; i < bmpData.Height; i++)
            {
                System.Runtime.InteropServices.Marshal.Copy(argbValues, i * rgbWidth, IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), rgbWidth);
            }
            imageHQ.UnlockBits(bmpData);
            alphaHQ.UnlockBits(bmpDataA);
            alphaHQ.Dispose();

            if (keepMipMaps && sizeHQ && dds.Width * 2 == width && dds.Height * 2 == height && dds.MipMaps == outputMipMaps[RLE2dimensions_comboBox.SelectedIndex] - 1)
            {
                dds.AddMipMap(imageHQ);
            }
            else
            {
                dds.CreateImage(imageHQ, false);
                dds.GenerateMipMaps();
            }
            imageHQ.Dispose();
            return dds;
        }

        private void FolderSelect_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog folder = new VistaFolderBrowserDialog();
            folder.ShowNewFolderButton = false;
            folder.Description = "Select folder containing packages to be converted";
            folder.UseDescriptionForTitle = true;
            folder.ShowDialog();
            FolderName.Text = folder.SelectedPath;
        }

        private void OutputSelect_button_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog folder = new VistaFolderBrowserDialog();
            folder.ShowNewFolderButton = true;
            folder.Description = "Select folder for converted packages";
            folder.UseDescriptionForTitle = true;
            folder.ShowDialog();
            OutputName.Text = folder.SelectedPath;
        }

        private void FolderGo_button_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(FolderName.Text) || !Directory.Exists(OutputName.Text))
            {
                MessageBox.Show("You have not selected valid input and output folders!");
                return;
            }
            string[] paths = Directory.GetFiles(FolderName.Text, "*.package", Subfolders_checkBox.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Array.Sort(paths);
            bool sizeHQ = HQ_radioButton.Checked;
            int counter = 0;
            DialogResult res = DialogResult.Retry;
            bool applyAll = false;
            string errorPacks = "";
            foreach (string packpath in paths)
            {
                counter++;
                toolStripStatusLabel1.Text = "Package " + counter.ToString() + " of " + paths.Length.ToString() + " : ";
                Package pack = null;
                pack = OpenPackage(packpath, false);
                if (pack == null) continue;
                try
                {
                    ProcessPackage(pack, Path.GetFileName(packpath), sizeHQ);
                }
                catch (Exception ep)
                {
                    errorPacks += packpath + " (" + ep.Message + ")" + Environment.NewLine;
                }
                string newpath = packpath.Replace(FolderName.Text, OutputName.Text);
                string newdir = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(newdir)) Directory.CreateDirectory(newdir);
                string newpackname = newdir + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(packpath) +
                    (sizeHQ ? "HQ.package" : "NonHQ.package");
                if (File.Exists(newpackname))
                {
                    if (!applyAll)
                    {
                        using (DupFileDialog dup = new DupFileDialog("A package already exists with the name: " + Environment.NewLine + newpackname))
                        {
                            res = dup.ShowDialog();
                            applyAll = dup.ApplyToAll;
                        }
                    }
                    if (res == DialogResult.OK)
                    {
                        // go on to save package
                    }
                    else if (res == DialogResult.Retry)         //get a non-duplicate file name
                    {
                        int append = 1;
                        newpackname = OutputName.Text + Path.GetFileNameWithoutExtension(packpath) +
                                            (sizeHQ ? "HQ_" : "NonHQ_") + append.ToString() + ".package";
                        while (File.Exists(newpackname))
                        {
                            append++;
                            newpackname = OutputName.Text + Path.GetFileNameWithoutExtension(packpath) +
                                            (sizeHQ ? "HQ_" : "NonHQ_") + append.ToString() + ".package";
                        }
                    }
                    else if (res == DialogResult.Ignore)        //discard new package
                    {
                        continue;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        toolStripStatusLabel1.Text = "Ready";
                        toolStripStatusLabel2.Text = "";
                        toolStripStatusLabel3.Text = "";
                        return;
                    }
                }
                pack.SaveAs(newpackname);
                pack.Dispose();
            }
            if (errorPacks.Length > 0)
            {
                MessageBox.Show("The following package(s) were not successfully converted." + Environment.NewLine + Environment.NewLine +
                    errorPacks + Environment.NewLine + "Please convert them individually to get detailed error messages.");
            }
            toolStripStatusLabel1.Text = "Ready";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
        }

        // Sharpen code from Allek, niaher, David Johnson, Andre Fiedler, and L4a-Thompson on StackOverflow

        /// <summary>
        /// Sharpens the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="whichMatrix">Choice of matrix</param>
        /// <param name="strength">strength 0 - 99</param>
        /// <returns></returns>
        public Bitmap Sharpen(Image image, WhichMatrix whichMatrix, double strength)
        {
            using (var bitmap = image as Bitmap)
            {
                if (bitmap != null)
                {
                    var sharpenImage = bitmap.Clone() as Bitmap;

                    int width = image.Width;
                    int height = image.Height;

                    // Lock image bits for read/write.
                    if (sharpenImage != null)
                    {
                        BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, sharpenImage.PixelFormat);

                        // Declare an array to hold the bytes of the bitmap.
                        int bytes = Math.Abs(pbits.Stride) * pbits.Height;
                        var rgbValues = new byte[bytes];
                        int rowsize = Math.Abs(pbits.Stride);

                        // Copy the RGB values into the array.
                        for (int y = 0; y < pbits.Height; y++)
                        {
                            System.Runtime.InteropServices.Marshal.Copy(IntPtr.Add(pbits.Scan0, y * pbits.Stride), rgbValues, y * rowsize, rowsize);
                        }

                        Sharpen(rgbValues, sharpenImage.Width, sharpenImage.Height, Image.GetPixelFormatSize(sharpenImage.PixelFormat) / 8, whichMatrix, strength);

                        // Copy the RGB values back to the bitmap.
                        for (int y = 0; y < pbits.Height; y++)
                        {
                            System.Runtime.InteropServices.Marshal.Copy(rgbValues, y * rowsize, IntPtr.Add(pbits.Scan0, y * pbits.Stride), rowsize);
                        }
                        // Release image bits.
                        sharpenImage.UnlockBits(pbits);
                        pbits = null;
                    }

                    return sharpenImage;
                }
            }
            return null;
        }

        public void Sharpen(byte[] imageByteArray, int width, int height, int bytesPerPixel, WhichMatrix whichMatrix, double strength)
        {
            double correctionFactor = 0;

            //strenght muß für den jeweiligen filter angepasst werden
            switch (whichMatrix)
            {
                case WhichMatrix.Gaussian3x3:
                    //diese Matrix benötigt einen strenght Wert von 0 bis -9.9 default ist -2.5
                    //und einen korekturwert von 16
                    strength = (strength * -1) / 10;
                    correctionFactor = 16;
                    break;

                case WhichMatrix.Mean3x3:
                    //diese Matrix benötigt einen strenght Wert von 0 bis -9 default ist -2.25
                    //und einen Korrekturwert von 10
                    strength = strength * -9 / 100;
                    correctionFactor = 10;
                    break;

                case WhichMatrix.Gaussian5x5Type1:
                    //diese Matrix benötigt einen strenght Wert von 0 bis 2.5 default ist 1.25
                    //und einen Korrekturwert von 12
                    strength = strength * 2.5 / 100;
                    correctionFactor = 12;
                    break;

                default:
                    break;
            }


            // Create sharpening filter.
            var filter = Matrix(whichMatrix);

            //const int filterSize = 3; // wenn die Matrix 3 Zeilen und 3 Spalten besitzt dann 3 bei 4 = 4 usw.                    
            int filterSize = filter.GetLength(0);

            double bias = 1.0 - strength;
            double factor = strength / correctionFactor;

            //const int s = filterSize / 2;
            int s = filterSize / 2; // Filtersize ist keine Constante mehr darum wurde der befehl const entfernt

            int stride = width * bytesPerPixel;
            // var result = new Color[width, height];
            myColor[,] result = new myColor[width, height];

            int rgb;
            // Fill the color array with the new sharpened color values.
            for (int x = s; x < width - s; x++)
            {
                for (int y = s; y < height - s; y++)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterSize; filterX++)
                    {
                        for (int filterY = 0; filterY < filterSize; filterY++)
                        {
                            int imageX = (x - s + filterX + width) % width;
                            int imageY = (y - s + filterY + height) % height;

                            rgb = (imageY * stride) + (bytesPerPixel * imageX);

                            red += imageByteArray[rgb + 2] * filter[filterX, filterY];
                            green += imageByteArray[rgb + 1] * filter[filterX, filterY];
                            blue += imageByteArray[rgb + 0] * filter[filterX, filterY];
                        }

                        rgb = (y * stride) + (bytesPerPixel * x);

                        int r = Math.Min(Math.Max((int)(factor * red + (bias * imageByteArray[rgb + 2])), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + (bias * imageByteArray[rgb + 1])), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + (bias * imageByteArray[rgb + 0])), 0), 255);

                        // result[x, y] = System.Drawing.Color.FromArgb(r, g, b);
                        result[x, y] = new myColor(r, g, b);
                    }
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = s; x < width - s; x++)
            {
                for (int y = s; y < height - s; y++)
                {
                    rgb = (y * stride) + (bytesPerPixel * x);

                    imageByteArray[rgb + 2] = result[x, y].R;
                    imageByteArray[rgb + 1] = result[x, y].G;
                    imageByteArray[rgb + 0] = result[x, y].B;
                }
            }

            result = null;
        }

        private class myColor
        {
            public byte R;
            public byte G;
            public byte B;
            public myColor(int r, int g, int b)
            {
                R = (byte)r;
                G = (byte)g;
                B = (byte)b;
            }
        }

        public enum WhichMatrix
        {
            Gaussian3x3,
            Mean3x3,
            Gaussian5x5Type1
        }

        private double[,] Matrix(WhichMatrix welcheMatrix)
        {
            double[,] selectedMatrix = null;

            switch (welcheMatrix)
            {
                case WhichMatrix.Gaussian3x3:
                    selectedMatrix = new double[,]
                    {
                    { 1, 2, 1, },
                    { 2, 4, 2, },
                    { 1, 2, 1, },
                    };
                    break;

                case WhichMatrix.Gaussian5x5Type1:
                    selectedMatrix = new double[,]
                    {
                    {-1, -1, -1, -1, -1},
                    {-1,  2,  2,  2, -1},
                    {-1,  2,  16, 2, -1},
                    {-1,  2, -1,  2, -1},
                    {-1, -1, -1, -1, -1}
                    };
                    break;

                case WhichMatrix.Mean3x3:
                    selectedMatrix = new double[,]
                    {
                    { 1, 1, 1, },
                    { 1, 1, 1, },
                    { 1, 1, 1, },
                    };
                    break;
            }

            return selectedMatrix;
        }

        bool IsPowerOfTwo(int i)
        {
            uint x = (uint)i;
            return (x & (x - 1)) == 0;
        }

        internal class Resource : AResource
        {
            internal Resource(int APIversion, Stream s) : base(APIversion, s) { }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            protected override Stream UnParse()
            {
                return this.stream;
            }
        }

        public enum ResourceTypes : uint
        {
            LRLE = 0x2BC04EDFU,
            RLE2 = 0x3453CF95U,
            RLES = 0xBA856C78,
            DDS = 0x00B2D882,
            SKIN = 0xB6C8B6A0,
            CASP = 0x034AEECB
        }

        private void HQ_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            Set_HQ();
        }

        private void NonHQ_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            Set_HQ();
        }

        private void Set_HQ()
        {
            if (HQ_radioButton.Checked)
            {
                RLE2dimensions_comboBox.SelectedIndex = 0;
                RLESdimensions_comboBox.SelectedIndex = 1;
                KeepSize_checkBox.Enabled = true;
                Sharpen_checkBox.Enabled = true;
            }
            else if (NonHQ_radioButton.Checked)
            {
                RLE2dimensions_comboBox.SelectedIndex = 1;
                RLESdimensions_comboBox.SelectedIndex = 2;
                KeepSize_checkBox.Enabled = false;
                Sharpen_checkBox.Enabled = false;
            }
            Sharpen_panel.Enabled = Sharpen_checkBox.Checked && Sharpen_checkBox.Enabled;
        }

        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        private void Sharpen_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            Sharpen_panel.Enabled = Sharpen_checkBox.Checked;
        }

        private void Sharpen_maskedTextBox_TextChanged(object sender, EventArgs e)
        {
            int tmp;
            if (!Int32.TryParse(Sharpen_maskedTextBox.Text, out tmp))
            {
                MessageBox.Show("You have entered an invalid sharpening strength!");
                return;
            }
            sharpenStrength = tmp;
        }

    }
}
