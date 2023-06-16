using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using System.Linq;

namespace CG_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        public static void Negative(Image<Bgr, byte> img)
        {
            int x, y;

            Bgr aux;
            for (y = 0; y < img.Height; y++)
            {
                for (x = 0; x < img.Width; x++)
                {
                    // acesso directo : mais lento 
                    aux = img[y, x];
                    img[y, x] = new Bgr(255 - aux.Blue, 255 - aux.Green, 255 - aux.Red);
                }
            }
        }

        /// <summary>
        /// Convert to negative
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void NegativeDirectAcess(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to negtaive
                            blue = (byte)(255 - blue);
                            green = (byte)(255 - green);
                            red = (byte)(255 - red);


                            // store in the image
                            dataPtr[0] = blue;
                            dataPtr[1] = green;
                            dataPtr[2] = red;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Convert to gray
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)Math.Round(((int)blue + green + red) / 3.0);

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Adjust brightness and contrast
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {

            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int blueTemp, greenTemp, redTemp;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            //adjust brightness and constrast
                            blueTemp = (int)Math.Round(contrast * dataPtr[0] + bright);
                            if (blueTemp > 255)
                            {
                                blueTemp = 255;
                            }
                            else if (blueTemp < 0)
                            {
                                blueTemp = 0;
                            }

                            greenTemp = (int)Math.Round(contrast * dataPtr[1] + bright);
                            if (greenTemp > 255)
                            {
                                greenTemp = 255;
                            }
                            else if (greenTemp < 0)
                            {
                                greenTemp = 0;
                            }

                            redTemp = (int)Math.Round(contrast * dataPtr[2] + bright);
                            if (redTemp > 255)
                            {
                                redTemp = 255;
                            }
                            else if (redTemp < 0)
                            {
                                redTemp = 0;
                            }

                            // store in the image
                            dataPtr[0] = (byte)(blueTemp);
                            dataPtr[1] = (byte)(greenTemp);
                            dataPtr[2] = (byte)(redTemp);

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }

        }

        /// <summary>
        /// Keeps only the red channel
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int nChan = m.nChannels; // number of channels - 3

                int steps = m.widthStep;
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        (dataPtr + nChan * x + steps * y)[0] = (dataPtr + nChan * x + steps * y)[2];
                        (dataPtr + nChan * x + steps * y)[1] = (dataPtr + nChan * x + steps * y)[2];
                        //(dataPtr + nChan * x + steps * y)[2] = (byte)(255 - (dataPtr + nChan * x + steps * y)[2]);
                    }
                }
            }
        }

        /// <summary>
        /// translation
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> img2, int dx, int dy)
        {

            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = img2.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            if (x - dx < 0 || y - dy < 0 || x - dx >= m.width || y - dy >= m.height)
                            {
                                *blue = 0;
                                *green = 0;
                                *red = 0;
                                continue;
                            }

                            *blue = (dataPtr2 + (y - dy) * m.widthStep + (x - dx) * nChan)[0];
                            *green = (dataPtr2 + (y - dy) * m.widthStep + (x - dx) * nChan)[1];
                            *red = (dataPtr2 + (y - dy) * m.widthStep + (x - dx) * nChan)[2];
                        }
                    }
                }

            }

        }

        /// <summary>
        /// rotation
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> img2, float angle)
        {

            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = img2.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, xTemp, yTemp;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            xTemp = (int)(Math.Round(((x - (width / 2.0)) * Math.Cos(angle)) - (((height / 2.0) - y) * Math.Sin(angle)) + (width / 2.0)));
                            yTemp = (int)(Math.Round((height / 2.0) - ((x - (width / 2.0)) * Math.Sin(angle)) - ((height / 2.0) - y) * Math.Cos(angle)));

                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            if (xTemp < 0 || yTemp < 0 || xTemp >= m.width || yTemp >= m.height)
                            {
                                *blue = 0;
                                *green = 0;
                                *red = 0;
                                continue;
                            }

                            *blue = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[0];
                            *green = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[1];
                            *red = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[2];
                        }
                    }
                }

            }

        }

        /// <summary>
        /// scale
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> img2, float scale)
        {

            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = img2.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, xTemp, yTemp;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            xTemp = (int)(x / scale);
                            yTemp = (int)(y / scale);

                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            if (xTemp < 0 || yTemp < 0 || xTemp >= m.width || yTemp >= m.height)
                            {
                                *blue = 0;
                                *green = 0;
                                *red = 0;
                                continue;
                            }

                            *blue = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[0];
                            *green = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[1];
                            *red = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[2];
                        }
                    }
                }

            }

        }

        /// <summary>
        /// scale (x,y)
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> img2, float scale, int centerX, int centerY)
        {

            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = img2.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, xTemp, yTemp;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            xTemp = (int)(Math.Round((x - (width / 2)) / scale) + centerX);
                            yTemp = (int)(Math.Round((y - (height / 2)) / scale) + centerY);

                            if (x + centerX < 0)
                            {
                                xTemp = (int)(Math.Round((x - (width / 2)) / scale));
                            }

                            if (y + centerY < 0)
                            {
                                yTemp = (int)(Math.Round((y - (height / 2)) / scale));
                            }

                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            if (xTemp < 0 || yTemp < 0 || xTemp >= m.width || yTemp >= m.height)
                            {
                                *blue = 0;
                                *green = 0;
                                *red = 0;
                                continue;
                            }

                            *blue = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[0];
                            *green = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[1];
                            *red = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan)[2];
                        }
                    }
                }

            }

        }

        /// <summary>
        /// mean - solution A (3x3)
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> img2)
        {

            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = img2.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, xTemp, yTemp, blueSum = 0, greenSum = 0, redSum = 0;

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            blueSum = greenSum = redSum = 0;

                            for (yTemp = -1; yTemp <= 1; yTemp++)
                            {
                                for (xTemp = -1; xTemp <= 1; xTemp++)
                                {
                                    blueSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[0];
                                    greenSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[1];
                                    redSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[2];
                                }
                            }


                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);


                            *blue = (byte)Math.Round(blueSum / 9.0);
                            *green = (byte)Math.Round(greenSum / 9.0);
                            *red = (byte)Math.Round(redSum / 9.0);
                        }
                    }

                    //bordas da imagem
                    //borda esquerda
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum += 2 * (int)(dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0];
                            greenSum += 2 * (int)(dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1];
                            redSum += 2 * (int)(dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2];

                            blueSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[0];
                            greenSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[1];
                            redSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[2];
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 2);


                        *blue = (byte)Math.Round(blueSum / 9.0);
                        *green = (byte)Math.Round(greenSum / 9.0);
                        *red = (byte)Math.Round(redSum / 9.0);
                    }

                    //borda cima
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0];
                            greenSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1];
                            redSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2];

                            blueSum += (int)(dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[0];
                            greenSum += (int)(dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[1];
                            redSum += (int)(dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[2];
                        }

                        blue = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 2);

                        *blue = (byte)Math.Round(blueSum / 9.0);
                        *green = (byte)Math.Round(greenSum / 9.0);
                        *red = (byte)Math.Round(redSum / 9.0);
                    }

                    //borda direita
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum += 2 * (int)(dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0];
                            greenSum += 2 * (int)(dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1];
                            redSum += 2 * (int)(dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2];

                            blueSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[0];
                            greenSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[1];
                            redSum += (int)(dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[2];
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 2);


                        *blue = (byte)Math.Round(blueSum / 9.0);
                        *green = (byte)Math.Round(greenSum / 9.0);
                        *red = (byte)Math.Round(redSum / 9.0);
                    }

                    //borda baixo
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0];
                            greenSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1];
                            redSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2];

                            blueSum += (int)(dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[0];
                            greenSum += (int)(dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[1];
                            redSum += (int)(dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[2];
                        }

                        blue = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 2);

                        *blue = (byte)Math.Round(blueSum / 9.0);
                        *green = (byte)Math.Round(greenSum / 9.0);
                        *red = (byte)Math.Round(redSum / 9.0);
                    }

                    //cantos
                    //canto superior direito
                    blueSum = greenSum = redSum = 0;

                    blueSum += 4 * (int)(dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0];
                    greenSum += 4 * (int)(dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1];
                    redSum += 4 * (int)(dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2];

                    blueSum += (int)(dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[0];
                    greenSum += (int)(dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[1];
                    redSum += (int)(dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[2];

                    blue = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 2);

                    *blue = (byte)Math.Round(blueSum / 9.0);
                    *green = (byte)Math.Round(greenSum / 9.0);
                    *red = (byte)Math.Round(redSum / 9.0);

                    //canto superior esquerdo
                    blueSum = greenSum = redSum = 0;

                    blueSum += 4 * (int)(dataPtr2 + 0 * m.widthStep + 0 * nChan)[0];
                    greenSum += 4 * (int)(dataPtr2 + 0 * m.widthStep + 0 * nChan)[1];
                    redSum += 4 * (int)(dataPtr2 + 0 * m.widthStep + 0 * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + 1 * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + 1 * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + 0 * m.widthStep + 1 * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + 1 * m.widthStep + 0 * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + 1 * m.widthStep + 0 * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + 1 * m.widthStep + 0 * nChan)[2];

                    blueSum += (int)(dataPtr2 + 1 * m.widthStep + 1 * nChan)[0];
                    greenSum += (int)(dataPtr2 + 1 * m.widthStep + 1 * nChan)[1];
                    redSum += (int)(dataPtr2 + 1 * m.widthStep + 1 * nChan)[2];

                    blue = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 2);

                    *blue = (byte)Math.Round(blueSum / 9.0);
                    *green = (byte)Math.Round(greenSum / 9.0);
                    *red = (byte)Math.Round(redSum / 9.0);

                    //canto inferior esquerdo
                    blueSum = greenSum = redSum = 0;

                    blueSum += 4 * (int)(dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0];
                    greenSum += 4 * (int)(dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1];
                    redSum += 4 * (int)(dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2];

                    blueSum += (int)(dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[0];
                    greenSum += (int)(dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[1];
                    redSum += (int)(dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[2];

                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 2);

                    *blue = (byte)Math.Round(blueSum / 9.0);
                    *green = (byte)Math.Round(greenSum / 9.0);
                    *red = (byte)Math.Round(redSum / 9.0);

                    //canto inferior direito
                    blueSum = greenSum = redSum = 0;

                    blueSum += 4 * (int)(dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0];
                    greenSum += 4 * (int)(dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1];
                    redSum += 4 * (int)(dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2];

                    blueSum += 2 * (int)(dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0];
                    greenSum += 2 * (int)(dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1];
                    redSum += 2 * (int)(dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2];

                    blueSum += (int)(dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[0];
                    greenSum += (int)(dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[1];
                    redSum += (int)(dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[2];

                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 2);

                    *blue = (byte)Math.Round(blueSum / 9.0);
                    *green = (byte)Math.Round(greenSum / 9.0);
                    *red = (byte)Math.Round(redSum / 9.0);
                }

            }

        }

        /// <summary>
        /// mean non-uniform (3x3)
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] matrix, float matrixWeight)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, xTemp, yTemp, blueSum = 0, greenSum = 0, redSum = 0;

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            blueSum = greenSum = redSum = 0;

                            for (yTemp = -1; yTemp <= 1; yTemp++)
                            {
                                for (xTemp = -1; xTemp <= 1; xTemp++)
                                {
                                    blueSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[0] * matrix[xTemp + 1, yTemp + 1]);
                                    greenSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[1] * matrix[xTemp + 1, yTemp + 1]);
                                    redSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[2] * matrix[xTemp + 1, yTemp + 1]);
                                }
                            }


                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                            greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                            redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                            *blue = (byte)blueSum;
                            *green = (byte)greenSum;
                            *red = (byte)redSum;
                        }
                    }

                    //bordas da imagem
                    //borda esquerda
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix[0, yTemp + 1]);
                            greenSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix[0, yTemp + 1]);
                            redSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix[0, yTemp + 1]);

                            blueSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix[1, yTemp + 1]);
                            greenSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix[1, yTemp + 1]);
                            redSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix[1, yTemp + 1]);

                            blueSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[0] * matrix[2, yTemp + 1]);
                            greenSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[1] * matrix[2, yTemp + 1]);
                            redSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[2] * matrix[2, yTemp + 1]);
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 2);

                        blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                        greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                        redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                        *blue = (byte)blueSum;
                        *green = (byte)greenSum;
                        *red = (byte)redSum;
                    }

                    //borda cima
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix[xTemp + 1, 0]);
                            greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix[xTemp + 1, 0]);
                            redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix[xTemp + 1, 0]);

                            blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix[xTemp + 1, 1]);
                            greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix[xTemp + 1, 1]);
                            redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix[xTemp + 1, 1]);

                            blueSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[0] * matrix[xTemp + 1, 2]);
                            greenSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[1] * matrix[xTemp + 1, 2]);
                            redSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[2] * matrix[xTemp + 1, 2]);
                        }

                        blue = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 2);

                        blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                        greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                        redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                        *blue = (byte)blueSum;
                        *green = (byte)greenSum;
                        *red = (byte)redSum;
                    }

                    //borda direita
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix[0, yTemp + 1]);
                            greenSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix[0, yTemp + 1]);
                            redSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix[0, yTemp + 1]);

                            blueSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix[1, yTemp + 1]);
                            greenSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix[1, yTemp + 1]);
                            redSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix[1, yTemp + 1]);

                            blueSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[0] * matrix[2, yTemp + 1]);
                            greenSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[1] * matrix[2, yTemp + 1]);
                            redSum += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[2] * matrix[2, yTemp + 1]);
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 2);

                        blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                        greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                        redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                        *blue = (byte)blueSum;
                        *green = (byte)greenSum;
                        *red = (byte)redSum;
                    }

                    //borda baixo
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum = greenSum = redSum = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix[xTemp + 1, 0]);
                            greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix[xTemp + 1, 0]);
                            redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix[xTemp + 1, 0]);

                            blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix[xTemp + 1, 1]);
                            greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix[xTemp + 1, 1]);
                            redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix[xTemp + 1, 1]);

                            blueSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[0] * matrix[xTemp + 1, 2]);
                            greenSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[1] * matrix[xTemp + 1, 2]);
                            redSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[2] * matrix[xTemp + 1, 2]);
                        }

                        blue = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 2);

                        blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                        greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                        redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                        *blue = (byte)blueSum;
                        *green = (byte)greenSum;
                        *red = (byte)redSum;
                    }

                    //cantos
                    //canto superior direito
                    blueSum = greenSum = redSum = 0;

                    //canto quadruplicado
                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix[1, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix[1, 1]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix[1, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix[1, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix[1, 0]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix[1, 0]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix[2, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix[2, 1]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix[2, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix[2, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix[2, 0]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix[2, 0]);

                    //margem duplicada superior
                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix[0, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix[0, 0]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix[0, 0]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix[0, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix[0, 1]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix[0, 1]);

                    //margem duplicada direita
                    blueSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix[1, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix[1, 2]);
                    redSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix[1, 2]);

                    blueSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix[2, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix[2, 2]);
                    redSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix[2, 2]);

                    //restante
                    blueSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[0] * matrix[0, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[1] * matrix[0, 2]);
                    redSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[2] * matrix[0, 2]);


                    blue = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 2);

                    blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                    greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                    redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                    *blue = (byte)blueSum;
                    *green = (byte)greenSum;
                    *red = (byte)redSum;

                    //canto superior esquerdo
                    blueSum = greenSum = redSum = 0;

                    //canto quadruplicado
                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix[0, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix[0, 0]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix[0, 0]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix[0, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix[0, 1]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix[0, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix[1, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix[1, 0]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix[1, 0]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix[1, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix[1, 1]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix[1, 1]);

                    //margem duplicada superior
                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix[2, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix[2, 0]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix[2, 0]);

                    blueSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix[2, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix[2, 1]);
                    redSum += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix[2, 1]);

                    //margem duplicada esquerda
                    blueSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix[0, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix[0, 2]);
                    redSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix[0, 2]);

                    blueSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix[1, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix[1, 2]);
                    redSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix[1, 2]);

                    //restante
                    blueSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[0] * matrix[2, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[1] * matrix[2, 2]);
                    redSum += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[2] * matrix[2, 2]);


                    blue = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 2);

                    blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                    greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                    redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                    *blue = (byte)blueSum;
                    *green = (byte)greenSum;
                    *red = (byte)redSum;

                    //canto inferior esquerdo
                    blueSum = greenSum = redSum = 0;

                    //canto quadruplicado
                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix[1, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix[1, 1]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix[1, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix[0, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix[0, 1]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix[0, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix[1, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix[1, 2]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix[1, 2]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix[0, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix[0, 2]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix[0, 2]);

                    //margem duplicada inferior
                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix[2, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix[2, 1]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix[2, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix[2, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix[2, 2]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix[2, 2]);

                    //margem duplicada esquerda
                    blueSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix[0, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix[0, 0]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix[0, 0]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix[1, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix[1, 0]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix[1, 0]);

                    //restante
                    blueSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[0] * matrix[2, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[1] * matrix[2, 0]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[2] * matrix[2, 0]);


                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 2);

                    blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                    greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                    redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                    *blue = (byte)blueSum;
                    *green = (byte)greenSum;
                    *red = (byte)redSum;

                    //canto inferior direito
                    blueSum = greenSum = redSum = 0;

                    //canto quadruplicado
                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix[1, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix[1, 1]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix[1, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix[1, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix[1, 2]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix[1, 2]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix[2, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix[2, 1]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix[2, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix[2, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix[2, 2]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix[2, 2]);

                    //margem duplicada inferior
                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix[0, 1]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix[0, 1]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix[0, 1]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix[0, 2]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix[0, 2]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix[0, 2]);

                    //margem duplicada direita
                    blueSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix[1, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix[1, 0]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix[1, 0]);

                    blueSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix[2, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix[2, 0]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix[2, 0]);

                    //restante
                    blueSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[0] * matrix[0, 0]);
                    greenSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[1] * matrix[0, 0]);
                    redSum += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[2] * matrix[0, 0]);


                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 2);

                    blueSum = (int)Math.Max(0, Math.Min(255, Math.Round(blueSum / (decimal)matrixWeight)));
                    greenSum = (int)Math.Max(0, Math.Min(255, Math.Round(greenSum / (decimal)matrixWeight)));
                    redSum = (int)Math.Max(0, Math.Min(255, Math.Round(redSum / (decimal)matrixWeight)));

                    *blue = (byte)blueSum;
                    *green = (byte)greenSum;
                    *red = (byte)redSum;
                }

            }
        }

        /// <summary>
        /// sobel (3x3)
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, xTemp, yTemp, blueSum1 = 0, greenSum1 = 0, redSum1 = 0, blueSum2 = 0, greenSum2 = 0, redSum2 = 0;

                float[,] matrix1 = new float[,] { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } };
                float[,] matrix2 = new float[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                            for (yTemp = -1; yTemp <= 1; yTemp++)
                            {
                                for (xTemp = -1; xTemp <= 1; xTemp++)
                                {
                                    blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, yTemp + 1]);
                                    greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, yTemp + 1]);
                                    redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, yTemp + 1]);

                                    blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, yTemp + 1]);
                                    greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, yTemp + 1]);
                                    redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, yTemp + 1]);
                                }
                            }


                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                            greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                            redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                            *blue = (byte)blueSum1;
                            *green = (byte)greenSum1;
                            *red = (byte)redSum1;
                        }
                    }

                    //bordas da imagem
                    //borda esquerda
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix1[0, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix1[0, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix1[0, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix1[1, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix1[1, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix1[1, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[0] * matrix1[2, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[1] * matrix1[2, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[2] * matrix1[2, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix2[0, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix2[0, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix2[0, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix2[1, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix2[1, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix2[1, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[0] * matrix2[2, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[1] * matrix2[2, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[2] * matrix2[2, yTemp + 1]);
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda cima
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 0]);
                            greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 0]);
                            redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 0]);

                            blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 2]);
                            greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 2]);
                            redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 2]);

                            blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 0]);
                            greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 0]);
                            redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 0]);

                            blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 2]);
                            greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 2]);
                            redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 2]);
                        }

                        blue = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda direita
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix1[0, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix1[0, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix1[0, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[0] * matrix1[2, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[1] * matrix1[2, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[2] * matrix1[2, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix2[0, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix2[0, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix2[0, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[0] * matrix2[2, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[1] * matrix2[2, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[2] * matrix2[2, yTemp + 1]);
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda baixo
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 0]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 0]);
                            redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 0]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 2]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 2]);
                            redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 2]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 0]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 0]);
                            redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 0]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 2]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 2]);
                            redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 2]);
                        }

                        blue = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //cantos
                    //canto superior direito
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 0]);

                    //margem duplicada superior
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 1]);

                    //margem duplicada direita
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 2]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 2]);


                    blue = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;

                    //canto superior esquerdo
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[0, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[0, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[1, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[1, 1]);

                    //margem duplicada superior
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix1[2, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix1[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix2[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix2[2, 1]);

                    //margem duplicada esquerda
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix1[0, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix1[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix2[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix2[1, 2]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[2] * matrix2[2, 2]);


                    blue = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;

                    //canto inferior esquerdo
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[1, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[0, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[1, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[0, 2]);

                    //margem duplicada inferior
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix1[2, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix2[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix2[2, 2]);

                    //margem duplicada esquerda
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix1[0, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix1[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix2[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix2[1, 0]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[2] * matrix1[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[2] * matrix2[2, 0]);


                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;

                    //canto inferior direito
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 2]);

                    //margem duplicada inferior
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 2]);

                    //margem duplicada direita
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 0]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 0]);


                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;
                }
            }
        }

        /// <summary>
        /// Diferentiation (3x3)
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, blueSum1 = 0, greenSum1 = 0, redSum1 = 0, blueSum2 = 0, greenSum2 = 0, redSum2 = 0;

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 0; y < height - 1; y++)
                    {
                        for (x = 0; x < width - 1; x++)
                        {
                            blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                            blueSum1 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[0];
                            greenSum1 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[1];
                            redSum1 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[2];

                            blueSum1 -= (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 1) * nChan)[0];
                            greenSum1 -= (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 1) * nChan)[1];
                            redSum1 -= (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 1) * nChan)[2];

                            blueSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[0];
                            greenSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[1];
                            redSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[2];

                            blueSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 0) * nChan)[0];
                            greenSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 0) * nChan)[1];
                            redSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 0) * nChan)[2];

                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                            greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                            redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                            *blue = (byte)blueSum1;
                            *green = (byte)greenSum1;
                            *red = (byte)redSum1;
                        }
                    }

                    //bordas da imagem
                    //borda direita
                    for (y = 0; y < height - 1; y++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        blueSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (width - 1) * nChan)[0];
                        greenSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (width - 1) * nChan)[1];
                        redSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (width - 1) * nChan)[2];

                        blueSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (width - 1) * nChan)[0];
                        greenSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (width - 1) * nChan)[1];
                        redSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (width - 1) * nChan)[2];

                        blue = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda baixo
                    for (x = 0; x < width - 1; x++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        blueSum1 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[0];
                        greenSum1 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[1];
                        redSum1 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[2];

                        blueSum1 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[0];
                        greenSum1 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[1];
                        redSum1 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[2];

                        blue = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //cantos
                    //canto inferior direito
                    blueSum1 = greenSum1 = redSum1 = 0;

                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 2);

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;
                }
            }
        }

        /// <summary>
        /// Median with 3 dimensions
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Median(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {

            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                MIplImage n = imgCopy.MIplImage;
                byte* dataPtrWrite = (byte*)m.imageData.ToPointer(); // Pointer to the original image
                byte* dataPtrRead = (byte*)n.imageData.ToPointer(); // Pointer to the image copy

                int width = img.Width;
                int height = img.Height;
                int nChanWrite = m.nChannels; // number of channels - 3
                int nChanRead = n.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, menor;
                int[] pvals = new int[9];
                int[] dvals = new int[9];
                int stepWrite = m.widthStep;
                int stepRead = n.widthStep;

                if (nChanWrite == 3) // image in RGB
                {
                    #region Center
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            #region P1
                            //Current reference pixel: x-1, y-1

                            //D12
                            //target pixel: x, y-1
                            int d12 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D13
                            //target pixel: x+1, y-1
                            int d13 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D14
                            //target pixel: x-1, y
                            int d14 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D15
                            //target pixel: x, y
                            int d15 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D16
                            //target pixel: x+1, y
                            int d16 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D17
                            //target pixel: x-1, y+1
                            int d17 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D18
                            //target pixel: x, y+1
                            int d18 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            //D19
                            //target pixel: x+1, y+1
                            int d19 = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[0] = d12 + d13 + d14 + d15 + d16 + d17 + d18 + d19;
                            #endregion

                            #region P2
                            //Current reference pixel: x, y-1

                            //D21
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D23
                            //target pixel: x+1, y-1
                            dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D24
                            //target pixel: x-1, y
                            dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D25
                            //target pixel: x, y
                            dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D26
                            //target pixel: x+1, y
                            dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D27
                            //target pixel: x-1, y+1
                            dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D28
                            //target pixel: x, y+1
                            dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            //D29
                            //target pixel: x+1, y+1
                            dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[1] = dvals[0] + dvals[2] + dvals[3] + dvals[4] + dvals[5] + dvals[6] + dvals[7] + dvals[8];
                            #endregion

                            #region P3
                            //Current reference pixel: x+1, y-1

                            //D31
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D32
                            //target pixel: x, y-1
                            dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D34
                            //target pixel: x-1, y
                            dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D35
                            //target pixel: x, y
                            dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D36
                            //target pixel: x+1, y
                            dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D37
                            //target pixel: x-1, y+1
                            dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D38
                            //target pixel: x, y+1
                            dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            //D39
                            //target pixel: x+1, y+1
                            dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[2] = dvals[0] + dvals[1] + dvals[3] + dvals[4] + dvals[5] + dvals[6] + dvals[7] + dvals[8];
                            #endregion

                            #region P4
                            //Current reference pixel: x-1, y

                            //D41
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D42
                            //target pixel: x, y-1
                            dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D43
                            //target pixel: x+1, y-1
                            dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D45
                            //target pixel: x, y
                            dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D46
                            //target pixel: x+1, y
                            dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D47
                            //target pixel: x-1, y+1
                            dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D48
                            //target pixel: x, y+1
                            dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            //D49
                            //target pixel: x+1, y+1
                            dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[3] = dvals[0] + dvals[1] + dvals[2] + dvals[4] + dvals[5] + dvals[6] + dvals[7] + dvals[8];
                            #endregion

                            #region P5
                            //Current reference pixel: x, y

                            //D51
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D52
                            //target pixel: x, y-1
                            dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D53
                            //target pixel: x+1, y-1
                            dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D54
                            //target pixel: x-1, y
                            dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D56
                            //target pixel: x+1, y
                            dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D57
                            //target pixel: x-1, y+1
                            dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D58
                            //target pixel: x, y+1
                            dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            //D59
                            //target pixel: x+1, y+1
                            dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[4] = dvals[0] + dvals[1] + dvals[2] + dvals[3] + dvals[5] + dvals[6] + dvals[7] + dvals[8];
                            #endregion

                            #region P6
                            //Current reference pixel: x+1, y

                            //D61
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D62
                            //target pixel: x, y-1
                            dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D63
                            //target pixel: x+1, y-1
                            dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D64
                            //target pixel: x-1, y
                            dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D65
                            //target pixel: x, y
                            dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D67
                            //target pixel: x-1, y+1
                            dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D68
                            //target pixel: x, y+1
                            dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            //D69
                            //target pixel: x+1, y+1
                            dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[5] = dvals[0] + dvals[1] + dvals[2] + dvals[3] + dvals[4] + dvals[6] + dvals[7] + dvals[8];
                            #endregion

                            #region P7
                            //Current reference pixel: x-1, y+1

                            //D71
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D72
                            //target pixel: x, y-1
                            dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D73
                            //target pixel: x+1, y-1
                            dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D74
                            //target pixel: x-1, y
                            dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D75
                            //target pixel: x, y
                            dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D76
                            //target pixel: x+1, y
                            dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D78
                            //target pixel: x, y+1
                            dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            //D79
                            //target pixel: x+1, y+1
                            dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[6] = dvals[0] + dvals[1] + dvals[2] + dvals[3] + dvals[4] + dvals[5] + dvals[7] + dvals[8];
                            #endregion

                            #region P8
                            //Current reference pixel: x, y+1

                            //D81
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D82
                            //target pixel: x, y-1
                            dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D83
                            //target pixel: x+1, y-1
                            dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D84
                            //target pixel: x-1, y
                            dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D85
                            //target pixel: x, y
                            dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D86
                            //target pixel: x+1, y
                            dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D87
                            //target pixel: x-1, y+1
                            dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D89
                            //target pixel: x+1, y+1
                            dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                            pvals[7] = dvals[0] + dvals[1] + dvals[2] + dvals[3] + dvals[4] + dvals[5] + dvals[6] + dvals[8];
                            #endregion

                            #region P9
                            //Current reference pixel: x+1, y+1

                            //D91
                            //target pixel: x-1, y-1
                            dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                            //D92
                            //target pixel: x, y-1
                            dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                            //D93
                            //target pixel: x+1, y-1
                            dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                            //D94
                            //target pixel: x-1, y
                            dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                            //D95
                            //target pixel: x, y
                            dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                            //D96
                            //target pixel: x+1, y
                            dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                            //D97
                            //target pixel: x-1, y+1
                            dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                            //D98
                            //target pixel: x, y+1
                            dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                                + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                            pvals[8] = dvals[0] + dvals[1] + dvals[2] + dvals[3] + dvals[4] + dvals[5] + dvals[6] + dvals[7];
                            #endregion

                            menor = 0;

                            for (int idx = 1; idx < 9; idx++)
                            {
                                if (pvals[idx] < pvals[menor])
                                {
                                    menor = idx;
                                }
                            }

                            switch (menor)
                            {
                                case 0:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2];
                                    break;
                                case 1:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2];
                                    break;
                                case 2:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2];
                                    break;
                                case 3:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2];
                                    break;
                                case 4:
                                    break;
                                case 5:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2];
                                    break;
                                case 6:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2];
                                    break;
                                case 7:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2];
                                    break;
                                case 8:
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1];
                                    (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2];
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region Borders
                    #region Horizontal superior
                    for (x = 1, y = 0; x < width - 1; x++)
                    {
                        //P1 = P4, P2 = P5, P3 = P6
                        #region P4
                        //Current reference pixel: x-1, y

                        //D41
                        //target pixel: x-1, y-1
                        dvals[0] = 0;

                        //D42
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D43
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D47
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D48
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D49
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[3] = dvals[0] + dvals[1] * 2 + dvals[2] * 2 + dvals[6] + dvals[7] + dvals[8];
                        pvals[0] = pvals[3];
                        #endregion

                        #region P5
                        //Current reference pixel: x, y

                        //D51
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D52
                        //target pixel: x, y-1
                        dvals[1] = 0;

                        //D53
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D57
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D58
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D59
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[4] = dvals[0] * 2 + dvals[1] + dvals[2] * 2 + dvals[6] + dvals[7] + dvals[8];
                        pvals[1] = pvals[4];
                        #endregion

                        #region P6
                        //Current reference pixel: x+1, y

                        //D61
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D62
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D63
                        //target pixel: x+1, y-1
                        dvals[2] = 0;

                        //D67
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D68
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D69
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[5] = dvals[0] * 2 + dvals[1] * 2 + dvals[2] + dvals[6] + dvals[7] + dvals[8];
                        pvals[2] = pvals[5];
                        #endregion

                        #region P7
                        //Current reference pixel: x-1, y+1

                        //D71
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D72
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D73
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D78
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D79
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[6] = dvals[0] * 2 + dvals[1] * 2 + dvals[2] * 2 + dvals[7] + dvals[8];
                        #endregion

                        #region P8
                        //Current reference pixel: x, y+1

                        //D81
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D82
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D83
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D87
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D89
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[7] = dvals[0] * 2 + dvals[1] * 2 + dvals[2] * 2 + dvals[6] + dvals[8];
                        #endregion

                        #region P9
                        //Current reference pixel: x+1, y+1

                        //D91
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D92
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D93
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D97
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D98
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        pvals[8] = dvals[0] * 2 + dvals[1] * 2 + dvals[2] * 2 + dvals[6] + dvals[7];
                        #endregion

                        menor = 0;

                        for (int idx = 1; idx < 9; idx++)
                        {
                            if (pvals[idx] < pvals[menor])
                            {
                                menor = idx;
                            }
                        }

                        switch (menor)
                        {
                            case 0:
                            case 3:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2];
                                break;
                            case 1:
                            case 4:
                                break;
                            case 2:
                            case 5:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2];
                                break;
                            case 6:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2];
                                break;
                            case 7:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2];
                                break;
                            case 8:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2];
                                break;
                        }
                    }
                    #endregion

                    #region Horizontal inferior
                    for (x = 1, y = height - 1; x < width - 1; x++)
                    {
                        //P7 = P4, P8 = P5, P9 = P6

                        #region P1
                        //Current reference pixel: x-1, y-1

                        //D12
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D13
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D14
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D15
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D16
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        pvals[0] = dvals[1] + dvals[2] + dvals[3] * 2 + dvals[4] * 2 + dvals[5] * 2;
                        #endregion

                        #region P2
                        //Current reference pixel: x, y-1

                        //D21
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D23
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D24
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D25
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D26
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        pvals[1] = dvals[0] + dvals[2] + dvals[3] * 2 + dvals[4] * 2 + dvals[5] * 2;
                        #endregion

                        #region P3
                        //Current reference pixel: x+1, y-1

                        //D31
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D32
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D34
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D35
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D36
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        pvals[2] = dvals[0] + dvals[1] + dvals[3] * 2 + dvals[4] * 2 + dvals[5] * 2;
                        #endregion

                        #region P4
                        //Current reference pixel: x-1, y

                        //D41
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D42
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D43
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D45
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D46
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D47
                        //target pixel: x-1, y+1
                        dvals[6] = 0;

                        pvals[3] = dvals[0] + dvals[1] + dvals[2] + dvals[4] * 2 + dvals[5] * 2 + dvals[6];
                        pvals[6] = pvals[3];
                        #endregion

                        #region P5
                        //Current reference pixel: x, y

                        //D51
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D52
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D53
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D54
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D56
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D58
                        //target pixel: x, y+1
                        dvals[7] = 0;

                        pvals[4] = dvals[0] + dvals[1] + dvals[2] + dvals[3] * 2 + dvals[5] * 2 + dvals[7];
                        pvals[7] = pvals[4];
                        #endregion

                        #region P6
                        //Current reference pixel: x+1, y

                        //D61
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D62
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D63
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D64
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D65
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D69
                        //target pixel: x+1, y+1
                        dvals[8] = 0;

                        pvals[5] = dvals[0] + dvals[1] + dvals[2] + dvals[3] * 2 + dvals[4] * 2 + dvals[8];
                        pvals[8] = pvals[5];
                        #endregion

                        menor = 0;

                        for (int idx = 1; idx < 9; idx++)
                        {
                            if (pvals[idx] < pvals[menor])
                            {
                                menor = idx;
                            }
                        }

                        switch (menor)
                        {
                            case 0:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2];
                                break;
                            case 1:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2];
                                break;
                            case 2:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2];
                                break;
                            case 3:
                            case 6:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2];
                                break;
                            case 4:
                            case 7:
                                break;
                            case 5:
                            case 8:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2];
                                break;
                        }
                    }
                    #endregion

                    #region Vertical esquerda
                    for (x = 0, y = 1; y < height - 1; y++)
                    {
                        //P1 = P2, P4 = P5, P7 = P8

                        #region P2
                        //Current reference pixel: x, y-1

                        //D21
                        //target pixel: x-1, y-1
                        dvals[0] = 0;

                        //D23
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D25
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D26
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D28
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D29
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[1] = dvals[0] + dvals[2] + dvals[4] * 2 + dvals[5] + dvals[7] * 2 + dvals[8];
                        pvals[0] = pvals[1];
                        #endregion

                        #region P3
                        //Current reference pixel: x+1, y-1

                        //D32
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D35
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D36
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D38
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D39
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[2] = dvals[1] * 2 + dvals[4] * 2 + dvals[5] + dvals[7] * 2 + dvals[8];
                        #endregion

                        #region P5
                        //Current reference pixel: x, y

                        //D52
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D53
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D54
                        //target pixel: x-1, y
                        dvals[3] = 0;

                        //D56
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D58
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D59
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[4] = dvals[1] * 2 + dvals[2] + dvals[3] + dvals[5] + dvals[7] * 2 + dvals[8];
                        pvals[3] = pvals[4];
                        #endregion

                        #region P6
                        //Current reference pixel: x+1, y

                        //D62
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D63
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D65
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D68
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        //D69
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[5] = dvals[1] * 2 + dvals[2] + dvals[4] * 2 + dvals[7] * 2 + dvals[8];
                        #endregion

                        #region P8
                        //Current reference pixel: x, y+1

                        //D82
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D83
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D85
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D86
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D87
                        //target pixel: x-1, y+1
                        dvals[6] = 0;

                        //D89
                        //target pixel: x+1, y+1
                        dvals[8] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                        pvals[7] = dvals[1] * 2 + dvals[2] + dvals[4] * 2 + dvals[5] + dvals[6] + dvals[8];
                        pvals[6] = pvals[7];
                        #endregion

                        #region P9
                        //Current reference pixel: x+1, y+1

                        //D92
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D93
                        //target pixel: x+1, y-1
                        dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                        //D95
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D96
                        //target pixel: x+1, y
                        dvals[5] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                        //D98
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        pvals[8] = dvals[1] * 2 + dvals[2] + dvals[4] * 2 + dvals[5] + dvals[7] * 2;
                        #endregion

                        menor = 0;

                        for (int idx = 1; idx < 9; idx++)
                        {
                            if (pvals[idx] < pvals[menor])
                            {
                                menor = idx;
                            }
                        }

                        switch (menor)
                        {
                            case 0:
                            case 1:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2];
                                break;
                            case 2:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2];
                                break;
                            case 3:
                            case 4:
                                break;
                            case 5:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2];
                                break;
                            case 6:
                            case 7:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2];
                                break;
                            case 8:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2];
                                break;
                        }
                    }
                    #endregion

                    #region Vertical direita
                    for (x = width - 1, y = 1; y < height - 1; y++)
                    {
                        //P3 = P2, P6 = P5, P9 = P8

                        #region P1
                        //Current reference pixel: x-1, y-1

                        //D12
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D14
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D15
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D17
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D18
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        pvals[0] = dvals[1] * 2 + dvals[3] + dvals[4] * 2 + dvals[6] + dvals[7] * 2;
                        #endregion

                        #region P2
                        //Current reference pixel: x, y-1

                        //D21
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D23
                        //target pixel: x+1, y-1
                        dvals[2] = 0;

                        //D24
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D25
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D27
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D28
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        pvals[1] = dvals[0] + dvals[2] + dvals[3] + dvals[4] * 2 + dvals[6] + dvals[7] * 2;
                        pvals[2] = pvals[1];
                        #endregion

                        #region P4
                        //Current reference pixel: x-1, y

                        //D41
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D42
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D45
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D47
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D48
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        pvals[3] = dvals[0] + dvals[1] * 2 + dvals[4] * 2 + dvals[6] + dvals[7] * 2;
                        #endregion

                        #region P5
                        //Current reference pixel: x, y

                        //D51
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D52
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D54
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D56
                        //target pixel: x+1, y
                        dvals[5] = 0;

                        //D57
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D58
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        pvals[4] = dvals[0] + dvals[1] * 2 + dvals[3] + dvals[5] + dvals[6] + dvals[7] * 2;
                        pvals[5] = pvals[4];
                        #endregion

                        #region P7
                        //Current reference pixel: x-1, y+1

                        //D71
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D72
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D74
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D75
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D78
                        //target pixel: x, y+1
                        dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                        pvals[6] = dvals[0] + dvals[1] * 2 + dvals[3] + dvals[4] * 2 + dvals[7] * 2;
                        #endregion

                        #region P8
                        //Current reference pixel: x, y+1

                        //D81
                        //target pixel: x-1, y-1
                        dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                        //D82
                        //target pixel: x, y-1
                        dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                        //D84
                        //target pixel: x-1, y
                        dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                        //D85
                        //target pixel: x, y
                        dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                        //D87
                        //target pixel: x-1, y+1
                        dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                            + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                        //D89
                        //target pixel: x+1, y+1
                        dvals[8] = 0;

                        pvals[7] = dvals[0] + dvals[1] * 2 + dvals[3] + dvals[4] * 2 + dvals[6] + dvals[8];
                        pvals[8] = pvals[7];
                        #endregion

                        menor = 0;

                        for (int idx = 1; idx < 9; idx++)
                        {
                            if (pvals[idx] < pvals[menor])
                            {
                                menor = idx;
                            }
                        }

                        switch (menor)
                        {
                            case 0:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2];
                                break;
                            case 1:
                            case 2:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2];
                                break;
                            case 3:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2];
                                break;
                            case 4:
                            case 5:
                                break;
                            case 6:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2];
                                break;
                            case 7:
                            case 8:
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1];
                                (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2];
                                break;
                        }
                    }
                    #endregion
                    #endregion

                    #region Corners

                    #region canto superior esquerdo
                    x = 0; y = 0;
                    //P1 = P5, P2 = P5, P3 = P6, P4 = P5, P7 = P8

                    #region P5
                    //Current reference pixel: x, y

                    //D51
                    //target pixel: x-1, y-1
                    int d51 = 0;

                    //D52
                    //target pixel: x, y-1
                    int d52 = 0;

                    //D54
                    //target pixel: x-1, y
                    int d54 = 0;

                    //D56
                    //target pixel: x+1, y
                    int d56 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                    //D58
                    //target pixel: x, y+1
                    int d58 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                    //D59
                    //target pixel: x+1, y+1
                    int d59 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                    pvals[4] = d51 + d52 + d54 + d56 * 2 + d58 * 2 + d59;
                    pvals[0] = pvals[4];
                    pvals[1] = pvals[4];
                    pvals[3] = pvals[4];
                    #endregion

                    #region P6
                    //Current reference pixel: x+1, y

                    //D63
                    //target pixel: x+1, y-1
                    int d63 = 0;

                    //D65
                    //target pixel: x, y
                    int d65 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D68
                    //target pixel: x, y+1
                    int d68 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                    //D69
                    //target pixel: x+1, y+1
                    int d69 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                    pvals[5] = d63 + d65 * 4 + d68 * 2 + d69;
                    pvals[2] = pvals[5];
                    #endregion

                    #region P8
                    //Current reference pixel: x, y+1

                    //D85
                    //target pixel: x, y
                    int d85 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D86
                    //target pixel: x+1, y
                    int d86 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                    //D87
                    //target pixel: x-1, y+1
                    int d87 = 0;

                    //D89
                    //target pixel: x+1, y+1
                    int d89 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2]);

                    pvals[7] = d85 * 4 + d86 * 2 + d87 + d89;
                    pvals[6] = pvals[7];
                    #endregion

                    #region P9
                    //Current reference pixel: x+1, y+1

                    //D95
                    //target pixel: x, y
                    int d95 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D96
                    //target pixel: x+1, y
                    int d96 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                    //D98
                    //target pixel: x, y+1
                    int d98 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                    pvals[8] = d95 * 4 + d96 * 2 + d98 * 2;
                    #endregion

                    menor = 0;

                    for (int idx = 1; idx < 9; idx++)
                    {
                        if (pvals[idx] < pvals[menor])
                        {
                            menor = idx;
                        }
                    }

                    switch (menor)
                    {
                        case 0:
                        case 1:
                        case 3:
                        case 4:
                            break;
                        case 2:
                        case 5:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2];
                            break;
                        case 6:
                        case 7:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2];
                            break;
                        case 8:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2];
                            break;
                    }
                    #endregion

                    #region canto inferior esquerdo
                    x = 0; y = height - 1;
                    //P1 = P2, P4 = P5, P7 = P5, P8 = P5, P9 = P6

                    #region P2
                    //Current reference pixel: x, y-1

                    //D21
                    //target pixel: x-1, y-1
                    int d21 = 0;

                    //D23
                    //target pixel: x+1, y-1
                    int d23 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                    //D25
                    //target pixel: x, y
                    int d25 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D26
                    //target pixel: x+1, y
                    int d26 = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                    pvals[1] = d21 + d23 + d25 * 4 + d26 * 2;
                    pvals[0] = pvals[1];
                    #endregion

                    #region P3
                    //Current reference pixel: x+1, y-1

                    //D32
                    //target pixel: x, y-1
                    int d32 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                    //D35
                    //target pixel: x, y
                    int d35 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D36
                    //target pixel: x+1, y
                    int d36 = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                    pvals[2] = d32 * 2 + d35 * 4 + d36 * 2;
                    #endregion

                    #region P5
                    //Current reference pixel: x, y

                    //D52
                    //target pixel: x, y-1
                    dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                    //D53
                    //target pixel: x+1, y-1
                    dvals[2] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                    //D54
                    //target pixel: x-1, y
                    dvals[3] = 0;

                    //D56
                    //target pixel: x+1, y
                    dvals[5] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2]);

                    //D57
                    //target pixel: x-1, y+1
                    dvals[6] = 0;

                    //D58
                    //target pixel: x, y+1
                    dvals[7] = 0;

                    pvals[4] = dvals[1] * 2 + dvals[2] + dvals[3] + dvals[5] * 2 + dvals[6] + dvals[7];
                    pvals[3] = pvals[4];
                    pvals[6] = pvals[4];
                    pvals[7] = pvals[4];
                    #endregion

                    #region P6
                    //Current reference pixel: x+1, y

                    //D62
                    //target pixel: x, y-1
                    dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                    //D63
                    //target pixel: x+1, y-1
                    dvals[2] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2]);

                    //D65
                    //target pixel: x, y
                    dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D69
                    //target pixel: x+1, y+1
                    dvals[8] = 0;

                    pvals[5] = dvals[1] * 2 + dvals[2] + dvals[4] * 4 + dvals[8];
                    pvals[8] = pvals[5];
                    #endregion

                    menor = 0;

                    for (int idx = 1; idx < 9; idx++)
                    {
                        if (pvals[idx] < pvals[menor])
                        {
                            menor = idx;
                        }
                    }

                    switch (menor)
                    {
                        case 0:
                        case 1:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2];
                            break;
                        case 2:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * (y - 1))[2];
                            break;
                        case 4:
                        case 3:
                        case 6:
                        case 7:
                            break;
                        case 5:
                        case 8:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x + 1) + stepRead * y)[2];
                            break;
                    }
                    #endregion

                    #region canto superior direito
                    x = width - 1; y = 0;
                    //P1 = P4, P2 = P5, P3 = P5, P6 = P5, P9 = P8

                    #region P4
                    //Current reference pixel: x-1, y

                    //D41
                    //target pixel: x-1, y-1
                    dvals[0] = 0;

                    //D45
                    //target pixel: x, y
                    dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D47
                    //target pixel: x-1, y+1
                    dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                    //D48
                    //target pixel: x, y+1
                    dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                    pvals[3] = dvals[0] + dvals[4] * 4 + dvals[6] + dvals[7] * 2;
                    pvals[0] = pvals[3];
                    #endregion

                    #region P5
                    //Current reference pixel: x, y

                    //D52
                    //target pixel: x, y-1
                    dvals[1] = 0;

                    //D53
                    //target pixel: x+1, y-1
                    dvals[2] = 0;

                    //D54
                    //target pixel: x-1, y
                    dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                    //D56
                    //target pixel: x+1, y
                    dvals[5] = 0;

                    //D57
                    //target pixel: x-1, y+1
                    dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                    //D58
                    //target pixel: x, y+1
                    dvals[7] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                    pvals[4] = dvals[1] + dvals[2] + dvals[3] * 2 + dvals[5] + dvals[6] + dvals[7] * 2;
                    pvals[1] = pvals[4];
                    pvals[2] = pvals[4];
                    pvals[5] = pvals[4];
                    #endregion

                    #region P7
                    //Current reference pixel: x-1, y+1

                    //D74
                    //target pixel: x-1, y
                    dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                    //D75
                    //target pixel: x, y
                    dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D78
                    //target pixel: x, y+1
                    dvals[7] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2]);

                    pvals[6] = dvals[3] * 2 + dvals[4] * 4 + dvals[7] * 2;
                    #endregion

                    #region P9
                    //Current reference pixel: x+1, y+1

                    //D94
                    //target pixel: x-1, y
                    dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                    //D95
                    //target pixel: x, y
                    dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D97
                    //target pixel: x-1, y+1
                    dvals[6] = Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x + 1) + stepRead * (y + 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                    //D98
                    //target pixel: x, y+1
                    dvals[7] = 0;

                    pvals[8] = dvals[3] * 2 + dvals[4] * 4 + dvals[6] + dvals[7];
                    pvals[8] = pvals[7];
                    #endregion

                    menor = 0;

                    for (int idx = 1; idx < 9; idx++)
                    {
                        if (pvals[idx] < pvals[menor])
                        {
                            menor = idx;
                        }
                    }

                    switch (menor)
                    {
                        case 3:
                        case 0:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2];
                            break;
                        case 4:
                        case 1:
                        case 2:
                        case 5:
                            break;
                        case 6:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2];
                            break;
                        case 7:
                        case 8:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y + 1))[2];
                            break;
                    }
                    #endregion

                    #region canto inferior direito
                    x = width - 1; y = height - 1;
                    //P3 = P2, P6 = P5, P7 = P4, P8= P5, P9 = P5

                    #region P1
                    //Current reference pixel: x-1, y-1

                    //D12
                    //target pixel: x, y-1
                    dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                    //D14
                    //target pixel: x-1, y
                    dvals[3] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                    //D15
                    //target pixel: x, y
                    dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    pvals[0] = dvals[1] * 2 + dvals[3] * 2 + dvals[4] * 4;
                    #endregion

                    #region P2
                    //Current reference pixel: x, y-1

                    //D21
                    //target pixel: x-1, y-1
                    dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                    //D23
                    //target pixel: x+1, y-1
                    dvals[2] = 0;

                    //D24
                    //target pixel: x-1, y
                    dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                    //D25
                    //target pixel: x, y
                    dvals[4] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * (y - 1))[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    pvals[1] = dvals[0] + dvals[2] + dvals[3] * 2 + dvals[4] * 4;
                    pvals[2] = pvals[1];
                    #endregion

                    #region P4
                    //Current reference pixel: x-1, y

                    //D41
                    //target pixel: x-1, y-1
                    dvals[0] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                    //D42
                    //target pixel: x, y-1
                    dvals[1] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                    //D45
                    //target pixel: x, y
                    dvals[4] = Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * y)[2]);

                    //D47
                    //target pixel: x-1, y+1
                    dvals[6] = 0;

                    pvals[3] = dvals[0] + dvals[1] * 2 + dvals[4] * 4 + dvals[6];
                    pvals[6] = pvals[3];
                    #endregion

                    #region P5
                    //Current reference pixel: x, y

                    //D51
                    //target pixel: x-1, y-1
                    dvals[0] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2]);

                    //D52
                    //target pixel: x, y-1
                    dvals[1] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2]);

                    //D54
                    //target pixel: x-1, y
                    dvals[3] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2]);

                    //D56
                    //target pixel: x+1, y
                    dvals[5] = 0;

                    //D57
                    //target pixel: x-1, y+1
                    dvals[6] = Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[0] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[0])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[1] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[1])
                        + Math.Abs((dataPtrRead + nChanRead * x + stepRead * y)[2] - (dataPtrRead + nChanRead * (x - 1) + stepRead * (y + 1))[2]);

                    //D58
                    //target pixel: x, y+1
                    dvals[7] = 0;

                    //D59
                    //target pixel: x+1, y+1
                    dvals[8] = 0;

                    pvals[4] = dvals[0] + dvals[1] * 2 + dvals[3] * 2 + dvals[5] + dvals[6] + dvals[7] + dvals[8];
                    pvals[5] = pvals[4];
                    pvals[7] = pvals[4];
                    pvals[8] = pvals[4];
                    #endregion

                    menor = 0;

                    for (int idx = 1; idx < 9; idx++)
                    {
                        if (pvals[idx] < pvals[menor])
                        {
                            menor = idx;
                        }
                    }

                    switch (menor)
                    {
                        case 0:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * (y - 1))[2];
                            break;
                        case 1:
                        case 2:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * x + stepRead * (y - 1))[2];
                            break;
                        case 3:
                        case 6:
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[0] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[0];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[1] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[1];
                            (dataPtrWrite + nChanWrite * x + stepWrite * y)[2] = (dataPtrRead + nChanRead * (x - 1) + stepRead * y)[2];
                            break;
                        case 4:
                        case 5:
                        case 7:
                        case 8:
                            break;
                    }
                    #endregion
                    #endregion
                }
            }

            /*
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int[] dist1 = new int[9], dist2 = new int[9], dist3 = new int[9],
                      dist4 = new int[9], dist5 = new int[9], dist6 = new int[9],
                      dist7 = new int[9], dist8 = new int[9], dist9 = new int[9],
                      distSum = new int[9];

                ///somar as distancias ao array 
                
                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {

                            //distancios para o primeiro pixel
                            dist1[0] = 0; 
                            dist1[1] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2]);
                            dist1[2] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2]);
                            dist1[3] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist1[4] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + x * m.nChannels)[2]);
                            dist1[5] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2]);
                            dist1[6] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist1[7] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2]);
                            dist1[8] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o segundo pixel
                            dist2[0] = dist1[1];
                            dist2[1] = 0;
                            dist2[2] = Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2]);
                            dist2[3] = Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist2[4] = Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + y * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + y * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + y * m.widthStep + x * m.nChannels)[2]);
                            dist2[5] = Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2]);
                            dist2[6] = Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist2[7] = Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2]);
                            dist2[8] = Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o terceiro pixel
                            dist3[0] = dist1[2];
                            dist3[1] = dist2[2];
                            dist3[2] = 0;
                            dist3[3] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist3[4] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + x * m.nChannels)[2]);
                            dist3[5] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2]);
                            dist3[6] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist3[7] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2]);
                            dist3[8] = Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o quarto pixel
                            dist4[0] = dist1[3];
                            dist4[1] = dist2[3];
                            dist4[2] = dist3[3];
                            dist4[3] = 0;
                            dist4[4] = Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + x * m.nChannels)[2]);
                            dist4[5] = Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2]);
                            dist4[6] = Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist4[7] = Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2]);
                            dist4[8] = Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o quinto pixel
                            dist5[0] = dist1[4];
                            dist5[1] = dist2[4];
                            dist5[2] = dist3[4];
                            dist5[3] = dist4[4];
                            dist5[4] = 0;
                            dist5[5] = Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[0] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[1] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[2] - (dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2]);
                            dist5[6] = Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist5[7] = Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2]);
                            dist5[8] = Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o sexto pixel
                            dist6[0] = dist1[5];
                            dist6[1] = dist2[5];
                            dist6[2] = dist3[5];
                            dist6[3] = dist4[5];
                            dist6[4] = dist5[5];
                            dist6[5] = 0;
                            dist6[6] = Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2]);
                            dist6[7] = Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2]);
                            dist6[8] = Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + y * m.widthStep + (x + 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o setimo pixel
                            dist7[0] = dist1[6];
                            dist7[1] = dist2[6];
                            dist7[2] = dist3[6];
                            dist7[3] = dist4[6];
                            dist7[4] = dist5[6];
                            dist7[5] = dist6[6];
                            dist7[6] = 0;
                            dist7[7] = Math.Abs((dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2]);
                            dist7[8] = Math.Abs((dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o oitavo pixel
                            dist8[0] = dist1[7];
                            dist8[1] = dist2[7];
                            dist8[2] = dist3[7];
                            dist8[3] = dist4[7];
                            dist8[4] = dist5[7];
                            dist8[5] = dist6[7];
                            dist8[6] = dist7[7];
                            dist8[7] = 0;
                            dist8[8] = Math.Abs((dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[0] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0]) +
                                       Math.Abs((dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[1] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1]) +
                                       Math.Abs((dataPtr + (y + 1) * m.widthStep + x * m.nChannels)[2] - (dataPtr + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2]);

                            //distancias para o nono pixel
                            dist9[0] = dist1[8];
                            dist9[1] = dist2[8];
                            dist9[2] = dist3[8];
                            dist9[3] = dist4[8];
                            dist9[4] = dist5[8];
                            dist9[5] = dist6[8];
                            dist9[6] = dist7[8];
                            dist9[7] = dist8[8];
                            dist9[8] = 0;

                            //somar os arrays
                            distSum[0] = dist1.Sum();
                            distSum[1] = dist2.Sum();
                            distSum[2] = dist3.Sum();
                            distSum[3] = dist4.Sum();
                            distSum[4] = dist5.Sum();
                            distSum[5] = dist6.Sum();
                            distSum[6] = dist7.Sum();
                            distSum[7] = dist8.Sum();
                            distSum[8] = dist9.Sum();

                            //escolher o array com a menor soma
                            int sumMenor = 0;//atribuir o valor do primeiro array
                            for (int i = 1; i < 9; i++)
                            { 
                                //caso a soma seja maior que a anterior
                                if (distSum[sumMenor] > distSum[i])
                                {
                                    sumMenor = i;
                                }
                            }

                            //ir buscar o pixel em calculo, por camadas
                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            //escolher o pixel a usar
                            switch (sumMenor)
                            {
                                case 0:
                                    *blue = (byte)(dataPtr2 + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + (y - 1) * m.widthStep + (x - 1) * m.nChannels)[2];
                                    break;
                                case 1:
                                    *blue = (byte)(dataPtr2 + (y - 1) * m.widthStep + x * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + (y - 1) * m.widthStep + x * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + (y - 1) * m.widthStep + x * m.nChannels)[2];
                                    break;
                                case 2:
                                    *blue = (byte)(dataPtr2 + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + (y - 1) * m.widthStep + (x + 1) * m.nChannels)[2];
                                    break;
                                case 3:
                                    *blue = (byte)(dataPtr2 + y * m.widthStep + (x - 1) * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + y * m.widthStep + (x - 1) * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + y * m.widthStep + (x - 1) * m.nChannels)[2];
                                    break;
                                case 4:
                                    *blue = (byte)(dataPtr2 + y * m.widthStep + x * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + y * m.widthStep + x * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + y * m.widthStep + x * m.nChannels)[2];
                                    break;
                                case 5:
                                    *blue = (byte)(dataPtr2 + y * m.widthStep + (x + 1) * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + y * m.widthStep + (x + 1) * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + y * m.widthStep + (x + 1) * m.nChannels)[2];
                                    break;
                                case 6:
                                    *blue = (byte)(dataPtr2 + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + (y + 1) * m.widthStep + (x - 1) * m.nChannels)[2];
                                    break;
                                case 7:
                                    *blue = (byte)(dataPtr2 + (y + 1) * m.widthStep + x * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + (y + 1) * m.widthStep + x * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + (y + 1) * m.widthStep + x * m.nChannels)[2];
                                    break;
                                case 8:
                                    *blue = (byte)(dataPtr2 + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[0];
                                    *green = (byte)(dataPtr2 + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[1];
                                    *red = (byte)(dataPtr2 + (y + 1) * m.widthStep + (x + 1) * m.nChannels)[2];
                                    break;
                            }
                        }
                    }
         
                    //bordas da imagem
                    //borda esquerda
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix1[0, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix1[0, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix1[0, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix1[1, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix1[1, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix1[1, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[0] * matrix1[2, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[1] * matrix1[2, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[2] * matrix1[2, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix2[0, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix2[0, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix2[0, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[0] * matrix2[1, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[1] * matrix2[1, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 0 * nChan)[2] * matrix2[1, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[0] * matrix2[2, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[1] * matrix2[2, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + 1 * nChan)[2] * matrix2[2, yTemp + 1]);
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + 0 * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda cima
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 0]);
                            greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 0]);
                            redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 0]);

                            blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 2]);
                            greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 2]);
                            redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 2]);

                            blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 0]);
                            greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 0]);
                            redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 0]);

                            blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 2]);
                            greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 2]);
                            redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 2]);
                        }

                        blue = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + 0 * m.widthStep + x * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda direita
                    for (y = 1; y < height - 1; y++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (yTemp = -1; yTemp <= 1; yTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix1[0, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix1[0, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix1[0, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, yTemp + 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[0] * matrix1[2, yTemp + 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[1] * matrix1[2, yTemp + 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[2] * matrix1[2, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix2[0, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix2[0, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix2[0, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, yTemp + 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[0] * matrix2[2, yTemp + 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[1] * matrix2[2, yTemp + 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (y + yTemp) * m.widthStep + (width - 2) * nChan)[2] * matrix2[2, yTemp + 1]);
                        }

                        blue = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda baixo
                    for (x = 1; x < width - 1; x++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        for (xTemp = -1; xTemp <= 1; xTemp++)
                        {
                            blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 0]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 0]);
                            redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 0]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 1]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 1]);
                            redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 1]);

                            blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[0] * matrix1[xTemp + 1, 2]);
                            greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[1] * matrix1[xTemp + 1, 2]);
                            redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[2] * matrix1[xTemp + 1, 2]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 0]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 0]);
                            redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 0]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 1]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 1]);
                            redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 1]);

                            blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[0] * matrix2[xTemp + 1, 2]);
                            greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[1] * matrix2[xTemp + 1, 2]);
                            redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (x + xTemp) * nChan)[2] * matrix2[xTemp + 1, 2]);
                        }

                        blue = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //cantos
                    //canto superior direito
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 0]);

                    //margem duplicada superior
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 1]);

                    //margem duplicada direita
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 2]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 2]);


                    blue = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + (width - 1) * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;

                    //canto superior esquerdo
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[0, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[0, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[1, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix1[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 0 * nChan)[2] * matrix2[1, 1]);

                    //margem duplicada superior
                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix1[2, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix1[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix2[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + 0 * m.widthStep + 1 * nChan)[2] * matrix2[2, 1]);

                    //margem duplicada esquerda
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix1[0, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix1[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix2[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 0 * nChan)[2] * matrix2[1, 2]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + 1 * m.widthStep + 1 * nChan)[2] * matrix2[2, 2]);


                    blue = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + 0 * m.widthStep + 0 * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;

                    //canto inferior esquerdo
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[1, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[0, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[1, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix1[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 0 * nChan)[2] * matrix2[0, 2]);

                    //margem duplicada inferior
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix1[2, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix2[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + 1 * nChan)[2] * matrix2[2, 2]);

                    //margem duplicada esquerda
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix1[0, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix1[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix2[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 0 * nChan)[2] * matrix2[1, 0]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[2] * matrix1[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + 1 * nChan)[2] * matrix2[2, 0]);


                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + 0 * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;

                    //canto inferior direito
                    blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                    //canto quadruplicado
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 2]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 2]);

                    //margem duplicada inferior
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 1]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 1]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 1]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 2]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 2]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 2]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 1]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 1]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 1]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 2]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 2]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 1) * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 2]);

                    //margem duplicada direita
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix1[1, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix1[1, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix1[1, 0]);

                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix1[2, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix1[2, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix1[2, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix2[1, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix2[1, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix2[1, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[0] * matrix2[2, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[1] * matrix2[2, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 1) * nChan)[2] * matrix2[2, 0]);

                    //restante
                    blueSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[0] * matrix1[0, 0]);
                    greenSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[1] * matrix1[0, 0]);
                    redSum1 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[2] * matrix1[0, 0]);

                    blueSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[0] * matrix2[0, 0]);
                    greenSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[1] * matrix2[0, 0]);
                    redSum2 += (int)Math.Round((dataPtr2 + (height - 2) * m.widthStep + (width - 2) * nChan)[2] * matrix2[0, 0]);


                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 2);

                    blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                    greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                    redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;
                }
            }*/
        }

        /// <summary>
        /// Function that solves the puzzle
        /// </summary>
        /// <param name="img">Input/Output image</param>
        /// <param name="imgCopy">Image Copy</param>
        /// <param name="Pieces_positions">List of positions (Left-x,Top-y,Right-x,Bottom-y) of all detected pieces</param>
        /// <param name="Pieces_angle">List of detected pieces' angles</param>
        /// <param name="level">Level of image</param>
        public static void puzzle(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, out List<int[]> Pieces_positions, out List<int> Pieces_angle, int level)
        {

            unsafe
            {
                //Dummy
                Pieces_positions = new List<int[]>();
                int[] piece_vector = new int[4];

                piece_vector[0] = 65;   // x- Top-Left 
                piece_vector[1] = 385;  // y- Top-Left
                piece_vector[2] = 1089; // x- Bottom-Right
                piece_vector[3] = 1411; // y- Bottom-Right

                Pieces_positions.Add(piece_vector);

                Pieces_angle = new List<int>();
                Pieces_angle.Add(0); // angle
                //

                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte blueFundo, redFundo, greenFundo;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                //cor do pixel do fundo
                blueFundo = (byte)(dataPtr + 0 * m.widthStep + 0 * nChan + 0);
                greenFundo = (byte)(dataPtr + 0 * m.widthStep + 0 * nChan + 1);
                redFundo = (byte)(dataPtr + 0 * m.widthStep + 0 * nChan + 2);

                //binarização da imagem
                /*for(y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        
                    }
                }*/

            }
                //binarizaçao para distinguir o fundo das imagens
                //algoritmo de componentes ligados
        }

        /// <summary>
        /// Converto to BW
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToBW(Emgu.CV.Image<Bgr, byte> img, int threshold)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, red, green, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)Math.Round((blue + green + red) / 3.0);

                            if (gray <= threshold)
                            {
                                dataPtr[0] = 0;
                                dataPtr[1] = 0;
                                dataPtr[2] = 0;
                            }
                            else
                            {
                                dataPtr[0] = 255;
                                dataPtr[1] = 255;
                                dataPtr[2] = 255;
                            }

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Converto to BW - Otsu
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToBW_Otsu(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                double width = img.Width;
                double height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                double q1, q2, u1, u2, total = width * height, sigma = -1, sigmaTemp, threshold = -1;
                int[] histogramaGray = Histogram_Gray(img);

                for (int t = 0; t < 256; t++)
                {
                    q1 = 0;
                    q2 = 0;
                    u1 = 0;
                    u2 = 0;

                    for (int i = 0; i <= t; i++)
                    {
                        q1 += histogramaGray[i] / total;
                        u1 += i * (histogramaGray[i] / total);
                    }

                    for (int j = t + 1; j < 256; j++)
                    {
                        q2 += histogramaGray[j] / total;
                        u2 += j * (histogramaGray[j] / total);
                    }

                    if (q1 == 0)
                    {
                        u1 = 0;
                    }
                    else
                    {
                        u1 = u1 / q1;
                    }
                    if (q2 == 0)
                    {
                        u2 = 0;
                    }
                    else
                    {
                        u2 = u2 / q2;
                    }

                    sigmaTemp = (q1 * q2 * Math.Pow(u1 - u2,2));

                    if (sigmaTemp > sigma)
                    {
                        sigma = sigmaTemp;
                        threshold = t;
                    }
                }
                ConvertToBW(img, (int)threshold);
            }
        }

        /// <summary>
        /// Histogram_Gray
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static int[] Histogram_Gray(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, red, green, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int[] histogramaGray = new int[256];

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)Math.Round((blue + green + red) / 3.0);

                            histogramaGray[gray]++;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
                return histogramaGray;
            }
        }

        /// <summary>
        /// Histogram_RGB
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static int[,] Histogram_RGB(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int[,] histogramaRGB = new int[3,256];

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            histogramaRGB[0,blue]++;
                            histogramaRGB[1, green]++;
                            histogramaRGB[2, red]++;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
                return histogramaRGB;
            }
        }

        /// <summary>
        /// Histogram_RGBG
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static int[,] Histogram_All(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                byte blue, red, green, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                int[,] histogramaRGBG = new int[4, 256];

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            gray = (byte)Math.Round((blue + green + red) / 3.0);

                            histogramaRGBG[0, gray]++;
                            histogramaRGBG[1, blue]++;
                            histogramaRGBG[2, green]++;
                            histogramaRGBG[3, red]++;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
                return histogramaRGBG;
            }
        }

        /// <summary>
        /// mean - solution B (3x3)
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Mean_solutionB(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {



        }

        /// <summary>
        /// mean - solution C (7x7)
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Mean_solutionC(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int size)
        {



        }

        /// <summary>
        /// Roberts
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y, blueSum1 = 0, greenSum1 = 0, redSum1 = 0, blueSum2 = 0, greenSum2 = 0, redSum2 = 0;

                if (nChan == 3) // image in RGB
                {
                    //meio da imagem
                    for (y = 0; y < height - 1; y++)
                    {
                        for (x = 0; x < width - 1; x++)
                        {
                            blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                            blueSum1 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[0];
                            greenSum1 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[1];
                            redSum1 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 0) * nChan)[2];

                            blueSum1 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 1) * nChan)[0];
                            greenSum1 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 1) * nChan)[1];
                            redSum1 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 1) * nChan)[2];

                            blueSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 1) * nChan)[0];
                            greenSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 1) * nChan)[1];
                            redSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (x + 1) * nChan)[2];

                            blueSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 0) * nChan)[0];
                            greenSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 0) * nChan)[1];
                            redSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (x + 0) * nChan)[2];

                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1) + Math.Abs(blueSum2))));
                            greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1) + Math.Abs(greenSum2))));
                            redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1) + Math.Abs(redSum2))));

                            *blue = (byte)blueSum1;
                            *green = (byte)greenSum1;
                            *red = (byte)redSum1;
                        }
                    }

                    //bordas da imagem
                    //borda direita
                    for (y = 0; y < height - 1; y++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        blueSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (width - 1) * nChan)[0];
                        greenSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (width - 1) * nChan)[1];
                        redSum2 += (int)(dataPtr2 + (y + 0) * m.widthStep + (width - 1) * nChan)[2];

                        blueSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (width - 1) * nChan)[0];
                        greenSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (width - 1) * nChan)[1];
                        redSum2 -= (int)(dataPtr2 + (y + 1) * m.widthStep + (width - 1) * nChan)[2];

                        blue = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 0);
                        green = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 1);
                        red = (byte*)(dataPtr + y * m.widthStep + (width - 1) * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum2)) + (Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum2)) + (Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum2)) + (Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //borda baixo
                    for (x = 0; x < width - 1; x++)
                    {
                        blueSum1 = greenSum1 = redSum1 = blueSum2 = greenSum2 = redSum2 = 0;

                        blueSum1 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[0];
                        greenSum1 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[1];
                        redSum1 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[2];

                        blueSum1 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[0];
                        greenSum1 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[1];
                        redSum1 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[2];

                        blueSum2 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[0];
                        greenSum2 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[1];
                        redSum2 += (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 1) * nChan)[2];

                        blueSum2 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[0];
                        greenSum2 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[1];
                        redSum2 -= (int)(dataPtr2 + (height - 1) * m.widthStep + (x + 0) * nChan)[2];

                        blue = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 0);
                        green = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 1);
                        red = (byte*)(dataPtr + (height - 1) * m.widthStep + x * nChan + 2);

                        blueSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(blueSum1)) + (Math.Abs(blueSum2))));
                        greenSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(greenSum1)) + (Math.Abs(greenSum2))));
                        redSum1 = (int)Math.Max(0, Math.Min(255, (Math.Abs(redSum1)) + (Math.Abs(redSum2))));

                        *blue = (byte)blueSum1;
                        *green = (byte)greenSum1;
                        *red = (byte)redSum1;
                    }

                    //cantos
                    //canto inferior direito
                    blueSum1 = greenSum1 = redSum1 = 0;

                    blue = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 0);
                    green = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 1);
                    red = (byte*)(dataPtr + (height - 1) * m.widthStep + (width - 1) * nChan + 2);

                    *blue = (byte)blueSum1;
                    *green = (byte)greenSum1;
                    *red = (byte)redSum1;
                }
            }
        }

        /// <summary>
        /// rotation - bilinear interpolation
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Rotation_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Pointer to the image

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;
                float xF, yF, xDiff, yDiff;
                int xFloor, yFloor, xCeil, yCeil;
                //Pixeis de cada ponta
                byte* pixelTL, pixelTR, pixelBL, pixelBR;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xF = (x - (width / 2.0f)) * (float)Math.Cos(angle) - ((height / 2.0f) - y) * (float)Math.Sin(angle) + (width / 2.0f); //Realiza as contas usadas para calcular o offset de x
                            yF = (height / 2.0f) - ((x - (width / 2.0f)) * (float)Math.Sin(angle)) - ((height / 2.0f) - y) * (float)Math.Cos(angle); //Realiza as contas usadas para calcular o offset de x

                            xFloor = (int)Math.Floor(xF); // Realiza arredondamentos
                            yFloor = (int)Math.Floor(yF); // Realiza arredondamentos
                            xCeil = (int)Math.Ceiling(xF); // Realiza arredondamentos
                            yCeil = (int)Math.Ceiling(yF); // Realiza arredondamentos


                            //Caso esteja fora, realizar a duplicaçãao de margens
                            if (xFloor < 0 || yFloor < 0 || xCeil >= m.width || yCeil >= m.height)
                            {
                                // Duplicate margin
                                xFloor = Math.Max(0, Math.Min(xFloor, m.width - 1));
                                yFloor = Math.Max(0, Math.Min(yFloor, m.height - 1));
                                xCeil = Math.Max(0, Math.Min(xCeil, m.width - 1));
                                yCeil = Math.Max(0, Math.Min(yCeil, m.height - 1));
                            }

                            //Faz o offset
                            xDiff = xF - xFloor;
                            yDiff = yF - yFloor;

                            //Calcula o valor dos mesmos
                            pixelTL = (dataPtr2 + yFloor * m.widthStep + xFloor * nChan);
                            pixelTR = (dataPtr2 + yFloor * m.widthStep + xCeil * nChan);
                            pixelBL = (dataPtr2 + yCeil * m.widthStep + xFloor * nChan);
                            pixelBR = (dataPtr2 + yCeil * m.widthStep + xCeil * nChan);


                            //Aplicada a rotação
                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);


                            *blue = (byte)((1 - xDiff) * (1 - yDiff) * pixelTL[0] +
                                           xDiff * (1 - yDiff) * pixelTR[0] +
                                           (1 - xDiff) * yDiff * pixelBL[0] +
                                           xDiff * yDiff * pixelBR[0]);

                            *green = (byte)((1 - xDiff) * (1 - yDiff) * pixelTL[1] +
                                            xDiff * (1 - yDiff) * pixelTR[1] +
                                            (1 - xDiff) * yDiff * pixelBL[1] +
                                            xDiff * yDiff * pixelBR[1]);

                            *red = (byte)((1 - xDiff) * (1 - yDiff) * pixelTL[2] +
                                          xDiff * (1 - yDiff) * pixelTR[2] +
                                          (1 - xDiff) * yDiff * pixelBL[2] +
                                          xDiff * yDiff * pixelBR[2]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// scale - bilinear interpolation
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Scale_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                // Obtem os ponteiros para os dados das imagens
                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Ponteiro para a imagem
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Ponteiro para a imagem

                byte* blue, red, green;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // número de canais - 3
                int padding = m.widthStep - m.nChannels * m.width; // bytes de alinhamento (padding)
                int x, y, xTemp, yTemp;

                if (nChan == 3) // imagem em RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            // Calcula as coordenadas escaladas na imagem img2
                            float xScaled = x / scaleFactor;
                            float yScaled = y / scaleFactor;

                            // Obtem as coordenadas inteiras dos pixels vizinhos
                            xTemp = (int)xScaled;
                            yTemp = (int)yScaled;

                            // Calcular as diferenças entre as coordenadas reais e os pixels vizinhos
                            float dx = xScaled - xTemp;
                            float dy = yScaled - yTemp;

                            // Obtem os ponteiros para os canais do pixel atual na imagem img
                            blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                            green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                            red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                            // Verifica se as coordenadas estão dentro dos limites da imagem img2
                            if (xTemp < 0 || yTemp < 0 || xTemp >= m.width - 1 || yTemp >= m.height - 1)
                            {
                                *blue = 0;
                                *green = 0;
                                *red = 0;
                                continue;
                            }

                            // Obtem os ponteiros para os pixels vizinhos na imagem img2
                            byte* p1 = (dataPtr2 + yTemp * m.widthStep + xTemp * nChan);
                            byte* p2 = p1 + nChan;
                            byte* p3 = p1 + m.widthStep;
                            byte* p4 = p3 + nChan;

                            // Realiza interpolação bilinear

                            // Calcula os pesos
                            float invDx = 1.0f - dx;
                            float invDy = 1.0f - dy;

                            float w1 = invDx * invDy;
                            float w2 = dx * invDy;
                            float w3 = invDx * dy;
                            float w4 = dx * dy;

                            // Atualiza os valores dos canais do pixel atual na imagem img
                            *blue = (byte)(w1 * p1[0] + w2 * p2[0] + w3 * p3[0] + w4 * p4[0]);
                            *green = (byte)(w1 * p1[1] + w2 * p2[1] + w3 * p3[1] + w4 * p4[1]);
                            *red = (byte)(w1 * p1[2] + w2 * p2[2] + w3 * p3[2] + w4 * p4[2]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// scale (x,y) - bilinear interpolation
        /// Absolute Addressing
        /// </summary>
        /// <param name="img">image</param>
        public static void Scale_point_xy_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                MIplImage m2 = imgCopy.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Ponteiro para a imagem original
                byte* dataPtr2 = (byte*)m2.imageData.ToPointer(); // Ponteiro para a imagem de escala

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // número de canais - 3
                int padding = m.widthStep - m.nChannels * m.width; // bytes de alinhamento (padding)
                int x, y, xTemp, yTemp;

                if (nChan == 3) // imagem em formato RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            // Calcula as coordenadas na imagem de escala
                            xTemp = (int)((x - (width / 2)) / scaleFactor) + centerX;
                            yTemp = (int)((y - (height / 2)) / scaleFactor) + centerY;

                            if (xTemp < 0 || yTemp < 0 || xTemp >= m2.width || yTemp >= m2.height)
                            {
                                // Se as coordenadas estão fora dos limites da imagem de escala,
                                // atribui 0 para os valores das componentes de cor na imagem original
                                byte* blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                                byte* green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                                byte* red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                                *blue = 0;
                                *green = 0;
                                *red = 0;
                            }
                            else
                            {
                                int x1 = (int)xTemp;
                                int x2 = x1 + 1;
                                int y1 = (int)yTemp;
                                int y2 = y1 + 1;

                                double deltaX = xTemp - x1;
                                double deltaY = yTemp - y1;

                                // Ponteiros para os pixels vizinhos na imagem de escala
                                byte* p1 = (dataPtr2 + y1 * m2.widthStep + x1 * nChan);
                                byte* p2 = (dataPtr2 + y1 * m2.widthStep + x2 * nChan);
                                byte* p3 = (dataPtr2 + y2 * m2.widthStep + x1 * nChan);
                                byte* p4 = (dataPtr2 + y2 * m2.widthStep + x2 * nChan);

                                // Ponteiros para as componentes de cor na imagem original
                                byte* blue = (byte*)(dataPtr + y * m.widthStep + x * nChan + 0);
                                byte* green = (byte*)(dataPtr + y * m.widthStep + x * nChan + 1);
                                byte* red = (byte*)(dataPtr + y * m.widthStep + x * nChan + 2);

                                // Interpolação bilinear para calcular os valores das componentes de cor
                                *blue = (byte)((1 - deltaX) * (1 - deltaY) * p1[0] +
                                               deltaX * (1 - deltaY) * p2[0] +
                                               (1 - deltaX) * deltaY * p3[0] +
                                               deltaX * deltaY * p4[0]);
                                *green = (byte)((1 - deltaX) * (1 - deltaY) * p1[1] +
                                                deltaX * (1 - deltaY) * p2[1] +
                                                (1 - deltaX) * deltaY * p3[1] +
                                                deltaX * deltaY * p4[1]);
                                *red = (byte)((1 - deltaX) * (1 - deltaY) * p1[2] +
                                              deltaX * (1 - deltaY) * p2[2] +
                                              (1 - deltaX) * deltaY * p3[2] +
                                              deltaX * deltaY * p4[2]);
                            }
                        }
                    }
                }
            }
        }



        //Mediana
        //Puzzle

        //Mean_solutionB
        //Mean_solutionC

        //Rotation_bilinear
        //Scale_bilinear (inc)
        //Scale_point_xy_bilinear (inc)

    }
}