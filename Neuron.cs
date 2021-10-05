using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    class Neuron
    {
        public double output;
        public List<double> matrix = new List<double>();
        public double correction;
        public double bias = 1;
        public Neuron() { }
        public Neuron(int number_of_neurons_in_previous_layer)
        {
            Random rnd = new Random();
            for (int i = 0; i < number_of_neurons_in_previous_layer + 1; i++)
                matrix.Add(rnd.NextDouble() * 2 - 1);
        }
        public Neuron Copy(Neuron oldNeuron)
        {
            Neuron newNeuron = new Neuron();
            foreach (var number in oldNeuron.matrix)
                newNeuron.matrix.Add(number);
            newNeuron.output = oldNeuron.output;
            return newNeuron;
        }
    }
}
