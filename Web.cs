using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    class Web
    {
        public List<List<Neuron>> layers = new List<List<Neuron>>();
        public List<int> web_structure;
        public Web Copy(Web my_old_web)
        {
            Web my_new_web = new Web();
            my_new_web.layers = new List<List<Neuron>>();
            foreach (var layer in my_old_web.layers)
            {
                my_new_web.layers.Add(new List<Neuron>());
                foreach (var single_neuron in layer)
                    my_new_web.layers[my_new_web.layers.Count - 1].Add(single_neuron.Copy(single_neuron));
            }
            my_new_web.web_structure = my_old_web.web_structure;
            return my_new_web;
        }
        public void Fill(Web my_web)
        {
            for (int i = 0; i < my_web.web_structure.Count; i++)
            {
                my_web.layers.Add(new List<Neuron>());
                for (int j = 0; j < my_web.web_structure[i]; j++)
                {
                    if (i == 0)
                        my_web.layers[i].Add(new Neuron());
                    else
                        my_web.layers[i].Add(new Neuron(my_web.web_structure[i - 1]));
                }
            }
        }
        public void FillSetValues(Web my_web)
        {
            Neuron n01 = new Neuron() {};
            Neuron n02 = new Neuron() {};
            Neuron n11 = new Neuron() { matrix = new List<double>() { 0.3, 0.1, 0.2 } };
            Neuron n12 = new Neuron() { matrix = new List<double>() { 0.6, 0.4, 0.5 } };
            Neuron n21 = new Neuron() { matrix = new List<double>() { 0.9, 0.7, -0.8 } };
            my_web.layers.Add(new List<Neuron>() { n01, n02 });
            my_web.layers.Add(new List<Neuron>() { n11, n12 });
            my_web.layers.Add(new List<Neuron>() { n21 });
        }
    }
}
