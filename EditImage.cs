using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionNeuralNetwork
{
    static class EditImage
    {
        public static List<List<double>> Swap1DListTo2DList(List<double> imageInArray, int size)
        {
            List<List<double>> image = new List<List<double>>();
            for (int i = 0; i < imageInArray.Count-size+1; i+=size)
            {
                image.Add(new List<double>());
                for (int j = 0; j < size; j++)
                {
                    image.Last().Add(imageInArray[i+j]);
                }
            }

            return image;
        }
        public static List<double> SwapAll2DListsTo1DList(List<List<List<double>>> listOfConvImages)
        {
            List<double> array = new List<double>();
            foreach (var list in listOfConvImages)
                for (int i = 0; i < list.Count; i++)
                    for (int j = 0; j < list.Count; j++)
                        array.Add(list[i][j]);
            return array;
        }
        public static List<List<double>> Convolution(List<List<double>> image, List<List<double>> filter, int stride)
        {
            var newImage = new List<List<double>>();
            for (int i = filter.Count / 2; i < image.Count-1; i += stride)
            {
                newImage.Add(new List<double>());
                for (int j = filter.Count / 2; j < image[0].Count-1; j += stride)
                {
                    List<List<double>> matrix = CreateTmpMatrix(i, j, image, filter.Count);
                    double sum = MultiplicationOfMatrices(matrix, filter);
                    newImage.Last().Add(sum);
                }
            }
            return newImage;
        }

        

        private static List<List<double>> CreateTmpMatrix(int i, int j, List<List<double>> image, int matrixSize)
        {
            List<List<double>> matrix = new List<List<double>>();
            for (int x = 0; x < matrixSize-1; x++)
            {
                matrix.Add(new List<double>());
                for (int y = 0; y < matrixSize-1; y++)
                    matrix.Last().Add(image[i - matrixSize / 2 + x][j - matrixSize / 2 + y]);
            }
            return matrix;
        }

        public static double MultiplicationOfMatrices(List<List<double>> matrix, List<List<double>> filter)
        {
            double sum = 0;
            int size = matrix.Count;
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    sum += matrix[x][y] * filter[x][y];
            return sum / (size * size)<1? sum / (size * size):1;
        }
        public static List<List<double>> ReLU(List<List<double>> image)
        {
            for (int i = 0; i < image.Count; i++)
                for (int j = 0; j < image[0].Count; j++)
                    image[i][j] = image[i][j] < 0 ? 0 : image[i][j];
            return image;
        }
        internal static List<List<double>> Pooling(List<List<double>> image, int maxPoolingSize, int stride)
        {
            var size = image.Count % 2 == 1 ? image.Count + 1 : image.Count;
            var imageAfterPooling = new List<List<double>>();
            for (int i = 1; i < size; i += stride)
            {
                imageAfterPooling.Add(new List<double>());
                for (int j = 1; j < size; j += stride)
                {
                    double highestValue = ChooseHighestValue(i, j, image, maxPoolingSize);
                    imageAfterPooling.Last().Add(highestValue);
                }
            }
            return imageAfterPooling;
        }

        private static double ChooseHighestValue(int i, int j, List<List<double>> image, int maxPoolingSize)
        {
            List<double> values = new List<double>();
            for (int x = 0; x < maxPoolingSize; x++)
            {
                for (int y = 0; y < maxPoolingSize; y++)
                {
                    int imageX = i - (maxPoolingSize / 2) + x;
                    int imageY = j - (maxPoolingSize / 2) + y;
                    if (imageX < image.Count && imageY < image[0].Count)
                        values.Add(image[imageX][imageY]);
                    else
                        values.Add(0);
                }
            }
            return values.Max();
        }
        public static Bitmap ChangeListsToBitmap(List<List<double>> image)
        {
            var bitmap = new Bitmap(image.Count, image[0].Count);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int value = Convert.ToInt32(image[x][y]*255);
                    bitmap.SetPixel(y, x, Color.FromArgb(255, value, value, value));
                }
            }
            return bitmap;
        }

        public static List<List<List<double>>> GetSome3x3Filters()
        {
            //wykrywanie krawędzi 12 sztuk
            List<List<List<double>>> listOfFilters = new List<List<List<double>>>()
            {
                new List<List<double>>() {
                    new List<double>(){-1,-1,-1},
                    new List<double>(){2,2,2},
                    new List<double>(){-1,-1,-1},
                },
                new List<List<double>>() {
                    new List<double>(){-1,2,-1},
                    new List<double>(){-1,2,-1},
                    new List<double>(){-1,2,-1},
                },
                new List<List<double>>() {
                    new List<double>(){2,-1,-1},
                    new List<double>(){-1,2,-1},
                    new List<double>(){-1,-1,2},
                },
                new List<List<double>>() {
                    new List<double>(){-1,-1,2},
                    new List<double>(){-1,2,-1},
                    new List<double>(){2,-1,-1},
                },
                new List<List<double>>() {
                    new List<double>(){-1,-1,-1},
                    new List<double>(){-1,8,-1},
                    new List<double>(){-1,-1,-1},
                },
                new List<List<double>>() {
                    new List<double>(){1,1,1},
                    new List<double>(){1,-8,1},
                    new List<double>(){1,1,1},
                },
                new List<List<double>>() {
                    new List<double>(){0,-1,0},
                    new List<double>(){-1,4,-1},
                    new List<double>(){0,-1,0},
                },
                new List<List<double>>() {
                    new List<double>(){0,1,0},
                    new List<double>(){1,-4,1},
                    new List<double>(){0,1,0},
                },
                new List<List<double>>() {
                    new List<double>(){-1,-1,0},
                    new List<double>(){-1,0,1},
                    new List<double>(){0,1,1},
                },
                 new List<List<double>>() {
                    new List<double>(){1,1,0},
                    new List<double>(){1,0,-1},
                    new List<double>(){0,-1,-1},
                },
                new List<List<double>>() {
                    new List<double>(){0,-1,-1},
                    new List<double>(){1,0,-1},
                    new List<double>(){1,1,0},
                },
                new List<List<double>>() {
                    new List<double>(){0,1,1},
                    new List<double>(){-1,0,1},
                    new List<double>(){-1,-1,0},
                },

                // nowe filtry, do sprawdzenia czy działają
                new List<List<double>>() {
                    new List<double>(){-1,1,1},
                    new List<double>(){1,-1,1},
                    new List<double>(){-1,-1,1},
                },
                new List<List<double>>() {
                    new List<double>(){-1,2,-1},
                    new List<double>(){2,-1,2},
                    new List<double>(){-1,-1,-1},
                },
                new List<List<double>>() {
                    new List<double>(){-1,-1,1},
                    new List<double>(){-1,1,-1},
                    new List<double>(){1,1,-1},
                },
                new List<List<double>>() {
                    new List<double>(){-1,-1,1},
                    new List<double>(){-1,1,-1},
                    new List<double>(){1,1,1},
                },
                new List<List<double>>() {
                    new List<double>(){1,1,1},
                    new List<double>(){-1,-1,1},
                    new List<double>(){-1,-1,1},
                },
                new List<List<double>>() {
                    new List<double>(){1,1,1},
                    new List<double>(){1,-1,-1},
                    new List<double>(){1,-1,-1},
                },
                new List<List<double>>() {
                    new List<double>(){-1,-1,1},
                    new List<double>(){-1,1,-1},
                    new List<double>(){1,1,-1},
                },
            };
            return listOfFilters;
        }
    }
}
