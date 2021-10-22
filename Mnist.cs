using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConvolutionNeuralNetwork
{
    static class Mnist
    {
        public static List<InputsAndOutputs> GetImagesFromFile(string fileName, List<int> structure)
        {
            List<InputsAndOutputs> data = new List<InputsAndOutputs>();
            if (fileName.Contains(".json"))
            {
                data = JsonConvert.DeserializeObject<List<InputsAndOutputs>>(File.ReadAllText(fileName));
                if (structure[0] != data[0].Inputs.Count)
                    throw new Exception("Niepoprawna ilość wejść");
            }
            else if(fileName.Contains(".csv"))
            {
                List<string> file = File.ReadAllLines(fileName).ToList();
                file.RemoveAt(0);
                foreach (var line in file)
                {
                    List<string> chars_line = line.Split(',').ToList();
                    List<double> image = new List<double>();
                    foreach (var single_char in chars_line)
                        image.Add(Convert.ToDouble(single_char)/255);
                    image.RemoveAt(image.Count - 1);

                    int[] output = new int[10];
                    output[Convert.ToInt32(chars_line[chars_line.Count - 1])] = 1;
                    data.Add(new InputsAndOutputs(image, output.ToList()));
                }
            }
            else
            {
                throw new Exception("Błędny typ pliku?");
            }
            return data;
            // przy zczytywaniu danych z pliku trzeba będzie dodać 10 wyjść zamiast jednego (jedynka na dobym indexie i reszta zera)
        }

    }
}
