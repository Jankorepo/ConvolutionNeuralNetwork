using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            Web neural_web = new Web();
            //neural_web.web_structure = new List<int>() { 784,28,10 };
            neural_web.web_structure = new List<int>() { 720, 36, 8 };
            neural_web.Fill(neural_web);
            double learningRate = 0.15;
            //bool convolution = neural_web.web_structure[0]==784?false:true;
            //convolution = neural_web.web_structure[0] == 720 ? false : true;
            //List<InputsAndOutputs> listOfTrainingImages2 = Mnist.GetImagesFromFile(@"mnist_784_csv.csv", neural_web.web_structure);
            //var listOfTrainingImages = listOfTrainingImages2.GetRange(0, 65000);
            //var listOfTestImages = listOfTrainingImages2.GetRange(65000, 5000);



            var listOfTrainingImages = JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki1.json"));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki2.json")));
            var listOfTestImages = JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Znaki do testowania.json"));
            //poprawić filtry?
            List<List<List<double>>> filters = EditImage.GetSome3x3Filters();
            double goodAnswers = 0;
            var rand = new Random();
            int epochs = 10000;
            DateTime startTime = DateTime.Now;

            List<MultithreadingResults> list = new List<MultithreadingResults>();
            List<Thread> threadList = new List<Thread>()
            {
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 4)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 6)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 8)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 12)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 16)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 20)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 24)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 28)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 32)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 36)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 40)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 44)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 48)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 52)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 56)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 64)),
                new Thread(()=>RunNeuralWeb(listOfTrainingImages, listOfTestImages, rand, list, 72)),
            };
            int nameNum = 1;
            foreach (var thread in threadList)
            {
                thread.Name ="Wątek nr"+ nameNum++.ToString();
                thread.Start();
            }

            int xx = 0;
            //for (int k = 0; k < 10; k++)
            //{
            //    for (int i = 0; i < epochs; i++)
            //    {

            //        var choosenImage1 = listOfTrainingImages[rand.Next(listOfTrainingImages.Count - 1)];
            //        var imageIn2DArray1 = EditImage.Swap1DListTo2DList(choosenImage1.Inputs, 28);
            //        List<List<List<double>>> listOfConvImages1 = new List<List<List<double>>>();


            //        //for (int j = 0; j < filters.Count; j++)
            //        //{
            //        //    //var convImage = EditImage.Pooling(imageIn2DArray1, 2, 2);
            //        //    var convImage = EditImage.Convolution(imageIn2DArray1, filters[j], stride: 1);
            //        //    convImage = EditImage.ReLU(convImage);
            //        //    convImage = EditImage.Pooling(convImage, 2, 2);
            //        //    listOfConvImages1.Add(convImage);
            //        //}

            //        //var inputs1 = EditImage.SwapAll2DListsTo1DList(listOfConvImages1);

            //        //if (inputs1.Count != neural_web.web_structure[0])
            //        //    throw new Exception();
            //        CalculateWebData.Calculate_Output(neural_web, choosenImage1.Inputs);
            //        CalculateWebData.BackwardPropagation(neural_web, choosenImage1.Outputs, learningRate);

            //        //int num1 = -1;
            //        //int num2 = choosenImage1.Outputs.IndexOf(choosenImage1.Outputs.Where(x => x == 1).FirstOrDefault());
            //        //double max = 0;
            //        //foreach (var item in neural_web.layers.Last())
            //        //{
            //        //    if (item.output > max && item.output > 0.1)
            //        //    {
            //        //        num1 = neural_web.layers.Last().IndexOf(item);
            //        //        max = item.output;
            //        //    }

            //        //}
            //        //if (i > 49000)
            //        //    if (num1 == num2)
            //        //        goodAnswers++;
            //        //if (i % 1000 == 0 && i != 0)
            //        //    Console.WriteLine(i.ToString());
            //    }

            //    //sprawdzenie wyników
            //    goodAnswers = 0;

            //    foreach (var image in listOfTestImages)
            //    {
            //        CalculateWebData.Calculate_Output(neural_web, image.Inputs);
            //        int num1 = -1;
            //        int num2 = image.Outputs.IndexOf(image.Outputs.Where(x => x == 1).FirstOrDefault());
            //        double max = 0;
            //        foreach (var item in neural_web.layers.Last())
            //        {
            //            if (item.output > max && item.output > 0.1)
            //            {
            //                num1 = neural_web.layers.Last().IndexOf(item);
            //                max = item.output;
            //            }

            //        }
            //        if (num1 == num2)
            //            goodAnswers++;
            //    }
            //    MultithreadingResults res = new MultithreadingResults(neural_web, Math.Round(goodAnswers / listOfTestImages.Count, 4) * 100);
            //}


            //DateTime stopTime = DateTime.Now;
            //TimeSpan roznica = stopTime - startTime;
            //string result = "\nStruktura: ";
            //foreach (var item in neural_web.web_structure)
            //{
            //    result += item.ToString() + ",";
            //}
            //result += "  Ilość próbek: " + epochs.ToString() + ",  ";
            //result += "  Wynik: " + Math.Round(goodAnswers / listOfTestImages.Count, 2).ToString() +"%,  ";
            //result += "  Czas: " + Math.Round(roznica.TotalMinutes,1).ToString()+"min";
            ////result += "  Konwolucje: " + convolution.ToString();
            //File.AppendAllText("Wyniki uczenia sieci.txt", result);




            //var imageIn2DArray = EditImage.Swap1DListTo2DList(choosenImage.Inputs, 28);
            //var bitmap = EditImage.ChangeListsToBitmap(imageIn2DArray);
            //bitmap.Save("mnist" + 5 + ".png", ImageFormat.Png);

            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();

        }
        public static void RunNeuralWeb(List<InputsAndOutputs> trainData, List<InputsAndOutputs> testData, Random rand, List<MultithreadingResults> list, int neurons)
        {
            Web neural_web = new Web();
            neural_web.web_structure = new List<int>() { 720, neurons, 8 };
            neural_web.Fill(neural_web);
            double learningRate = 0.15;
            int epochs = 10000;
            
            double savedGoodAnsers=0;
            for (int k = 0; k < 15; k++)
            {
                double goodAnswers = 0;
                for (int i = 0; i < epochs; i++)
                {

                    var choosenImage1 = trainData[rand.Next(trainData.Count - 1)];
                    CalculateWebData.Calculate_Output(neural_web, choosenImage1.Inputs);
                    CalculateWebData.BackwardPropagation(neural_web, choosenImage1.Outputs, learningRate);
                }
                foreach (var image in testData)
                {
                    CalculateWebData.Calculate_Output(neural_web, image.Inputs);
                    int num1 = -1;
                    int num2 = image.Outputs.IndexOf(image.Outputs.Where(x => x == 1).FirstOrDefault());
                    double max = 0;
                    foreach (var item in neural_web.layers.Last())
                    {
                        if (item.output > max && item.output > 0.1)
                        {
                            num1 = neural_web.layers.Last().IndexOf(item);
                            max = item.output;
                        }

                    }
                    if (num1 == num2)
                        goodAnswers++;
                }
                savedGoodAnsers = goodAnswers;
            }
            Console.WriteLine(Thread.CurrentThread.Name + ": neurony "+neurons.ToString()+": odpowiedzi " +
                (Math.Round(savedGoodAnsers / testData.Count, 4) * 100) + "\n");
            list.Add(new MultithreadingResults(neural_web, Math.Round(savedGoodAnsers / testData.Count, 4) * 100));
        }
    }
}
