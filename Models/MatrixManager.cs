using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Models
{
    public static class MatrixManager
    {
        public static byte[,] HammingCodesMatrixWithoutParity = new byte[3, 7]
        {
            { 1, 1, 0, 1, 1, 0, 0},
            { 1, 1, 1, 0, 0, 1, 0},
            { 1, 0, 1, 1, 0, 0, 1}
        };

        public static byte[,] HammingCodesMatrixWithParity = new byte[4, 8]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1},
            { 0, 1, 1, 0, 1, 1, 0, 0},
            { 0, 1, 1, 1, 0, 0, 1, 0},
            { 0, 1, 0, 1, 1, 0, 0, 1}
        };
        public static byte[,] GeneratingMatrix = new byte[4, 7]
        {
            { 1, 0, 0, 0, 1, 1, 1},
            { 0, 1, 0, 0, 1, 1, 0},
            { 0, 0, 1, 0, 0, 1, 1},
            { 0, 0, 0, 1, 1, 0, 1}
        };

    }
}
