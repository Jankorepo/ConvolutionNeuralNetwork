using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            bool useConvolutions = false;
            int secondLayerNeuron = 28;
            int lastLayerNeuron = 8;
            int epochs = 10000;
            int repeat = 20;
            double learningRate = 0.1;

            //List<InputsAndOutputs> listOfTrainingImages2 = Mnist.GetImagesFromFile(@"mnist_784_csv.csv", neural_web.web_structure);
            //var listOfTrainingImages = listOfTrainingImages2.GetRange(0, 65000);
            //var listOfTestImages = listOfTrainingImages2.GetRange(65000, 5000);

            var listOfTrainingImages = JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki1.json"));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki2.json")));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki3.json")));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki4.json")));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki5.json")));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki6.json")));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki7.json")));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki8.json")));
            listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki9.json")));
            //listOfTrainingImages.AddRange(JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Znaki do testowania.json")));
            
            var listOfTestImages = JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(@"Zapisane znaki, Przemek.json"));
            //poprawić filtry?
            List<List<List<double>>> filters = EditImage.GetSome3x3Filters();
            double goodAnswers = 0;

            Web neural_web = new Web();
            var rand = new Random();

            for (int k = 0; k < repeat; k++)
            {
                for (int i = 0; i < epochs; i++)
                {
                    if (useConvolutions)
                    {
                        var choosenImage = listOfTrainingImages[rand.Next(listOfTrainingImages.Count - 1)];
                        var imageIn2DArray1 = EditImage.Swap1DListTo2DList(choosenImage.Inputs);
                        List<List<List<double>>> listOfConvImages = new List<List<List<double>>>();
                        for (int j = 0; j < filters.Count; j++)
                        {
                            //var convImage = EditImage.Pooling(imageIn2DArray1, 2, 2);
                            var convImage = EditImage.Convolution(imageIn2DArray1, filters[j], stride: 1);
                            convImage = EditImage.ReLU(convImage);
                            convImage = EditImage.Pooling(convImage, 2, 2);
                            listOfConvImages.Add(convImage);
                        }

                        var inputs = EditImage.SwapAll2DListsTo1DList(listOfConvImages);

                        if (neural_web.layers.Count != 0)
                        {
                            CalculateWebData.Calculate_Output(neural_web, inputs);
                            CalculateWebData.BackwardPropagation(neural_web, choosenImage.Outputs, learningRate);
                        }
                        else
                        {
                            neural_web.web_structure = new List<int>() { inputs.Count(), secondLayerNeuron, lastLayerNeuron };
                            neural_web.Fill(neural_web);
                        }
                    }


                    else if (!useConvolutions)
                    {
                        var choosenImage = listOfTrainingImages[rand.Next(listOfTrainingImages.Count - 1)];
                        if (neural_web.layers.Count != 0)
                        {
                            CalculateWebData.Calculate_Output(neural_web, choosenImage.Inputs);
                            CalculateWebData.BackwardPropagation(neural_web, choosenImage.Outputs, learningRate);
                        }
                        else
                        {
                            neural_web.web_structure = new List<int>() { listOfTrainingImages[0].Inputs.Count(), secondLayerNeuron, lastLayerNeuron };
                            neural_web.Fill(neural_web);
                        }
                    }
                }
                goodAnswers = 0;
                double celnosc = 0;
                double predkosc = 0;
                double regeneracja = 0;
                double wytrzymalosc = 0;
                double ogien = 0;
                double powietrze = 0;
                double woda = 0;
                double ziemia = 0;
                foreach (var image in listOfTestImages)
                {
                    if (useConvolutions)
                    {
                        var imageIn2DArray1 = EditImage.Swap1DListTo2DList(image.Inputs);
                        List<List<List<double>>> listOfConvImages1 = new List<List<List<double>>>();
                        for (int j = 0; j < filters.Count; j++)
                        {
                            //var convImage = EditImage.Pooling(imageIn2DArray1, 2, 2);
                            var convImage = EditImage.Convolution(imageIn2DArray1, filters[j], stride: 1);
                            convImage = EditImage.ReLU(convImage);
                            convImage = EditImage.Pooling(convImage, 2, 2);
                            listOfConvImages1.Add(convImage);
                        }
                        var inputs = EditImage.SwapAll2DListsTo1DList(listOfConvImages1);
                        CalculateWebData.Calculate_Output(neural_web, inputs);
                    }
                    else if (!useConvolutions)
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
                    {
                        goodAnswers++;
                        if (num1 == 0)
                            celnosc++;
                        else if (num1 == 1)
                            predkosc++;
                        else if (num1 == 2)
                            regeneracja++;
                        else if (num1 == 3)
                            wytrzymalosc++;
                        else if (num1 == 4)
                            ogien++;
                        else if (num1 == 5)
                            powietrze++;
                        else if (num1 == 6)
                            woda++;
                        else if (num1 == 7)
                            ziemia++;
                    }
                }
                Console.WriteLine("\nIlość nauczonych epok: " + ((k + 1) * epochs).ToString());
                Console.WriteLine("Średnia dobrych odpowiedzi: " + (Math.Round(goodAnswers / listOfTestImages.Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Celność: " + (Math.Round(celnosc /
                    listOfTestImages.Where(x => x.Outputs[0] == 1).ToList().Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Prędkość: " + (Math.Round(predkosc /
                    listOfTestImages.Where(x => x.Outputs[1] == 1).ToList().Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Regeneracja: " + (Math.Round(regeneracja /
                    listOfTestImages.Where(x => x.Outputs[2] == 1).ToList().Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Wytrzymałość: " + (Math.Round(wytrzymalosc /
                    listOfTestImages.Where(x => x.Outputs[3] == 1).ToList().Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Ogień: " + (Math.Round(ogien /
                    listOfTestImages.Where(x => x.Outputs[4] == 1).ToList().Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Powietrze: " + (Math.Round(powietrze /
                    listOfTestImages.Where(x => x.Outputs[5] == 1).ToList().Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Woda: " + (Math.Round(woda /
                    listOfTestImages.Where(x => x.Outputs[6] == 1).ToList().Count, 4) * 100).ToString() + "%");
                Console.WriteLine("Ziemia: " + (Math.Round(ziemia /
                    listOfTestImages.Where(x => x.Outputs[7] == 1).ToList().Count, 4) * 100).ToString() + "%");
            }

            //var imageIn2DArray = EditImage.Swap1DListTo2DList(choosenImage.Inputs, 28);
            //var bitmap = EditImage.ChangeListsToBitmap(imageIn2DArray);
            //bitmap.Save("mnist" + 5 + ".png", ImageFormat.Png);
            foreach (var neuron in neural_web.layers.Last())
            {
                neuron.output = 0;
                neuron.correction = 0;
            }
            File.WriteAllText(@"Web.json", JsonConvert.SerializeObject(neural_web));
            Console.ReadKey();
        }
    }
}
