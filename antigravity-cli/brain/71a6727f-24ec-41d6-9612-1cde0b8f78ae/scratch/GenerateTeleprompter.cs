using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace TeleprompterGenerator
{
    [ComImport, Guid("2cd2d921-c447-44a7-a13c-4adabfc247e3"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMFAttributes
    {
        void GetItem();
        void GetItemType();
        void CompareItem();
        void Compare();
        void GetUINT32();
        void GetUINT64();
        void GetDouble();
        void GetGUID();
        void GetStringLength();
        void GetString();
        void GetAllocatedString();
        void GetBlobSize();
        void GetBlob();
        void GetAllocatedBlob();
        void GetUnknown();
        void SetItem();
        void DeleteItem();
        void DeleteAllItems();
        void SetUINT32([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, uint unValue);
        void SetUINT64([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, ulong unValue);
        void SetDouble([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, double fValue);
        void SetGUID([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidValue);
        void SetString();
        void SetBlob();
        void SetUnknown();
        void LockStore();
        void UnlockStore();
        void GetCount();
        void GetItemByIndex();
        void CopyAllItems();
    }

    [ComImport, Guid("045FA593-8799-42b8-BC8D-8968C6453507"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMFMediaBuffer
    {
        void Lock(out IntPtr ppbBuffer, out int pcbMaxLength, out int pcbCurrentLength);
        void Unlock();
        void GetCurrentLength(out int pcbCurrentLength);
        void SetCurrentLength(int cbCurrentLength);
        void GetMaxLength(out int pcbMaxLength);
    }

    [ComImport, Guid("c40a00f2-b93a-4d80-ae8c-5a1c634f58e4"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMFSample
    {
        // IMFAttributes methods
        void GetItem();
        void GetItemType();
        void CompareItem();
        void Compare();
        void GetUINT32();
        void GetUINT64();
        void GetDouble();
        void GetGUID();
        void GetStringLength();
        void GetString();
        void GetAllocatedString();
        void GetBlobSize();
        void GetBlob();
        void GetAllocatedBlob();
        void GetUnknown();
        void SetItem();
        void DeleteItem();
        void DeleteAllItems();
        void SetUINT32([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, uint unValue);
        void SetUINT64([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, ulong unValue);
        void SetDouble([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, double fValue);
        void SetGUID([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidKey, [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidValue);
        void SetString();
        void SetBlob();
        void SetUnknown();
        void LockStore();
        void UnlockStore();
        void GetCount();
        void GetItemByIndex();
        void CopyAllItems();

        // IMFSample methods
        void GetSampleFlags();
        void SetSampleFlags(uint dwSampleFlags);
        void GetSampleTime(out long phnsSampleTime);
        void SetSampleTime(long hnsSampleTime);
        void GetSampleDuration(out long phnsSampleDuration);
        void SetSampleDuration(long hnsSampleDuration);
        void GetBufferCount();
        void GetBufferByIndex();
        void ConvertToContiguousBuffer();
        void AddBuffer([MarshalAs(UnmanagedType.Interface)] IMFMediaBuffer pBuffer);
        void RemoveBufferByIndex();
        void RemoveAllBuffers();
        void GetTotalLength();
        void CopyToBuffer();
    }

    [ComImport, Guid("3137f1cd-fe5e-4805-a5d8-fb477448cb3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMFSinkWriter
    {
        void AddStream([MarshalAs(UnmanagedType.Interface)] IMFAttributes pTargetMediaType, out int pdwStreamIndex);
        void SetInputMediaType(int dwStreamIndex, [MarshalAs(UnmanagedType.Interface)] IMFAttributes pInputMediaType, [MarshalAs(UnmanagedType.Interface)] IMFAttributes pEncodingParameters);
        void BeginWriting();
        void WriteSample(int dwStreamIndex, [MarshalAs(UnmanagedType.Interface)] IMFSample pSample);
        void SendStreamTick(int dwStreamIndex, long llTimestamp);
        void PlaceMarker(int dwStreamIndex, IntPtr pvContext);
        void NotifyEndOfSegment(int dwStreamIndex);
        void Flush(int dwStreamIndex);
        void Finalize_();
        void GetServiceForStream();
        void GetStatistics();
    }

    class Program
    {
        [DllImport("mfplat.dll", ExactSpelling = true, PreserveSig = false)]
        static extern void MFStartup(uint Version, uint dwFlags);

        [DllImport("mfplat.dll", ExactSpelling = true, PreserveSig = false)]
        static extern void MFShutdown();

        [DllImport("mfplat.dll", ExactSpelling = true, PreserveSig = false)]
        static extern void MFCreateMediaType([MarshalAs(UnmanagedType.Interface)] out IMFAttributes ppMFType);

        [DllImport("mfplat.dll", ExactSpelling = true, PreserveSig = false)]
        static extern void MFCreateMemoryBuffer(int cbMaxLength, [MarshalAs(UnmanagedType.Interface)] out IMFMediaBuffer ppBuffer);

        [DllImport("mfplat.dll", ExactSpelling = true, PreserveSig = false)]
        static extern void MFCreateSample([MarshalAs(UnmanagedType.Interface)] out IMFSample ppIMFSample);

        [DllImport("mfreadwrite.dll", ExactSpelling = true, PreserveSig = false)]
        static extern void MFCreateSinkWriterFromURL(
            [MarshalAs(UnmanagedType.LPWStr)] string pwszOutputURL,
            IntPtr pByteStream,
            [MarshalAs(UnmanagedType.Interface)] IMFAttributes pAttributes,
            [MarshalAs(UnmanagedType.Interface)] out IMFSinkWriter ppSinkWriter);

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr count);

        static readonly Guid MF_MT_MAJOR_TYPE = new Guid("48eba18e-f8c9-4687-bf11-0a74c9f96a8f");
        static readonly Guid MF_MT_SUBTYPE = new Guid("f7e34c9a-42e8-4714-b74b-cb29d72c35e5");
        static readonly Guid MF_MT_AVG_BITRATE = new Guid("20332624-fb0d-4d9e-bd0d-cbf6786c102e");
        static readonly Guid MF_MT_INTERLACE_MODE = new Guid("e2724bb8-e676-4806-b4b2-a8d6efb44ccd");
        static readonly Guid MF_MT_FRAME_SIZE = new Guid("1652c33d-d6b2-4012-b834-72030849a37d");
        static readonly Guid MF_MT_FRAME_RATE = new Guid("c459a2e8-3d2c-4e44-b132-fee5156c7bb0");
        static readonly Guid MF_MT_PIXEL_ASPECT_RATIO = new Guid("c6376a1e-8d0a-4027-be45-6d9a0ad39bb6");
        static readonly Guid MF_MT_DEFAULT_STRIDE = new Guid("644b4e48-1e02-4516-b0eb-c01ca9d49ac6");

        static readonly Guid MFMediaType_Video = new Guid("73646976-0000-0010-8000-00AA00389B71");
        static readonly Guid MFVideoFormat_H264 = new Guid("34363248-0000-0010-8000-00AA00389B71");
        static readonly Guid MFVideoFormat_RGB32 = new Guid("00000016-0000-0010-8000-00AA00389B71");

        static ulong Pack2UInt32AsUInt64(uint high, uint low)
        {
            return ((ulong)high << 32) | low;
        }

        static void Main(string[] args)
        {
            string outputPath = args.Length > 0 ? args[0] : "Prompteur_Plaidoyer.mp4";
            int width = 1280;
            int height = 720;
            int fps = 30;
            long frameDurationHns = 10000000L / fps;

            string[] paragraphs = new string[]
            {
                "Quand on regarde une vidéo, un film ou une interview sur nos écrans, il y a une question qu'on commence tous à se poser : est-ce que ce que je vois est vrai ? Est-ce que la voix que j'entends, les respirations, les silences, viennent vraiment d'une personne réelle ?",
                "Pendant longtemps, la réponse était évidente. Derrière chaque image, derrière chaque prise de son et chaque montage, il y avait forcément le regard, le travail et la patience d'un être humain. Mais tout est en train de basculer.",
                "Aujourd'hui, si vous demandez à une intelligence artificielle de générer ce discours avec un avatar vidéo, le résultat sera techniquement irréprochable. L'avatar n'aura aucun trac. Il ne cherchera jamais ses mots. Son regard sera fixe, sa voix sera lisse, son ton sera faussement théâtral. Ce sera parfait. Et c'est justement ça, le problème.",
                "Dans nos métiers de l'image et du son, on essaie de nous faire croire que l'IA va simplement « faciliter le travail créatif ». En réalité, le calcul est purement logique : un algorithme va plus vite, ne demande pas de salaire, et produit à la chaîne. Alors petit à petit, on remplace des monteurs, des techniciens, des graphistes, des musiciens.",
                "Ce n'est pas de la science-fiction, c'est déjà là. Aux États-Unis, les acteurs et les scénaristes ont dû faire grève pendant des mois parce que les studios voulaient scanner leurs visages et les réutiliser à l'infini sans les payer. En France, les comédiens de doublage se battent aujourd'hui parce que des logiciels clonent leurs voix en quelques clics.",
                "Mais au-delà des emplois qu'on supprime, qu'est-ce qu'on est en train de perdre ?\nUne intelligence artificielle ne crée rien. Elle copie, elle mélange, elle recycle ce que des humains ont déjà inventé avant elle.",
                "Le réalisateur Hayao Miyazaki, quand on lui a présenté une animation créée par une machine, a eu cette phrase très simple : « C'est une insulte à la vie elle-même. »",
                "Parce que ce qui nous touche dans un film, dans un documentaire ou dans une chanson, ce n'est pas la perfection technique. C'est l'erreur, c'est le silence, c'est l'hésitation, c'est la sensibilité de celui qui a tenu la caméra ou réglé le son. L'algorithme imite l'émotion, mais l'humain, lui, la vit vraiment.",
                "Alors voilà, je ne vous aurai pas fait un grand numéro d'acteur aujourd'hui. Je vous ai parlé simplement, avec mes mots. Mais posez-vous juste cette question : demain, sur vos écrans, est-ce que vous préférez voir des humains imparfaits qui ressentent les choses… ou des machines parfaites qui font semblant ?"
            };

            // Pre-render the entire scrolling text onto one tall Bitmap so per-frame drawing is ultra fast!
            int textMarginX = 160;
            int textBoxWidth = width - 2 * textMarginX;
            int paragraphSpacing = 62;

            Font textFont = new Font("Segoe UI", 34f, FontStyle.Bold, GraphicsUnit.Pixel);
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            sf.Alignment = StringAlignment.Center;
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            List<float> paraHeights = new List<float>();
            float totalTextHeight = 0;

            using (Bitmap dummy = new Bitmap(1, 1))
            using (Graphics gMeasure = Graphics.FromImage(dummy))
            {
                gMeasure.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                foreach (string p in paragraphs)
                {
                    SizeF sz = gMeasure.MeasureString(p, textFont, textBoxWidth, sf);
                    paraHeights.Add(sz.Height);
                    totalTextHeight += sz.Height + paragraphSpacing;
                }
            }

            int stripHeight = (int)Math.Ceiling(totalTextHeight) + 200;
            Bitmap textStrip = new Bitmap(width, stripHeight, PixelFormat.Format32bppRgb);
            using (Graphics gStrip = Graphics.FromImage(textStrip))
            {
                gStrip.Clear(Color.Black);
                gStrip.SmoothingMode = SmoothingMode.AntiAlias;
                gStrip.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                float curY = 20f;
                for (int i = 0; i < paragraphs.Length; i++)
                {
                    RectangleF rect = new RectangleF(textMarginX, curY, textBoxWidth, paraHeights[i] + 20);
                    gStrip.DrawString(paragraphs[i], textFont, Brushes.White, rect, sf);

                    // Draw a subtle separator dot between paragraphs to indicate breathing pause
                    if (i < paragraphs.Length - 1)
                    {
                        float dotY = curY + paraHeights[i] + (paragraphSpacing / 2f) - 3f;
                        using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(110, 255, 255, 255)))
                        {
                            gStrip.FillEllipse(dotBrush, (width / 2f) - 4f, dotY, 8f, 8f);
                        }
                    }

                    curY += paraHeights[i] + paragraphSpacing;
                }
            }

            // Reading line Y position on screen (around 36% from top so eyes stay near the top camera)
            int readingLineY = (int)(height * 0.36);

            // Start with first line at readingLineY, end when the last line has scrolled past readingLineY
            float startScrollY = 0f;
            float endScrollY = totalTextHeight - 80f;
            float scrollDistance = endScrollY - startScrollY;

            // Timing:
            // 5 seconds countdown at start
            // 150 seconds (2 min 30 s) of calm, natural scrolling
            // 4 seconds hold at end
            int countdownSeconds = 5;
            int scrollSeconds = 150;
            int holdEndSeconds = 4;

            int countdownFrames = countdownSeconds * fps;
            int scrollFrames = scrollSeconds * fps;
            int holdEndFrames = holdEndSeconds * fps;
            int totalFrames = args.Length > 1 ? 60 : (countdownFrames + scrollFrames + holdEndFrames);

            Console.WriteLine("Total text height: " + totalTextHeight + " px, Total frames: " + totalFrames + " (" + (totalFrames / fps) + " s)");

            MFStartup(0x00020070, 0);

            try
            {
                IMFSinkWriter sinkWriter;
                MFCreateSinkWriterFromURL(outputPath, IntPtr.Zero, null, out sinkWriter);

                IMFAttributes mediaTypeOut;
                MFCreateMediaType(out mediaTypeOut);
                mediaTypeOut.SetGUID(MF_MT_MAJOR_TYPE, MFMediaType_Video);
                mediaTypeOut.SetGUID(MF_MT_SUBTYPE, MFVideoFormat_H264);
                mediaTypeOut.SetUINT32(MF_MT_AVG_BITRATE, 2500000);
                mediaTypeOut.SetUINT32(MF_MT_INTERLACE_MODE, 2); // Progressive
                mediaTypeOut.SetUINT64(MF_MT_FRAME_SIZE, Pack2UInt32AsUInt64((uint)width, (uint)height));
                mediaTypeOut.SetUINT64(MF_MT_FRAME_RATE, Pack2UInt32AsUInt64((uint)fps, 1));
                mediaTypeOut.SetUINT64(MF_MT_PIXEL_ASPECT_RATIO, Pack2UInt32AsUInt64(1, 1));

                int streamIndex;
                sinkWriter.AddStream(mediaTypeOut, out streamIndex);

                IMFAttributes mediaTypeIn;
                MFCreateMediaType(out mediaTypeIn);
                mediaTypeIn.SetGUID(MF_MT_MAJOR_TYPE, MFMediaType_Video);
                mediaTypeIn.SetGUID(MF_MT_SUBTYPE, MFVideoFormat_RGB32);
                mediaTypeIn.SetUINT32(MF_MT_INTERLACE_MODE, 2);
                mediaTypeIn.SetUINT64(MF_MT_FRAME_SIZE, Pack2UInt32AsUInt64((uint)width, (uint)height));
                mediaTypeIn.SetUINT64(MF_MT_FRAME_RATE, Pack2UInt32AsUInt64((uint)fps, 1));
                mediaTypeIn.SetUINT64(MF_MT_PIXEL_ASPECT_RATIO, Pack2UInt32AsUInt64(1, 1));
                // Negative stride because GDI+ bitmaps are top-down while RGB32 in MF expects bottom-up unless negative stride is specified
                // Actually let's set positive stride and flip or set negative stride: let's flip with RotateFlip or copy bottom-up, or set negative stride!
                // Copying or setting negative stride: let's set negative stride (-width * 4) cast to uint!
                int posStride = width * 4;
                mediaTypeIn.SetUINT32(MF_MT_DEFAULT_STRIDE, (uint)posStride);

                sinkWriter.SetInputMediaType(streamIndex, mediaTypeIn, null);
                sinkWriter.BeginWriting();

                Bitmap frameBmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                Font countdownFont = new Font("Segoe UI", 80f, FontStyle.Bold, GraphicsUnit.Pixel);
                Font subFont = new Font("Segoe UI", 24f, FontStyle.Regular, GraphicsUnit.Pixel);
                StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                int bufferSize = width * height * 4;

                for (int frame = 0; frame < totalFrames; frame++)
                {
                    using (Graphics g = Graphics.FromImage(frameBmp))
                    {
                        g.Clear(Color.Black);

                        float scrollY;
                        if (frame < countdownFrames)
                        {
                            scrollY = startScrollY;
                        }
                        else if (frame < countdownFrames + scrollFrames)
                        {
                            float t = (float)(frame - countdownFrames) / (float)scrollFrames;
                            scrollY = startScrollY + t * scrollDistance;
                        }
                        else
                        {
                            scrollY = endScrollY;
                        }

                        // Draw the visible slice of textStrip onto frameBmp
                        // When scrollY = 0, top of textStrip is at readingLineY
                        int destY = (int)Math.Round(readingLineY - scrollY);
                        g.DrawImageUnscaled(textStrip, 0, destY);

                        // Top and bottom subtle vignette fade so lines enter/exit smoothly
                        using (LinearGradientBrush topFade = new LinearGradientBrush(new Rectangle(0, 0, width, 110), Color.Black, Color.FromArgb(0, 0, 0, 0), LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(topFade, 0, 0, width, 110);
                        }
                        using (LinearGradientBrush botFade = new LinearGradientBrush(new Rectangle(0, height - 110, width, 110), Color.FromArgb(0, 0, 0, 0), Color.Black, LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(botFade, 0, height - 110, width, 110);
                        }

                        // Draw eye-level reading zone markers on left and right margins
                        using (Pen guidePen = new Pen(Color.FromArgb(90, 255, 255, 255), 2f))
                        using (SolidBrush arrowBrush = new SolidBrush(Color.FromArgb(180, 255, 255, 255)))
                        {
                            PointF[] leftArrow = new PointF[]
                            {
                                new PointF(95, readingLineY + 12),
                                new PointF(95, readingLineY + 34),
                                new PointF(115, readingLineY + 23)
                            };
                            PointF[] rightArrow = new PointF[]
                            {
                                new PointF(width - 95, readingLineY + 12),
                                new PointF(width - 95, readingLineY + 34),
                                new PointF(width - 115, readingLineY + 23)
                            };
                            g.FillPolygon(arrowBrush, leftArrow);
                            g.FillPolygon(arrowBrush, rightArrow);
                        }

                        // Progress bar at the very bottom (subtle)
                        if (frame >= countdownFrames)
                        {
                            float prog = Math.Min(1f, (float)(frame - countdownFrames) / (float)scrollFrames);
                            using (SolidBrush progBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
                            {
                                g.FillRectangle(progBrush, 0, height - 4, width * prog, 4);
                            }
                        }

                        // Countdown overlay during the first 5 seconds
                        if (frame < countdownFrames)
                        {
                            int secLeft = countdownSeconds - (frame / fps);
                            using (SolidBrush overlay = new SolidBrush(Color.FromArgb(190, 0, 0, 0)))
                            {
                                g.FillRectangle(overlay, 0, 0, width, height - (height - readingLineY));
                            }
                            g.DrawString("Début dans " + secLeft + "...", subFont, Brushes.LightGray, new RectangleF(0, 80, width, 50), centerFormat);
                        }
                    }

                    // Copy frameBmp to IMFMediaBuffer
                    BitmapData bmpData = frameBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

                    IMFMediaBuffer mediaBuffer;
                    MFCreateMemoryBuffer(bufferSize, out mediaBuffer);

                    IntPtr pDest;
                    int maxLen, curLen;
                    mediaBuffer.Lock(out pDest, out maxLen, out curLen);

                    // Copy row by row bottom-up just in case H.264 encoder ignores negative stride on some drivers, wait:
                    // Let's check: if we set positive stride on mediaTypeIn and copy bottom-up (or leave top-down), which does MF expect for RGB32?
                    // In Windows Media Foundation, MFVideoFormat_RGB32 has default positive stride = bottom-up, OR we can just copy row-by-row bottom-up with default stride!
                    // Actually, let's check both or use positive stride + bottom-up copy, Wait: in MF, does H264 encoder expect top-down or bottom-up when MF_MT_DEFAULT_STRIDE is not set?
                    // Let's check Microsoft docs: "For uncompressed RGB formats, the default stride is positive, which means the image is stored bottom-up in memory. However, the H.264 encoder MFT expects top-down input or checks MF_MT_DEFAULT_STRIDE."
                    // Wait: if we just copy top-down without setting MF_MT_DEFAULT_STRIDE, is it upside-down or right-side-up? Let's test on 1 frame or check!
                    memcpy(pDest, bmpData.Scan0, (UIntPtr)bufferSize);

                    mediaBuffer.Unlock();
                    mediaBuffer.SetCurrentLength(bufferSize);
                    frameBmp.UnlockBits(bmpData);

                    IMFSample sample;
                    MFCreateSample(out sample);
                    sample.AddBuffer(mediaBuffer);
                    sample.SetSampleTime(frame * frameDurationHns);
                    sample.SetSampleDuration(frameDurationHns);

                    sinkWriter.WriteSample(streamIndex, sample);

                    Marshal.ReleaseComObject(sample);
                    Marshal.ReleaseComObject(mediaBuffer);

                    if (frame % 300 == 0)
                    {
                        Console.WriteLine("Encoded " + (frame / fps) + "s / " + (totalFrames / fps) + "s");
                    }
                }

                sinkWriter.Finalize_();
                Marshal.ReleaseComObject(sinkWriter);
                frameBmp.Dispose();
            }
            finally
            {
                textStrip.Dispose();
                textFont.Dispose();
                MFShutdown();
            }

            Console.WriteLine("Done! Saved to " + outputPath);
        }
    }
}
