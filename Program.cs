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
            neural_web.web_structure = new List<int>() { 2,2,1 };
            neural_web.FillSetValues(neural_web);
            double learningRate = 0.1;
            //List<InputsAndOutputs> listOfTrainingImages = Mnist.GetImagesFromFile(@"mnist_784_csv.csv", neural_web.web_structure);
            List<InputsAndOutputs> listOfTrainingImages = new List<InputsAndOutputs>() { new InputsAndOutputs(new List<double>() { 1, 0 }, new List<int>() { 1 }) };

            //poprawić filtry?

            double goodAnswers = 0;
            var rand = new Random();
            int epochs = 100;
            for (int i = 0; i < epochs; i++)
            {
                var choosenImage1= listOfTrainingImages[rand.Next(listOfTrainingImages.Count - 1)];
                //var imageIn2DArray1 = EditImage.Swap1DListTo2DList(choosenImage1.Inputs, 28);
                //List<List<List<double>>> listOfConvImages1 = new List<List<List<double>>>();
                
                //for (int x = 0; x < EditImage.GetSome3x3Filters().Count; x++)
                //{
                //    var convImage = EditImage.Convolution(imageIn2DArray1, EditImage.GetSome3x3Filters()[x], stride: 1);
                //    convImage = EditImage.ReLU(convImage);
                //    convImage = EditImage.Pooling(convImage, 2, 2);
                //    convImage = EditImage.Pooling(convImage, 2, 2);
                //    listOfConvImages1.Add(convImage);
                //}
                //var inputs1=EditImage.SwapAll2DListsTo1DList(listOfConvImages1);

                


                CalculateWebData.Calculate_Output(neural_web, choosenImage1.Inputs);
                CalculateWebData.BackwardPropagation(neural_web, choosenImage1.Outputs, learningRate);


            }

            Console.WriteLine(goodAnswers.ToString());


            //var imageIn2DArray = EditImage.Swap1DListTo2DList(choosenImage.Inputs, 28);
            //var bitmap = EditImage.ChangeListsToBitmap(imageIn2DArray);
            //bitmap.Save("mnist" + 5 + ".png", ImageFormat.Png);

            Console.ReadKey();
        }
    }
}
