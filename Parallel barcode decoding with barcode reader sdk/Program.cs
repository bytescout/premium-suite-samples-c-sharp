//*******************************************************************************************//
//                                                                                           //
// Download Free Evaluation Version From: https://bytescout.com/download/web-installer       //
//                                                                                           //
// Also available as Web API! Get Your Free API Key: https://app.pdf.co/signup               //
//                                                                                           //
// Copyright © 2017-2020 ByteScout, Inc. All rights reserved.                                //
// https://www.bytescout.com                                                                 //
// https://pdf.co                                                                            //
//                                                                                           //
//*******************************************************************************************//


using System;
using System.IO;
using System.Threading;
using Bytescout.BarCodeReader;

namespace ParallelDecoding
{
    class Program
    {
        const string InputFile = @".\example.pdf";

        // Limit to 4 threads in queue.
        // Set this value to number of your processor cores for max performance.
        private static readonly Semaphore ThreadLimiter = new Semaphore(4, 4);


        static void Main()
        {
            const int numberOfRuns = 10;
            ManualResetEvent[] doneEvents = new ManualResetEvent[numberOfRuns];


            for (int i = 0; i < numberOfRuns; i++)
            {
                // Wait for the queue
                ThreadLimiter.WaitOne();

                doneEvents[i] = new ManualResetEvent(false);

                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object state)
                {
                    int threadIndex = (int) state;

                    Console.WriteLine("Thread #" + threadIndex + " started...");

                    try
                    {
                        Reader reader = new Reader();
                        reader.RegistrationName = "demo";
                        reader.RegistrationKey = "demo";

                        // Set barcode type to find
                        reader.BarcodeTypesToFind.Code128 = true;

                        /* -----------------------------------------------------------------------
                        NOTE: We can read barcodes from specific page to increase performance.
                        For sample please refer to "Decoding barcodes from PDF by pages" program.
                        ----------------------------------------------------------------------- */

                        // Read barcodes
                        FoundBarcode[] barcodes = reader.ReadFrom(InputFile);
						
                        Console.WriteLine("Thread #" + threadIndex + " finished with " + barcodes.Length + " barcodes found.");

						// Cleanup
						reader.Dispose();
					}
                    catch (Exception exception)
                    {
                        Console.WriteLine("Thread #" + threadIndex + " failed with exception:\r\n" + exception.Message);
                    }
                    finally
                    {
                        // Signal the thread is finished
                        doneEvents[threadIndex].Set();

                        // Release semaphore
                        ThreadLimiter.Release();
                    }
                }), i);
            }

            WaitHandle.WaitAll(doneEvents);

            Console.WriteLine("All threads done.");
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
