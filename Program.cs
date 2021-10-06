using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            Web neural_web = new Web();
            neural_web.web_structure = new List<int>() { 432,32,10 };
            neural_web.Fill(neural_web);
            double learningRate = 0.15;
            List<InputsAndOutputs> listOfTrainingImages = Mnist.GetImagesFromFile(@"mnist_784_csv.csv", neural_web.web_structure);
            for (int i = 0; i < listOfTrainingImages.Count - 1; i++)
            {
                for (int j = 0; j < listOfTrainingImages[i].Inputs.Count; j++)
                {
                    listOfTrainingImages[i].Inputs[j] = listOfTrainingImages[i].Inputs[j] / 255;
                }
            }

            //poprawić filtry?

            double goodAnswers = 0;
            var rand = new Random();
            int epochs = 50000;
            for (int i = 0; i < epochs; i++)
            {
                var choosenImage1= listOfTrainingImages[rand.Next(listOfTrainingImages.Count - 1)];
                var imageIn2DArray1 = EditImage.Swap1DListTo2DList(choosenImage1.Inputs, 28);
                List<List<List<double>>> listOfConvImages1 = new List<List<List<double>>>();


                for (int j = 0; j < EditImage.GetSome3x3Filters().Count; j++)
                {
                    var convImage = EditImage.Convolution(imageIn2DArray1, EditImage.GetSome3x3Filters()[j], stride: 1);
                    convImage = EditImage.ReLU(convImage);
                    convImage = EditImage.Pooling(convImage, 2, 2);
                    convImage = EditImage.Convolution(convImage, EditImage.GetSome3x3Filters()[j], stride: 1);
                    convImage = EditImage.ReLU(convImage);
                    convImage = EditImage.Pooling(convImage, 2, 2);
                    listOfConvImages1.Add(convImage);
                }

                var inputs1 = EditImage.SwapAll2DListsTo1DList(listOfConvImages1);

                if (inputs1.Count != neural_web.web_structure[0])
                    throw new Exception();
                CalculateWebData.Calculate_Output(neural_web, inputs1);
                CalculateWebData.BackwardPropagation(neural_web, choosenImage1.Outputs, learningRate);

                int num1 = -1;
                int num2 = choosenImage1.Outputs.IndexOf(choosenImage1.Outputs.Where(x => x == 1).FirstOrDefault());
                double max = 0;
                foreach (var item in neural_web.layers.Last())
                {
                    if (item.output > max && item.output>0.3)
                    {
                        num1 = neural_web.layers.Last().IndexOf(item);
                        max = item.output;
                    }

                }
                if (i > 49000)
                    if (num1 == num2)
                        goodAnswers++;



            }

            Console.WriteLine(goodAnswers.ToString());


            //var imageIn2DArray = EditImage.Swap1DListTo2DList(choosenImage.Inputs, 28);
            //var bitmap = EditImage.ChangeListsToBitmap(imageIn2DArray);
            //bitmap.Save("mnist" + 5 + ".png", ImageFormat.Png);

            Console.ReadKey();

            
        }

    }
}
