//*******************************************************************************************//
//                                                                                           //
// Download Free Evaluation Version From: https://bytescout.com/download/web-installer       //
//                                                                                           //
// Also available as Web API! Get Your Free API Key: https://app.pdf.co/signup               //
//                                                                                           //
// Copyright © 2017-2019 ByteScout, Inc. All rights reserved.                                //
// https://www.bytescout.com                                                                 //
// https://pdf.co                                                                            //
//                                                                                           //
//*******************************************************************************************//


using System;
using System.Diagnostics;
using Bytescout.PDFRenderer;


namespace PDF2PNG
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of Bytescout.PDFRenderer.RasterRenderer object and register it.
            RasterRenderer renderer = new RasterRenderer();
            renderer.RegistrationName = "demo";
            renderer.RegistrationKey = "demo";

            RenderingOptions renderingOptions = new RenderingOptions();

            // Enable text objects rendering
            renderingOptions.RenderTextObjects = true;
            // DISABLE vector objects rendering
            renderingOptions.RenderVectorObjects = false;
            // DISABLE image objects rendering
            renderingOptions.RenderImageObjects = false;
            
            // Load PDF document.
            renderer.LoadDocumentFromFile("multipage.pdf");

            for (int i = 0; i < renderer.GetPageCount(); i++)
            {
                // Render document page to PNG image file.
                renderer.Save("image" + i + ".png", RasterImageFormat.PNG, i, 300, renderingOptions);
            }

            // Cleanup
            renderer.Dispose();

            // Open result document in default associated application (for demo purpose)
            ProcessStartInfo processStartInfo = new ProcessStartInfo("image0.png");
            processStartInfo.UseShellExecute = true;
            Process.Start(processStartInfo);
        }
    }
}
