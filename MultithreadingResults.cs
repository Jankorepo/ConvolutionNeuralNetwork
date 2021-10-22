using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    class MultithreadingResults
    {
        Web web;
        double result;
        public MultithreadingResults(Web web,double result)
        {
            this.web = web;
            this.result = result;
        }
    }
}
