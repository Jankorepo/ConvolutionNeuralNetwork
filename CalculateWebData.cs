using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    static class CalculateWebData
    {
        static public void Calculate_Output(Web my_web, List<double> inputs)
        {
            for (int i = 0; i < my_web.layers[0].Count; i++)
                my_web.layers[0][i].output = inputs[i];
            GetNeuronsOutputs(my_web);
        }

        private static void GetNeuronsOutputs(Web my_web)
        {
            for (int i = 1; i < my_web.layers.Count; i++)
                for (int j = 0; j < my_web.layers[i].Count; j++)
                {
                    double neuron_result = CalculateSingleNeuronResult(my_web.layers[i][j], my_web.layers[i - 1]);
                    my_web.layers[i][j].output = 1 / (1 + (Math.Exp((-1) * (neuron_result))));
                }
        }

        private static double CalculateSingleNeuronResult(Neuron neuron, List<Neuron> previous_layer)
        {
            double result = neuron.bias * neuron.matrix[0];
            for (int i = 0; i < previous_layer.Count; i++)
                result += previous_layer[i].output * neuron.matrix[i + 1];
            return result;
        }

        static public void BackwardPropagation(Web my_web, List<int> outputs, double learning_rate)
        {
            for (int i = my_web.layers.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < my_web.layers[i].Count; j++)
                {
                    if (i == my_web.layers.Count - 1)
                        my_web.layers[i][j].correction = ((outputs[j] - my_web.layers[i][j].output) * learning_rate) *
                            my_web.layers[i][j].output * (1 - my_web.layers[i][j].output);
                    else
                    {
                        List<double> list_of_corrections = GetAllCorrectionsOfNextLayer(my_web.layers[i + 1], j);
                        my_web.layers[i][j].correction = GetCorrectionOfNeuronInHiddenLayer(list_of_corrections) *
                            my_web.layers[i][j].output * (1 - my_web.layers[i][j].output);
                    }
                }
            }
            for (int i = 1; i < my_web.layers.Count; i++)
            {
                for (int j = 0; j < my_web.layers[i].Count; j++)
                {
                    UpgradeTheNeuronWeigts(my_web.layers[i][j], my_web.layers[i - 1]);
                }
            }
        }
        private static List<double> GetAllCorrectionsOfNextLayer(List<Neuron> next_layer, int number_of_neuron)
        {
            List<double> tmp_list = new List<double>();
            for (int i = 0; i < next_layer.Count; i++)
                tmp_list.Add(next_layer[i].correction * next_layer[i].matrix[number_of_neuron + 1]);
            return tmp_list;
        }
        private static double GetCorrectionOfNeuronInHiddenLayer(List<double> list_of_corrections)
        {
            double sum_of_corrections = 0;
            foreach (var correction_from_single_neuron in list_of_corrections)
                sum_of_corrections += correction_from_single_neuron;
            return sum_of_corrections;
        }
        private static void UpgradeTheNeuronWeigts(Neuron neuron, List<Neuron> previous_layer)
        {
            neuron.matrix[0] += neuron.bias * neuron.correction;
            for (int i = 1; i < neuron.matrix.Count; i++)
                neuron.matrix[i] += previous_layer[i - 1].output * neuron.correction;
        }
    }
}
